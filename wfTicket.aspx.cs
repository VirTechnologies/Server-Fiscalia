using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Threading;

public partial class wfTicket : System.Web.UI.Page
{
    Panel div;

    public int TotalFilas { get; set; }
    private const int _firstEditCellIndex = 6; // INICIA LA EDICIÓN DE LAS COLUMNAS DESDE LA SEXTA COLUMNA

    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        string idModulo = "66";

        //=============== ESTAS 4 LINEAS SON PARA LAS FECHAS ===============================
        DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
        myDTF.ShortDatePattern = "dd/MM/yyyy";
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");

        // CONFIGURA LOS BOTONES DE ACUERDO A LOS PERMISOS
        if (!((objUsuario.PermisoModulo(idModulo, Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo(idModulo, Session["IDUSUARIO"].ToString(), "U"))))
        {
            FilaBotones.Visible = false;
        }
        else
        {
            FilaBotones.Visible = true;
        }

        //if (!objUsuario.PermisoModulo(idModulo, Session["IDUSUARIO"].ToString(), "D"))
        //    btnAgregarCampo.Visible = false;

        if (!Page.IsPostBack)
        {
            // PRIMERA VEZ QUE CARGA
            Session["TablaCampos"] = TablaManipular = null;
            this.gvCampos.DataSource = TablaManipular;
            this.gvCampos.DataBind();
        }
        else
        {
            notificacion.Visible = false;
            if (this.gvCampos.SelectedIndex > -1)
            {
                // Call UpdateRow on every postback
                TotalFilas = gvCampos.Rows.Count;
                this.gvCampos.UpdateRow(this.gvCampos.SelectedIndex, false);
            }
        }
    }

    protected void gvCampos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            // Get the LinkButton control in the first cell
            LinkButton _singleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsSingle = ClientScript.GetPostBackClientHyperlink(_singleClickButton, "");

            // If the page contains validator controls then call 
            // Page_ClientValidate before allowing a cell to be edited
            if (Page.Validators.Count > 0)
                _jsSingle = _jsSingle.Insert(11, "if(Page_ClientValidate())");

            // Add events to each editable cell
            for (int columnIndex = _firstEditCellIndex; columnIndex < e.Row.Cells.Count-1; columnIndex++)
            {
                // Add the column index as the event argument parameter
                string js = _jsSingle.Insert(_jsSingle.Length - 2, columnIndex.ToString());
                // Add this javascript to the onclick Attribute of the cell
                e.Row.Cells[columnIndex].Attributes["onclick"] = js;
                // Add a cursor style to the cells
                e.Row.Cells[columnIndex].Attributes["style"] += "cursor:pointer;cursor:hand;";
            }

            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            //if (!obj.PermisoModulo("46", Session["IDUSUARIO"].ToString(), "U"))
            //    e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;

            // COLUMNA 2 ES ORDEN
            if (e.Row.Cells[2].Text == "1")
            {
                // PRIMERA FILA
                Button btnUp = (Button)e.Row.FindControl("btnUp");
                btnUp.CssClass = "btn btn-primary btn-sm disabled";
                btnUp.Enabled = false;
            }

            // COLUMNA 2 ES ORDEN
            if (e.Row.Cells[2].Text == TotalFilas.ToString())
            {
                // ULTIMA FILA
                Button btnDown = (Button)e.Row.FindControl("btnDown");
                btnDown.CssClass = "btn btn-primary btn-sm disabled";
                btnDown.Enabled = false;
            }

            // COLUMNA 3 ES CAMPO OBLIGATORIO
            if (Convert.ToBoolean(e.Row.Cells[3].Text))
            {
                e.Row.Cells[3].Text = "SI";
                Button btnBorrar = (Button)e.Row.FindControl("btnBorrar");
                btnBorrar.Visible = false;
            }
            else
            {
                e.Row.Cells[3].Text = "NO";
            }

        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
            }
        }
    }

    protected void gvCampos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow filaClic;

        int index = Convert.ToInt32(e.CommandArgument);
        filaClic = gvCampos.Rows[index];
        if (e.CommandName == "Up")
        {
            DataTable dt = (DataTable)Session["TablaCampos"];
            DataRow[] filaMover = dt.DefaultView.Table.Select($"idCampo = {gvCampos.Rows[index].Cells[1].Text}");
            DataRow[] filaAnterior = dt.DefaultView.Table.Select($"idCampo = {gvCampos.Rows[index-1].Cells[1].Text}");
            int ordenFila = Convert.ToInt32(filaMover[0]["orden"]);
            int ordenFilaAnterior = Convert.ToInt32(filaAnterior[0]["orden"]);

            dt.DefaultView.Table.Rows[index]["orden"] = ordenFilaAnterior;
            dt.DefaultView.Table.Rows[index - 1]["orden"] = ordenFila;

            dt.DefaultView.Sort = "orden";
            dt.AcceptChanges();

            TotalFilas = dt.Rows.Count;
            Session["TablaCampos"] = dt.DefaultView.ToTable();

            gvCampos.DataSource = Session["TablaCampos"];
            gvCampos.DataBind();
        }
        if (e.CommandName == "Down")
        {
            DataTable dt = (DataTable)Session["TablaCampos"];
            DataRow[] filaMover = dt.DefaultView.Table.Select($"idCampo = {gvCampos.Rows[index].Cells[1].Text}");
            DataRow[] filaSiguiente = dt.DefaultView.Table.Select($"idCampo = {gvCampos.Rows[index + 1].Cells[1].Text}");
            int ordenFila = Convert.ToInt32(filaMover[0]["orden"]);
            int ordenFilaSiguiente = Convert.ToInt32(filaSiguiente[0]["orden"]);

            dt.DefaultView.Table.Rows[index]["orden"] = ordenFilaSiguiente;
            dt.DefaultView.Table.Rows[index + 1]["orden"] = ordenFila;

            dt.DefaultView.Sort = "orden";
            dt.AcceptChanges();

            TotalFilas = dt.Rows.Count;
            Session["TablaCampos"] = dt.DefaultView.ToTable();
            gvCampos.DataSource = Session["TablaCampos"];
            gvCampos.DataBind();

        }
        if (e.CommandName == "Borrar")
        {
            if (filaClic.Cells[3].Text == "NO")
            {
                DataTable dt = (DataTable)Session["TablaCampos"];
                dt.DefaultView.Table.Rows[index].Delete();

                dt.DefaultView.Sort = "orden";
                dt.AcceptChanges();

                TotalFilas = dt.Rows.Count;
                Session["TablaCampos"] = dt.DefaultView.ToTable();
                gvCampos.DataSource = Session["TablaCampos"];
                gvCampos.DataBind();
            }
        }
        if (e.CommandName == "SingleClick")
        {
            GridView _gridView = (GridView)sender;

            // Get the row index
            int _rowIndex = int.Parse(e.CommandArgument.ToString());
            // Parse the event argument (added in RowDataBound) to get the selected column index
            int _columnIndex = int.Parse(Request.Form["__EVENTARGUMENT"]);

            if ((filaClic.Cells[3].Text == "SI") && (_columnIndex == 6))
            {
                e.Handled = true;
                //_gridView.DataSource = TablaManipular;
                //_gridView.DataBind();
            }
            else
            {
                // Set the Gridview selected index
                _gridView.SelectedIndex = _rowIndex;
                // Bind the Gridview

                //CargarCampos();
                _gridView.DataSource = TablaManipular;
                _gridView.DataBind();

                // Get the display control for the selected cell and make it invisible
                Control _displayControl = _gridView.Rows[_rowIndex].Cells[_columnIndex].Controls[1];
                _displayControl.Visible = false;
                // Get the edit control for the selected cell and make it visible
                Control _editControl = _gridView.Rows[_rowIndex].Cells[_columnIndex].Controls[3];
                _editControl.Visible = true;
                // Clear the attributes from the selected cell to remove the click event
                _gridView.Rows[_rowIndex].Cells[_columnIndex].Attributes.Clear();

                // Set focus on the selected edit control
                ScriptManager.RegisterStartupScript(this, GetType(), "SetFocus", "document.getElementById('" + _editControl.ClientID + "').focus();", true);
                // If the edit control is a dropdownlist set the
                // SelectedValue to the value of the display control
                if (_editControl is DropDownList && _displayControl is Label)
                {
                    ((DropDownList)_editControl).SelectedValue = ((Label)_displayControl).Text;
                }
                // If the edit control is a textbox then select the text
                if (_editControl is TextBox)
                {
                    ((TextBox)_editControl).Attributes.Add("onfocus", "this.select()");
                }
                // If the edit control is a checkbox set the
                // Checked value to the value of the display control
                if (_editControl is CheckBox && _displayControl is Label)
                {
                    ((CheckBox)_editControl).Checked = bool.Parse(((Label)_displayControl).Text);
                }
            }
        }


        //CargarCampos();
    }


    protected void btnSalir_Click(object sender, EventArgs e)
    {
        //Session["Volver"] = "S";
        Response.Redirect("wfBienvenida.aspx");
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        // Add a new empty row
        DataTable dt = TablaManipular;
        int newid = dt.Rows.Count + 1;
        dt.Rows.Add(new object[] { newid, newid, false, "2", "Adicional", "", 18, "Helvetica LT Std", "Centro" });
        TablaManipular = dt;

        // Repopulate the GridView
        this.gvCampos.DataSource = TablaManipular;
        this.gvCampos.DataBind();
    }

    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        NSSSqlUtil blObj = new NSSSqlUtil();
        String msgError = "";
        DataSet dsInterno = null;
        String queryEjecutado = "";
        clsblUtiles blU = new clsblUtiles();
        string error = "ERROR";
        string msjs = "";
        try
        {
            // TODO GRABAR CAMBIOS
            clsblParametricas blParam = new clsblParametricas();
            DataTable dt = (DataTable)Session["TablaCampos"];

            error = blParam.ActualizarCamposTicket(dt, ref queryEjecutado);
            if (error == "")
            {
                msjs = "Ticket Guardado exitosamente!<br>Enviando actualización a Oficinas!<br>";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
            }
            else
            {
                msjs = "Error grabando la información del ticket!<br>";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            }
            lbConfirmacion.Text = msjs;
            notificacion.Visible = true;
        }
        catch (Exception ex)
        {
            msjs = "Error grabando información: " + ex.ToString();
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
        }

        try
        {
            if (error == "")
            {
                List<string> Sentencias = new List<string>();
                Sentencias.Add(queryEjecutado);
                blU.EncolarMensajesRabbit(Sentencias, "", true);
                msjs += "¡Actualización enviada exitosamente!";
            }
        }
        catch (Exception ex)
        {
            msjs += "Error enviando actualización: " + ex.Message + "<br>";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
        }
        lbConfirmacion.Text = msjs;
        notificacion.Visible = true;
    }


    #region Render Override

    // Register the dynamically created client scripts
    protected override void Render(HtmlTextWriter writer)
    {
        // The client events for GridView1 were created in GridView1_RowDataBound
        foreach (GridViewRow r in gvCampos.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                for (int columnIndex = _firstEditCellIndex; columnIndex < r.Cells.Count-1; columnIndex++)
                {
                    Page.ClientScript.RegisterForEventValidation(r.UniqueID + "$ctl00", columnIndex.ToString());
                }
            }
        }

        base.Render(writer);
    }

    #endregion


    protected void gvCampos_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        if (e.RowIndex > -1)
        {
            // Loop though the columns to find a cell in edit mode
            for (int i = _firstEditCellIndex; i < _gridView.Columns.Count-1; i++)
            {
                // Get the editing control for the cell
                Control _editControl = _gridView.Rows[e.RowIndex].Cells[i].Controls[3];
                if (_editControl.Visible)
                {
                    int _dataTableColumnIndex = i - 1;

                    try
                    {
                        // Get the id of the row
                        //Label idLabel = (Label)_gridView.Rows[e.RowIndex].FindControl("IdLabel");
                        //int id = int.Parse(idLabel.Text);

                        int id = int.Parse(_gridView.Rows[e.RowIndex].Cells[2].Text);

                        // Get the value of the edit control and update the DataTable
                        DataTable dt = TablaManipular;
                        DataRow dr = dt.Rows.Find(id);
                        dr.BeginEdit();
                        if (_editControl is TextBox)
                        {
                            dr[_dataTableColumnIndex] = ((TextBox)_editControl).Text;
                        }
                        else if (_editControl is DropDownList)
                        {
                            dr[_dataTableColumnIndex] = ((DropDownList)_editControl).SelectedValue;
                        }
                        else if (_editControl is CheckBox)
                        {
                            dr[_dataTableColumnIndex] = ((CheckBox)_editControl).Checked;
                        }
                        dr.EndEdit();

                        // Save the updated DataTable
                        TablaManipular = dt;

                        // Clear the selected index to prevent 
                        // another update on the next postback
                        _gridView.SelectedIndex = -1;

                        // Repopulate the GridView
                        _gridView.DataSource = dt;
                        _gridView.DataBind();
                    }
                    catch (ArgumentException)
                    {
                        // Repopulate the GridView
                        _gridView.DataSource = TablaManipular;
                        _gridView.DataBind();
                    }
                }
            }
        }
    }

    private DataTable TablaManipular
    {
        get
        {
            DataTable dt = (DataTable)Session["TablaCampos"];

            if (dt == null)
            {
                clsblParametricas blParam = new clsblParametricas();
                clsblUtiles blU = new clsblUtiles();
                DataSet dsCampos = new DataSet();

                string error = blParam.ConsultaCamposTicket(ref dsCampos);
                if (error == "")
                {
                    if (dsCampos.Tables[0].Rows.Count <= 0)
                    {
                        lbConfirmacion.Text = "No hay campos definidos para el ticket!";
                        notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                        notificacion.Visible = true;
                    }
                    else
                    {
                        dt = dsCampos.Tables[0];

                        //// Add the id column as a primary key
                        DataColumn[] keys = new DataColumn[1];
                        keys[0] = dt.Columns["idCampo"];
                        dt.PrimaryKey = keys;

                        gvCampos.DataSource = dsCampos;
                        TotalFilas = dsCampos.Tables[0].Rows.Count;
                        gvCampos.DataBind();
                        Session["DS"] = gvCampos;
                    }
                }
                else
                {
                    lbConfirmacion.Text = "Error consultando los campos del ticket!";
                    notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                    notificacion.Visible = true;
                }

                TablaManipular = dt;
            }

            return dt;
        }
        set
        {
            Session["TablaCampos"] = value;
        }
    }


    private void CargarCampos()
    {
        clsblParametricas blParam = new clsblParametricas();
        clsblUtiles blU = new clsblUtiles();
        DataSet dsCampos = new DataSet();

        string error = blParam.ConsultaCamposTicket(ref dsCampos);
        if (error == "")
        {
            if (dsCampos.Tables[0].Rows.Count <= 0)
            {
                lbConfirmacion.Text = "No hay campos definidos para el ticket!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacion.Visible = true;
            }
            else
            {
                gvCampos.DataSource = dsCampos;
                TotalFilas = dsCampos.Tables[0].Rows.Count;
                gvCampos.DataBind();
                Session["DS"] = gvCampos;
            }
        }
        else
        {
            lbConfirmacion.Text = "Error consultando los campos del ticket!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }

    }

    private void CrearCampoDinamico(DataRow Campo)
    {
        switch (Campo["Tipo"].ToString())
        {
            //case "1": // TIPO IMAGEN
            //    {
            //        break;
            //    }
            //case "2": // TIPO TEXTO
            //    {
            //        break;
            //    }
            default: // SOLO CARGAR TEXTBOX CON LA INFORMACIÓN
                {
                    TextBox tb = new TextBox()
                    {
                        ID = "Campo" + Campo["nombre"].ToString(),
                        Text = Campo["contenido"].ToString(),
                        //Font.Size = Campo["tamanoFuente"].ToString(),
                        ForeColor = System.Drawing.Color.Black,
                        CssClass = "form-control",
                        Visible = true
                    };

                    switch (Convert.ToInt32(Campo["align"].ToString()))
                    {
                        case 0: tb.Style["text-align"] = "left"; break;
                        case 1: tb.Style["text-align"] = "right"; break;
                        case 2: tb.Style["text-align"] = "center"; break;
                        default: tb.Style["text-align"] = "justify"; break;
                    }

                    //switch (Convert.ToInt32(["estilo"].ToString())
                    //{
                    //    case 1: tb.Attributes["text-align"] = "left"; break;
                    //    case 2: tb.Attributes["text-align"] = "right"; break;
                    //    case 3: tb.Attributes["text-align"] = "right"; break;
                    //    default: tb.Attributes["text-align"] = "center"; break;
                    //}

                    if (Convert.ToBoolean(Campo["obligatorio"].ToString()))
                        tb.ReadOnly = true;

                    div.Controls.Add(tb);

                    break;
                }
        }
    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wfEstadisticasAgendados : System.Web.UI.Page
{
    int IndexPage = 0;
    DataSet dsTurnos = new DataSet();
    GridView gvExportar = new GridView();
    Image sortImage = new Image();

    public string OrdenarDir
    {
        get
        {
            if (ViewState["OrdenarDir"] == null)
                return string.Empty;
            else
                return ViewState["OrdenarDir"].ToString();
        }
        set
        {
            ViewState["OrdenarDir"] = value;
        }
    }
    private string _ordenarDir;

    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUtiles blU = new clsblUtiles();

        if (!Page.IsPostBack)
        {
            blU.LlenaDDLObligatorio(ddlOficinaId, "Oficinas", "Id", "Nombre","","", "Nombre");
        }
        //if (txSQL.Text != "")
        {
            Filtrar();
        }
        Session["txSQL"] = "";
    }

    private void Filtrar()
    {
        clsblEstadisticas blEstadi = new clsblEstadisticas();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        string JSonData = "[";
        int i = 0;
        DateTime fecha;
        string script = "";
        string fechaIni = "", fechaFin = "";

        lbConfirmacion.Text = "";
        try
        {
            if (tbFechaIni.Text != "")
            {
                fechaIni = tbFechaIni.Text + " 00:00:00.000";
                fecha = blU.FechaDeString(fechaIni);
            }
        }
        catch (Exception)
        {
            lbConfirmacion.Text = "El formato de la fecha inicial es inválido!";
            return;
        }
        try
        {
            if (tbFechaFin.Text != "")
            {
                fechaFin = tbFechaFin.Text + " 23:59:59.998";
                fecha = blU.FechaDeString(fechaFin);
            }
        }
        catch (Exception)
        {
            lbConfirmacion.Text = "El formato de la fecha final es inválido!";
            return;
        }
        TablaInfoTurnos.Visible = true;
        
        /* 2021 - TURNOS AGENDADOS */
        msgError = blEstadi.ConsultaTurnosAgendados(ref dsTurnos, "AGENDADOS", ddlOficinaId.SelectedValue, tbFechaIni.Text, tbFechaFin.Text);
        if (msgError == "")
        {
            JSonData = "[";
            gvAgendados.DataSource = dsTurnos;
            gvAgendados.DataBind();
            if (gvAgendados.Rows.Count > 0)
            {
                ViewState["DataSet"] = dsTurnos;

                tbNoRegistros.Text = dsTurnos.Tables[0].Rows.Count.ToString();
                lblNoRegistros.Visible = tbNoRegistros.Visible = true;
                lblSinRegistros.Visible = false;

                if (OrdenarDir == "")
                {
                    SetSortDirection("");
                }

                //gvAgendados.HeaderRow.TableSection = TableRowSection.TableHeader;
                //for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
                //{
                //    String Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                //    Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();

                //    JSonData += "{'groupname': '" + dsTurnos.Tables[0].Rows[i]["Oficina"].ToString() + "', 'count': " + dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
                //    if (i < dsTurnos.Tables[0].Rows.Count - 1)
                //        JSonData += ",";
                //}
                //JSonData += "];";
                //script += " \n var data4=" + JSonData + "\n countChart(data4, 'chartdivAgendados', 'Número de turnos agendados'); \n";
            }
            else
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
                //btnExportar.Visible = false;
            }
            txSQL.Text = "Consultó";
        }
        else
        {
            txSQL.Text = "";
            return;
        }


        if (script!="")
            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
    }


    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        ViewState["OrdenarCampo"] = null;
        ViewState["OrdenarDir"] = null;
        Filtrar();
    }

    protected void gvAgendados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        /*
           0 cancelado
           1 agendado pero no tomado 
           3 agendado y tomado, así lo debe llamar oscar
           4 en agendamiento     
        */

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            switch (e.Row.Cells[1].Text)
            {
                case "W": e.Row.Cells[1].Text = "Página"; break;
                case "A": e.Row.Cells[1].Text = "Asesor"; break;
            }

            switch (e.Row.Cells[2].Text)
            {
                case "0": e.Row.Cells[2].Text = "Cancelado"; break;
                case "1": e.Row.Cells[2].Text = "Abandonado"; break;
                case "3": e.Row.Cells[2].Text = "Tomado"; break;
                case "4": e.Row.Cells[2].Text = "Solicitado"; break;
            }

        }

    }

    protected void gvAgendados_PageIndexChanged(object sender, EventArgs e)
    {
        gvAgendados.PageIndex = IndexPage;
    }

    protected void gvAgendados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SetSortDirection(OrdenarDir, false);
        DataTable SearchDT = ((DataSet)ViewState["DataSet"]).Tables[0];
        if ((ViewState["OrdenarCampo"] != null) && (ViewState["OrdenarDir"] != null))
            SearchDT.DefaultView.Sort = ViewState["OrdenarCampo"].ToString() + " " + ViewState["OrdenarDir"].ToString();

        gvAgendados.DataSource = SearchDT;

        IndexPage = e.NewPageIndex;
        gvAgendados.PageIndex = IndexPage;
        gvAgendados.DataBind();

        //AgregarTituloTabla("ESTADÍSTICA GENERAL DE TURNOS", ref gvAgendados, gvAgendados.Columns.Count);
        if (ViewState["OrdenarCampo"] != null)
        {
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in gvAgendados.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == ViewState["OrdenarCampo"].ToString())
                {
                    columnIndex = gvAgendados.HeaderRow.Cells.GetCellIndex(headerCell);
                    break;
                }
            }
            gvAgendados.HeaderRow.Cells[columnIndex].CssClass = "table-sorted";
            gvAgendados.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
        }

    }

    protected void SetSortDirection(string sortDirection, bool cambiar = true)
    {
        if (cambiar)
        {
            if (sortDirection == "ASC")
            {
                _ordenarDir = "DESC";
                sortImage.ImageUrl = @"images\arrow-down.png";
            }
            else
            {
                _ordenarDir = "ASC";
                sortImage.ImageUrl = @"images\arrow-up.png";
            }
        }
        else
        {
            _ordenarDir = sortDirection;
        }


        // Save new values in ViewState.
        ViewState["OrdenarDir"] = _ordenarDir;
    }

    protected void gvAgendados_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (dsTurnos != null)
        {
            // GUARDA EL CAMPO POR EL QUE SE ORDENA
            if (ViewState["OrdenarCampo"] == null)
            {
                ViewState["OrdenarCampo"] = e.SortExpression;
                SetSortDirection("ASC");
            }
            else
            {
                if (e.SortExpression == ViewState["OrdenarCampo"].ToString())
                    SetSortDirection(OrdenarDir);
                else
                {
                    ViewState["OrdenarCampo"] = e.SortExpression;
                    SetSortDirection("DESC");
                }
            }

            // ORDENAR LOS DATOS
            dsTurnos.Tables[0].DefaultView.Sort = e.SortExpression + " " + _ordenarDir;
            DataTable dt = dsTurnos.Tables[0].DefaultView.ToTable();
            dsTurnos.Tables[0].Rows.Clear();
            foreach (DataRow row in dt.Rows)
                dsTurnos.Tables[0].Rows.Add(row.ItemArray);

            gvAgendados.DataSource = dsTurnos;
            gvAgendados.AllowSorting = true;
            gvAgendados.DataBind();

            OrdenarDir = _ordenarDir;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in gvAgendados.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = gvAgendados.HeaderRow.Cells.GetCellIndex(headerCell);
                    break;
                }
            }
            gvAgendados.HeaderRow.Cells[columnIndex].CssClass = "table-sorted";
            gvAgendados.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            //AgregarTituloTabla("ESTADÍSTICA GENERAL DE TURNOS", ref gvTurnosTotales, gvTurnosTotales.Columns.Count);
            Session["DS"] = gvAgendados;
        }
    }

}
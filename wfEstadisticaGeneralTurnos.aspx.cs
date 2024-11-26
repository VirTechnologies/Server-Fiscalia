using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Globalization;

public partial class wfEstadisticaGeneralTurnos : System.Web.UI.Page
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

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");

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

    public override void VerifyRenderingInServerForm(Control control)
    { /* Do nothing */ }

    public override bool EnableEventValidation
    {
        get { return false; }
        set { /* Do nothing */}
    }

    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        ViewState["OrdenarCampo"] = null;
        ViewState["OrdenarDir"] = null;
        Filtrar();
    }

    private void Filtrar()
    {
        clsblEstadisticas blEstadi = new clsblEstadisticas();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DateTime fecha;
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
        msgError = blEstadi.ConsultaGeneralTurnos(ref dsTurnos, ddlOficinaId.SelectedValue, fechaIni, fechaFin);
        if (msgError == "")
        {
            if ((ViewState["OrdenarCampo"] != null) && (ViewState["OrdenarDir"] != null))
                dsTurnos.Tables[0].DefaultView.Sort = ViewState["OrdenarCampo"].ToString() + " " + ViewState["OrdenarDir"].ToString();

            gvTurnosTotales.DataSource = dsTurnos;
            gvTurnosTotales.DataBind();

            if (dsTurnos.Tables[0].Rows.Count > 0)
            {
                ViewState["DataSet"] = dsTurnos;




                tbNoRegistros.Text = dsTurnos.Tables[0].Rows.Count.ToString();
                lblNoRegistros.Visible = tbNoRegistros.Visible = true;
                lblSinRegistros.Visible = false;

                if (OrdenarDir == "")
                {
                    SetSortDirection("");
                }
                AgregarTituloTabla("ESTADÍSTICA GENERAL DE TURNOS", ref gvTurnosTotales, gvTurnosTotales.Columns.Count);
                btnExportar.Visible = true;

                //if (Session["DS"] == null)
                //    Session["DS"] = gvTurnosTotales;
            }
            else
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
                btnExportar.Visible = false;
            }
            txSQL.Text = "Consultó";
        }
        else
        {
            txSQL.Text = "";
            return;
        }
    }

    protected void gvTurnosTotales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SetSortDirection(OrdenarDir, false);
        DataTable SearchDT = ((DataSet)ViewState["DataSet"]).Tables[0];
        if ((ViewState["OrdenarCampo"] != null) && (ViewState["OrdenarDir"] != null))
            SearchDT.DefaultView.Sort = ViewState["OrdenarCampo"].ToString() + " " + ViewState["OrdenarDir"].ToString();

        gvTurnosTotales.DataSource = SearchDT;

        IndexPage = e.NewPageIndex;
        gvTurnosTotales.PageIndex = IndexPage;
        gvTurnosTotales.DataBind();

        AgregarTituloTabla("ESTADÍSTICA GENERAL DE TURNOS", ref gvTurnosTotales, gvTurnosTotales.Columns.Count);
        if (ViewState["OrdenarCampo"] != null)
        {
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in gvTurnosTotales.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == ViewState["OrdenarCampo"].ToString())
                {
                    columnIndex = gvTurnosTotales.HeaderRow.Cells.GetCellIndex(headerCell);
                    break;
                }
            }
            gvTurnosTotales.HeaderRow.Cells[columnIndex].CssClass = "table-sorted";
            gvTurnosTotales.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
        }
    }

    protected void gvTurnosTotales_PageIndexChanged(object sender, EventArgs e)
    {
        gvTurnosTotales.PageIndex = IndexPage;
        //Filtrar();
    }

    protected void SetSortDirection(string sortDirection, bool cambiar=true)
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

    protected void gvTurnosTotales_Sorting(object sender, GridViewSortEventArgs e)
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

            gvTurnosTotales.DataSource = dsTurnos;
            gvTurnosTotales.AllowSorting = true;
            gvTurnosTotales.DataBind();

            OrdenarDir = _ordenarDir;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in gvTurnosTotales.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = gvTurnosTotales.HeaderRow.Cells.GetCellIndex(headerCell);
                    break;
                }
            }
            gvTurnosTotales.HeaderRow.Cells[columnIndex].CssClass = "table-sorted";
            gvTurnosTotales.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            AgregarTituloTabla("ESTADÍSTICA GENERAL DE TURNOS", ref gvTurnosTotales, gvTurnosTotales.Columns.Count);
            Session["DS"] = gvTurnosTotales;
        }
    }

    protected void gvTurnosTotales_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataSet dsR = new DataSet();
            DateTime fechaTurno;

            try
            {
                TimeSpan t = TimeSpan.FromSeconds(double.Parse(e.Row.Cells[7].Text));
                string tiempoFormateado = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
                e.Row.Cells[7].Text = tiempoFormateado;
            }
            catch { }

            /*
            public enum EstadoTurno
                {
                    Generado = 1, EnSala = 2, Llamado = 3, Abandonado = 4,
                    EnAtencion = 5, Cerrado = 6, Redireccionado = 7, NoLlamado = 8
            }
            */

            switch (e.Row.Cells[3].Text)
            {
                case "1":
                    e.Row.Cells[3].Text = "Generado";
                    e.Row.Cells[6].Text = e.Row.Cells[7].Text = e.Row.Cells[8].Text = e.Row.Cells[9].Text = e.Row.Cells[10].Text = "Pendiente";
                    break;
                case "2":
                    e.Row.Cells[3].Text = "En sala";
                    if (DateTime.TryParse(e.Row.Cells[5].Text, out fechaTurno))
                    {
                        if (fechaTurno.Date == DateTime.Now.Date)
                        {
                            e.Row.Cells[6].Text = e.Row.Cells[7].Text = e.Row.Cells[8].Text = e.Row.Cells[9].Text = e.Row.Cells[10].Text = "Pendiente";
                        }
                        else
                        {
                            e.Row.Cells[6].Text = "Sin gestionar";
                            e.Row.Cells[7].Text = e.Row.Cells[8].Text = e.Row.Cells[9].Text = e.Row.Cells[10].Text = "N/A";
                        }
                    }
                    break;
                case "3":
                    DateTime hora1 = new DateTime();

                    string tiempoatencion1 = "";




                    hora1 = Convert.ToDateTime(e.Row.Cells[5].Text);
                    hora1 = hora1.AddMinutes(4);
                    tiempoatencion1 = "00h:04m:01s";

                    e.Row.Cells[3].Text = "Cerrado";
                    e.Row.Cells[6].Text = hora1.ToString("d/M/yyyy HH:mm");
                    e.Row.Cells[9].Text = tiempoatencion1;
                    break;
                case "4":
                    e.Row.Cells[3].Text = "Abandonado";
                    e.Row.Cells[9].Text = "N/A";
                    break;
                case "5"://// jose

                    DateTime hora = new DateTime();

                    string tiempoatencion = "";




                    hora = Convert.ToDateTime(e.Row.Cells[5].Text);
                    hora = hora.AddMinutes(4);
                    tiempoatencion = "00h:04m:01s";

                            e.Row.Cells[3].Text = "Cerrado";
                            e.Row.Cells[6].Text = hora.ToString("d/M/yyyy HH:mm");
                            e.Row.Cells[9].Text = tiempoatencion;
                    break;
                case "6":
                    e.Row.Cells[3].Text = "Cerrado";
                    try
                    {
                        TimeSpan t = TimeSpan.FromSeconds(double.Parse(e.Row.Cells[9].Text));
                        string tiempoFormateado = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
                        e.Row.Cells[9].Text = tiempoFormateado;
                    }
                    catch { }
                    break;
                case "7":
                    e.Row.Cells[3].Text = "Redireccionado";
                    e.Row.Cells[6].Text = "N/A";
                    break;
                case "8":
                    e.Row.Cells[3].Text = "No llamado";
                    e.Row.Cells[6].Text = e.Row.Cells[7].Text = e.Row.Cells[8].Text = e.Row.Cells[9].Text = "N/A";
                    break;
            }
        }
    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {
        GridView grilla = new GridView();

        DataTable SearchDT = ((DataSet)ViewState["DataSet"]).Tables[0];
        if ((ViewState["OrdenarCampo"] != null) && (ViewState["OrdenarDir"] != null))
            SearchDT.DefaultView.Sort = ViewState["OrdenarCampo"].ToString() + " " + ViewState["OrdenarDir"].ToString();

        SearchDT.Columns[0].ColumnName = "Oficina";
        SearchDT.Columns[1].ColumnName = "Tipo de Atención";
        SearchDT.Columns[2].ColumnName = "Turno";
        SearchDT.Columns[3].ColumnName = "Estado";
        SearchDT.Columns[4].ColumnName = "Servicio solicitado";
        SearchDT.Columns[5].ColumnName = "Fecha Inicio";
        SearchDT.Columns[6].ColumnName = "Fecha Fin";
        SearchDT.Columns[7].ColumnName = "Tiempo de espera";
        SearchDT.Columns[8].ColumnName = "Fecha Llamado";
        SearchDT.Columns[9].ColumnName = "Tiempo de Atención";
        SearchDT.Columns[10].ColumnName = "Causa Abandono";
        SearchDT.Columns[11].ColumnName = "Asesor que atendió";
        SearchDT.Columns[12].ColumnName = "Documento cliente";

        grilla.DataSource = SearchDT;
        grilla.AllowPaging = false;
        grilla.AllowSorting = false;
        grilla.RowDataBound += gvTurnosTotales_RowDataBound;
        grilla.DataBind();

        AgregarTituloTabla("ESTADÍSTICA GENERAL DE TURNOS", ref grilla, SearchDT.Columns.Count);
        grilla.RowDataBound -= gvTurnosTotales_RowDataBound;

        //Session["DS"] = grilla;

        string momento = DateTime.Now.ToString("dd-MM-yyyy");
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        Page pagina = new Page();
        HtmlForm form = new HtmlForm();
        grilla.AllowPaging = false;
        grilla.EnableViewState = true;
        pagina.EnableEventValidation = false;
        pagina.DesignerInitialize();
        form.Controls.Add(grilla);
        pagina.Controls.Add(form);
        pagina.RenderControl(htw);
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=Estadistica General Turnos " + momento + ".xls");
        Response.Charset = "UTF-8";

        Response.ContentEncoding = Encoding.Default;
        Response.Write(sb.ToString());
        Response.End();

    }

    private void CrearTablaExportar()
    {
        if ((ViewState["OrdenarCampo"] != null) && (ViewState["OrdenarDir"] != null))
            dsTurnos.Tables[0].DefaultView.Sort = ViewState["OrdenarCampo"].ToString() + " " + ViewState["OrdenarDir"].ToString();
        DataView view = new DataView(dsTurnos.Tables[0].DefaultView.ToTable());
        DataTable dtTemp = view.ToTable("Selected", false, "Oficina", "TipoAtencion", "Turno", "Estado", "Servicio", "FechaInicio", "FechaFin", "TiempoEspera", "FechaLlamado", "TiempoAtencion", "Asesor", "Documento");
        dtTemp.Columns[0].ColumnName = "Oficina";
        dtTemp.Columns[1].ColumnName = "Tipo de Atención";
        dtTemp.Columns[2].ColumnName = "Turno";
        dtTemp.Columns[3].ColumnName = "Estado";
        dtTemp.Columns[4].ColumnName = "Servicio solicitado";
        dtTemp.Columns[5].ColumnName = "Fecha Inicio";
        dtTemp.Columns[6].ColumnName = "Fecha Fin";
        dtTemp.Columns[7].ColumnName = "Tiempo de espera";
        dtTemp.Columns[8].ColumnName = "Fecha Llamado";
        dtTemp.Columns[9].ColumnName = "Tiempo de Atención";
        dtTemp.Columns[10].ColumnName = "Asesor que atendió";
        dtTemp.Columns[11].ColumnName = "Documento cliente";

        gvExportar.DataSource = dtTemp;
        gvExportar.DataBind();
        //AgregarTituloTabla("ESTADÍSTICA GENERAL DE TURNOS", ref gvExportar, dtTemp.Columns.Count);
        //Session["DS"] = gvExportar;
        AgregarTituloTabla("ESTADÍSTICA GENERAL DE TURNOS", ref gvTurnosTotales, gvTurnosTotales.Columns.Count);
        //Session["DS"] = gvTurnosTotales;
    }

    private void AgregarTituloTabla(string titulo, ref GridView gv, int colspan)
    {
        GridViewRow row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
        TableCell th = new TableHeaderCell();

        th.HorizontalAlign = HorizontalAlign.Center;
        th.ColumnSpan = colspan;
        th.CssClass = "text-center table-primary h3";
        th.Style.Add("font-size", "20px");
        th.Text = titulo;
        row.Cells.Add(th);
        ((Table)gv.Controls[0]).Rows.AddAt(0, row);
    }

    protected void gvTurnosTotales_Sorted(object sender, EventArgs e)
    {
        Session["DS"] = gvTurnosTotales;
    }


}
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

public partial class wfEncuestaRespuestas : System.Web.UI.Page
{
    DataSet dsRespuestas = new DataSet();
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
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        clsblParametricas blPara = new clsblParametricas();

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");

        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            lblNombreEncuesta.Text = Session["tbnombre_encuesta"].ToString();
        }
        //if (txSQL.Text != "")
        {
            Filtrar();
        }
        //else
        //{
        //    gvRespuestas.Visible = false;
        //}

        if (blU.ValorObjetoString(Session["Volver"]) == "S")
        {
            lblNombreEncuesta.Text = Session["tbnombre_encuesta"].ToString();
            if (Session["txSQL"].ToString() != "")
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["tbnombre_encuesta"] = "";
        }
    }

    private void Filtrar()
    {
        clsblParametricas blParam = new clsblParametricas();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        String strSQL = "";
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

        msgError = blParam.ConsultaRespuestasEncuesta(ref dsRespuestas, hfid.Value, fechaIni, fechaFin);
        lblNombreEncuesta.Text = Session["tbnombre_encuesta"].ToString();
        if (msgError == "")
        {
            gvRespuestas.DataSource = dsRespuestas;
            gvRespuestas.DataBind();
            if (gvRespuestas.Rows.Count <= 0)
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
                btnExportar.Visible = false;
                gvRespuestas.Visible = false;
            }
            else
            {
                if (OrdenarDir == "")
                {
                    gvRespuestas.HeaderRow.Cells[0].CssClass = "btn-secondary";
                    SetSortDirection("");
                    gvRespuestas.HeaderRow.Cells[0].Controls.Add(sortImage);
                }

                tbNoRegistros.Text = gvRespuestas.Rows.Count.ToString();
                lblNoRegistros.Visible = tbNoRegistros.Visible = true;
                lblSinRegistros.Visible = false;

                AgregarTituloTabla(lblNombreEncuesta.Text, ref gvRespuestas, gvRespuestas.Columns.Count);

                if (Session["DS"] == null)
                {
                    CrearTablaExportar();
                }
                gvRespuestas.Visible = true;
                btnExportar.Visible = true;
            }
            txSQL.Text = "Consultó";
        }
        else
        {
            txSQL.Text = "";
            return;
        }
    }

    private void CrearTablaExportar()
    {
        DataView view = new DataView(dsRespuestas.Tables[0].DefaultView.ToTable());
        DataTable dtTemp = view.ToTable("Selected", false, "oficina", "pregunta", "opcion", "total");
        dtTemp.Columns[0].ColumnName = "Oficina";
        dtTemp.Columns[1].ColumnName = "Pregunta";
        dtTemp.Columns[2].ColumnName = "Respuesta";
        dtTemp.Columns[3].ColumnName = "Ocurrencia";

        gvExportar.DataSource = dtTemp;
        gvExportar.DataBind();
        AgregarTituloTabla(lblNombreEncuesta.Text, ref gvExportar, dtTemp.Columns.Count);
        Session["DS"] = gvExportar;
    }

    protected void gvRespuestas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //CrearTablaExportar();
        //clsblUsuarios obj = new clsblUsuarios();
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    String idS = "";
        //    DataSet dsR = new DataSet();

        //}
        //else
        //{
        //    if (e.Row.RowType == DataControlRowType.Header)
        //        e.Row.Cells[0].Visible = false;
        //}
        //if (!obj.PermisoModulo("61", Session["IDUSUARIO"].ToString(), "U"))
        //{
        //    e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        //}
    }

    protected void gvRespuestas_Sorting(object sender, GridViewSortEventArgs e)
    {
        SetSortDirection(OrdenarDir);
        if (dsRespuestas != null)
        {
            //Sort the data.
            dsRespuestas.Tables[0].DefaultView.Sort = e.SortExpression + " " + _ordenarDir;
            DataTable dt = dsRespuestas.Tables[0].DefaultView.ToTable();
            dsRespuestas.Tables[0].Rows.Clear();
            foreach (DataRow row in dt.Rows)
                dsRespuestas.Tables[0].Rows.Add(row.ItemArray);

            gvRespuestas.DataSource = dsRespuestas;
            gvRespuestas.AllowSorting = true;
            gvRespuestas.DataBind();
            OrdenarDir = _ordenarDir;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in gvRespuestas.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = gvRespuestas.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }
            gvRespuestas.HeaderRow.Cells[columnIndex].CssClass = "btn-secondary";
            gvRespuestas.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            AgregarTituloTabla(lblNombreEncuesta.Text, ref gvRespuestas, gvRespuestas.Columns.Count);

            CrearTablaExportar();
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["DS"] = null;
        Session["Volver"] = "S";
        Session["txSQL"] = txSQL.Text;
        Session["tbnombre_encuesta"] = "";
        Response.Redirect("wfEncuestas.aspx");
    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {
        ExportarExcel();
    }

    private void ExportarExcel()
    {
        if (Session["DS"] != null)
        {
            string momento = DateTime.Now.ToString("ddMMyyyy-HHmmss");
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            Page pagina = new Page();
            HtmlForm form = new HtmlForm();
            GridView grilla = new GridView();
            grilla = (GridView)Session["DS"];
            grilla.EnableViewState = true;
            pagina.EnableEventValidation = false;
            pagina.DesignerInitialize();
            form.Controls.Add(grilla);
            pagina.Controls.Add(form);
            pagina.RenderControl(htw);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", $"attachment;filename=EncuestaRespuestas{momento}.xls");
            Response.Charset = "UTF-8";

            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }
    }

    private void AgregarTituloTabla(string titulo, ref GridView gv, int colspan)
    {
        GridViewRow row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
        TableCell th = new TableHeaderCell();

        th.HorizontalAlign = HorizontalAlign.Center;
        th.ColumnSpan = colspan;
        th.CssClass = "text-center table-primary h3";
        th.Text = titulo;
        row.Cells.Add(th);
        ((Table)gv.Controls[0]).Rows.AddAt(0, row);
    }


    protected void SetSortDirection(string sortDirection)
    {
        if (sortDirection == "DESC")
        {
            _ordenarDir = "ASC";
            sortImage.ImageUrl = "images\\arrow-down.png";
        }
        else
        {
            _ordenarDir = "DESC";
            sortImage.ImageUrl = "images\\arrow-up.png";
        }
    }

    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        ViewState["OrdenarCampo"] = null;
        ViewState["OrdenarDir"] = null;
        Filtrar();
    }


}
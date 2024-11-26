using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;

public partial class wfEstadisticasActividadAsesor : System.Web.UI.Page
{
    int IndexPage = 0;
    DataSet dsActivity = new DataSet();
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
            blU.LlenaDDLObligatorio(ddlOficinaId, "Oficinas", "Id", "Nombre", "", "", "Nombre");
            blU.LlenaDDLObligatorio(ddlRazonId, "ReasonsForSuspension", "Id", "Description", "", "", "Id");
        }
        //if (txSQL.Text != "")
            Filtrar();
        Session["txSQL"] = "";
    }


    private void Filtrar()
    {
        clsblEstadisticas blEstadi = new clsblEstadisticas();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        int i = 0;
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
            TablaInfoTurnos.Visible = false;
            lbConfirmacion.Text = "El formato de la fecha inicial es inválido!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
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
            TablaInfoTurnos.Visible = false;
            lbConfirmacion.Text = "El formato de la fecha final es inválido!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
            return;
        }
        msgError = blEstadi.ConsultaActividadUsuarios(ref dsActivity, ddlOficinaId.SelectedValue, "", ddlRazonId.SelectedValue, fechaIni, fechaFin);
        if (msgError == "")
        {
            gvActivity.DataSource = dsActivity;
            gvActivity.DataBind();

            if (dsActivity.Tables[0].Rows.Count > 0)
            {
                if (OrdenarDir == "")
                {
                    gvActivity.HeaderRow.Cells[0].CssClass = "btn-secondary";
                    SetSortDirection("");
                    gvActivity.HeaderRow.Cells[0].Controls.Add(sortImage);
                }
                tbNoRegistros.Text = dsActivity.Tables[0].Rows.Count.ToString();
                lblNoRegistros.Visible = tbNoRegistros.Visible = true;
                lblSinRegistros.Visible = false;
                AgregarTituloTabla("REGISTRO DE ACTIVIDAD ASESORES", ref gvActivity, gvActivity.Columns.Count);
                TablaInfoTurnos.Visible = true;
            }
            else
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
            }
            txSQL.Text = "Consulto";

            //for (i = 0; i < dsActivity.Tables[0].Rows.Count; i++)
            //{
            //    String Mean = dsActivity.Tables[0].Rows[i]["NoTurnos"].ToString();
            //    Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();

            //    JSonData += "{'groupname': '" + dsActivity.Tables[0].Rows[i]["Abreviatura"].ToString() + "', 'count': " + dsActivity.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
            //    if (i < dsActivity.Tables[0].Rows.Count - 1)
            //        JSonData += ",";
            //}
            //JSonData += "];";
            //script += " \n var data=" + JSonData + "\n countChart(data, 'chartdiv', 'Número de turnos solicitados por servicio'); \n";
        };
    }

    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        Filtrar();
    }

    protected void gvActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvActivity.PageIndex = IndexPage = e.NewPageIndex;
    }

    protected void gvActivity_PageIndexChanged(object sender, EventArgs e)
    {
        gvActivity.PageIndex = IndexPage;
        Filtrar();
    }


    protected void gvActivity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }


    protected void gvActivity_Sorting(object sender, GridViewSortEventArgs e)
    {
        SetSortDirection(OrdenarDir);
        if (dsActivity != null)
        {
            //Sort the data.
            dsActivity.Tables[0].DefaultView.Sort = e.SortExpression + " " + _ordenarDir;
            DataTable dt = dsActivity.Tables[0].DefaultView.ToTable();
            dsActivity.Tables[0].Rows.Clear();
            foreach (DataRow row in dt.Rows)
                dsActivity.Tables[0].Rows.Add(row.ItemArray);

            gvActivity.DataSource = dsActivity;
            gvActivity.AllowSorting = true;
            gvActivity.DataBind();
            OrdenarDir = _ordenarDir;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in gvActivity.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = gvActivity.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }
            gvActivity.HeaderRow.Cells[columnIndex].CssClass = "btn-secondary";
            gvActivity.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            AgregarTituloTabla("REGISTRO DE ACTIVIDAD ASESORES", ref gvActivity, gvActivity.Columns.Count);

            //CrearTablaExportar();
        }

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


}
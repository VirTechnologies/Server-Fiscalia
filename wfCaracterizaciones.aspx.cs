using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class wfUsuariosCajas : System.Web.UI.Page
{
    DataSet dsCaracterizaciones = new DataSet();
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

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        if (!(objUsuario.PermisoModulo("58", blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
            btnAgregar.Visible = false;

        //if (!Page.IsPostBack)
        //{
        //    Filtrar();
        //}

        if (blU.ValorObjetoString(Session["Volver"]) == "S")
        {
            if (Session["txSQL"].ToString() != "")
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["Nombre"] = "";
        }

        Filtrar();
    }

    private void Filtrar()
    {
        clsblParametricas blParam = new clsblParametricas();
        String msgError;
        String strSQL = "";

        msgError = blParam.ConsultaCaracterizaciones(ref dsCaracterizaciones, false);
        if (msgError == "")
        {
            gvCaracterizaciones.DataSource = dsCaracterizaciones;
            gvCaracterizaciones.DataBind();
            if (gvCaracterizaciones.Rows.Count <= 0)
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
            }
            else
            {
                tbNoRegistros.Text = gvCaracterizaciones.Rows.Count.ToString();
                lblNoRegistros.Visible = tbNoRegistros.Visible = true;
                lblSinRegistros.Visible = false;
            }
            txSQL.Text = "Consultó";
        }
        else
        {
            txSQL.Text = "";
            return;
        }
    }

    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        Filtrar();
    }

    protected void gvCaracterizaciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String idS = "";
            DataSet dsR = new DataSet();

            idS = e.Row.Cells[0].Text;

            LinkButton AdministrarButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 1].Controls[0];
            LinkButton queryButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 2].Controls[0];

            AdministrarButton.CommandName = "ADMINISTRAR";
            AdministrarButton.CommandArgument = idS;
            queryButton.CommandName = "CONSULTAR";
            queryButton.CommandArgument = idS;

            if (!obj.PermisoModulo("58", Session["IDUSUARIO"].ToString(), "U"))
            {
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
                e.Row.Cells[e.Row.Cells.Count - 2].Visible = false;
            }

            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("58", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            e.Row.Cells[e.Row.Cells.Count - 2].Visible = false;
        }
    }

    protected void gvCaracterizaciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idS = "";

        if (e.CommandName != "")
        {
            idS = e.CommandArgument.ToString();
            Session["Nombre"] = tbNombre.Text;
            Session["txSQL"] = txSQL.Text;
        }
        if (e.CommandName.ToString() == "ADMINISTRAR")
            Response.Redirect("wfCaracterizacion.aspx?id=" + idS);
        if (e.CommandName.ToString() == "CONSULTAR")
            Response.Redirect("wfCaracterizacion.aspx?consulta=si&id=" + idS);
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        Session["txSQL"] = txSQL.Text;
        Response.Redirect("wfCaracterizacion.aspx");
    }
}
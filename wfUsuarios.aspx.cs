using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Threading;

public partial class wfUsuarios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        //=============== ESTAS 4 LINEAS SON PARA LAS FECHAS ===============================
        DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
        myDTF.ShortDatePattern = "dd/MM/yyyy";
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");

        clsblUtiles blU = new clsblUtiles();

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");

        if (!((objUsuario.PermisoModulo("17", Session["IDUSUARIO"].ToString(), "I"))|| ((objUsuario.PermisoModulo("17", Session["IDUSUARIO"].ToString(), "U")))))
            btnAgregar.Visible = false;
        //else
        //    btnAgregar.Attributes.Add("onclick", "window.open('Modales.aspx?pagina=wfUsuario',800,600); ");
        if (!Page.IsPostBack)
        {
            blU.LlenaDDLObligatorio(ddl_perfil, "PERFILES", "id_perfil", "perfil","","","");
        }

        //if (txSQL.Text != "")
           Filtrar();

        if (blU.ValorObjetoString(Session["Volver"]) == "S")
        {
            Nombre.Text = Session["tbnombre"].ToString();
            if (Session["txSQL"].ToString() != "")
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["tbnombre"] = "";
        }

    }

    private void Filtrar()
    {
        DataSet dsUsuarios = new DataSet();
        clsblUsuarios blUsuarios = new clsblUsuarios();
        String msgError;
        String strSQL = "";

        msgError = blUsuarios.FiltraUsuarios(ref dsUsuarios, Nombre.Text, tblogin.Text, ddl_perfil.SelectedValue);
        if (msgError == "")
        {
            gvUsuarios.DataSource = dsUsuarios;
            gvUsuarios.DataBind();
            //lblNoRegistros.Text = gvUsuarios.Rows.Count.ToString();
            if (gvUsuarios.Rows.Count <= 0)
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
            }
            else
            {
                tbNoRegistros.Text = gvUsuarios.Rows.Count.ToString();
                lblNoRegistros.Visible = tbNoRegistros.Visible = true;
                lblSinRegistros.Visible = false;
            }
            txSQL.Text = strSQL;
        }
        else
        {
            txSQL.Text = "";
            return;
        }
    }

    protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String idS="";
            DataSet dsR = new DataSet();
            LinkButton AdministrarButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 2].Controls[0];
            LinkButton EliminarButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 1].Controls[0];
            LinkButton queryButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 3].Controls[0];

            idS = e.Row.Cells[0].Text;
            //EliminarButton.Attributes.Add("onclick", "openModalWindow('Modales.aspx?pagina=wfUsuario&elimina=si&id_usuario=" + idS + "',800,600); ");
            //AdministrarButton.Attributes.Add("onclick", "openModalWindow('Modales.aspx?pagina=wfUsuario&id_usuario=" + idS + "',800,600); ");
            //queryButton.Attributes.Add("onclick", "openModalWindow('Modales.aspx?pagina=wfUsuario&consulta=si&id_usuario=" + idS + "',800,600); ");

            AdministrarButton.CommandName = "ADMINISTRAR";
            AdministrarButton.CommandArgument = idS;
            queryButton.CommandName = "CONSULTAR";
            queryButton.CommandArgument = idS;
            EliminarButton.CommandName = "ELIMINAR";
            EliminarButton.CommandArgument = idS;

            if (!obj.PermisoModulo("17", Session["IDUSUARIO"].ToString(), "U"))
                e.Row.Cells[e.Row.Cells.Count - 3].Visible = false;
            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("17", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 2].Visible = false;
            e.Row.Cells[e.Row.Cells.Count - 3].Visible = false;
        }
    }

    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        Filtrar();
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        Session["tbnombre"] = Nombre.Text;
        Session["txSQL"] = txSQL.Text;
        Response.Redirect("wfUsuario.aspx");
    }



    protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string idS = "";
        if (e.CommandName != "")
        {
            idS = e.CommandArgument.ToString();
            Session["tbnombre"] = Nombre.Text;
            Session["txSQL"] = txSQL.Text;
        }
        if (e.CommandName.ToString() == "ADMINISTRAR")
            Response.Redirect("wfUsuario.aspx?id_usuario=" + idS);
        if (e.CommandName.ToString() == "CONSULTAR")
            Response.Redirect("wfUsuario.aspx?consulta=si&id_usuario=" + idS);
        if (e.CommandName.ToString() == "ELIMINAR")
            Response.Redirect("wfUsuario.aspx?elimina=si&id_usuario=" + idS);

        //EliminarButton.Attributes.Add("onclick", "openModalWindow('Modales.aspx?pagina=wfUsuario&elimina=si&id_usuario=" + idS + "',800,600); ");
        //AdministrarButton.Attributes.Add("onclick", "openModalWindow('Modales.aspx?pagina=wfUsuario&id_usuario=" + idS + "',800,600); ");
        //queryButton.Attributes.Add("onclick", "openModalWindow('Modales.aspx?pagina=wfUsuario&consulta=si&id_usuario=" + idS + "',800,600); ");

    }
}

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
using System.Net;
using System.Net.Mail;

public partial class wfPerfiles : System.Web.UI.Page
{
    int Altor = 250;
    int Anchor = 450;

    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario =new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        if (blU.ValorObjetoString(Session["IDUSUARIO"])=="") 
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        if (!(objUsuario.PermisoModulo("3", blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
            btnAgregar.Visible = false;
        //if (txSQL.Text != "") 
            Filtrar();
        if (blU.ValorObjetoString(Session["Volver"])== "S")
        {
            tbPerfil.Text = Session["tbPerfil"].ToString();
            if (Session["txSQL"].ToString() != "")
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["tbPerfil"] = "";
        }
    }

    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        Filtrar();
    }

    private void Filtrar()
    {
        DataSet dsPerfiles = new DataSet();
        clsblUsuarios blUsuarios = new clsblUsuarios();
        String msgError;
        String strSQL = "";

        msgError = blUsuarios.ConsultaPerfiles(ref dsPerfiles, tbPerfil.Text);
        if (msgError == "")
        {
            gvPerfiles.DataSource = dsPerfiles;
            gvPerfiles.DataBind();

            if (gvPerfiles.Rows.Count <= 0)
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
            }
            else
            {
                tbNoRegistros.Text = gvPerfiles.Rows.Count.ToString();
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

    protected void gvPerfiles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String idS="",id_perfil="";
            DataSet dsR = new DataSet();
            LinkButton ModulosButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 1].Controls[0];

            obj.ConsultaUsuarios(ref dsR, Session["IDUSUARIO"].ToString());
            id_perfil = dsR.Tables[0].Rows[0]["id_perfil"].ToString();
            idS = e.Row.Cells[0].Text;
            if (id_perfil == "1")
            {
                ModulosButton.CommandName = "MODULOS";
                ModulosButton.CommandArgument = idS;
            }
            else
            {
                ModulosButton.CommandName = "MODULOSC";
                ModulosButton.CommandArgument = idS;
            }

            LinkButton AdministrarButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 3].Controls[0];
            LinkButton EliminarButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 2].Controls[0];
            LinkButton queryButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 4].Controls[0];

            AdministrarButton.CommandName = "ADMINISTRAR";
            AdministrarButton.CommandArgument = idS;
            queryButton.CommandName = "CONSULTAR";
            queryButton.CommandArgument = idS;
            EliminarButton.CommandName = "ELIMINAR";
            EliminarButton.CommandArgument = idS;
            if (!obj.PermisoModulo("3", Session["IDUSUARIO"].ToString(), "U"))
                e.Row.Cells[e.Row.Cells.Count - 3].Visible = false;
            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("3", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 2].Visible = false;
            e.Row.Cells[e.Row.Cells.Count - 3].Visible = false;
        }
    }

    protected void gvPerfiles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idS = "";

        if (e.CommandName != "")
        {
            idS = e.CommandArgument.ToString();
            Session["tbPerfil"] = tbPerfil.Text;
            Session["txSQL"] = txSQL.Text;
        }
        if (e.CommandName.ToString() == "MODULOS")
            Response.Redirect("wfPerfiles_Modulos.aspx?id_perfil=" + idS);
        if (e.CommandName.ToString() == "MODULOSC")
            Response.Redirect("wfPerfiles_Modulos.aspx?consulta=si&id_perfil=" + idS);
        if (e.CommandName.ToString() == "ADMINISTRAR")
            Response.Redirect("wfPerfil.aspx?id_perfil=" + idS);
        if (e.CommandName.ToString() == "CONSULTAR")
            Response.Redirect("wfPerfil.aspx?consulta=si&id_perfil=" + idS);
        if (e.CommandName.ToString() == "ELIMINAR")
            Response.Redirect("wfPerfil.aspx?elimina=si&id_perfil=" + idS);
    }


    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        Session["tbPerfil"] = tbPerfil.Text;
        Session["txSQL"] = txSQL.Text;
        Response.Redirect("wfPerfil.aspx");
    }

}

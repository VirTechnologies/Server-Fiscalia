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
using System.Data.OleDb;


public partial class wfModulos : System.Web.UI.Page
{
    int ancho= 550;
    int alto = 450;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario =new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        if (Session["IDUSUARIO"].ToString()=="") 
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        if (!(objUsuario.PermisoModulo("43", Session["IDUSUARIO"].ToString(), "I")))
            bAgregar.Visible = false;
        //if (txSQL.Text != "") 
            Filtrar();
        if (blU.ValorObjetoString(Session["Volver"]) == "S")
        {
            tbnombre_modulo.Text = Session["tbModulo"].ToString();
            if (Session["txSQL"].ToString() != "")
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["tbPerfil"] = "";
        }
    }

    protected void bConsultar_Click(object sender, EventArgs e)
    {
        Filtrar();
    }

    private void Filtrar()
    {
        DataSet dsModulos = new DataSet();
        clsblUsuarios blUsuarios  = new clsblUsuarios();
        String msgError;
        String strSQL="";

        msgError = blUsuarios.FiltraModulos(ref dsModulos, strSQL, tbnombre_modulo.Text);
        if (msgError !="")
        {
            txSQL.Text = "";
            return;
        }
        gvModulos.DataSource = dsModulos;
        gvModulos.DataBind();
        if (gvModulos.Rows.Count <= 0)
        {
            lblNoRegistros.Visible = tbNoRegistros.Visible = false;
            lblSinRegistros.Visible = true;
        }
        else
        {
            tbNoRegistros.Text = gvModulos.Rows.Count.ToString();
            lblNoRegistros.Visible = tbNoRegistros.Visible = true;
            lblSinRegistros.Visible = false;
        }

        txSQL.Text = strSQL;
    }

    protected void gvModulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvModulos.PageIndex = e.NewPageIndex;
    }

    protected void gvModulos_PageIndexChanged(object sender, EventArgs e)
    {
        Filtrar();
    }


    protected void gvModulos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String idS;
            LinkButton AdministrarButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 1].Controls[0];
            LinkButton queryButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 2].Controls[0];

            idS = e.Row.Cells[0].Text;

            AdministrarButton.CommandName = "ADMINISTRAR";
            AdministrarButton.CommandArgument = idS;
            queryButton.CommandName = "CONSULTAR";
            queryButton.CommandArgument = idS;

            //AdministrarButton.Attributes.Add("onclick", "window.open('Modales.aspx?pagina=wfModulo&id_modulo=" + idS + "'," + ancho.ToString() + "," + alto.ToString() + "); ");
            //queryButton.Attributes.Add("onclick", "window.open('Modales.aspx?pagina=wfModulo&consulta=si&id_modulo=" + idS + "'," + ancho.ToString() + "," + alto.ToString() + "); ");
            if (!obj.PermisoModulo("43", Session["IDUSUARIO"].ToString(), "U"))
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
              e.Row.Cells[0].Visible = false;
        }
    }

    protected void bAgregar_Click(object sender, EventArgs e)
    {
        Response.Redirect("wfModulo.aspx", false);
    }

    protected void gvModulos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idS = "";

        if (e.CommandName != "")
        {
            idS = e.CommandArgument.ToString();
            Session["tbModulo"] = tbnombre_modulo.Text;
            Session["txSQL"] = txSQL.Text;
        }
        if (e.CommandName.ToString() == "ADMINISTRAR")
            Response.Redirect("wfModulo.aspx?id_modulo=" + idS);
        if (e.CommandName.ToString() == "CONSULTAR")
            Response.Redirect("wfModulo.aspx?consulta=si&id_modulo=" + idS);

    }
}
;
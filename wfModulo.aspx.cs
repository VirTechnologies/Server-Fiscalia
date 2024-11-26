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


public partial class wfModulo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DataSet dsModulo= new DataSet();

        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
        if (!((objUsuario.PermisoModulo("43", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("1", Session["IDUSUARIO"].ToString(), "U"))))
        {
            bGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (!objUsuario.PermisoModulo("43", Session["IDUSUARIO"].ToString(), "D") || (Request.QueryString["consulta"] == "si"))
            bEliminar.Visible = false;
        if (Session["IDUSUARIO"].ToString()=="") 
            Response.Redirect("wfSesionTimeOut.aspx?modal=1");
        if ((Request.QueryString["consulta"] != "si") && (hfConsulta.Value != "si"))
            bCancelar.Attributes.Add("onclick", "ClickCerrar();");
        else
            bCancelar.Attributes.Add("onclick", "window.close();");
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = Request.QueryString["consulta"];
            hfid_modulo.Value = Request.QueryString["id_modulo"];

            string query = "SELECT id_modulo, modulo + ' - ' + descripcion as modulo FROM Modulos ORDER BY id_modulo_padre";
            blU.LlenaDDLObligatorio(ddlid_modulo_padre, "MODULOS", "id_modulo", "modulo","",query, "");
            if (hfid_modulo.Value != "" && hfid_modulo.Value != "")
            {
                msgError = objUsuario.ConsultaModulos(ref dsModulo, hfid_modulo.Value);
                if (msgError == "")
                {
                    ddlid_modulo_padre.SelectedValue = dsModulo.Tables[0].Rows[0]["id_modulo_padre"].ToString();
                    tbmodulo.Text = dsModulo.Tables[0].Rows[0]["modulo"].ToString();
                    tbdescripcion.Text = dsModulo.Tables[0].Rows[0]["descripcion"].ToString();
                    rbVisible.SelectedValue = "0";
                    if (dsModulo.Tables[0].Rows[0]["visible"].ToString() != "")
                    {
                        if (dsModulo.Tables[0].Rows[0]["id_modulo_padre"].ToString()!="0")
                            rbVisible.SelectedValue = "1";
                    }
                    if (dsModulo.Tables[0].Rows[0]["id_modulo_padre"].ToString() != "")
                    {
                        if (dsModulo.Tables[0].Rows[0]["id_modulo_padre"].ToString() != "0")
                            ddlid_modulo_padre.SelectedValue = dsModulo.Tables[0].Rows[0]["id_modulo_padre"].ToString();
                    }
                    tbpagina.Text = dsModulo.Tables[0].Rows[0]["pagina"].ToString();
                    tborden.Text = dsModulo.Tables[0].Rows[0]["orden"].ToString();
                }
            }
            else
                bEliminar.Visible = false;
        }
        if (hfConsulta.Value == "si")
        {
            bGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
    }


    protected void bGrabar_Click(object sender, EventArgs e)
    {
        NSSSqlUtil blObj =new NSSSqlUtil();
        String msgError="";
        DataSet dsInterno = null;
        String strAux="";

        blObj.LlavePrimaria = "id_modulo";
        blObj.NombreTabla = "MODULOS";

        blObj.Add("descripcion", tbdescripcion.Text);
        blObj.Add("modulo", tbmodulo.Text);
        blObj.Add("visible", rbVisible.SelectedValue);
        if (ddlid_modulo_padre.SelectedItem.ToString() != "") 
            blObj.Add("id_modulo_padre", ddlid_modulo_padre.SelectedValue);
        else
            blObj.Add("id_modulo_padre", "0");
        blObj.Add("pagina", tbpagina.Text);
        blObj.Add("orden", tborden.Text);
        try
        {
            if (hfid_modulo.Value == "")
            {
                msgError = blObj.NssEjecutarSQL("INSERT", ref dsInterno, ref strAux, "", "", null, null);
                hfid_modulo.Value = strAux;
            }
            else
            {
                blObj.Add("id_modulo", hfid_modulo.Value);
                msgError = blObj.NssEjecutarSQL("UPDATE", ref dsInterno, ref strAux, "", "", null, null);
            }
            lbConfirmacion.Text = "¡Registro grabado correctamente!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
            notificacion.Visible = true;
        }
        catch (Exception ex)
        {
            lbConfirmacion.Text = "¡Error al grabar! " + ex.ToString();
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        };
    }

    protected void bEliminar_Click(object sender, EventArgs e)
    {
        NSSSqlUtil blObj =new NSSSqlUtil();
        String msgError="";
        DataSet dsInterno = null;
        String strAux = "";

        blObj.LlavePrimaria = "id_modulo";
        blObj.NombreTabla = "MODULOS";
        blObj.Add(blObj.LlavePrimaria, hfid_modulo.Value);
        msgError = blObj.NssEjecutarSQL("DELETE", ref dsInterno, ref strAux, "", "", null, null);
        if (msgError == "")
        {
            lbConfirmacion.Text = "Registro borrado correctamente!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
            notificacion.Visible = true;

            bGrabar.Visible = false;
            bEliminar.Visible = false;
        }
        else
        {
            lbConfirmacion.Text = "Este registro está referenciado y no puede ser borrado!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void bCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("wfModulos.aspx", false);
    }
}

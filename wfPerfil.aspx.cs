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

public partial class wfPerfil : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DataSet dsPerfiles= new DataSet();

        //=============== ESTAS 4 LINEAS SON PARA LAS FECHAS ===============================
        DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
        myDTF.ShortDatePattern = "dd/MM/yyyy";
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
        if (blU.ValorObjetoString(Session["IDUSUARIO"])=="") 
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");

        HtmlGenericControl ctl = new HtmlGenericControl();
        ctl = (HtmlGenericControl)Page.FindControl("body");
        if (ctl!=null) 
            ctl.Attributes.Add("onbeforeunload", "DeseaCerrar();");

        // Configura los botones de acuerdo a los permisos
        if (! ((objUsuario.PermisoModulo("3", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("3", Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (objUsuario.PermisoModulo("3", Session["IDUSUARIO"].ToString(), "D")) 
            btnEliminar.Visible = false;
        if (! Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            id_perfil.Text = blU.ValorObjetoString(Request.QueryString["id_perfil"]);
            hfElimina.Value = blU.ValorObjetoString(Request.QueryString["elimina"]);
            if (id_perfil.Text == "1")
                btnEliminar.Visible = false;
            if (id_perfil.Text != "")
            {
                msgError = objUsuario.ConsultaPerfil(ref dsPerfiles, id_perfil.Text);
                if (msgError == "")
                {
                    id_perfil.Text = dsPerfiles.Tables[0].Rows[0]["id_perfil"].ToString();
                    perfil.Text = dsPerfiles.Tables[0].Rows[0]["perfil"].ToString();
                    descripcion.Text = dsPerfiles.Tables[0].Rows[0]["descripcion"].ToString();
                }
            }
            else
                btnEliminar.Visible = false;
        }
        if (hfConsulta.Value == "si")
        {
            btnGrabar.Visible = false;
            btnEliminar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (hfElimina.Value == "si")
        {
            btnGrabar.Visible = false;
            btnEliminar.Visible = true;
            objUsuario.SoloLectura(this);
        }
    }
    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        NSSSqlUtil blObj =new NSSSqlUtil();
        String msgError="";
        DataSet dsInterno = null;
        String strAux="";


        blObj.LlavePrimaria = "id_perfil";
        blObj.NombreTabla = "PERFILES";
        blObj.Add("perfil", perfil.Text);
        blObj.Add("descripcion", descripcion.Text);
        if ((id_perfil.Text == "" && blObj.Duplicado("perfil", perfil.Text)))
        {
            lbConfirmacion.Text = "Error! Ya existe el perfil " + perfil.Text;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-warning";
            notificacion.Visible = true;
        }
        else
        {
            if (id_perfil.Text == "")
            {
                msgError = blObj.NssEjecutarSQL("INSERT", ref dsInterno, ref strAux, "", "", null, null);
                id_perfil.Text = strAux;
            }
            else
            {
                blObj.Add("id_perfil", id_perfil.Text);
                msgError = blObj.NssEjecutarSQL("UPDATE", ref dsInterno, ref strAux, "", "", null, null);
            }
            if (msgError == "")
            {
                lbConfirmacion.Text = "¡Registro grabado correctamente!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
                notificacion.Visible = true;
            }
            else
            {
                lbConfirmacion.Text = "¡Error al grabar el registro!" + msgError;
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacion.Visible = true;
            }
        }
    }


    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        NSSSqlUtil blObj = new NSSSqlUtil();
        String msg = "";
        DataSet dsInterno = null;
        DataSet dsUsuarios = null;
        String strAux = "";
        bool PuedeBorrar = true;
        clsblUsuarios objUsuario = new clsblUsuarios();

        if (id_perfil.Text == "1")
        {
            msg = "No se puede borrar el perfil por que es administrativo.";
            lbConfirmacion.Text = "¡Error al eliminar el registro! \r\n" + msg;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
            return;
        }

        msg = objUsuario.FiltraUsuarios(ref dsUsuarios, "", "", id_perfil.Text);
        if (msg == "")
        {
            if (dsUsuarios.Tables.Count > 0)
            {
                if (dsUsuarios.Tables[0].Rows.Count > 0)
                    PuedeBorrar = false;
            }
        }
        if (PuedeBorrar)
        {
            objUsuario.EliminaPermisosPerfil(id_perfil.Text);
            blObj.LlavePrimaria = "id_perfil";
            blObj.NombreTabla = "PERFILES";
            blObj.Add("id_perfil", id_perfil.Text);
            msg = blObj.NssEjecutarSQL("DELETE", ref dsInterno, ref strAux, "", "", null, null);
            if (msg == "") 
            {
                lbConfirmacion.Text = "¡Registro borrado correctamente!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
                notificacion.Visible = true;
            }
            else
            {
                lbConfirmacion.Text = "¡Error al eliminar el registro!" + msg;
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacion.Visible = true;
            }
        }
        else
        {
            lbConfirmacion.Text = "¡Este perfil tiene usuarios asignados y no se puede eliminar!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfPerfiles.aspx");
    }
}

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

public partial class wfCambioClave : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUtiles blU =new clsblUtiles();

        //=============== ESTAS 4 LINEAS SON PARA LAS FECHAS ===============================
        DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
        myDTF.ShortDatePattern = "dd/MM/yyyy";
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;

        hfOpcional.Value = blU.ValorObjetoString(Request.QueryString["opcional"]);
        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
        {
            if (hfOpcional.Value != "false")
            Response.Redirect("Index.aspx");
        }
        else
        {
            if (!Page.IsPostBack)
            {
                Response.Expires = 0;
                Response.Cache.SetNoStore();
                Response.AppendHeader("Pragma", "no-cache");
                if (hfOpcional.Value == "false")
                {
                    btSalir.Visible = false;
                }
                else
                {
                    btSalir.Visible = true;
                }
            }
        }

    }
    protected void btCambia_Click(object sender, EventArgs e)
    {
        NSSSqlUtil blObj = new NSSSqlUtil();
        DataSet dsInterno = null;
        String strAux = "", msg;
        bool esCorrecto;
        DataSet dsUsuarios = new DataSet();
        DataSet dsUser = new DataSet();
        String dias_contrasena;
        clsblUsuarios blUsuario = new clsblUsuarios();
        DateTime FechaHoy = DateTime.Now;
        DateTime FechaCambio;

        esCorrecto = blUsuario.VerificaPwd(ref dsUser, Session["IDUSUARIO"].ToString(), FormsAuthentication.HashPasswordForStoringInConfigFile(tbClaveAntigua.Text, "MD5").ToLower());
        if (esCorrecto)
        {
            if (tbpwdNuevo.Text == tbPwdOtra.Text)
            {
                blObj.LlavePrimaria = "id_usuario";
                blObj.NombreTabla = "USUARIOSADMIN";
                //blObj.IsIdentity = true;
                blObj.Add("id_usuario", Session["IDUSUARIO"].ToString());
                blObj.Add("password", FormsAuthentication.HashPasswordForStoringInConfigFile(tbpwdNuevo.Text, "MD5").ToLower());
                blUsuario.ConsultaUsuarios(ref dsUsuarios, Session["IDUSUARIO"].ToString());
                dias_contrasena = dsUsuarios.Tables[0].Rows[0]["dias_contrasena"].ToString();
                FechaCambio = FechaHoy.AddDays(Convert.ToInt32(dias_contrasena));
                blObj.Add("fecha_cambio_contrasena", FechaCambio.ToString("dd/MM/yyyy hh:mm:ss"));
                msg = blObj.NssEjecutarSQL("UPDATE", ref dsInterno, ref strAux, "", "", null, null);
                if (msg == "")
                {
                    lbConfirmacion.Text = "¡Se actualizó su clave con éxito!";
                    notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
                    notificacion.Visible = true;
                }
                else
                {
                    lbConfirmacion.Text = "¡Error al actualizar la clave! " + msg;
                    notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                    notificacion.Visible = true;
                }
            }
            else
            {
                lbConfirmacion.Text = "¡La contraseña y la confirmación de la contraseña no coinciden!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-warning";
                notificacion.Visible = true;
            }
        }
        else
        {
            lbConfirmacion.Text = "¡La contraseña antigua no corresponde con la registrada!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-warning";
            notificacion.Visible = true;
        }
        //Page.Header.Controls.Add(new LiteralControl(string.Format(@" <META http-equiv='REFRESH' content=3.1;url={0}> ", Request.Url.AbsoluteUri)));
    }

    protected void btSalir_Clic(object sender, EventArgs e)
    {
        Response.Redirect("Index.aspx");
    }
}

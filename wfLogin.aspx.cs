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
using System.Globalization;
using System.Threading;
using System.Web.Helpers;

public partial class wfLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AntiForgeryToken.Text = AntiForgery.GetHtml().ToHtmlString();
        //=============== ESTAS 4 LINEAS SON PARA LAS FECHAS ===============================
        DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
        myDTF.ShortDatePattern = "dd/MM/yyyy";
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;
        lbCambiarClave.Visible = false;
        Label3.Visible = false;
    }

    protected void btnEntrar_Click(object sender, EventArgs e)
    {
        clsblUsuarios obj;
        DataSet dsUsuarios=new DataSet();
        String msg="";
        clsblUtiles blu=new clsblUtiles();
        DataSet dsUsuario =new DataSet();
        DateTime FechaHoy;
        DateTime FechaCambio;
        String id_perfil;
        String script  = "";

        tbError.Text = "";
        tbError.Visible = false;
        obj = new clsblUsuarios();
        msg = obj.VerificaLogin(ref dsUsuarios, usuario.Text, (FormsAuthentication.HashPasswordForStoringInConfigFile(clave.Text, "MD5")).ToLower());
        if (msg == "")
        {
            if (dsUsuarios.Tables[0].Rows.Count > 0)
            {
                notificacion.Visible = false;
                Session["IDUSUARIO"] = dsUsuarios.Tables[0].Rows[0]["id_usuario"].ToString();
                obj.ConsultaUsuarios(ref dsUsuario, Session["IDUSUARIO"].ToString());
                id_perfil = dsUsuario.Tables[0].Rows[0]["id_perfil"].ToString();
                FechaHoy = DateTime.Now;
                if (dsUsuario.Tables[0].Rows[0]["fecha_cambio_contrasena"].ToString() != "")
                    FechaCambio = (DateTime)dsUsuario.Tables[0].Rows[0]["fecha_cambio_contrasena"];
                else
                    FechaCambio = DateTime.Parse("01/01/01");
                FechaCambio = FechaCambio.AddDays(1);
                if (FechaCambio.Year != 1)
                {
                    if (FechaHoy < FechaCambio)
                    {
                        script = "<script> parent.location='index2.aspx'; </script>";
                        ClientScript.RegisterStartupScript(this.GetType(), "login", script);
                    }
                    else
                    {
                        lbConfirmacion.Text = "¡La contraseña ha caducado!";
                        notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                        notificacion.Visible = true;
                        lbCambiarClave.Attributes.Add("href", "javascript:void(0);");
                        lbCambiarClave.Attributes.Add("class", "verDetalles");
                        lbCambiarClave.Attributes.Add("data-id", Session["IDUSUARIO"].ToString());
                        //lbCambiarClave.OnClientClick = "openModalWindow('Modales.aspx?pagina=wfCambioClave&id_usuario=" + Session["IDUSUARIO"].ToString() + "',360,280);";
                        Label3.Visible = true;
                        lbCambiarClave.Visible = true;
                    }
                }
            }
            else
            {
                lbConfirmacion.Text = "¡Los datos suministrados son incorrectos!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacion.Visible = true;
            }
        }
        else
        {
            lbConfirmacion.Text = msg;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
            //tbError.Text = msg;
            //tbError.Visible = true;
        }
    }

}

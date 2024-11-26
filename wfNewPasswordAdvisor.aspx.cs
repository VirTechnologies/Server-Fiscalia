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
using System.Net;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

public partial class wfNewPasswordAdvisor : System.Web.UI.Page
{
    private string NombreUsuario;
    private string EmailUsuario;
    private string PasswordActual;
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUtiles blU = new clsblUtiles();
        DataSet dsAsesores = new DataSet();
        clsblParametricas blPara = new clsblParametricas();

        //=============== ESTAS 4 LINEAS SON PARA LAS FECHAS ===============================
        DateTimeFormatInfo myDTF = new DateTimeFormatInfo
        {
            ShortDatePattern = "dd/MM/yyyy"
        };
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
        {
        }
        else
        {
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            string msgError = blPara.ConsultaUsuariosAsesores(ref dsAsesores, hfid.Value, "");
            if (msgError == "")
            {
                NombreUsuario = dsAsesores.Tables[0].Rows[0]["Nombre"].ToString();
                EmailUsuario = dsAsesores.Tables[0].Rows[0]["Email"].ToString();
                PasswordActual = dsAsesores.Tables[0].Rows[0]["PasswordHash"].ToString();
                LblInfoUsuario.Text = $"{NombreUsuario} - {EmailUsuario}";
            }
            if (!Page.IsPostBack)
            {
            }
        }

    }
    protected void btCambia_Click(object sender, EventArgs e)
    {
        string msg;

        if (tbpwdNuevo.Text == tbPwdOtra.Text)
        {
            try
            {
                string UrlApi = ConfigurationManager.AppSettings["HostApi"];

                string metodo = "/api/auth/changePassword";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(UrlApi + metodo);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                var user = new Dictionary<string, object>
                {
                    { "Name", NombreUsuario },
                    { "Email", EmailUsuario },
                    { "PhoneNumber", " " },
                    { "IsAdvisor", 1 },
                    { "Password", PasswordActual },
                    { "NewPassword", tbpwdNuevo.Text }
                };

                string jsonEnvio = JsonConvert.SerializeObject(user);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonEnvio);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var result = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                //Encola en rabbit los cambios
                clsblUtiles blU = new clsblUtiles();
                List<string> Sentencias = new List<string>();
                Sentencias.Add(result);
                blU.EncolarMensajesRabbit(Sentencias, "", true);

                lbConfirmacion.Text = "¡Contraseña grabada correctamente!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
                notificacion.Visible = true;
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();
                        lbConfirmacion.Text = "¡Error al grabar el registro! " + text;
                        Console.WriteLine(text);
                    }
                }
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacion.Visible = true;
            }
            catch (Exception ex)
            {
                lbConfirmacion.Text = "¡Error al grabar el registro! " + ex.ToString();
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacion.Visible = true;
            }

            //if (true)
            //{
            //    lbConfirmacion.Text = "¡Se actualizó su clave con éxito!";
            //    notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
            //    notificacion.Visible = true;
            //}
            //else
            //{
            //    lbConfirmacion.Text = "¡Error al actualizar la clave! " + msg;
            //    notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            //    notificacion.Visible = true;
            //}
        }
        else
        {
            lbConfirmacion.Text = "¡La contraseña y la confirmación de la contraseña no coinciden!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-warning";
            notificacion.Visible = true;
        }
    }

    protected void CambiarClave()
    {
        try
        {
            string UrlApi = ConfigurationManager.AppSettings["HostApi"];

            string metodo = "/api/auth/changePassword";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(UrlApi + metodo);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            var user = new Dictionary<string, object>
            {
                { "Name", NombreUsuario },
                { "Email", EmailUsuario },
                { "PhoneNumber", " " },
                { "IsAdvisor", " " },
                { "Password", PasswordActual },
                { "NewPassword", tbpwdNuevo.Text }
            };

            string jsonEnvio = JsonConvert.SerializeObject(user);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonEnvio);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();


            var result = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            //Encola en rabbit los cambios
            clsblUtiles blU = new clsblUtiles();
            List<string> Sentencias = new List<string>();
            Sentencias.Add(result);
            blU.EncolarMensajesRabbit(Sentencias, "", true);


            lbConfirmacion.Text = "¡Registro grabado correctamente!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
            notificacion.Visible = true;
        }
        catch (WebException ex)
        {
            using (WebResponse response = ex.Response)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                using (Stream data = response.GetResponseStream())
                {
                    string text = new StreamReader(data).ReadToEnd();
                    lbConfirmacion.Text = "¡Error al grabar el registro! " + text;
                    Console.WriteLine(text);
                }
            }
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;

        }
        catch (Exception ex)
        {
            lbConfirmacion.Text = "¡Error al grabar el registro! " + ex.ToString();
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }



    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfUsuariosCajas.aspx");
    }
}

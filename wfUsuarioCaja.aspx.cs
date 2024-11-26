using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Web.UI;

public partial class wfUsuarioCaja : System.Web.UI.Page
{

    string EmaiOld;

    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DataSet dsAsesores = new DataSet();
        clsblParametricas blPara = new clsblParametricas();

        
        //=============== ESTAS 4 LINEAS SON PARA LAS FECHAS ===============================
        DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
        myDTF.ShortDatePattern = "dd/MM/yyyy";
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");

        // Configura los botones de acuerdo a los permisos
        if (!((objUsuario.PermisoModulo("58", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("58", Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (!objUsuario.PermisoModulo("58", Session["IDUSUARIO"].ToString(), "D"))
            btnEliminar.Visible = false;
        if (!Page.IsPostBack)
        {
            blU.LlenaDDLObligatorio(ddlProcedencia, "Procedencias", "Id", "Nombre", "", "", "Nombre");
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            if (hfid.Value != "")
            {
                msgError = blPara.ConsultaUsuariosAsesores(ref dsAsesores, hfid.Value, "");
                if (msgError == "")
                {

                    tbNombre.Text = dsAsesores.Tables[0].Rows[0]["Nombre"].ToString();
                    tbUserName.Text = dsAsesores.Tables[0].Rows[0]["UserName"].ToString();
                    CambioClave.Visible = false;
                    //tbPasswordHash.Text = dsAsesores.Tables[0].Rows[0]["PasswordHash"].ToString();
                    Session["EmailOld"] = tbEmail.Text = dsAsesores.Tables[0].Rows[0]["Email"].ToString();
                    tbPhoneNumber.Text = dsAsesores.Tables[0].Rows[0]["PhoneNumber"].ToString();
                    ddlPerfilOficina.SelectedValue = dsAsesores.Tables[0].Rows[0]["isAdvisor"].ToString();
                    ddlProcedencia.SelectedValue = dsAsesores.Tables[0].Rows[0]["ProcedenciaId"].ToString();
                }
            }
            else
            {
                btnEliminar.Visible = false;
                CambioClave.Visible = true;
            }
        }
        if (hfConsulta.Value == "si")
        {
            btnGrabar.Visible = false;
            btnEliminar.Visible = false;
            objUsuario.SoloLectura(this);
        }

    }

    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        Variables[] Pares;
        string elJson = "";
        string msgError = "";
        clsblParametricas blParam = new clsblParametricas();
        DataSet dsOficinas = new DataSet();
        bool esAsesor = false;
        int i = 0;

        try
        {
            string UrlApi = ConfigurationManager.AppSettings["HostApi"];

            string metodo = "";


            if (hfid.Value != "")
            {
                EmaiOld = Session["EmailOld"].ToString();
                metodo = "/api/auth/editUser/" + EmaiOld;

            }
            else
            {
                metodo = "/api/auth/createUser";

            }
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(UrlApi + metodo);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            //     nuevoPassword = tbPasswordHash.Text;


            esAsesor = Convert.ToBoolean(ddlPerfilOficina.SelectedValue);

            var user = new Dictionary<string, object>
            {
                { "Name", tbNombre.Text },
                { "Email", tbUserName.Text },
                { "PhoneNumber", tbPhoneNumber.Text },
                { "IsAdvisor", esAsesor },
                { "Password", tbPasswordHash.Text },
                { "ProcedenciaId", ddlProcedencia.SelectedValue }
            };

            string jsonEnvio = JsonConvert.SerializeObject(user);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                //string json = "{\"Name\": \"" + tbNombre.Text + 
                //              "\", \"Email\": \"" + tbEmail.Text +
                //              "\", \"PhoneNumber\": \"" + tbPhoneNumber.Text +
                //              "\", \"IsAdvisor\": \"" + superAsesor +
                //              "\", \"Password\": \"" + tbPasswordHash.Text + "\"}";

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


            //Pares = new Variables[2];
            //Pares[0] = new Variables();
            //Pares[0].Variable = "action : SQLAction";
            //Pares[1] = new Variables();
            //Pares[1].Variable = "SQLStatement : " + result;
            //elJson = JsonConvert.SerializeObject(Pares);
            //msgError = blParam.ConsultaOficinas(ref dsOficinas, "", "");
            ////for (i = 0; i < dsOficinas.Tables[0].Rows.Count; i++)
            ////    Sender.sendMessage("office" + dsOficinas.Tables[0].Rows[i]["Id"].ToString(), elJson, "localhost", 5672, "admin", "admin");
            //Sender.sendMessage("office105", elJson, "localhost", 5672, "admin", "admin");

            lbConfirmacion.Text = "¡Registro grabado correctamente!";
            //        CambiarClave();
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

        /*
                try
                {
                    blObj.LlavePrimaria = "id";
                    blObj.NombreTabla = "Usuarios";
                    blObj.Add("Nombre", tbNombre.Text);
                    blObj.Add("UserName", tbUserName.Text);
                    blObj.Add("PasswordHash", tbPasswordHash.Text);
                    blObj.Add("Email", tbEmail.Text);
                    blObj.Add("PhoneNumber", tbPhoneNumber.Text);
                    if (hfid.Value == "")
                    {
                        msgError = blU.FiltraParametros(ref dsParametros, "ENCOLAR_RABBIT", "");
                        if (msgError == "")
                        {
                            if (dsParametros.Tables[0].Rows[0]["Valor"].ToString().ToUpper() == "SI")
                            {
                                Pares = new Variables[5];
                                Pares[0] = new Variables();
                                Pares[0].Variable = "action : CreateUser";
                                Pares[1] = new Variables();
                                Pares[1].Variable = "Nombre : " + tbNombre.Text;
                                Pares[2] = new Variables();
                                Pares[2].Variable = "Email : " + tbEmail.Text;
                                Pares[3] = new Variables();
                                Pares[3].Variable = "PhoneNumber : " + tbPhoneNumber.Text;
                                Pares[4] = new Variables();
                                Pares[4].Variable = "Password : " + tbPasswordHash.Text;
                                elJson = JsonConvert.SerializeObject(Pares);
                                msgError = blParam.ConsultaOficinas(ref dsOficinas, "", "");
                                for (i = 0; i < dsOficinas.Tables[0].Rows.Count; i++)
                                    Sender.sendMessage("office" + dsOficinas.Tables[0].Rows[i]["Id"].ToString(), elJson, "localhost", 5672, "admin", "admin");
                            }
                        }
                        btnGrabar.Visible = false;
                    }
                    else
                    {
                        blObj.Add("id", hfid.Value);
                        msgError = blObj.NssEjecutarSQL("UPDATE", ref dsInterno, ref strAux, "", "", null, null);

                        List<string> Sentencias = new List<string>();
                        Sentencias.Add(blObj.strSQLExecuted);
                        blU.EncolarMensajesRabbit(Sentencias, "", true);
                    }
                    lbConfirmacion.CssClass = "text-success";
                    lbConfirmacion.Text = "¡Registro grabado correctamente!";
                }
                catch (Exception ex)
                {
                    lbConfirmacion.CssClass = "text-danger";
                    lbConfirmacion.Text = "¡Error al grabar el registro!" + ex.ToString();
                }
        */
    }

    protected void CambiarClave()
    {
        Variables[] Pares;
        string elJson = "";
        string msgError = "";
        clsblParametricas blParam = new clsblParametricas();
        DataSet dsOficinas = new DataSet();
        bool esAsesor = false;
        int i = 0;

        try
        {
            string UrlApi = ConfigurationManager.AppSettings["HostApi"];

            string metodo = "";


            metodo = "/api/auth/changePassword/";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(UrlApi + metodo);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";



            esAsesor = Convert.ToBoolean(ddlPerfilOficina.SelectedValue);

            var user = new Dictionary<string, object>
            {
                { "Name", tbNombre.Text },
                { "Email", tbEmail.Text },
                { "PhoneNumber", tbPhoneNumber.Text },
                { "IsAdvisor", esAsesor },
                { "Password", tbPasswordHash.Text },
                { "NewPassword", tbPasswordHash.Text }
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


            //Pares = new Variables[2];
            //Pares[0] = new Variables();
            //Pares[0].Variable = "action : SQLAction";
            //Pares[1] = new Variables();
            //Pares[1].Variable = "SQLStatement : " + result;
            //elJson = JsonConvert.SerializeObject(Pares);
            //msgError = blParam.ConsultaOficinas(ref dsOficinas, "", "");
            ////for (i = 0; i < dsOficinas.Tables[0].Rows.Count; i++)
            ////    Sender.sendMessage("office" + dsOficinas.Tables[0].Rows[i]["Id"].ToString(), elJson, "localhost", 5672, "admin", "admin");
            //Sender.sendMessage("office105", elJson, "localhost", 5672, "admin", "admin");

            lbConfirmacion.Text = "¡Registro grabado correctamente!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
            notificacion.Visible = true;
            //       AntesPassword = "";
            //       AntesPassword = "";
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



    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        NSSSqlUtil blObj = new NSSSqlUtil();
        String msg = "";
        DataSet dsInterno = null;
        String strAux = "";
        clsblUtiles blU = new clsblUtiles();

        try
        {
            blObj.LlavePrimaria = "id";
            blObj.NombreTabla = "Usuarios";
            blObj.Add("id", hfid.Value);

            msg = blObj.NssEjecutarSQL("DELETE", ref dsInterno, ref strAux, "", "", null, null);

            if (string.IsNullOrEmpty(msg))
            {
                List<string> Sentencias = new List<string>();
                Sentencias.Add(blObj.strSQLExecuted);
                blU.EncolarMensajesRabbit(Sentencias, "", true);

                lbConfirmacion.Text = "¡Registro borrado correctamente!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
                notificacion.Visible = true;
                btnGrabar.Visible = false;
                btnEliminar.Visible = false;
            }
            else
            {
                lbConfirmacion.Text = "¡Error eliminando el usuario! " + msg;
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacion.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lbConfirmacion.Text = "¡Error eliminando el usuario! " + ex.Message;
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
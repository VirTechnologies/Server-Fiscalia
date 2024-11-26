using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;

public partial class wfServicio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DataSet dsServicios = new DataSet();
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
        if (!((objUsuario.PermisoModulo("50", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("50", Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }

            
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            blU.LlenaDDLObligatorio(ddlIdServicioPadre, "Servicios", "Id", "Nombre", "", "", "Nombre");
            if (hfid.Value != "")
            {
                msgError = blPara.ConsultaServicios(ref dsServicios, hfid.Value, "");
                if (msgError == "")
                {
                    tbNombre.Text = dsServicios.Tables[0].Rows[0]["Nombre"].ToString();
                    tbAbreviatura.Text = dsServicios.Tables[0].Rows[0]["Abreviatura"].ToString();
                    tbBlacktouch.Text = dsServicios.Tables[0].Rows[0]["TextToSpeech"].ToString();
                    if (dsServicios.Tables[0].Rows[0]["ServicioPadreId"].ToString() != "")
                        ddlIdServicioPadre.SelectedValue = dsServicios.Tables[0].Rows[0]["ServicioPadreId"].ToString();
                    if (dsServicios.Tables[0].Rows[0]["VisibleEnKiosco"].ToString() == "True")
                        cbVisibleKiosco.Checked = true;
                    else
                        cbVisibleKiosco.Checked = false;
                }
            }

        }
        if (hfConsulta.Value == "si")
        {
            btnGrabar.Visible = false;
            
            cbVisibleKiosco.Enabled = false;
            objUsuario.SoloLectura(this);
        }
    }

    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        NSSSqlUtil blObj = new NSSSqlUtil();
        String msgError = "";
        DataSet dsInterno = null;
        String strAux = "";
        clsblUtiles blU = new clsblUtiles();

        try
        {
            blObj.LlavePrimaria = "id";
            blObj.NombreTabla = "Servicios";
            blObj.Add("Nombre", tbNombre.Text);
            blObj.Add("Abreviatura", tbAbreviatura.Text);
            blObj.Add("TextToSpeech", tbBlacktouch.Text);
            blObj.Add("ServicioPadreId", ddlIdServicioPadre.SelectedValue);
            if (cbVisibleKiosco.Checked)
                blObj.Add("VisibleEnKiosco", "1");
            else
                blObj.Add("VisibleEnKiosco", "0");

            if (hfid.Value == "")
            {
                msgError = blObj.NssEjecutarSQL("INSERT", ref dsInterno, ref strAux, "", "", null, null);
                hfid.Value = strAux;
            }
            else
            {
                blObj.Add("id", hfid.Value);
                msgError = blObj.NssEjecutarSQL("UPDATE", ref dsInterno, ref strAux, "", "", null, null);
            }
            List<string> Sentencias = new List<string>();
            Sentencias.Add(blObj.strSQLExecuted);
            blU.EncolarMensajesRabbit(Sentencias, "", true);
            lbConfirmacion.Text = "¡Registro grabado correctamente!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
            notificacion.Visible = true;
        }
        catch (Exception ex)
        {
            lbConfirmacion.Text = "¡Error al grabar el registro!" + ex.ToString();
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
            blObj.NombreTabla = "Servicios";
            blObj.Add("id", hfid.Value);
            msg = blObj.NssEjecutarSQL("DELETE", ref dsInterno, ref strAux, "", "", null, null);
            List<string> Sentencias = new List<string>();
            Sentencias.Add(blObj.strSQLExecuted);
            blU.EncolarMensajesRabbit(Sentencias, "", true);

            lbConfirmacion.Text = "¡Registro borrado correctamente!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
            notificacion.Visible = true;
            btnGrabar.Visible = false;
            //btnEliminar.Visible = false;
        }
        catch (Exception ex)
        {
            lbConfirmacion.Text = "¡Error eliminando el servicio!" + ex.Message;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfServicios.aspx");
    }
}
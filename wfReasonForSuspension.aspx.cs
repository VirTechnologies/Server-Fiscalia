﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Threading;

public partial class wfReasonForSuspension : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DataSet dsReasons = new DataSet();
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
        if (!((objUsuario.PermisoModulo("48", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("48", Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (!objUsuario.PermisoModulo("48", Session["IDUSUARIO"].ToString(), "D"))
            btnEliminar.Visible = false;
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            if (hfid.Value != "")
            {
                msgError = blPara.ConsultaReasonsForSuspension(ref dsReasons, hfid.Value, "");
                if (msgError == "")
                {
                    tbNombre.Text = dsReasons.Tables[0].Rows[0]["Description"].ToString();
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
    }

    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        clsblUtiles blU = new clsblUtiles();
        NSSSqlUtil blObj = new NSSSqlUtil();
        String msgError = "";
        DataSet dsInterno = null;
        String strAux = "";

        try
        {
            blObj.LlavePrimaria = "id";
            blObj.NombreTabla = "ReasonsForSuspension";
            blObj.IsIdentity = true;
            blObj.Add("Description", tbNombre.Text);
            blObj.Add("Activity", "3");
            
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
            blObj.NombreTabla = "ReasonsForSuspension";
            blObj.IsIdentity = true;
            blObj.Add("id", hfid.Value);
            msg = blObj.NssEjecutarSQL("DELETE", ref dsInterno, ref strAux, "", "", null, null);

            List<string> Sentencias = new List<string>();
            Sentencias.Add(blObj.strSQLExecuted);
            blU.EncolarMensajesRabbit(Sentencias, "", true);

            lbConfirmacion.Text = "¡Registro borrado correctamente!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
            notificacion.Visible = true;

            btnGrabar.Visible = false;
            btnEliminar.Visible = false;
        }
        catch (Exception ex)
        {
            lbConfirmacion.Text = "¡Error eliminando razón de suspensión!" + ex.Message;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfReasonsForSuspension.aspx");
    }
}
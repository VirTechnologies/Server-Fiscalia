using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;

public partial class wfServicioTipoAtencion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DataSet dsServicioTipo = new DataSet();
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
        if (!((objUsuario.PermisoModulo("55", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("55", Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (!objUsuario.PermisoModulo("55", Session["IDUSUARIO"].ToString(), "D"))
            btnEliminar.Visible = false;
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            blU.LlenaDDLObligatorio(ddlServicioId, "Servicios", "Id", "Nombre", "", "SELECT Id,Nombre FROM Servicios WHERE ServicioPadreId IS NULL", "Nombre");
            blU.LlenaDDLObligatorio(ddlTipoAtencionId, "TipoDeAtencion", "Id", "Nombre", "", "", "Nombre");
            if (hfid.Value != "")
            {
                msgError = blPara.ConsultaServiciosTiposDeAtencion(ref dsServicioTipo, hfid.Value, "");
                if (msgError == "")
                {
                    ddlServicioId.SelectedValue = dsServicioTipo.Tables[0].Rows[0]["ServicioId"].ToString();
                    ddlTipoAtencionId.SelectedValue = dsServicioTipo.Tables[0].Rows[0]["TipoAtencionId"].ToString();
                    tbHoraDesde.Text = Convert.ToDateTime(dsServicioTipo.Tables[0].Rows[0]["HoraDesde"].ToString()).ToString("HH:mm");
                    tbHoraFin.Text = Convert.ToDateTime(dsServicioTipo.Tables[0].Rows[0]["HoraFin"].ToString()).ToString("HH:mm");
                    tbAbreviatura.Text = dsServicioTipo.Tables[0].Rows[0]["Abreviatura"].ToString();
                    tbTiempoAtencionMinutos.Text = dsServicioTipo.Tables[0].Rows[0]["TiempoAtencionMinutos"].ToString();
                    tbTiempoEsperaMinutos.Text = dsServicioTipo.Tables[0].Rows[0]["TiempoEsperaMinutos"].ToString();
                    cbHabilitaFranjaHoraria.Checked = false;
                    if (dsServicioTipo.Tables[0].Rows[0]["HabilitaFranjaHoraria"].ToString() != "")
                    {
                        if ((bool)dsServicioTipo.Tables[0].Rows[0]["HabilitaFranjaHoraria"])
                            cbHabilitaFranjaHoraria.Checked = true;
                    }
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
        NSSSqlUtil blObj = new NSSSqlUtil();
        String msgError = "";
        DataSet dsInterno = null;
        String strAux = "";

        try
        {
            blObj.LlavePrimaria = "Id";
            blObj.NombreTabla = "ServicioTipoAtencion";
            if (hfid.Value == "")
            {
                if (blObj.Duplicado("Abreviatura", tbAbreviatura.Text))
                {
                    lbConfirmacion.Text = "Ya existe esa abreviatura!";
                    notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                    notificacion.Visible = true;
                    return;
                }
            }
            blObj.Add("ServicioId", ddlServicioId.SelectedValue);
            blObj.Add("TipoAtencionId", ddlTipoAtencionId.SelectedValue);
            if (hfid.Value == "")  ////jose 20230719
            {
                blObj.Add("HoraDesde", tbHoraDesde.Text);
                blObj.Add("HoraFin", tbHoraFin.Text);
            }
            blObj.Add("Abreviatura", tbAbreviatura.Text);
            blObj.Add("TiempoAtencionMinutos", tbTiempoAtencionMinutos.Text);
            blObj.Add("TiempoEsperaMinutos", tbTiempoEsperaMinutos.Text);
            if (cbHabilitaFranjaHoraria.Checked)
                blObj.Add("HabilitaFranjaHoraria", "1");
            else
                blObj.Add("HabilitaFranjaHoraria", "0");
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
            clsblUtiles blU = new clsblUtiles();
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

        try
        {
            blObj.LlavePrimaria = "Id";
            blObj.NombreTabla = "ServicioTipoAtencion";
            blObj.Add("id", hfid.Value);
            msg = blObj.NssEjecutarSQL("DELETE", ref dsInterno, ref strAux, "", "", null, null);
            clsblUtiles blU = new clsblUtiles();
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
            lbConfirmacion.Text = "¡Error eliminando el registro!" + ex.Message;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfServiciosTipoAtencion.aspx");
    }

}
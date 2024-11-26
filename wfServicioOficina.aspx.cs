using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Threading;

public partial class wfServicioOficina : System.Web.UI.Page
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
        if (!((objUsuario.PermisoModulo("52", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("52", Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            hfServicioId.Value = blU.ValorObjetoString(Request.QueryString["ServicioId"]);
            hfOficinaId.Value = blU.ValorObjetoString(Request.QueryString["OficinaId"]);
            if (hfid.Value != "")
            {
                msgError = blPara.ConsultaServiciosOficina(ref dsServicios, hfid.Value, "", "", null, null);
                if (msgError == "")
                {
                    tbNombreOficina.Text = dsServicios.Tables[0].Rows[0]["NombreOficina"].ToString();
                    tbNombreServicio.Text = dsServicios.Tables[0].Rows[0]["NombreServicio"].ToString();
                    tbHoraIni.Text = dsServicios.Tables[0].Rows[0]["HoraIni"].ToString();
                    tbHoraFin.Text = dsServicios.Tables[0].Rows[0]["HoraFin"].ToString();
                    tbMensajeServicio.Text = dsServicios.Tables[0].Rows[0]["MensajeServicio"].ToString();
                    tbTiempoMaximoSegundos.Text = dsServicios.Tables[0].Rows[0]["TiempoMaximoSegundos"].ToString();
                    tbNumeroLlamados.Text = dsServicios.Tables[0].Rows[0]["NumeroLlamados"].ToString();
                }
            }else
            {
                DataSet dsServicio = new DataSet();
                DataSet dsOficina = new DataSet();

                msgError = blPara.ConsultaServicios(ref dsServicio,hfServicioId.Value,"");
                msgError = blPara.ConsultaOficinas(ref dsOficina, hfOficinaId.Value, "");
                tbNombreServicio.Text = dsServicio.Tables[0].Rows[0]["Nombre"].ToString();
                tbNombreOficina.Text = dsOficina.Tables[0].Rows[0]["Nombre"].ToString();
            }
        }
        if (hfConsulta.Value == "si")
        {
            btnGrabar.Visible = false;
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
            blObj.LlavePrimaria = "id";
            blObj.NombreTabla = "ServiciosEnOficina";
            blObj.Add("HoraIni", tbHoraIni.Text);
            blObj.Add("HoraFin", tbHoraFin.Text);
            blObj.Add("MensajeServicio", tbMensajeServicio.Text);
            blObj.Add("TiempoMaximoSegundos", tbTiempoMaximoSegundos.Text);
            blObj.Add("NumeroLlamados", tbNumeroLlamados.Text);
            if (hfid.Value == "")
            {
                blObj.Add("OficinaId", hfOficinaId.Value);
                blObj.Add("ServicioId", hfServicioId.Value);
                msgError = blObj.NssEjecutarSQL("INSERT", ref dsInterno, ref strAux, "", "", null, null);
                hfid.Value = strAux;
            }
            else
            {
                blObj.Add("id", hfid.Value);
                msgError = blObj.NssEjecutarSQL("UPDATE", ref dsInterno, ref strAux, "", "", null, null);
            }
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

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfServiciosOficina.aspx");
    }
}
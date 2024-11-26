using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Threading;

public partial class wfTipoDeAtencion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DataSet dsTipo = new DataSet();
        clsblParametricas blPara = new clsblParametricas();

        //=============== ESTAS 4 LINEAS SON PARA LAS FECHAS ===============================
        DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
        myDTF.ShortDatePattern = "dd/MM/yyyy";
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
        if (!((objUsuario.PermisoModulo("54", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("54", Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (!objUsuario.PermisoModulo("54", Session["IDUSUARIO"].ToString(), "D"))
            btnEliminar.Visible = false;
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            if (hfid.Value != "")
            {
                msgError = blPara.ConsultaTiposDeAtencion(ref dsTipo, hfid.Value, "");
                if (msgError == "")
                {
                    tbNombre.Text = dsTipo.Tables[0].Rows[0]["Nombre"].ToString();
                    cbVisibleEnKiosco.Checked = false;
                    if (dsTipo.Tables[0].Rows[0]["IsVisibleInKiosk"].ToString() != "")
                    {
                        if ((bool)dsTipo.Tables[0].Rows[0]["IsVisibleInKiosk"])
                            cbVisibleEnKiosco.Checked = true;
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
            blObj.LlavePrimaria = "id";
            blObj.NombreTabla = "TipoDeAtencion";
            blObj.Add("Nombre", tbNombre.Text);
            if (cbVisibleEnKiosco.Checked)
              blObj.Add("IsVisibleInKiosk", "1");
            else
              blObj.Add("IsVisibleInKiosk", "0");
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
        String msg = "", strAux = "";
        DataSet dsInterno = null;
    
        try
        {
            blObj.LlavePrimaria = "id";
            blObj.NombreTabla = "TipoDeAtencion";
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
            lbConfirmacion.Text = "¡Error eliminando el tipo de atención!" + ex.Message;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfTiposDeAtencion.aspx");
    }

}
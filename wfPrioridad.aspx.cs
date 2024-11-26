using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;

public partial class wfPrioridad : System.Web.UI.Page
{
    Variables variables = new Variables();
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DataSet dsPrioridades = new DataSet();
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
        if (!((objUsuario.PermisoModulo("49", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("49", Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (!objUsuario.PermisoModulo("49", Session["IDUSUARIO"].ToString(), "D"))
            btnEliminar.Visible = false;
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            blU.LlenaDDLObligatorio(ddlPerfilDeAtencionId, "PerfilesDeAtencion", "Id", "Nombre", "", "", "Nombre");
            blU.LlenaDDLObligatorioUnico(ddlServicioTipoAtencionId, "ServicioTipoAtencion", "Id", "servicios", "", "", "servicios");
            blU.LlenaDDLObligatorio(ddltematica, "Grupos", "Id", "Nombre", "", "", "Nombre");

            if (hfid.Value != "")
            {
                msgError = blPara.ConsultaPrioridades(ref dsPrioridades, hfid.Value, "", "", "");
                if (msgError == "")
                {
                    blU.LlenaDDLObligatorio(ddlOficinaId, "Oficinas", "Id", "Nombre", "", "", "Nombre");
                    blU.LlenaDDLObligatorio(dllsala, "Sala", "Id", "Descripcion", "", "", "Descripcion");
                    if (dsPrioridades.Tables[0].Rows[0]["OficinaId"].ToString() != "")
                        ddlOficinaId.SelectedValue = dsPrioridades.Tables[0].Rows[0]["OficinaId"].ToString();
                    if (dsPrioridades.Tables[0].Rows[0]["TematicaId"].ToString() != "")
                        ddltematica.SelectedValue = dsPrioridades.Tables[0].Rows[0]["TematicaId"].ToString();
                    if (dsPrioridades.Tables[0].Rows[0]["IdSala"].ToString() != "")
                    {
                        dllsala.SelectedValue = dsPrioridades.Tables[0].Rows[0]["IdSala"].ToString();

                    }
                    if (dsPrioridades.Tables[0].Rows[0]["PerfilDeAtencionId"].ToString() != "")
                        ddlPerfilDeAtencionId.SelectedValue = dsPrioridades.Tables[0].Rows[0]["PerfilDeAtencionId"].ToString();
                    if (dsPrioridades.Tables[0].Rows[0]["ServicioTipoAtencionId"].ToString() != "")
                        ddlServicioTipoAtencionId.SelectedValue = dsPrioridades.Tables[0].Rows[0]["ServicioTipoAtencionId"].ToString();
                        ddlServicioTipoAtencionId.DataTextField = dsPrioridades.Tables[0].Rows[0]["Servicios"].ToString();
                    tbPonderacion.Text = dsPrioridades.Tables[0].Rows[0]["Ponderacion"].ToString();
                    tbCantMaxTurnos.Text = dsPrioridades.Tables[0].Rows[0]["CantMaxTurnos"].ToString();
                    if (dsPrioridades.Tables[0].Rows[0]["Habilitado"].ToString() == "True")
                        cbVisible.Checked = true;
                    else
                        cbVisible.Checked = false;
                }
            }
            else
                btnEliminar.Visible = false;
        }
        if (hfConsulta.Value == "si")
        {
            cbVisible.Enabled = false;
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
            blObj.NombreTabla = "Prioridad";
            blObj.Add("Ponderacion", tbPonderacion.Text);
            blObj.Add("CantMaxTurnos", tbCantMaxTurnos.Text);
            blObj.Add("OficinaId", ddlOficinaId.SelectedValue);
            blObj.Add("PerfilDeAtencionId", ddlPerfilDeAtencionId.SelectedValue);
            blObj.Add("ServicioTipoAtencionId", ddlServicioTipoAtencionId.SelectedValue);
			blObj.Add("GrupoId", ddltematica.SelectedValue);
            blObj.Add("SalaId", dllsala.SelectedValue);
            if (cbVisible.Checked)
                blObj.Add("Habilitado", "1");
            else
                blObj.Add("Habilitado", "0");

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
            blObj.LlavePrimaria = "id";
            blObj.NombreTabla = "Prioridad";
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
            lbConfirmacion.Text = "¡Error eliminando la prioridad!" + ex.Message;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfPrioridades.aspx");
    }


    protected void ddlOficinaId_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOficinaId.SelectedIndex > 0)
        {
            clsblUtiles blU = new clsblUtiles();
            blU.LlenaDDLObligatorio(dllsala, "Sala", "Id", "Descripcion", $"OficinaId = {ddlOficinaId.SelectedValue}", "", "Descripcion");
        }
        else
        {
            dllsala.Items.Clear();
        }
    }
    protected void ddltematica_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddltematica.SelectedIndex > 0)
        {
            clsblUtiles blU = new clsblUtiles();
           
           // blU.LlenaDDLObligatorio(ddlOficinaId, "Grupo_oficinas", "Id_Grupo", "Id_Oficina", $"Id_Grupo = {ddltematica.SelectedValue}", "", "Id_Oficina");
            blU.LlenaDDLObligatorioTematicas(ddlOficinaId, "Oficinas", "Id", "Nombre", $"Id_Grupo = {ddltematica.SelectedValue}", "", "Nombre");
        }

        else
        {
            dllsala.Items.Clear();
        }
    }
}
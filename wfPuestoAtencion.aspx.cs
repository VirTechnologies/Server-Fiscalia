using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wfPuestoAtencion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DataSet dsPuestos = new DataSet();
        clsblParametricas blPara = new clsblParametricas();

        //=============== ESTAS 4 LINEAS SON PARA LAS FECHAS ===============================
        DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
        myDTF.ShortDatePattern = "dd/MM/yyyy";
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
        //if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
        //    Response.Redirect("wfSesionTimeOut.aspx?modal=0");

        // Configura los botones de acuerdo a los permisos
        if (!((objUsuario.PermisoModulo("46", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("46", Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;

            objUsuario.SoloLectura(this);
        }
        if (!objUsuario.PermisoModulo("46", Session["IDUSUARIO"].ToString(), "D"))
            btnEliminar.Visible = false;
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            blU.LlenaDDLObligatorio(ddlIdOficina, "Oficinas", "Id", "Nombre", "", "", "Nombre");
            if (hfid.Value != "")
            {
                msgError = blPara.ConsultaPuestos(ref dsPuestos, hfid.Value, "");
                if (msgError == "")
                {
                    tbNombre.Text = dsPuestos.Tables[0].Rows[0]["Nombre"].ToString();
                    tbAbreviatura.Text = dsPuestos.Tables[0].Rows[0]["Abreviatura"].ToString();
//                    ddlSede.Text = dsPuestos.Tables[0].Rows[0]["Descripcion"].ToString();
                    Session["ID_sala"] = dsPuestos.Tables[0].Rows[0]["SalaId"].ToString();

                    ListItem Item2 = new ListItem();
                    Item2.Text = dsPuestos.Tables[0].Rows[0]["Descripcion"].ToString(); 
                    Item2.Value = "";
                    Item2.Attributes.Add("disabled", "''");
                    Item2.Selected = true;
                    ddlSede.Items.Insert(0, Item2);

                    if (dsPuestos.Tables[0].Rows[0]["OficinaId"].ToString() != "")
                    {
                        ddlIdOficina.SelectedValue = dsPuestos.Tables[0].Rows[0]["OficinaId"].ToString();
                    }
                    if (dsPuestos.Tables[0].Rows[0]["SalaId"].ToString() != "")
                    {
                        blU.LlenaDDLObligatorio(ddlSede, "Sala", "Id", "Descripcion", $"OficinaId = {ddlIdOficina.SelectedValue.ToString()}", "", "Descripcion");
                        ddlSede.SelectedValue = dsPuestos.Tables[0].Rows[0]["SalaId"].ToString();
                    }
                    //                    LlenaSede(dsPuestos.Tables[0].Rows[0]["OficinaId"].ToString());

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
        clsblUtiles blU = new clsblUtiles();

        try
        {
            blObj.LlavePrimaria = "id";
            blObj.NombreTabla = "PuestosDeAtencion";
            blObj.Add("Nombre", tbNombre.Text);
            blObj.Add("Abreviatura", tbAbreviatura.Text);
            blObj.Add("OficinaId", ddlIdOficina.SelectedValue);
            blObj.Add("SalaId", ddlSede.SelectedValue);

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
//            List<string> Sentencias = new List<string>();
//            Sentencias.Add(blObj.strSQLExecuted);
//            blU.EncolarMensajesRabbit(Sentencias, "", true);

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
            blObj.NombreTabla = "PuestosDeAtencion";
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
            lbConfirmacion.Text = "¡Error eliminando el puesto de atención!" + ex.Message;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfPuestosAtencion.aspx");
    }
    protected void ddlSede_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedoffice = ddlIdOficina.SelectedValue;
        if (!string.IsNullOrEmpty(selectedoffice))
        {
            LlenaSede(selectedoffice);
        }
    }

    private void LlenaSede(string OfficeId)
    {
        clsblUtiles blU = new clsblUtiles();
        blU.LlenaDDLObligatorio(ddlSede, "Sala", "Id", "Descripcion", $"OficinaId = {OfficeId}", "", "Descripcion");
    }

}
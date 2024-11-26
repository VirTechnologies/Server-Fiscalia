using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Threading;
using System.Web.UI;

public partial class wfOficina : System.Web.UI.Page
{
    string officeid;
        public string idDepartamento ;
        DataSet dsSedes = new DataSet();
        clsblParametricas blPara = new clsblParametricas();
        clsblUtiles blU = new clsblUtiles();
    bool primera;
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        String msgError;

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
        if (!((objUsuario.PermisoModulo("45", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("45", Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (!objUsuario.PermisoModulo("45", Session["IDUSUARIO"].ToString(), "D"))
            btnEliminar.Visible = false;
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            blU.LlenaDDLObligatorio(Nombre, "Oficinas", "Id", "Nombre", "", "", "Nombre");
            blU.LlenaDDLObligatorio(DlDepartamento, "Departamentos", "Id", "Nombre", "", "", "Nombre");
            blU.LlenaDDLObligatorio(ddlTipos, "TipoSedes", "Id", "Nombre", "", "", "Nombre");
            blU.LlenaDDLObligatorio(dltematica, "Grupos", "Id", "Nombre", "", "", "Nombre");

            if (hfid.Value != "")
            {
                msgError = blPara.ConsultarSalas(ref dsSedes, hfid.Value, "");
                if (msgError == "")
                {
                    idDepartamento = dsSedes.Tables[0].Rows[0]["IdDperatamento"].ToString();
                    tbId.Text = dsSedes.Tables[0].Rows[0]["Id"].ToString();
                    tbNombre.Text = dsSedes.Tables[0].Rows[0]["Descripcion"].ToString();
                    if (dsSedes.Tables[0].Rows[0]["TematicaId"].ToString() != "")
                        dltematica.SelectedValue = dsSedes.Tables[0].Rows[0]["TematicaId"].ToString();

                    if (dsSedes.Tables[0].Rows[0]["OficinaId"].ToString() != "")
                        Nombre.SelectedValue = dsSedes.Tables[0].Rows[0]["OficinaId"].ToString();
                    Nombre.DataTextField = dsSedes.Tables[0].Rows[0]["Nombre"].ToString();
                    tbIdOficina.Text = dsSedes.Tables[0].Rows[0]["OficinaId"].ToString();
                    if (dsSedes.Tables[0].Rows[0]["iddep"].ToString() != "")
                        DlDepartamento.SelectedValue = dsSedes.Tables[0].Rows[0]["iddep"].ToString();
                    DlDepartamento.DataTextField = dsSedes.Tables[0].Rows[0]["Departamento"].ToString();
                    ddlTipos.SelectedValue = dsSedes.Tables[0].Rows[0]["TipoId"].ToString(); ;

                    if (!string.IsNullOrEmpty(idDepartamento))
                    {                       
                        primera = true;
                        LlenaMunicipios(idDepartamento);
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
        DataSet Oficina = null;
        String strAux = "";
        clsblUtiles blU = new clsblUtiles();

        try
        {
            blObj.LlavePrimaria = "Id";
            blObj.NombreTabla = "Sala";
            blObj.IsIdentity = true;
            blObj.Add("Descripcion", tbNombre.Text);
            blObj.Add("MunId", DlMunicipio.SelectedValue);
            blObj.Add("OficinaId", Nombre.SelectedValue);





            //blObj.Add("Nombre", tbNombre.Text);
            if (hfid.Value == "")
            {

                msgError = blObj.NssEjecutarSQL("INSERT", ref dsInterno, ref strAux, "", "", null, null);
                hfid.Value = strAux;
            }
            else
            {
                blObj.Add("Id", hfid.Value);
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
            lbConfirmacion.Text = "¡Error al grabar el registro!";
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
            blObj.NombreTabla = "Oficinas";
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
            lbConfirmacion.Text = "¡Error eliminando el Seccional!" + ex.Message;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfSalas.aspx");
    }

    protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        string tempo = blPara.ConsultarDepartamentos(ref dsSedes, DlDepartamento.SelectedValue, "");
        string selectedDepartamentoId = dsSedes.Tables[0].Rows[0]["Dept_Id_Dane"].ToString();

        if (!string.IsNullOrEmpty(selectedDepartamentoId))
        {
            LlenaMunicipios(selectedDepartamentoId);
        }
    }

    private void LlenaMunicipios(string departamentoId)
    {
        clsblUtiles blU = new clsblUtiles();
        blU.LlenaDDLObligatorio(DlMunicipio, "Municipios", "Id", "Nombre", $"Dept_Id_Dane = {departamentoId}", "", "Nombre");

            if (primera)
            {
              if (dsSedes.Tables[0].Rows[0]["idmn"].ToString() != "")
                DlMunicipio.SelectedValue = dsSedes.Tables[0].Rows[0]["idmn"].ToString();
               DlMunicipio.DataTextField = dsSedes.Tables[0].Rows[0]["Municipio"].ToString();
               primera = false;
            }
    }

    protected void ddlTematica_SelectedIndexChanged(object sender, EventArgs e)
    {
        blPara.ConsltaSeccionalesPortematica(ref dsSedes, dltematica.SelectedValue);

        Nombre.DataSource = dsSedes;
        Nombre.DataValueField = dsSedes.Tables[0].Columns["Id"].ToString();
        Nombre.DataTextField = dsSedes.Tables[0].Columns["Nombre"].ToString();
        Nombre.DataBind();
    }

}
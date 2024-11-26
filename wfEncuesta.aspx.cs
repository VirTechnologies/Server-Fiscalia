using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wfEncuesta : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DataSet dsEncuestas = new DataSet();
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
        if (!((objUsuario.PermisoModulo("60", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("60", Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;
            tbfecha_encuesta.ReadOnly = true;
            tbfecha_encuesta.Enabled = false;
            btnAgregarPregunta.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (!objUsuario.PermisoModulo("60", Session["IDUSUARIO"].ToString(), "D"))
            btnEliminar.Visible = false;
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            if (hfid.Value != "")
            {
                msgError = blPara.ConsultaEncuestas(ref dsEncuestas, hfid.Value, "");
                if (msgError == "")
                {
                    tbnombre_encuesta.Text = dsEncuestas.Tables[0].Rows[0]["nombre_encuesta"].ToString();
                    tbobjetivo.Text = dsEncuestas.Tables[0].Rows[0]["objetivo"].ToString();
                    tbfecha_encuesta.Text = dsEncuestas.Tables[0].Rows[0]["fecha_encuesta"].ToString();
                    tbfecha_registro.Text = dsEncuestas.Tables[0].Rows[0]["fecha_registro"].ToString();
                    if (dsEncuestas.Tables[0].Rows[0]["Activa"].ToString() == "True")
                        lblEstado.Text = "ACTIVA";
                    else
                        lblEstado.Text = "INACTIVA";

                    FiltrarPreguntas();
                    tablaPreguntas.Visible = true;
                }
            }
            else
            {
                btnEliminar.Visible = false;
                tablaPreguntas.Visible = false;
            }
        }
        if (hfConsulta.Value == "si")
        {
            btnGrabar.Visible = false;
            btnEliminar.Visible = false;
            objUsuario.SoloLectura(this);
            btnAgregarPregunta.Visible = false;
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
            blObj.NombreTabla = "Encuestas";
            blObj.Add("nombre_encuesta", tbnombre_encuesta.Text);
            blObj.Add("objetivo", tbobjetivo.Text);
            blObj.Add("fecha_encuesta", tbfecha_encuesta.Text);
            if (hfid.Value == "")
            {
                blObj.Add("Activa", "0");
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

            tablaPreguntas.Visible = true;
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
            blObj.NombreTabla = "Encuestas";
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
            lbConfirmacion.Text = "¡Error eliminando la encuesta!" + ex.Message;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    private void FiltrarPreguntas()
    {
        DataSet dsPreguntas = new DataSet();
        clsblParametricas blParam = new clsblParametricas();
        String msgError;

        msgError = blParam.ConsultaEncuestasPreguntas(ref dsPreguntas, "", "", hfid.Value);
        if (msgError == "")
        {
            gvPreguntas.DataSource = dsPreguntas;
            gvPreguntas.DataBind();
        }
        else
        {
            return;
        }
    }


    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfEncuestas.aspx");
    }

    protected void gvPreguntas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String idS = "";
            DataSet dsR = new DataSet();

            idS = e.Row.Cells[0].Text;
            LinkButton AdministrarButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 1].Controls[0];
            LinkButton queryButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 2].Controls[0];

            AdministrarButton.CommandName = "ADMINISTRAR";
            AdministrarButton.CommandArgument = idS;
            queryButton.CommandName = "CONSULTAR";
            queryButton.CommandArgument = idS;

            switch (e.Row.Cells[e.Row.Cells.Count - 4].Text)
            {
                case "0": e.Row.Cells[e.Row.Cells.Count - 4].Text = "Con respuesta única"; break;
                case "1": e.Row.Cells[e.Row.Cells.Count - 4].Text = "Con múltiples respuestas"; break;
            }

            if (!obj.PermisoModulo("60", Session["IDUSUARIO"].ToString(), "U") || hfConsulta.Value == "si")
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (!obj.PermisoModulo("60", Session["IDUSUARIO"].ToString(), "U") || hfConsulta.Value == "si")
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("60", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }

    protected void gvPreguntas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idS = "";

        if (e.CommandName != "")
        {
            idS = e.CommandArgument.ToString();
        }
        if (e.CommandName.ToString() == "ADMINISTRAR")
            Response.Redirect("wfEncuestaPregunta.aspx?id=" + idS + "&EncuestaId=" + hfid.Value);
        if (e.CommandName.ToString() == "CONSULTAR")
            Response.Redirect("wfEncuestaPregunta.aspx?consulta=si&id=" + idS + "&EncuestaId=" + hfid.Value);
    }

    protected void btnAgregarPregunta_Click(object sender, EventArgs e)
    {
        Response.Redirect("wfEncuestaPregunta.aspx?EncuestaId=" + hfid.Value);
    }
}

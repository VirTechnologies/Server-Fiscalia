using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Threading;

public partial class wfEncuestaPregunta : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DataSet dsEncuestasPreguntas = new DataSet();
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
            objUsuario.SoloLectura(this);
        }
        if (!objUsuario.PermisoModulo("60", Session["IDUSUARIO"].ToString(), "D"))
            btnEliminar.Visible = false;
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            hfEncuestaId.Value = blU.ValorObjetoString(Request.QueryString["EncuestaId"]);
            if (hfid.Value != "")
            {
                msgError = blPara.ConsultaEncuestasPreguntas(ref dsEncuestasPreguntas, hfid.Value, "", hfEncuestaId.Value);
                if (msgError == "")
                {
                    tbdescripcion.Text = dsEncuestasPreguntas.Tables[0].Rows[0]["descripcion"].ToString();
                    if (dsEncuestasPreguntas.Tables[0].Rows[0]["tipo_pregunta"].ToString() != "")
                        ddltipo_pregunta.SelectedValue = dsEncuestasPreguntas.Tables[0].Rows[0]["tipo_pregunta"].ToString();
                    FiltrarPreguntasOpciones();
                    tborden.Text = dsEncuestasPreguntas.Tables[0].Rows[0]["orden"].ToString();
                    tablaOpciones.Visible = true;
                }
            }
            else
            {
                btnEliminar.Visible = false;
                tablaOpciones.Visible = false;
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
        NSSSqlUtil blObj = new NSSSqlUtil();
        String msgError = "";
        DataSet dsInterno = null;
        String strAux = "";
        clsblUtiles blU = new clsblUtiles();

        try
        {
            blObj.LlavePrimaria = "id";
            blObj.NombreTabla = "EncuestasPreguntas";
            blObj.Add("descripcion", tbdescripcion.Text);
            blObj.Add("tipo_pregunta", ddltipo_pregunta.SelectedValue);
            blObj.Add("orden", tborden.Text);
            blObj.Add("EncuestaId", hfEncuestaId.Value);
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

            tablaOpciones.Visible = true;
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
            blObj.NombreTabla = "EncuestasPreguntas";
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

    private void FiltrarPreguntasOpciones()
    {
        DataSet dsPreguntasOpciones = new DataSet();
        clsblParametricas blParam = new clsblParametricas();
        String msgError;

        msgError = blParam.ConsultaEncuestasPreguntasOpciones(ref dsPreguntasOpciones, "", "",hfid.Value);
        if (msgError == "")
        {
            gvPreguntasOpciones.DataSource = dsPreguntasOpciones;
            gvPreguntasOpciones.DataBind();
        }
        else
        {
            return;
        }
    }


    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfEncuesta.aspx?id="+hfEncuestaId.Value);
    }

    protected void gvPreguntasOpciones_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void gvPreguntasOpciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idS = "";

        if (e.CommandName != "")
        {
            idS = e.CommandArgument.ToString();
        }
        if (e.CommandName.ToString() == "ADMINISTRAR")
            Response.Redirect("wfEncuestaPreguntaOpcion.aspx?id=" + idS + "&PreguntaId="+hfid.Value + "&EncuestaId=" + hfEncuestaId.Value);
        if (e.CommandName.ToString() == "CONSULTAR")
            Response.Redirect("wfEncuestaPreguntaOpcion.aspx?consulta=si&id=" + idS + "&PreguntaId=" + hfid.Value+ "&EncuestaId=" + hfEncuestaId.Value);
    }

    protected void btnAgregarOpcion_Click(object sender, EventArgs e)
    {
        Response.Redirect("wfEncuestaPreguntaOpcion.aspx?EncuestaId=" + hfEncuestaId.Value + "&PreguntaId=" + hfid.Value);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Threading;

public partial class wfGrupo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        DataSet dsTematicas = new DataSet();
        clsblParametricas blPara = new clsblParametricas();
        string idModulo = "66";

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
        if (!((objUsuario.PermisoModulo(idModulo, Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo(idModulo, Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (!objUsuario.PermisoModulo(idModulo, Session["IDUSUARIO"].ToString(), "D"))
            btnEliminar.Visible = false;
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);
            blU.LlenaDDLObligatorio(ddlIdGrupoPadre, "Grupos", "Id", "Nombre","","", "Nombre");
            if (hfid.Value != "")
            {
                msgError = blPara.ConsultaGrupos(ref dsTematicas, hfid.Value, "");
                if (msgError == "")
                {
                    tbNombre.Text = dsTematicas.Tables[0].Rows[0]["Nombre"].ToString();
                    tbAbreviatura.Text = dsTematicas.Tables[0].Rows[0]["Abreviatura"].ToString();
                    tbdescripcion.Text = dsTematicas.Tables[0].Rows[0]["Descripcion"].ToString();
                    if (dsTematicas.Tables[0].Rows[0]["GrupoPadreId"].ToString()!="")
                      ddlIdGrupoPadre.SelectedValue = dsTematicas.Tables[0].Rows[0]["GrupoPadreId"].ToString();
                    if (dsTematicas.Tables[0].Rows[0]["Habilitado"].ToString() == "True")
                        cbhabilitado.Checked = true;
                    else
                        cbhabilitado.Checked = false;

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
            blObj.NombreTabla = "Grupos";
            blObj.IsIdentity = true;
            blObj.Add("Nombre", tbNombre.Text);
            blObj.Add("Abreviatura", tbAbreviatura.Text);
            blObj.Add("Descripcion", tbdescripcion.Text);
            blObj.Add("GrupoPadreId", ddlIdGrupoPadre.SelectedValue);
            if (cbhabilitado.Checked)
            {

                blObj.Add("Habilitado", "1");
            }
            else
            {
                blObj.Add("Habilitado", "0");
            }

            if (hfid.Value == "")
            {
                msgError = blObj.NssEjecutarSQL("INSERT", ref dsInterno, ref strAux, "", "", null, null);
                hfid.Value = strAux;
                lbConfirmacion.Text = "¡Registro grabado correctamente!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
                notificacion.Visible = true;
            }
            else
            {
                if (ddlIdGrupoPadre.SelectedValue != hfid.Value)
                {
                    blObj.Add("id", hfid.Value);
                    msgError = blObj.NssEjecutarSQL("UPDATE", ref dsInterno, ref strAux, "", "", null, null);
                    lbConfirmacion.Text = "¡Registro grabado correctamente!";
                    notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
                    notificacion.Visible = true;
                }
                else
                {
                    lbConfirmacion.Text = "El Grupo no puede ser su propio padre!";
                    notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                    notificacion.Visible = true;
                }

            }
            //List<string> Sentencias = new List<string>();
            //Sentencias.Add(blObj.strSQLExecuted);
            //blU.EncolarMensajesRabbit(Sentencias, "", true);

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
        string msg = "";
        string msgUsuario = "";
        DataSet dsInterno = null;
        String strAux = "";
        clsblUtiles blU = new clsblUtiles();
        clsblParametricas blPala = new clsblParametricas();
        int borrados = 0;

        try
        {
            blPala.EliminarGrupoComoPadre(ref dsInterno, hfid.Value, ref borrados);
            if (borrados > 0)
                msgUsuario += "Eliminación del grupo como padre realizada!" + Environment.NewLine;
        }
        catch (Exception ex)
        {
            lbConfirmacion.Text = "¡Error al eliminar el grupo como padre!\n" + ex.ToString();
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
            return;
        }

        try
        {
            blPala.BorrarOficinasDelGrupo(ref dsInterno, hfid.Value, ref borrados);
            if (borrados > 0)
                msgUsuario += "Desasignación de puntos de atención al grupo realizada!" + Environment.NewLine;
        }
        catch (Exception ex)
        {
            lbConfirmacion.Text = "¡Error al eliminar la asignación de puntos de atención!" + Environment.NewLine + ex.ToString();
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
            return;
        }

        try
        {
            blObj.LlavePrimaria = "id";
            blObj.NombreTabla = "Grupos";
            blObj.Add("id", hfid.Value);
            msg = blObj.NssEjecutarSQL("DELETE", ref dsInterno, ref strAux, "", "", null, null);
            //List<string> Sentencias = new List<string>();
            //Sentencias.Add(blObj.strSQLExecuted);
            //blU.EncolarMensajesRabbit(Sentencias, "", true);

            msgUsuario += "¡Registro borrado correctamente!\n";
            lbConfirmacion.Text = msgUsuario;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
            notificacion.Visible = true;
            btnGrabar.Visible = false;
            btnEliminar.Visible = false;
        }
        catch (Exception ex)
        {
            lbConfirmacion.Text = msgUsuario + "¡Error eliminando el grupo!" + ex.Message;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfGrupos.aspx");
    }
}
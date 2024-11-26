using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Threading;

public partial class wfServiciosPerfil : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
        myDTF.ShortDatePattern = "dd/MM/yyyy";
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;
        if (Session["IDUSUARIO"].ToString() == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        if (!(objUsuario.PermisoModulo("51", blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
            btnGrabar.Visible = false;
        if (!Page.IsPostBack)
        {
            blU.LlenaDDLObligatorio(ddlIdPerfil, "PerfilesDeAtencion", "Id", "Nombre", "", "", "Nombre");
        }
    }


    protected void ddlIdPerfil_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlIdPerfil.SelectedValue!="")
          FiltrarServicios();
        lbConfirmacion.Text = "";
        notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
    }

    protected void FiltrarServicios()
    {
        DataSet dsServicios = new DataSet();
        clsblParametricas blParam = new clsblParametricas();
        String msgError;
        String strSQL = "";

        msgError = blParam.ConsultaServicios(ref dsServicios, "", "");
        if (msgError == "")
        {
            gvServicios.DataSource = dsServicios;
            gvServicios.DataBind();

            if (gvServicios.Rows.Count <= 0)
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
            }
            else
            {
                tbNoRegistros.Text = gvServicios.Rows.Count.ToString();
                lblNoRegistros.Visible = tbNoRegistros.Visible = true;
                lblSinRegistros.Visible = false;
            }

            txSQL.Text = strSQL;
            gvServicios.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        else
        {
            txSQL.Text = "";
            return;
        }
    }

    protected void gvServicios_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataSet dsServPerfil = new DataSet();
        clsblUsuarios obj = new clsblUsuarios();
        clsblParametricas blParam = new clsblParametricas();
        string msgError = "";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String idS = "";
            DataSet dsR = new DataSet();

            idS = e.Row.Cells[0].Text;

            if (!obj.PermisoModulo("51", Session["IDUSUARIO"].ToString(), "U"))
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            e.Row.Cells[0].Visible = false;
            msgError = blParam.ConsultaServiciosPerfilDeAtencion(ref dsServPerfil, idS, ddlIdPerfil.SelectedValue);
            if (msgError=="")
            {
                CheckBox check = (CheckBox)e.Row.Cells[e.Row.Cells.Count - 1].FindControl("CheckBox_sel");

                if (dsServPerfil.Tables[0].Rows.Count > 0)
                    check.Checked = true;
                else
                    check.Checked = false;
            }
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("51", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }


    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        OleDbConnection Conexion = null;
        OleDbTransaction myTrans = null;
        clsblUtiles blUtiles = new clsblUtiles();
        clsblParametricas blParam = new clsblParametricas();
        NSSSqlUtil objNSS = null;
        string msg = "";
        int i;
        DataSet dsInterno = null;

        if (ddlIdPerfil.SelectedValue!="")
        {
            Conexion = new OleDbConnection();
            Conexion.ConnectionString = blUtiles.cadenaConexion();
            Conexion.Open();
            myTrans = Conexion.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                msg = blParam.BorraServiciosPerfilDeAtencion(ddlIdPerfil.SelectedValue, Conexion, myTrans);
                if (msg == "")
                {
                    for (i = 0; i < gvServicios.Rows.Count; i++)
                    {
                        CheckBox cb = (CheckBox)gvServicios.Rows[i].Cells[gvServicios.Rows[i].Cells.Count - 1].FindControl("CheckBox_sel");

                        if (cb.Checked)
                        {
                            string id = gvServicios.Rows[i].Cells[0].Text;
                            objNSS = new NSSSqlUtil();
                            objNSS.LlavePrimaria = "id";
                            objNSS.NombreTabla = "ServiciosEnPerfil";
                            objNSS.Add("ServicioId", id);
                            objNSS.Add("PerfilDeAtencionId", ddlIdPerfil.SelectedValue);
                            msg = objNSS.NssEjecutarSQL("INSERT", ref dsInterno, ref msg, "", "", Conexion, myTrans);
                            objNSS = null;
                        }
                    }
                }
                myTrans.Commit();
                lbConfirmacion.Text = "¡Servicios grabados corectamente!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
                notificacion.Visible = true;
            }
            catch (Exception ex)
            {
                myTrans.Rollback();
                lbConfirmacion.Text = "¡Error grabando servicios!" + ex.Message;
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacion.Visible = true;
            }
            finally
            {
                Conexion.Close();
            }
        }
    }
}
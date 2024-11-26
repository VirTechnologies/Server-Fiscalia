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

public partial class wfServiciosOficina : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
        myDTF.ShortDatePattern = "dd/MM/yyyy";
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        if (!(objUsuario.PermisoModulo("52", blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
            btnGrabar.Visible = false;
        if (!Page.IsPostBack)
        {
            blU.LlenaDDLObligatorio(ddlIdOficina, "Oficinas", "Id", "Nombre", "", "", "Nombre");
        }
    }

    protected void ddlIdOficina_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlIdOficina.SelectedValue!="")
        {
            FiltrarServicios();
            FiltrarPerfiles();
        }
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


            if (!obj.PermisoModulo("52", Session["IDUSUARIO"].ToString(), "U"))
                e.Row.Cells[e.Row.Cells.Count - 2].Visible = false;
            e.Row.Cells[0].Visible = false;
            msgError = blParam.ConsultaServiciosOficina(ref dsServPerfil, "", e.Row.Cells[0].Text, ddlIdOficina.SelectedValue, null, null);
            if (msgError == "")
            {
                CheckBox check = (CheckBox)e.Row.Cells[e.Row.Cells.Count - 1].FindControl("CheckBox_sel");

                if (dsServPerfil.Tables[0].Rows.Count > 0)
                {
                    idS = dsServPerfil.Tables[0].Rows[0]["Id"].ToString();
                    check.Checked = true;
                }
                else
                {
                    check.Checked = false;
                }
                if (!obj.PermisoModulo("52", Session["IDUSUARIO"].ToString(), "U"))
                    check.Enabled = false;
            }
            LinkButton AdministrarButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 2].Controls[0];
            AdministrarButton.CommandName = "ADMINISTRAR";
            AdministrarButton.CommandArgument = idS+";"+ e.Row.Cells[0].Text;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("52", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }


    protected void gvServicios_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idS = "";
        idS = e.CommandArgument.ToString();
        string[] idSList = idS.Split(';');
        Session["OficinaId"] = ddlIdOficina.SelectedValue;
        if (e.CommandName.ToString() == "ADMINISTRAR")
            Response.Redirect("wfServicioOficina.aspx?id=" + idSList[0]+"&ServicioID="+ idSList[1]+"&OficinaID="+ddlIdOficina.SelectedValue);
        if (e.CommandName.ToString() == "CONSULTAR")
            Response.Redirect("wfServicioOficina.aspx?consulta=si&id=" + idSList[0] + "&ServicioID=" + idSList[1] + "&OficinaID=" + ddlIdOficina.SelectedValue);
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
        DataSet dsServiciosOficina = new DataSet();

        if (ddlIdOficina.SelectedValue != "")
        {
            Conexion = new OleDbConnection();
            Conexion.ConnectionString = blUtiles.cadenaConexion();
            Conexion.Open();
            myTrans = Conexion.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                if (msg == "")
                {
                    for (i = 0; i < gvServicios.Rows.Count; i++)
                    {
                        string ServicioId = "", OficinaId = "";
                        bool ExisteServicio = false;
                        CheckBox cb = (CheckBox)gvServicios.Rows[i].Cells[gvServicios.Rows[i].Cells.Count - 1].FindControl("CheckBox_sel");

                        OficinaId = ddlIdOficina.SelectedValue;
                        ServicioId = gvServicios.Rows[i].Cells[0].Text;
                        msg = blParam.ConsultaServiciosOficina(ref dsServiciosOficina,"",ServicioId, OficinaId, Conexion, myTrans);
                        if (dsServiciosOficina.Tables[0].Rows.Count>0)
                        {
                            ExisteServicio = true;
                        }
                        if (cb.Checked)
                        {
                            if (!ExisteServicio)
                            {
                                string id = gvServicios.Rows[i].Cells[0].Text;
                                objNSS = new NSSSqlUtil();
                                objNSS.LlavePrimaria = "id";
                                objNSS.NombreTabla = "ServiciosEnOficina";
                                objNSS.Add("ServicioId", id);
                                objNSS.Add("OficinaId", ddlIdOficina.SelectedValue);
                                msg = objNSS.NssEjecutarSQL("INSERT", ref dsInterno, ref msg, "", "", Conexion, myTrans);
                                objNSS = null;
                            }
                        }else
                        {
                            if (ExisteServicio)
                            {
                                msg = blParam.BorraServicioEnOficina(OficinaId, ServicioId,Conexion, myTrans);
                            }
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

    protected void FiltrarPerfiles()
    {
        DataSet dsPerfiles = new DataSet();
        clsblParametricas blParam = new clsblParametricas();
        String msgError;
        String strSQL = "";

        msgError = blParam.ConsultaPerfilesAtencion(ref dsPerfiles, "", "");
        if (msgError == "")
        {
            gvPerfiles.DataSource = dsPerfiles;
            gvPerfiles.DataBind();

            if (gvPerfiles.Rows.Count <= 0)
            {
                lblNoRegistrosP.Visible = tbNoRegistrosP.Visible = false;
                lblSinRegistrosP.Visible = true;
            }
            else
            {
                tbNoRegistrosP.Text = gvPerfiles.Rows.Count.ToString();
                lblNoRegistrosP.Visible = tbNoRegistrosP.Visible = true;
                lblSinRegistrosP.Visible = false;
            }

            txSQL.Text = strSQL;
            gvPerfiles.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        else
        {
            txSQL.Text = "";
            return;
        }
    }

    protected void gvPerfiles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataSet dsOfiPerfil = new DataSet();
        clsblUsuarios obj = new clsblUsuarios();
        clsblParametricas blParam = new clsblParametricas();
        string msgError = "";

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String idS = "";
            DataSet dsR = new DataSet();


            if (!obj.PermisoModulo("52", Session["IDUSUARIO"].ToString(), "U"))
                e.Row.Cells[e.Row.Cells.Count - 2].Visible = false;
            e.Row.Cells[0].Visible = false;
            msgError = blParam.ConsultaPerfilesOficina(ref dsOfiPerfil, "", e.Row.Cells[0].Text, ddlIdOficina.SelectedValue, null, null);
            if (msgError == "")
            {
                CheckBox check = (CheckBox)e.Row.Cells[e.Row.Cells.Count - 1].FindControl("CheckBox_selP");

                if (dsOfiPerfil.Tables[0].Rows.Count > 0)
                {
                    idS = dsOfiPerfil.Tables[0].Rows[0]["Id"].ToString();
                    check.Checked = true;
                }
                else
                {
                    check.Checked = false;
                }
                if (!obj.PermisoModulo("52", Session["IDUSUARIO"].ToString(), "U"))
                    check.Enabled = false;
            }
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("52", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }

    protected void btnGrabarPerfiles_Click(object sender, EventArgs e)
    {
        OleDbConnection Conexion = null;
        OleDbTransaction myTrans = null;
        clsblUtiles blUtiles = new clsblUtiles();
        clsblParametricas blParam = new clsblParametricas();
        NSSSqlUtil objNSS = null;
        string msg = "";
        int i;
        DataSet dsInterno = null;
        DataSet dsPerfilesOficina = new DataSet();

        if (ddlIdOficina.SelectedValue != "")
        {
            Conexion = new OleDbConnection();
            Conexion.ConnectionString = blUtiles.cadenaConexion();
            Conexion.Open();
            myTrans = Conexion.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                msg = blParam.BorraPerfilesEnOficina(ddlIdOficina.SelectedValue, Conexion, myTrans);
                for (i = 0; i < gvPerfiles.Rows.Count; i++)
                {
                    CheckBox cb = (CheckBox)gvPerfiles.Rows[i].Cells[gvPerfiles.Rows[i].Cells.Count - 1].FindControl("CheckBox_selP");

                    if (cb.Checked)
                    {
                        string id = gvPerfiles.Rows[i].Cells[0].Text;
                        objNSS = new NSSSqlUtil();
                        objNSS.LlavePrimaria = "id";
                        objNSS.NombreTabla = "PerfilesEnOficina";
                        objNSS.Add("PerfilDeAtencionId", id);
                        objNSS.Add("OficinaId", ddlIdOficina.SelectedValue);
                        msg = objNSS.NssEjecutarSQL("INSERT", ref dsInterno, ref msg, "", "", Conexion, myTrans);
                        objNSS = null;
                    }
                }
                myTrans.Commit();
                lbConfirmacionP.Text = "¡Perfiles grabados corectamente!";
                notificacionP.Attributes["Class"] = "alert alert-dismissible alert-success";
                notificacionP.Visible = true;
            }
            catch (Exception ex)
            {
                myTrans.Rollback();
                lbConfirmacionP.Text = "¡Error grabando perfiles!" + ex.Message;
                notificacionP.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacionP.Visible = true;
            }
            finally
            {
                Conexion.Close();
            }
        }
    }

}
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wfPrioridadesDeAsesor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        if (!(objUsuario.PermisoModulo("56", blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
            btnGrabar.Visible = false;
        //if (txSQL.Text != "")
            Filtrar();
        if (!Page.IsPostBack)
        {
            blU.LlenaDDLObligatorio(ddlOficina, "Oficinas", "Id", "Nombre", "", "", "Nombre");
            blU.LlenaDDLObligatorio(ddlUsuarioId, "Usuarios", "Id", "Nombre", "", "", "Nombre");
           // blU.LlenaDDLObligatorio(ddlSalas, "Sala", "Id", "Descripcion", "", "", "Descripcion");
        }
        if (blU.ValorObjetoString(Session["Volver"]) == "S")
        {
            if (Session["txSQL"].ToString() != "")
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["OficinaId"] = "";
        }
        if (txSQL.Text != "")
            Filtrar();
    }

    private void Filtrar()
    {
        DataSet dsPrioridades = new DataSet();
        clsblParametricas blParam = new clsblParametricas();
        String msgError;
        notificacion.Visible = false;

        msgError = blParam.ConsultaPrioridadesHabilitadas(ref dsPrioridades, "", ddlOficina.SelectedValue, ddlSalas.SelectedValue);
        if (msgError == "")
        {
            gvPrioridades.DataSource = dsPrioridades;
            gvPrioridades.DataBind();

            if (gvPrioridades.Rows.Count <= 0)
            {
                btnGrabar.Visible = false;
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
            }
            else
            {
                tbNoRegistros.Text = gvPrioridades.Rows.Count.ToString();
                btnGrabar.Visible = true;
                lblNoRegistros.Visible = tbNoRegistros.Visible = true;
                lblSinRegistros.Visible = false;
            }
            txSQL.Text = gvPrioridades.Rows.Count.ToString();
        }
        else
        {
            txSQL.Text = "";
            return;
        }
    }


    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        Filtrar();
    }

    protected void gvPrioridades_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        clsblParametricas blPala = new clsblParametricas();

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String idS, IdTemp = "";
            DataSet dsR = new DataSet();
            CheckBox cb = new CheckBox();

            idS = e.Row.Cells[0].Text;
            cb.ID = "cb" + idS;
            if (blPala.ConsultaPrioridadesUsuario(ref IdTemp, idS, ddlUsuarioId.SelectedValue))
                cb.Checked = true;
            else
                cb.Checked = false;
            e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(cb);
            if (!obj.PermisoModulo("56", Session["IDUSUARIO"].ToString(), "U"))
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("56", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }

    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        NSSSqlUtil blObj = new NSSSqlUtil();
        String msgError = "";
        DataSet dsInterno = null;
        String strAux = "";
        clsblParametricas blPala = new clsblParametricas();

        //clsblUtiles blU = new clsblUtiles();
        //List<string> Sentencias = new List<string>();
        ////Sentencias.Add("delete from Usuarios where Id in(select Id from Usuarios where EMAIL IN('juangudelo@comfandi.com.co'))");
        //Sentencias.Add("alter table PrioridadDeAsesor add Habilitado VARCHAR (1) NULL");
        //blU.EncolarMensajesRabbit(Sentencias, "", true);
        //lbConfirmacion.CssClass = "text-success";
        //lbConfirmacion.Text = "¡Registro encolado correctamente!";


        try
        {
            int i;
            for (i = 0; i < gvPrioridades.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)(gvPrioridades.Rows[i].Cells[gvPrioridades.Rows[i].Cells.Count - 1].Controls[0]);
                string Ids = cb.ID.Substring(2);
                string Id = "";
                bool ExistePrioridad = blPala.ExistePrioridadesUsuario(ref Id, Ids, ddlUsuarioId.SelectedValue);

                blObj = new NSSSqlUtil();
                blObj.LlavePrimaria = "id";
                blObj.NombreTabla = "PrioridadDeAsesor";
                blObj.Add("PrioridadId", Ids);
                blObj.Add("UsuarioId", ddlUsuarioId.SelectedValue);
                blObj.Add("OficinaId", Ids);
                blObj.Add("ConteoActual", "0");
                blObj.Add("SalaId", ddlSalas.SelectedValue);

                if (cb.Checked)
                {
                    if (!ExistePrioridad)
                    {
                        blObj.Add("Habilitado", "S");
                        msgError = blObj.NssEjecutarSQL("INSERT", ref dsInterno, ref strAux, "", "", null, null);
                        clsblUtiles blU = new clsblUtiles();
                        List<string> Sentencias = new List<string>();
                        Sentencias.Add(blObj.strSQLExecuted);
                        blU.EncolarMensajesRabbit(Sentencias, "", true);
                    }
                    else
                    {
                        blObj.Add("Habilitado", "S");
                        blObj.Add("id", Id);
                        msgError = blObj.NssEjecutarSQL("UPDATE", ref dsInterno, ref strAux, "", "", null, null);
                        clsblUtiles blU = new clsblUtiles();
                        List<string> Sentencias = new List<string>();
                        Sentencias.Add(blObj.strSQLExecuted);
                        blU.EncolarMensajesRabbit(Sentencias, "", true);
                    }
                }
                else
                {
                    if (ExistePrioridad)
                    {
                        blObj.Add("Habilitado", "N");
                        blObj.Add("id", Id);
                        msgError = blObj.NssEjecutarSQL("UPDATE", ref dsInterno, ref strAux, "", "", null, null);
                        clsblUtiles blU = new clsblUtiles();
                        List<string> Sentencias = new List<string>();
                        Sentencias.Add(blObj.strSQLExecuted);
                        blU.EncolarMensajesRabbit(Sentencias, "", true);
                    }
                }

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

    protected void ddlOficinaId_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOficina.SelectedIndex > 0)
        {
            clsblUtiles blU = new clsblUtiles();
            blU.LlenaDDLObligatorio(ddlSalas, "Sala", "Id", "Descripcion", $"OficinaId = {ddlOficina.SelectedValue}", "", "Descripcion");
        }
        else
        {
            ddlSalas.Items.Clear();
        }
    }
}
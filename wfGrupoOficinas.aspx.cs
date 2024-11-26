using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wfGrupoOficinas : System.Web.UI.Page
{
    public string idModulo = "66";

    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        if (!(objUsuario.PermisoModulo(idModulo, blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
            btnGrabar.Visible = false;
        //if (txSQL.Text != "")
            Filtrar();
        if (!Page.IsPostBack)
        {
            blU.LlenaDDLObligatorio(ddlGrupoId, "Grupos", "Id", "Nombre", "", "", "Id");
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
        DataSet dsOficinasDisponibles = new DataSet();
        clsblParametricas blParam = new clsblParametricas();
        String msgError;
        notificacion.Visible = false;

        msgError = blParam.ConsultaOficinasGrupo(ref dsOficinasDisponibles, ddlGrupoId.SelectedValue, "");
        if (msgError == "")
        {
            gvGrupoOficinas.DataSource = dsOficinasDisponibles;
            gvGrupoOficinas.DataBind();
            if (gvGrupoOficinas.Rows.Count <= 0)
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
                lbConfirmacion.Text = "¡No se han creado puntos de atención!\n";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-warning";
                notificacion.Visible = true;

                btnGrabar.Visible = false;
            }
            else
            {
                gvGrupoOficinas.Visible = true;
                tbNoRegistros.Text = gvGrupoOficinas.Rows.Count.ToString();
                lblNoRegistros.Visible = tbNoRegistros.Visible = true;
                lblSinRegistros.Visible = false;
                //btnGrabar.Visible = true;
            }
            txSQL.Text = gvGrupoOficinas.Rows.Count.ToString();
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

    protected void gvOficinas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        clsblParametricas blPala = new clsblParametricas();
        bool PermisoEdicion = false;

        PermisoEdicion = obj.PermisoModulo(idModulo, Session["IDUSUARIO"].ToString(), "U");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string idGrupo = "", idOficina = "", IdTemp = "";
            DataSet dsR = new DataSet();
            CheckBox cb = new CheckBox();

            idGrupo = e.Row.Cells[0].Text;
            idOficina = e.Row.Cells[1].Text;

            cb.ID = "cb"+idOficina;
            if ((idGrupo == "") || (idGrupo == "&nbsp;"))
            {
                cb.Checked = false;
            }
            else
            {
                if (idGrupo == ddlGrupoId.SelectedValue)
                {
                    cb.Checked = true;
                }
            }
            e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(cb);

            if (!PermisoEdicion)
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!PermisoEdicion)
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Enabled = false;
        }
        else
        {
            btnGrabar.Visible = true;
        }
    }

    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        NSSSqlUtil blObj = new NSSSqlUtil();
        String msgError = "";
        DataSet dsInterno = null;
        String strAux = "";
        int i = 0;
        int borrados = 0;
        clsblParametricas blPala = new clsblParametricas();

        try
        {
            blPala.BorrarOficinasDelGrupo(ref dsInterno, ddlGrupoId.SelectedValue, ref borrados);
        }
        catch (Exception ex)
        {
            lbConfirmacion.Text = "¡Error al eliminar la asignación de puntos de atención!\n" + ex.ToString();
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
            return;
        }

        try
        {
            int cantChecked = 0;
            foreach (GridViewRow r in gvGrupoOficinas.Rows)
            {
                CheckBox chk = (CheckBox)r.Cells[gvGrupoOficinas.Rows[i].Cells.Count - 1].Controls[0];
                if (chk != null && chk.Checked)
                    cantChecked++;
            }

            if (cantChecked == 0)
            {
                if (borrados == 0)
                {
                    lbConfirmacion.Text = "¡No hay información para actualizar!";
                    notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                }
                else
                {
                    lbConfirmacion.Text = "¡Información actualizada correctamente!";
                    notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
                }
                notificacion.Visible = true;
            }
            else
            {
                for (i = 0; i < gvGrupoOficinas.Rows.Count; i++)
                {
                    CheckBox cb = (CheckBox)gvGrupoOficinas.Rows[i].Cells[gvGrupoOficinas.Rows[i].Cells.Count - 1].Controls[0];
                    string Ids = cb.ID.Substring(2);

                    blObj = new NSSSqlUtil();
                    blObj.IsIdentity = false;
                    blObj.LlavePrimaria = "Id_grupo";
                    blObj.NombreTabla = "Grupo_oficinas";
                    blObj.Add("Id_grupo", ddlGrupoId.SelectedValue);
                    blObj.Add("OficinaId", Ids);
                    if (cb.Checked)
                    {
                        msgError = blObj.NssEjecutarSQL("INSERT", ref dsInterno, ref strAux, "", "", null, null);
                    }
                }
                lbConfirmacion.Text = "¡Registros grabados correctamente!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
                notificacion.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lbConfirmacion.Text = "¡Error al grabar el registro!" + ex.ToString();
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void ddlGrupoId_SelectedIndexChanged(object sender, EventArgs e)
    {
        Filtrar();
        //txSQL.Text = "";
        ////lblSinRegistros.Text = "No se encontraron registros";
        //btnGrabar.Visible = gvGrupoOficinas.Visible = false;
    }
}
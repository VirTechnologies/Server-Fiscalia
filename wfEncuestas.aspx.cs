using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wfEncuestas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        //if (!(objUsuario.PermisoModulo("60", blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
        //    btnAgregar.Visible = false;
        //if (txSQL.Text != "")
            Filtrar();
        if (blU.ValorObjetoString(Session["Volver"]) == "S")
        {
            if (Session["tbnombre_encuesta"] != null)
                tbnombre_encuesta.Text = Session["tbnombre_encuesta"].ToString();
            if (!string.IsNullOrEmpty(Session["txSQL"].ToString()))
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["tbnombre_encuesta"] = "";
        }
    }

    private void Filtrar()
    {
        DataSet dsEncuestas = new DataSet();
        clsblParametricas blParam = new clsblParametricas();
        String msgError;
        String strSQL = "";

        msgError = blParam.ConsultaEncuestas(ref dsEncuestas, "", tbnombre_encuesta.Text);
        if (msgError == "")
        {
            gvEncuestas.DataSource = dsEncuestas;
            gvEncuestas.DataBind();

            if (gvEncuestas.Rows.Count <= 0)
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
            }
            else
            {
                tbNoRegistros.Text = gvEncuestas.Rows.Count.ToString();
                lblNoRegistros.Visible = tbNoRegistros.Visible = true;
                lblSinRegistros.Visible = false;
            }

            txSQL.Text = strSQL;
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

    protected void gvEncuestas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String idS = "";
            DataSet dsR = new DataSet();

            idS = e.Row.Cells[0].Text;
            LinkButton AdministrarButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 1].Controls[0];
            LinkButton queryButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 2].Controls[0];
            LinkButton btnVerRespuestas = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 3].Controls[0];
            if (e.Row.Cells[e.Row.Cells.Count - 5].Text == "True")
                e.Row.Cells[e.Row.Cells.Count - 5].Text = "ACTIVA";
            else
                e.Row.Cells[e.Row.Cells.Count - 5].Text = "INACTIVA";

            if (e.Row.Cells[e.Row.Cells.Count - 4].Text != "0")
            {
                btnVerRespuestas.CommandName = "RESPUESTAS";
                btnVerRespuestas.CommandArgument = idS + "|" + e.Row.Cells[e.Row.Cells.Count - 7].Text;
            }
            else
            {
                btnVerRespuestas.Visible = false;
                e.Row.Cells[e.Row.Cells.Count - 3].Text = "N/A";
            }

            AdministrarButton.CommandName = "ADMINISTRAR";
            AdministrarButton.CommandArgument = idS;
            queryButton.CommandName = "CONSULTAR";
            queryButton.CommandArgument = idS;

            if (!obj.PermisoModulo("60", Session["IDUSUARIO"].ToString(), "U"))
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("60", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }

    protected void gvEncuestas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idS = "";

        if (e.CommandName != "")
        {
            if (e.CommandName == "RESPUESTAS")
            {
                int posPalito = e.CommandArgument.ToString().IndexOf("|");
                idS = e.CommandArgument.ToString().Substring(0, posPalito);
                Session["tbnombre_encuesta"] = e.CommandArgument.ToString().Substring(posPalito+1);
                Response.Redirect("wfEncuestaRespuestas.aspx?id=" + idS);
            }
            else
            {
                idS = e.CommandArgument.ToString();
            }
            Session["txSQL"] = txSQL.Text;

        }
        if (e.CommandName == "ADMINISTRAR")
            Response.Redirect("wfEncuesta.aspx?id=" + idS);
        if (e.CommandName == "CONSULTAR")
            Response.Redirect("wfEncuesta.aspx?consulta=si&id=" + idS);
        //if (e.CommandName.ToString() == "RESPUESTAS")
        //    Response.Redirect("wfEncuestaRespuestas.aspx?id=" + idS);
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        Session["tbnombre_encuesta"] = tbnombre_encuesta.Text;
        Session["txSQL"] = txSQL.Text;
        Response.Redirect("wfEncuesta.aspx");
    }
}
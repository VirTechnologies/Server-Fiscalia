﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wfParametros : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        if (!(objUsuario.PermisoModulo("59", blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
            btnAgregar.Visible = false;
        //if (txSQL.Text != "")
            Filtrar();
        if (blU.ValorObjetoString(Session["Volver"]) == "S")
        {
            if (Session["txSQL"].ToString() != "")
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["Parametro"] = "";
        }
    }

    private void Filtrar()
    {
        DataSet dsParametros = new DataSet();
        clsblParametricas blParam = new clsblParametricas();
        String msgError;
        String strSQL = "";

        msgError = blParam.ConsultaParametrosGenerales(ref dsParametros, "", tbNombre.Text);
        if (msgError == "")
        {
            gvParametros.DataSource = dsParametros;
            gvParametros.DataBind();

            if (gvParametros.Rows.Count <= 0)
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
            }
            else
            {
                tbNoRegistros.Text = gvParametros.Rows.Count.ToString();
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

    protected void gvParametros_RowDataBound(object sender, GridViewRowEventArgs e)
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
            if (!obj.PermisoModulo("59", Session["IDUSUARIO"].ToString(), "U"))
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("59", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }

    protected void gvParametros_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idS = "";

        if (e.CommandName != "")
        {
            idS = e.CommandArgument.ToString();
            Session["Nombre"] = tbNombre.Text;
            Session["txSQL"] = txSQL.Text;
        }
        if (e.CommandName.ToString() == "ADMINISTRAR")
            Response.Redirect("wfParametro.aspx?id=" + idS);
        if (e.CommandName.ToString() == "CONSULTAR")
            Response.Redirect("wfParametro.aspx?consulta=si&id=" + idS);
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        Session["txSQL"] = txSQL.Text;
        Response.Redirect("wfParametro.aspx");
    }

}
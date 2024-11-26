﻿using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;




public partial class wfSubServicios: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        if (!(objUsuario.PermisoModulo("50", blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
            btnAgregar.Visible = false;
        //if (txSQL.Text != "")
            Filtrar();
        if (blU.ValorObjetoString(Session["Volver"]) == "S")
        {
            tbNombre.Text = Session["tbnombre"].ToString();
            if (Session["txSQL"].ToString() != "")
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["tbnombre"] = "";
        }
    }

    private void Filtrar()
    {
        DataSet dsServicios = new DataSet();
        clsblParametricas blParam = new clsblParametricas();
        String msgError;
        String strSQL = "";

        msgError = blParam.ConsultaSubServicios(ref dsServicios, "", tbNombre.Text);
        if (msgError == "")
        {
            gvServicios.DataSource = dsServicios;

            //cbVisibleKiosco.Checked = false;
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
            if (gvServicios.Rows.Count > 0)
                gvServicios.HeaderRow.TableSection = TableRowSection.TableHeader;
         
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



    protected void gvServicios_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int cellIndex = -1;
            for (int i = 0; i < gvServicios.Columns.Count; i++)
            {
                if (gvServicios.Columns[i].HeaderText == "VISIBLE")
                {
                    cellIndex = i;
                    break;
                }
            }

            if (cellIndex != -1)
            {

                if (e.Row.Cells[cellIndex].Text == "True")
                {
                    e.Row.Cells[cellIndex].Text = "Sí";
                }
                else if (e.Row.Cells[cellIndex].Text == "False")
                {
                    e.Row.Cells[cellIndex].Text = "No";
                }
            }


            String idS = "";
            DataSet dsR = new DataSet();

            idS = e.Row.Cells[0].Text;
            LinkButton AdministrarButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 1].Controls[0];
            LinkButton queryButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 2].Controls[0];

            AdministrarButton.CommandName = "ADMINISTRAR";
            AdministrarButton.CommandArgument = idS;
            queryButton.CommandName = "CONSULTAR";
            queryButton.CommandArgument = idS;
            if (!obj.PermisoModulo("50", Session["IDUSUARIO"].ToString(), "U"))
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("50", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }

    protected void gvServicios_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idS = "";

        if (e.CommandName != "")
        {
            idS = e.CommandArgument.ToString();
            Session["tbnombre"] = tbNombre.Text;
            Session["txSQL"] = txSQL.Text;
        }
        if (e.CommandName.ToString() == "ADMINISTRAR")
            Response.Redirect("wfSubservicio.aspx?id=" + idS);
        if (e.CommandName.ToString() == "CONSULTAR")
            Response.Redirect("wfSubServicio.aspx?consulta=si&id=" + idS);
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        Session["tbnombre"] = tbNombre.Text;
        Session["txSQL"] = txSQL.Text;
        Response.Redirect("wfSubServicio.aspx");
    }


    protected TableCell CrearCelda(String Texto, bool Titulo, int Width, Control ctl, int colSpan, string style)
    {
        TableCell cell = new TableCell();
        cell.Text = Texto;
        if (Width != -1)
            cell.Width = Unit.Percentage(Width);
        cell.ForeColor = System.Drawing.Color.Black;
        cell.HorizontalAlign = HorizontalAlign.Center;
        if (style != "")
            cell.CssClass = style;
        if (Titulo)
        {
            cell.Font.Bold = true;
        }
        else
        {
            cell.Font.Bold = false;
        }
        if (ctl != null)
        {
            cell.Controls.Add(ctl);
        }
        if (colSpan != -1)
            cell.ColumnSpan = colSpan;
        return cell;
    }

}
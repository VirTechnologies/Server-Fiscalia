using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wfPrioridades : System.Web.UI.Page
{
    Variables utl = new Variables();
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        if (!(objUsuario.PermisoModulo("49", blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
            btnAgregar.Visible = false;
        //if (txSQL.Text != "")
            Filtrar();
        if (!Page.IsPostBack)
        {
            blU.LlenaDDLObligatorio(ddlOficinaId, "Oficinas", "Id", "Nombre", "", "", "Nombre");
            blU.LlenaDDLObligatorio(ddltematica, "Grupos", "Id", "Nombre", "", "", "Nombre");

        }
        if (blU.ValorObjetoString(Session["Volver"]) == "S")
        {
            if (Session["txSQL"].ToString() != "")
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["OficinaId"] = "";
        }
    }

    private void Filtrar()
    {
        DataSet dsPrioridades = new DataSet();
        clsblParametricas blParam = new clsblParametricas();
        String msgError;
        String strSQL = "";

        msgError = blParam.ConsultaPrioridades(ref dsPrioridades, "", ddlOficinaId.SelectedValue, dllsala.SelectedValue, ddltematica.SelectedValue);
        if (msgError == "")
        {
            gvPrioridades.DataSource = dsPrioridades;
            gvPrioridades.DataBind();
            if (gvPrioridades.Rows.Count <= 0)
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
            }
            else
            {
                tbNoRegistros.Text = gvPrioridades.Rows.Count.ToString();
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

    protected void gvPrioridades_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int cellIndex = -1;
            for (int i = 0; i < gvPrioridades.Columns.Count; i++)
            {
                if (gvPrioridades.Columns[i].HeaderText == "Habilitado")
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
            if (!obj.PermisoModulo("49", Session["IDUSUARIO"].ToString(), "U"))
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("49", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }

    protected void gvPrioridades_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idS = "";
        if (e.CommandName != "")
        {
             utl.tematicaid = ddltematica.SelectedValue;
            idS = e.CommandArgument.ToString();
            Session["OficinaId"] = ddlOficinaId.SelectedValue;
            Session["txSQL"] = txSQL.Text;
            
        }
        if (e.CommandName.ToString() == "ADMINISTRAR")
            Response.Redirect("wfPrioridad.aspx?id=" + idS);
        if (e.CommandName.ToString() == "CONSULTAR")
            Response.Redirect("wfPrioridad.aspx?consulta=si&id=" + idS);
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        Session["txSQL"] = txSQL.Text;
        Response.Redirect("wfPrioridad.aspx");
    }

    protected void ddlOficinaId_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOficinaId.SelectedIndex > 0)
        {
            clsblUtiles blU = new clsblUtiles();
            blU.LlenaDDLObligatorio(dllsala, "Sala", "Id", "Descripcion", $"OficinaId = {ddlOficinaId.SelectedValue}", "", "Descripcion");
        }
        else
        {
            dllsala.Items.Clear();
        }
    } 
    protected void ddltematica_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOficinaId.SelectedIndex > 0)
        {
            clsblUtiles blU = new clsblUtiles();
            blU.LlenaDDLObligatorio(dllsala, "Sala", "Id", "Descripcion", $"OficinaId = {ddlOficinaId.SelectedValue}", "", "Descripcion");
        }
        else
        {
            dllsala.Items.Clear();
        }
    }
}
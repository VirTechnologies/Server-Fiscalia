using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class wfUsuariosCajas : System.Web.UI.Page
{
    DataSet dsAsesores = new DataSet();
    Image sortImage = new Image();

    public string OrdenarDir
    {
        get
        {
            if (ViewState["OrdenarDir"] == null)
                return string.Empty;
            else
                return ViewState["OrdenarDir"].ToString();
        }
        set
        {
            ViewState["OrdenarDir"] = value;
        }
    }
    private string _ordenarDir;


    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        if (!(objUsuario.PermisoModulo("58", blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
            btnAgregar.Visible = false;

        //if (!Page.IsPostBack)
        //{
        //    Filtrar();
        //}

        //if (txSQL.Text != "")
            Filtrar();
        if (blU.ValorObjetoString(Session["Volver"]) == "S")
        {
            if (Session["txSQL"].ToString() != "")
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["Nombre"] = "";
        }
    }

    private void Filtrar()
    {
        clsblParametricas blParam = new clsblParametricas();
        String msgError;
        String strSQL = "";

        msgError = blParam.ConsultaUsuariosAsesores(ref dsAsesores, "", tbNombre.Text);
        if (msgError == "")
        {
            gvAsesores.DataSource = dsAsesores;
            gvAsesores.DataBind();
            if (gvAsesores.Rows.Count <= 0)
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
            }
            else
            {
                tbNoRegistros.Text = gvAsesores.Rows.Count.ToString();
                lblNoRegistros.Visible = tbNoRegistros.Visible = true;
                lblSinRegistros.Visible = false;
            }
            txSQL.Text = "Consultó";
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

    protected void gvAsesores_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String idS = "";
            DataSet dsR = new DataSet();

            idS = e.Row.Cells[0].Text;
            LinkButton PasswordButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 1].Controls[0];
            LinkButton AdministrarButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 2].Controls[0];
            LinkButton queryButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 3].Controls[0];
            LinkButton sedes = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 4].Controls[0];

            AdministrarButton.CommandName = "ADMINISTRAR";
            AdministrarButton.CommandArgument = idS;
            queryButton.CommandName = "CONSULTAR";
            queryButton.CommandArgument = idS;
            PasswordButton.CommandName = "PASSWORD";
            PasswordButton.CommandArgument = idS;
            sedes.CommandName = "SEDES";
            sedes.CommandArgument = idS;

            if (e.Row.Cells[e.Row.Cells.Count - 5].Text.ToString() == "True")
                e.Row.Cells[e.Row.Cells.Count - 5].Text = "USUARIO";
            else
                e.Row.Cells[e.Row.Cells.Count - 5].Text = "SÚPER USUARIO";

            if (!obj.PermisoModulo("58", Session["IDUSUARIO"].ToString(), "U"))
            {
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
                e.Row.Cells[e.Row.Cells.Count - 2].Visible = false;
            }

            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("58", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            e.Row.Cells[e.Row.Cells.Count - 2].Visible = false;
        }
    }

    protected void gvAsesores_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idS = "";

        if (e.CommandName != "")
        {
            idS = e.CommandArgument.ToString();
            Session["Nombre"] = tbNombre.Text;
            Session["txSQL"] = txSQL.Text;
        }
        if (e.CommandName.ToString() == "ADMINISTRAR")
            Response.Redirect("wfUsuarioCaja.aspx?id=" + idS);
        if (e.CommandName.ToString() == "CONSULTAR")
            Response.Redirect("wfUsuarioCaja.aspx?consulta=si&id=" + idS);
        if (e.CommandName.ToString() == "SEDES")
            Response.Redirect("wfSedesUsuarios.aspx?consulta=si&id=" + idS);
        if (e.CommandName.ToString() == "PASSWORD")
            Response.Redirect("wfNewPasswordAdvisor.aspx?id=" + idS);
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        Session["txSQL"] = txSQL.Text;
        Response.Redirect("wfUsuarioCaja.aspx");
    }

    protected void gvAsesores_Sorting(object sender, GridViewSortEventArgs e)
    {
        SetSortDirection(OrdenarDir);
        if (dsAsesores != null)
        {
            //Sort the data.
            dsAsesores.Tables[0].DefaultView.Sort = e.SortExpression + " " + _ordenarDir;
            DataTable dt = dsAsesores.Tables[0].DefaultView.ToTable();
            dsAsesores.Tables[0].Rows.Clear();
            foreach (DataRow row in dt.Rows)
                dsAsesores.Tables[0].Rows.Add(row.ItemArray);

            gvAsesores.DataSource = dsAsesores;
            gvAsesores.AllowSorting = true;
            gvAsesores.DataBind();
            OrdenarDir = _ordenarDir;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in gvAsesores.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = gvAsesores.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }
            gvAsesores.HeaderRow.Cells[columnIndex].CssClass = "btn-secondary";
            gvAsesores.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
        }

    }

    protected void SetSortDirection(string sortDirection)
    {
        if (sortDirection == "DESC")
        {
            _ordenarDir = "ASC";
            sortImage.ImageUrl = "images\\arrow-down.png";
        }
        else
        {
            _ordenarDir = "DESC";
            sortImage.ImageUrl = "images\\arrow-up.png";
        }
    }


}
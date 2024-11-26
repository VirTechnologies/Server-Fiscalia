using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.Globalization;
using System.Threading;

public partial class wfPerfiles_Modulos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        DataSet dsPerfiles =new DataSet();
        clsblUsuarios objUsuario = new clsblUsuarios();
        String msg;
        //=============== ESTAS 4 LINEAS SON PARA LAS FECHAS ===============================
        DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
        myDTF.ShortDatePattern = "dd/MM/yyyy";
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;


        if (Session["IDUSUARIO"].ToString()=="") 
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");

        HtmlGenericControl ctl = new HtmlGenericControl();
        ctl = (HtmlGenericControl)Page.FindControl("body");
        if (ctl!=null) 
            ctl.Attributes.Add("onbeforeunload", "DeseaCerrar();");
        btnCerrar.Attributes.Add("onclick", "ClickCerrar();");
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
        id_perfil.Text = Request.QueryString["id_perfil"];
        obj = new clsblUsuarios();
        msg = obj.ConsultaPerfil(ref dsPerfiles, id_perfil.Text);
        if (msg == "")
            Label2.Text = dsPerfiles.Tables[0].Rows[0]["perfil"].ToString();
        hfConsulta.Value = Request.QueryString["consulta"];
        GenerarTablaControles(id_perfil.Text);
        if (hfConsulta.Value == "si")
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
    }

    protected void GenerarTablaControles(String id_perfil)
    {
        clsblUsuarios obj;
        DataSet dsModulos =new DataSet();
        DataSet dsSubModulos_1 = new DataSet();
        DataSet dsSubModulos_2 = new DataSet();
        String msg;
        int i, j, k;
        TableRow row;
        TableCell cell;
        //row = new Table2.

        obj = new clsblUsuarios();
        msg = obj.RetornarModulosPadre(ref dsModulos);
        if (msg == "")
        {
            for (i = 0;i<dsModulos.Tables[0].Rows.Count;i++)
            {
                row = new TableRow();
                //row.BackColor = System.Drawing.Color.SteelBlue;
                row.CssClass = "table-primary";
                cell = new TableCell();
                cell.Text = dsModulos.Tables[0].Rows[i]["modulo"].ToString().ToUpper();
                cell.Font.Bold = true;
                cell.ForeColor = System.Drawing.Color.Black;
                row.Cells.Add(cell);
                Table2.Rows.Add(row);

                row = new TableRow();
                row = GenerarFilaTitulo();
                //row.BackColor = System.Drawing.Color.SteelBlue;
                Table2.Rows.Add(row);
                row = new TableRow();
                row = GenerarFilaModulo(id_perfil, dsModulos.Tables[0].Rows[i]["id_modulo"].ToString(), 
                                        (i + 1).ToString() + ". " + dsModulos.Tables[0].Rows[i]["modulo"].ToString(), true);
                Table2.Rows.Add(row);
                msg = obj.RetornarSubModulos(ref dsSubModulos_1, dsModulos.Tables[0].Rows[i]["id_modulo"].ToString());
                if ((msg == "") && (dsSubModulos_1.Tables.Count > 0))
                {
                    for (j = 0;j<dsSubModulos_1.Tables[0].Rows.Count;j++)
                    {
                        row = new TableRow();
                        row = GenerarFilaModulo(id_perfil, dsSubModulos_1.Tables[0].Rows[j]["id_modulo"].ToString(), 
                                                "&nbsp;&nbsp;&nbsp;" + (i + 1).ToString() + "." + (j + 1).ToString() + ". " + dsSubModulos_1.Tables[0].Rows[j]["modulo"], true);
                        Table2.Rows.Add(row);
                        msg = obj.RetornarSubModulos(ref dsSubModulos_2, dsSubModulos_1.Tables[0].Rows[j]["id_modulo"].ToString());
                        if ((msg == "") && (dsSubModulos_2.Tables.Count > 0)) 
                        {
                            for (k = 0;k<dsSubModulos_2.Tables[0].Rows.Count;k++)
                            {
                                row = new TableRow();
                                row = GenerarFilaModulo(id_perfil, dsSubModulos_2.Tables[0].Rows[k]["id_modulo"].ToString(), 
                                                 "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + (i + 1).ToString() + "." + (j + 1).ToString() + ". " + (k + 1).ToString() + "." + dsSubModulos_2.Tables[0].Rows[k]["modulo"], false);
                                Table2.Rows.Add(row);
                            }
                        }
                    }
                }
                row = new TableRow();
                //row.BackColor = System.Drawing.Color.White;
                cell = new TableCell();
                cell.Text = "&nbsp;";
                cell.ColumnSpan = 6;
                row.Cells.Add(cell);
                Table2.Rows.Add(row);
            }
        }
    }

    protected TableRow GenerarFilaModulo(String id_perfil, String id_modulo,
                                         String nombre_modulo, bool bold)
    {
        TableRow row;
        TableCell cell;
        Panel div;
        HtmlInputCheckBox check;
        Label lbl;
        clsblUsuarios obj;
        DataSet dsPermisos = new DataSet();
        String msg;
        bool insert = false, update = false;
        bool delete = false, query = false;
        clsblUtiles blUtiles = new clsblUtiles();

        obj = new clsblUsuarios();
        msg = obj.RetornarPermisosModulo(ref dsPermisos, id_modulo, id_perfil);
        if (msg == "")
        {
            if (dsPermisos.Tables.Count > 0)
            {
                if (dsPermisos.Tables[0].Rows.Count > 0)
                {
                    if (blUtiles.BaseDeDatos() == "SQLSERVER")
                    {

                        if (dsPermisos.Tables[0].Rows[0]["insercion"].ToString() != "")
                            insert = (bool)dsPermisos.Tables[0].Rows[0]["insercion"];
                        if (dsPermisos.Tables[0].Rows[0]["modificacion"].ToString() != "")
                            update = (bool)dsPermisos.Tables[0].Rows[0]["modificacion"];
                        if (dsPermisos.Tables[0].Rows[0]["borrado"].ToString() != "")
                            delete = (bool)dsPermisos.Tables[0].Rows[0]["borrado"];
                        if (dsPermisos.Tables[0].Rows[0]["consulta"].ToString() != "")
                            query = (bool)dsPermisos.Tables[0].Rows[0]["consulta"];
                    }
                    if (blUtiles.BaseDeDatos() == "ORACLE")
                    {
                        if (dsPermisos.Tables[0].Rows[0]["insercion"].ToString() != "")
                            if (dsPermisos.Tables[0].Rows[0]["insercion"].ToString() == "1")
                                insert = true;
                        if (dsPermisos.Tables[0].Rows[0]["modificacion"].ToString() != "")
                            if (dsPermisos.Tables[0].Rows[0]["modificacion"].ToString() == "1")
                                update = true;
                        if (dsPermisos.Tables[0].Rows[0]["borrado"].ToString() != "")
                            if (dsPermisos.Tables[0].Rows[0]["borrado"].ToString() == "1")
                                delete = true;
                        if (dsPermisos.Tables[0].Rows[0]["consulta"].ToString() != "")
                            if (dsPermisos.Tables[0].Rows[0]["consulta"].ToString() == "1")
                                query = true;
                    }
                }
            }
        }
        row = new TableRow();
        //row.BackColor = System.Drawing.Color.White;
        cell = new TableCell();
        cell = CrearCelda(nombre_modulo, System.Drawing.Color.Black, 40);
        cell.Font.Bold = bold;
        row.Cells.Add(cell);

        cell = new TableCell();
        cell = CrearCelda("", System.Drawing.Color.Black, 12);

        check = new HtmlInputCheckBox();
        check.Attributes["Class"] = "custom-control-input";
        check.ID = "check_" + id_modulo + "_1";
        check.Visible = true;
        check.Checked = false;
        if (insert)
            check.Checked = true;
        if ((hfConsulta.Value == "si") || (permiteModulo(id_modulo)))
            check.Disabled = true;
        cell.HorizontalAlign = HorizontalAlign.Center;
        div = new Panel();
        div.CssClass = "custom-control custom-switch";
        lbl = new Label();
        lbl.AssociatedControlID = check.Name;
        lbl.CssClass = "custom-control-label";
        lbl.Text = "&nbsp;";
        div.Controls.Add(check);
        div.Controls.Add(lbl);
        cell.Controls.Add(div);
        row.Cells.Add(cell);

        cell = new TableCell();
        cell = CrearCelda("", System.Drawing.Color.Black, 12);
        check = new HtmlInputCheckBox();
        check.Attributes["Class"] = "custom-control-input";
        check.ID = "check_" + id_modulo + "_2";
        check.Visible = true;
        check.Checked = false;
        if (update)
            check.Checked = true;
        if ((hfConsulta.Value == "si") || (permiteModulo(id_modulo)))
            check.Disabled = true;
        //check.CssClass = "RadioButtonsAzules";
        cell.HorizontalAlign = HorizontalAlign.Center;
        div = new Panel();
        div.CssClass = "custom-control custom-switch";
        lbl = new Label();
        lbl.AssociatedControlID = check.Name;
        lbl.CssClass = "custom-control-label";
        lbl.Text = "&nbsp;";
        div.Controls.Add(check);
        div.Controls.Add(lbl);
        cell.Controls.Add(div);
        row.Cells.Add(cell);

        cell = new TableCell();
        cell = CrearCelda("", System.Drawing.Color.Black, 12);
        cell = new TableCell();
        check = new HtmlInputCheckBox();
        check.Attributes["Class"] = "custom-control-input";
        check.ID = "check_" + id_modulo + "_3";
        check.Visible = true;
        check.Checked = false;
        if (delete)
            check.Checked = true;
        if ((hfConsulta.Value == "si") || (permiteModulo(id_modulo)))
            check.Disabled = true;
        //check.CssClass = "RadioButtonsAzules";
        cell.HorizontalAlign = HorizontalAlign.Center;
        div = new Panel();
        div.CssClass = "custom-control custom-switch";
        lbl = new Label();
        lbl.AssociatedControlID = check.Name;
        lbl.CssClass = "custom-control-label";
        lbl.Text = "&nbsp;";
        div.Controls.Add(check);
        div.Controls.Add(lbl);
        cell.Controls.Add(div);
        row.Cells.Add(cell);

        cell = CrearCelda("", System.Drawing.Color.Black, 12);
        check = new HtmlInputCheckBox();
        check.Attributes["Class"] = "custom-control-input";
        check.ID = "check_" + id_modulo + "_4";
        check.Visible = true;
        check.Checked = false;
        if (query)
            check.Checked = true;
        if (hfConsulta.Value == "si")
            check.Disabled = true;
        //check.CssClass = "RadioButtonsAzules";
        cell.HorizontalAlign = HorizontalAlign.Center;
        div = new Panel();
        div.CssClass = "custom-control custom-switch";
        lbl = new Label();
        lbl.AssociatedControlID = check.Name;
        lbl.CssClass = "custom-control-label";
        lbl.Text = "&nbsp;";
        div.Controls.Add(check);
        div.Controls.Add(lbl);
        cell.Controls.Add(div);
        row.Cells.Add(cell);

        return row;
    }

    protected TableRow GenerarFilaTitulo()
    {
        TableRow row = new TableRow();
        TableCell cell;

        row = new TableRow();
        //row.BackColor = System.Drawing.Color.White;
        cell = new TableCell();
        cell = CrearCelda("MÓDULO", System.Drawing.Color.Black, 40);
        cell.Font.Bold = true;
        //cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);

        cell = new TableCell();
        cell = CrearCelda("INSERTAR", System.Drawing.Color.Black, 12);
        cell.Font.Bold = true;
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);

        cell = new TableCell();
        cell = CrearCelda("MODIFICAR", System.Drawing.Color.Black, 12);
        cell.Font.Bold = true;
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);

        cell = new TableCell();
        cell = CrearCelda("ELIMINAR", System.Drawing.Color.Black, 12);
        cell.Font.Bold = true;
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);

        cell = new TableCell();
        cell = CrearCelda("CONSULTAR", System.Drawing.Color.Black, 12);
        cell.Font.Bold = true;
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);

        row.CssClass = "table-primary";
        return row;
    }

    public TableCell CrearCelda(String Texto, System.Drawing.Color Color, int Ancho)
    {
        TableCell cell = new TableCell();

        cell.Text = Texto;
        cell.ForeColor = Color;
        cell.Width = Unit.Percentage(Ancho);
        return cell;
    }

    private bool permiteModulo(String id_modulo)
    {
        //if  (id_modulo == "14" || id_modulo == "27" || id_modulo == "28")
        //    return true;
        //else
        //    return false;
        return false;
    }


    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        OleDbConnection Conexion=null;
        OleDbTransaction myTrans = null;
        clsblUsuarios obj;
        clsblUtiles blUtiles =new clsblUtiles();
        DataSet dsModulos =new DataSet();
        String msg;
        int i;
        int insert, update, delete, query;

        Conexion = new OleDbConnection();
        Conexion.ConnectionString = blUtiles.cadenaConexion();
        Conexion.Open();
        myTrans = Conexion.BeginTransaction(IsolationLevel.ReadCommitted);
        try
        {
            obj = new clsblUsuarios();
            msg = obj.EliminaPermisosPerfil(id_perfil.Text, Conexion, myTrans);
            msg = obj.RetornarModulos(ref dsModulos);
            if (msg == "")
            {
                for (i = 0;i<dsModulos.Tables[0].Rows.Count;i++)
                {
                    insert = 0;
                    update = 0;
                    delete = 0;
                    query = 0;
                    if (Request.Form["check_" + dsModulos.Tables[0].Rows[i]["id_modulo"].ToString() + "_1"]!=null)
                        insert = 1;
                    if (Request.Form["check_" + dsModulos.Tables[0].Rows[i]["id_modulo"].ToString() + "_2"]!=null)
                        update = 1;
                    if (Request.Form["check_" + dsModulos.Tables[0].Rows[i]["id_modulo"].ToString() + "_3"]!=null)
                        delete = 1;
                    if (Request.Form["check_" + dsModulos.Tables[0].Rows[i]["id_modulo"].ToString() + "_4"]!=null)
                        query = 1;
                    if ((insert != 0) || (update != 0) || (delete != 0) || (query != 0))
                    {
                        msg = obj.GrabaPermisosPerfilModulo(id_perfil.Text, dsModulos.Tables[0].Rows[i]["id_modulo"].ToString(),
                                                            insert, update, delete, query, Conexion, myTrans);
                    }
                }
            }
            myTrans.Commit();
            lbConfirmacion.Text = "¡Permisos grabados corectamente!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
            notificacion.Visible = true;
        }
        catch (Exception ex) 
        {
            myTrans.Rollback();
            lbConfirmacion.Text = "¡Error grabando permisos!" + ex.Message;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
        finally
        {
            Conexion.Close();
        }
    }

    protected void btnCerrar_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfPerfiles.aspx");
    }
}

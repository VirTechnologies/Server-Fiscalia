using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class wfUsuario : System.Web.UI.Page
{

    String sin_asignar = "SINASIGNAR_";

    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();
        String msgError, strAux = "";
        DataSet dsUsuario = new DataSet();
        NSSSqlUtil blObj = new NSSSqlUtil();


        //=============== ESTAS 4 LINEAS SON PARA LAS FECHAS ===============================
        DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
        myDTF.ShortDatePattern = "dd/MM/yyyy";
        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
        Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
        if (Session["IDUSUARIO"].ToString() == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");

        HtmlGenericControl ctl = new HtmlGenericControl();
        ctl = (HtmlGenericControl)Page.FindControl("body");
        if (ctl != null)
            ctl.Attributes.Add("onbeforeunload", "DeseaCerrar();");

        if ((blU.ValorObjetoString(Request.QueryString["consulta"]) != "si") && (hfConsulta.Value != "si"))
            btnSalir.Attributes.Add("onclick", "ClickCerrar();");
        else
        {
            btnSalir.Attributes.Add("onclick", "window.close();");
            Label9.Visible = true;
            Label10.Visible = true;
            fecha_creacion.Visible = true;
            usuario_creacion.Visible = true;
        }
        // Configura los botones de acuerdo a los permisos
        if (!((objUsuario.PermisoModulo("17", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("17", Session["IDUSUARIO"].ToString(), "U"))))
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (objUsuario.PermisoModulo("17", Session["IDUSUARIO"].ToString(), "D"))
            btElimina.Visible = false;
        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            id_usuario.Text = blU.ValorObjetoString(Request.QueryString["id_usuario"]);
            hfElimina.Value = blU.ValorObjetoString(Request.QueryString["elimina"]);
            tbDias_Contrasena.Text = "90";
            blU.LlenaDDLObligatorio(id_perfil, "PERFILES", "id_perfil", "perfil", "", "", "");
            if (id_usuario.Text != "")
            {
                blObj.NombreTabla = "USUARIOSADMIN";
                msgError = blObj.NssEjecutarSQL("SELECT", ref dsUsuario, ref strAux, "id_usuario", id_usuario.Text, null, null);
                if (msgError == "")
                {
                    id_usuario.Text = dsUsuario.Tables[0].Rows[0]["id_usuario"].ToString();
                    nombre.Text = dsUsuario.Tables[0].Rows[0]["nombre"].ToString();
                    tblogin.Text = dsUsuario.Tables[0].Rows[0]["login"].ToString();
                    clave.Attributes.Add("value", sin_asignar);
                    reconfirmar_clave.Attributes.Add("value", sin_asignar);
                    id_perfil.SelectedValue = dsUsuario.Tables[0].Rows[0]["id_perfil"].ToString();
                    estado.SelectedValue = dsUsuario.Tables[0].Rows[0]["estado"].ToString();
                    if (dsUsuario.Tables[0].Rows[0]["fecha_creacion"].ToString() != "")
                        fecha_creacion.Text = dsUsuario.Tables[0].Rows[0]["fecha_creacion"].ToString();
                    id_usuario_creacion.Text = dsUsuario.Tables[0].Rows[0]["id_usuario_creacion"].ToString();
                    usuario_creacion.Text = objUsuario.NombreUsuario(id_usuario_creacion.Text).ToString();
                    numero_identificacion.Text = dsUsuario.Tables[0].Rows[0]["numero_identificacion"].ToString();
                    tbDireccion.Text = dsUsuario.Tables[0].Rows[0]["direccion"].ToString();
                    tbTelefono.Text = dsUsuario.Tables[0].Rows[0]["telefono"].ToString();
                    tbemail.Text = dsUsuario.Tables[0].Rows[0]["email"].ToString();
                    //tbCargo.Text = dsUsuario.Tables[0].Rows[0]["cargo"].ToString();
                    tbDias_Contrasena.Text = dsUsuario.Tables[0].Rows[0]["dias_contrasena"].ToString();
                    hfDias_Clave.Value = tbDias_Contrasena.Text;
                }
            }
        }
        if (hfConsulta.Value == "si")
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
        }
        if (hfElimina.Value == "si")
        {
            btnGrabar.Visible = false;
            objUsuario.SoloLectura(this);
            btElimina.Visible = true;
            RequiredFieldValidator3.Visible = false;
            RequiredFieldValidator7.Visible = false;
        }
    }

    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        NSSSqlUtil blObj = new NSSSqlUtil();
        String msgError = "", strAux = "";
        DateTime FechaHoy;
        bool Inserto = false;
        clsblUsuarios blUsuarios = new clsblUsuarios();
        clsblUtiles blUtiles = new clsblUtiles();
        DataSet dsInterno = null;

        try
        {
            if (clave.Text == reconfirmar_clave.Text)
            {
                blObj.NombreTabla = "USUARIOSADMIN";
                blObj.LlavePrimaria = "id_usuario";
                blObj.Add("nombre", nombre.Text);
                blObj.Add("numero_identificacion", numero_identificacion.Text);
                blObj.Add("login", tblogin.Text);
                blObj.Add("id_perfil", id_perfil.SelectedValue);
                blObj.Add("estado", estado.SelectedValue);
                if (tbDireccion.Text != "")
                    blObj.Add("direccion", tbDireccion.Text);
                if (tbTelefono.Text != "")
                    blObj.Add("telefono", tbTelefono.Text);
                blObj.Add("email", tbemail.Text);
                blObj.Add("cargo", tbCargo.Text);
                blObj.Add("dias_contrasena", tbDias_Contrasena.Text);
                if (id_usuario.Text == "" && blObj.Duplicado("login", tblogin.Text))
                {
                    lbConfirmacion.Text = "Error! Ya existe el login " + tblogin.Text;
                    notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                    notificacion.Visible = true;
                }
                else
                {
                    FechaHoy = DateTime.Now;
                    blObj.Add("fecha_creacion", FechaHoy.ToString("dd/MM/yyyy HH:mm:ss"));
                    blObj.Add("id_usuario_creacion", Session["IDUSUARIO"].ToString());
                    if ((id_usuario.Text == "") || (tbDias_Contrasena.Text != hfDias_Clave.Value))
                        blObj.Add("fecha_cambio_contrasena", FechaHoy.AddDays(Convert.ToInt32(tbDias_Contrasena.Text)).ToString("dd/MM/yyyy HH:mm:ss"));
                    if (id_usuario.Text == "")
                    {
                        blObj.Add("password", FormsAuthentication.HashPasswordForStoringInConfigFile(clave.Text, "MD5").ToLower());
                        msgError = blObj.NssEjecutarSQL("INSERT", ref dsInterno, ref strAux, "", "", null, null);
                        id_usuario.Text = strAux;
                        Inserto = true;
                    }
                    else
                    {
                        if (clave.Text != sin_asignar)
                            blObj.Add("password", FormsAuthentication.HashPasswordForStoringInConfigFile(clave.Text, "MD5").ToLower());
                        blObj.Add("id_usuario", id_usuario.Text);
                        msgError = blObj.NssEjecutarSQL("UPDATE", ref dsInterno, ref strAux, "", "", null, null);
                    }
                    lbConfirmacion.Text = "Registro grabado correctamente!";
                    notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
                    notificacion.Visible = true;
                    if (Inserto)
                    {
                        clsblUsuarios objUsuario = new clsblUsuarios();
                        fecha_creacion.Text = FechaHoy.ToString("dd/MM/yyyy");
                        usuario_creacion.Text = objUsuario.NombreUsuario(Session["IDUSUARIO"].ToString());
                    }
                }
            }
            else
            {
                lbConfirmacion.Text = "Clave incorrecta!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacion.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lbConfirmacion.Text = "Error al grabar el registro!" + msgError + " " + ex.ToString();
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void btElimina_Click(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        DataSet dsUsuarios = new DataSet();
        String msg = "", strAux = "";
        NSSSqlUtil blObj = new NSSSqlUtil();
        DataSet dsInterno = null;

        blObj.LlavePrimaria = "id_usuario";
        blObj.NombreTabla = "USUARIOSADMIN";
        blObj.Add("id_usuario", id_usuario.Text);
        msg = blObj.NssEjecutarSQL("DELETE", ref dsInterno, ref strAux, "", "", null, null);
        if (msg == "")
        {
            lbConfirmacion.Text = "¡Registro borrado correctamente!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
            notificacion.Visible = true;
        }
        else
        {
            lbConfirmacion.Text = "¡Error al eliminar el registro!" + msg;
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["Volver"] = "S";
        Response.Redirect("wfUsuarios.aspx");
    }
}

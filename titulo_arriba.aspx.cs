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

public partial class titulo_arriba : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario=new clsblUsuarios();

        lbCambiarClave.Visible = false;
        if (Session["IDUSUARIO"].ToString() != "")
        {
            lblUsuario.Text = objUsuario.PerfilUsuario(Session["IDUSUARIO"].ToString());
            lblUsuario.Text += " - " + objUsuario.NombreUsuario(Session["IDUSUARIO"].ToString());
            //lbCambiarClave.Attributes.Add("onclick", "openModalWindow('Modales.aspx?pagina=wfCambioClave',400,220);");
            lbCambiarClave.Visible = true;
        }
    }

    protected void lbCambiarClave_Click(object sender, EventArgs e)
    {
        Response.Redirect("wfCambioClave.aspx");
    }
}

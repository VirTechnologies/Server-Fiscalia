using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wfBienvenida : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["IDUSUARIO"] == null)
        {
            String script = "<script> parent.location='index.aspx?&TimeOut=si'; </script>";
            ClientScript.RegisterStartupScript(this.GetType(), "index", script);
        }

    }
}
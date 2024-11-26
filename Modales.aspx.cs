using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Modales : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String [] Keys = new String[30];
        String [] Values = new String[30];
        int i;
        String [] arr1;
        String [] arr2;
        NameValueCollection coll;
        String pagina="";
        String nombre="";
        String valor="";
        String src="";

        for (i = 0; i < 30; i++)
        {
            Keys[i] = "";
            Values[i] = "";
        }
        coll = Request.QueryString;
        // Get names of all keys into a string array.
        arr1 = coll.AllKeys;
        for (i = 0 ;i<= arr1.GetUpperBound(0);i++)
        {
            Keys[i] = Server.HtmlEncode(arr1[i]);
            // Get all values under this key.
            arr2 = coll.GetValues(i);
            Values[i] = Server.HtmlEncode(arr2[0]);
            if (Keys[i] == "pagina")
                pagina = Values[i];
            if (Keys[i] == "nombre")
                nombre = Values[i];
            if (Keys[i] == "valor")
                valor = Values[i];
        }
        src = pagina + ".aspx";
        if (nombre != "")
        {
            src += "?" + nombre + "=";
            src += valor;
        }
        i = 0;
        while (Keys[i] != "")
        {
            if ((Keys[i] != "pagina") && (Keys[i] != "nombre") && (Keys[i] != "valor")) 
            {
                if (src.IndexOf("?") != -1)
                    src += "&";
                else
                    src += "?";
                src += Keys[i] + "=" + Values[i];
            }
            i = i + 1;
        }
        Response.Write("<iframe src='" + src + "' width='100%' height='100%'></iframe>");
    }
}

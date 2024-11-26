using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;


public partial class ExcelDataGrid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter sw   = new StringWriter(sb);
        HtmlTextWriter htw  = new HtmlTextWriter(sw);
        Page pagina = new Page();
        HtmlForm form = new HtmlForm();
        GridView grilla = new GridView();
        grilla = (GridView)Session["DS"];
        grilla.EnableViewState = false;
        pagina.EnableEventValidation = false;
        pagina.DesignerInitialize();
        form.Controls.Add(grilla);
        pagina.Controls.Add(form);
        pagina.RenderControl(htw);
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment;filename=data.xls");
        Response.Charset = "";

        Response.ContentEncoding = Encoding.Default;
        Response.Write(sb.ToString());
        Response.End();
    }
}
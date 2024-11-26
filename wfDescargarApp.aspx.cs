using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wfDescargarApp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Filtrar();
    }

    private void Filtrar()
    {
        DataSet dsOficinas = new DataSet();
        clsblParametricas blParam = new clsblParametricas();
        String msgError;

        msgError = blParam.ConsultaOficinas(ref dsOficinas, "", "");
        if (msgError == "")
        {
            gvOficinas.DataSource = dsOficinas;
            gvOficinas.DataBind();
        }
        //{"officeCode":101}
    }


    protected void gvOficinas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String idS = "";
            DataSet dsR = new DataSet();
            Image img = new Image();

            idS = e.Row.Cells[0].Text;
            img.ImageUrl = "~/app/qr-code_"+idS+".png";
            img.Width = 200;
            img.Height = 200;
            e.Row.Cells[e.Row.Cells.Count-2].Text= "{\"officeCode\":"+idS+"}";
            e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(img);
        }
    }
}
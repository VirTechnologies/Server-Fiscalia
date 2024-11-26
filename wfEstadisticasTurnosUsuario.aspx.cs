using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wfEstadisticasTurnosUsuario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUtiles blU = new clsblUtiles();

        if (!Page.IsPostBack)
        {
        }
    }

    private void Filtrar()
    {
        DataSet dsTurnos = new DataSet();
        clsblEstadisticas blEstadi = new clsblEstadisticas();
        clsblUtiles blU = new clsblUtiles();
        String msgError;
        string JSonData = "[";
        int i = 0;
        DateTime fecha;
        string script = "";

        lbConfirmacion.Text = "";
        try
        {
            if (tbFechaIni.Text!="")
              fecha=blU.FechaDeString(tbFechaIni.Text);
        }
        catch (Exception)
        {
            TablaInfoTurnos.Visible = false;
            lbConfirmacion.Text = "El formato de la fecha inicial es inválido!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
            return;
        }
        try
        {
            if (tbFechaFin.Text != "")
                fecha = blU.FechaDeString(tbFechaFin.Text);
        }
        catch (Exception)
        {
            TablaInfoTurnos.Visible = false;
            lbConfirmacion.Text = "El formato de la fecha final es inválido!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
            return;
        }
        TablaInfoTurnos.Visible = true;
        msgError = blEstadi.ConsultaNumeroTurnosCliente(ref dsTurnos, tbIdentificacion.Text, tbFechaIni.Text, tbFechaFin.Text);
        if (msgError == "")
        {
            gvTurnosTotales.DataSource = dsTurnos;
            gvTurnosTotales.DataBind();
            if (gvTurnosTotales.Rows.Count > 0)
            {
                notificacion.Visible = false;
                //gvTurnosTotales.HeaderRow.TableSection = TableRowSection.TableHeader;
                //for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
                //{
                //    String Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                //    Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();

                //    JSonData += "{'groupname': '" + dsTurnos.Tables[0].Rows[i]["Oficina"].ToString() + "', 'count': " + dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
                //    if (i < dsTurnos.Tables[0].Rows.Count - 1)
                //        JSonData += ",";
                //}
                //JSonData += "];";
                //script += " \n var data=" + JSonData + "\n countChart(data, 'chartdiv', 'Número de turnos solicitados'); \n";
            }
            else
            {
                TablaInfoTurnos.Visible = false;
                lbConfirmacion.Text = "No se han encontrado turnos para la identificación!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-info";
                notificacion.Visible = true;
            }
        };
        //if (script!="")
        //    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
    }


    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        Filtrar();
    }
}
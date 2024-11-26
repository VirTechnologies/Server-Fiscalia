using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wfEstadisticasTurnosGrupo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUtiles blU = new clsblUtiles();

        if (!Page.IsPostBack)
        {
            blU.LlenaDDLObligatorio(ddlGrupoId, "Grupos", "Id", "Nombre", "", "", "Id");
            //blU.LlenaDDLObligatorio(ddlOficinaId, "Oficinas", "Id", "Nombre", "", "", "Nombre");
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

        //TablaInfoTurnos.Visible = true;
        msgError = blEstadi.ConsultaNumeroTurnosGrupo(ref dsTurnos, "SOLICITADOS", ddlGrupoId.SelectedValue, "", tbFechaIni.Text, tbFechaFin.Text);
        if (msgError == "")
        {
            gvTurnosTotales.DataSource = dsTurnos;
            gvTurnosTotales.DataBind();
            if (gvTurnosTotales.Rows.Count > 0)
            {
                TablaInfoTurnos.Visible = true;
                notificacion.Visible = false;
                gvTurnosTotales.HeaderRow.TableSection = TableRowSection.TableHeader;
                for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
                {
                    string Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                    Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();

                    JSonData += "{'groupname': '" + dsTurnos.Tables[0].Rows[i]["Oficina"].ToString() + "', 'count': " + dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
                    if (i < dsTurnos.Tables[0].Rows.Count - 1)
                        JSonData += ",";
                }
                JSonData += "];";
                script += " \n var data=" + JSonData + "\n countChart(data, 'chartdiv', 'Número de turnos solicitados'); \n";
            }
            else
            {
                TablaInfoTurnos.Visible = false;
                lbConfirmacion.Text = "No se han generado turnos para este grupo!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-info";
                notificacion.Visible = true;
            }
        }
        msgError = blEstadi.ConsultaNumeroTurnosGrupo(ref dsTurnos, "LLAMADOS", ddlGrupoId.SelectedValue, "", tbFechaIni.Text, tbFechaFin.Text);
        if (msgError == "")
        {
            JSonData = "[";
            gvAtendidos.DataSource = dsTurnos;
            gvAtendidos.DataBind();
            if (gvAtendidos.Rows.Count > 0)
                gvAtendidos.HeaderRow.TableSection = TableRowSection.TableHeader;
            for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
            {
                string Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();

                JSonData += "{'groupname': '" + dsTurnos.Tables[0].Rows[i]["Oficina"].ToString() + "', 'count': " + dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
                if (i < dsTurnos.Tables[0].Rows.Count - 1)
                    JSonData += ",";
            }
            JSonData += "];";
            script += " \n var data1=" + JSonData + "\n countChart(data1, 'chartdivLlamados', 'Número de turnos llamados'); \n";
        }
        msgError = blEstadi.ConsultaNumeroTurnosGrupo(ref dsTurnos, "CERRADOS", ddlGrupoId.SelectedValue, "", tbFechaIni.Text, tbFechaFin.Text);
        if (msgError == "")
        {
            JSonData = "[";
            gvCerrados.DataSource = dsTurnos;
            gvCerrados.DataBind();
            if (gvCerrados.Rows.Count > 0)
                gvCerrados.HeaderRow.TableSection = TableRowSection.TableHeader;
            for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
            {
                string Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();

                JSonData += "{'groupname': '" + dsTurnos.Tables[0].Rows[i]["Oficina"].ToString() + "', 'count': " + dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
                if (i < dsTurnos.Tables[0].Rows.Count - 1)
                    JSonData += ",";
            }
            JSonData += "];";
            script += " \n var data2=" + JSonData + "\n countChart(data2, 'chartdivCerrados', 'Número de turnos cerrados'); \n";
        }
        msgError = blEstadi.ConsultaNumeroTurnosGrupo(ref dsTurnos, "ABANDONADOS", ddlGrupoId.SelectedValue, "", tbFechaIni.Text, tbFechaFin.Text);
        if (msgError == "")
        {
            JSonData = "[";
            gvAbandonados.DataSource = dsTurnos;
            gvAbandonados.DataBind();
            if (gvAbandonados.Rows.Count > 0)
                gvAbandonados.HeaderRow.TableSection = TableRowSection.TableHeader;
            for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
            {
                string Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();

                JSonData += "{'groupname': '" + dsTurnos.Tables[0].Rows[i]["Oficina"].ToString() + "', 'count': " + dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
                if (i < dsTurnos.Tables[0].Rows.Count - 1)
                    JSonData += ",";
            }
            JSonData += "];";
            script += " \n var data3=" + JSonData + "\n countChart(data3, 'chartdivAbandonados', 'Número de turnos abandonados'); \n";
        }

       // msgError = blEstadi.ConsultaNumeroTurnosAgendadosGrupo(ref dsTurnos, "AGENDADOS", ddlGrupoId.SelectedValue, "", tbFechaIni.Text, tbFechaFin.Text);
        if (msgError == "")
        {
            JSonData = "[";
            gvAgendados.DataSource = dsTurnos;
            gvAgendados.DataBind();
            if (gvAgendados.Rows.Count > 0)
                gvAgendados.HeaderRow.TableSection = TableRowSection.TableHeader;
            for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
            {
                string Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();

                JSonData += "{'groupname': '" + dsTurnos.Tables[0].Rows[i]["Oficina"].ToString() + "', 'count': " + dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
                if (i < dsTurnos.Tables[0].Rows.Count - 1)
                    JSonData += ",";
            }
            JSonData += "];";
            script += " \n var data4=" + JSonData + "\n countChart(data4, 'chartdivAgendados', 'Número de turnos agendados'); \n";
        }

        if (script!="")
            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
    }


    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        Filtrar();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wfEstadisticasTurnosHora : System.Web.UI.Page
{
    public class turnosSede
    {
        public string sede;
        public int cantTurnos;
    }
    public class horasTurno
    {
        public string horaTexto;
        public int hora;
        public List<turnosSede> listSedes = new List<turnosSede>();
    }
    private List<horasTurno> turnosHora = new List<horasTurno>();
    private List<string> listaOficinas = new List<String>();

    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUtiles blU = new clsblUtiles();

        if (!Page.IsPostBack)
        {
            blU.LlenaDDLObligatorio(ddlOficinaId, "Oficinas", "Id", "Nombre","","", "Nombre");
            HoraI.Items.Add("");
            HoraF.Items.Add("");
            for (int i = 1; i <= 24; i++)
            {
                HoraI.Items.Add((i-1).ToString());
                HoraF.Items.Add(i.ToString());
            }
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
        string HoraInicio = "";
        string HoraFin = "";

        TablaInfoTurnos.Visible = false;
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
            int tempI = Convert.ToInt32(HoraI.Text);
            if ((tempI >= 0) & (tempI <= 24))
            {
                HoraInicio = $"{tbFechaIni.Text} {tempI:00}:00:00.000";
            }
            else
            {
                lbConfirmacion.Text = "El formato de la hora de inicio no es válido!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacion.Visible = true;
                return;
            }

            int tempF = Convert.ToInt32(HoraF.Text);

            if (tempI == tempF)
            {
                lbConfirmacion.Text = "La hora de inicio y la hora final no pueden ser la misma!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-warning";
                notificacion.Visible = true;
                return;
            }

            if ((tempF >= 0) & (tempF <= 24))
            {
                if (tempI > tempF)
                {
                    lbConfirmacion.Text = "La hora inicial no puede ser mayor que la final!";
                    notificacion.Attributes["Class"] = "alert alert-dismissible alert-warning";
                    notificacion.Visible = true;
                    return;
                }
                if (tempF > 0)
                    tempF--;
                HoraFin = $"{tbFechaIni.Text} {tempF:00}:59:59.998";
            }
            else
            {
                lbConfirmacion.Text = "El formato de la hora final no es válido!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacion.Visible = true;
                return;
            }
        }
        catch (Exception)
        {
            TablaInfoTurnos.Visible = false;
            lbConfirmacion.Text = "El formato de la hora no es válido!";
            notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
            notificacion.Visible = true;
            return;
        }
        msgError = blEstadi.ConsultaNumeroTurnosHora(ref dsTurnos, "SOLICITADOS", ddlOficinaId.SelectedValue, HoraInicio, HoraFin);
        if (msgError == "")
        {
            gvTurnosTotales.DataSource = dsTurnos;
            gvTurnosTotales.DataBind();
            if (gvTurnosTotales.Rows.Count > 0)
            {
                notificacion.Visible = false;
                TablaInfoTurnos.Visible = true;
                gvTurnosTotales.HeaderRow.TableSection = TableRowSection.TableHeader;
                for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
                {
                    String Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                    Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();
                    horasTurno info = new horasTurno();

                    int hora = Convert.ToInt32(dsTurnos.Tables[0].Rows[i]["hora"].ToString());

                    turnosSede ts = new turnosSede()
                    {
                        sede = dsTurnos.Tables[0].Rows[i]["Oficina"].ToString(),
                        cantTurnos = Convert.ToInt32(dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString())
                    };
                    if (!listaOficinas.Contains(ts.sede))
                        listaOficinas.Add(ts.sede);

                    horasTurno t = turnosHora.Select(x => x).Where(y => y.hora == hora).FirstOrDefault();
                    if (t == null)
                    {
                        t = new horasTurno();
                        t.hora = hora;
                        t.horaTexto = $"{hora}-{hora + 1}";
                    }
                    else
                    {
                        turnosHora.Remove(t);
                    }
                    t.listSedes.Add(ts);
                    turnosHora.Add(t);
                }

                turnosHora = turnosHora.OrderBy(x =>x.hora).ToList();
                foreach (var th in turnosHora)
                {
                    JSonData += "{'hora': '" + th.horaTexto + "'";
                    foreach (var hs in th.listSedes)
                    {
                        JSonData += ", '" + hs.sede + "': " + hs.cantTurnos;
                    }
                    JSonData += "}, ";
                }

                string Oficinas = "";
                foreach (var o in listaOficinas)
                {
                    Oficinas += $"{o},";
                }
                Oficinas = Oficinas.Substring(0, Oficinas.Length - 1);
                Oficinas += "";

                listaOficinas.Clear();
                turnosHora.Clear();

                JSonData += "];";
                script += " \n var data=" + JSonData + $"\n countChart(data, 'chartdiv', 'Turnos solicitados por hora', '{Oficinas}'); \n";
            }
            else
            {
                notificacion.Visible = false;
                TablaInfoTurnos.Visible = false;
                lbConfirmacion.Text = "No existe información para mostrar!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-info";
                notificacion.Visible = true;
            }
        };
        msgError = blEstadi.ConsultaNumeroTurnosHora(ref dsTurnos, "LLAMADOS", ddlOficinaId.SelectedValue, HoraInicio, HoraFin);
        if (msgError == "")
        {
            JSonData = "[";
            gvAtendidos.DataSource = dsTurnos;
            gvAtendidos.DataBind();
            if (gvAtendidos.Rows.Count > 0)
            {
                gvAtendidos.HeaderRow.TableSection = TableRowSection.TableHeader;

                for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
                {
                    String Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                    Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();
                    horasTurno info = new horasTurno();

                    int hora = Convert.ToInt32(dsTurnos.Tables[0].Rows[i]["hora"].ToString());

                    turnosSede ts = new turnosSede()
                    {
                        sede = dsTurnos.Tables[0].Rows[i]["Oficina"].ToString(),
                        cantTurnos = Convert.ToInt32(dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString())
                    };
                    if (!listaOficinas.Contains(ts.sede))
                        listaOficinas.Add(ts.sede);

                    horasTurno t = turnosHora.Select(x => x).Where(y => y.hora == hora).FirstOrDefault();
                    if (t == null)
                    {
                        t = new horasTurno();
                        t.hora = hora;
                        t.horaTexto = $"{hora}-{hora + 1}";
                    }
                    else
                    {
                        turnosHora.Remove(t);
                    }
                    t.listSedes.Add(ts);
                    turnosHora.Add(t);
                }

                turnosHora = turnosHora.OrderBy(x => x.hora).ToList();
                foreach (var th in turnosHora)
                {
                    JSonData += "{'hora': '" + th.horaTexto + "'";
                    foreach (var hs in th.listSedes)
                    {
                        JSonData += ", '" + hs.sede + "': " + hs.cantTurnos;
                    }
                    JSonData += "}, ";
                }

                string Oficinas = "";
                foreach (var o in listaOficinas)
                {
                    Oficinas += $"{o},";
                }
                Oficinas = Oficinas.Substring(0, Oficinas.Length - 1);
                Oficinas += "";

                listaOficinas.Clear();
                turnosHora.Clear();

                JSonData += "];";
                script += " \n var data1=" + JSonData + $"\n countChart(data1, 'chartdivLlamados', 'Turnos llamados por hora', '{Oficinas}'); \n";
            }
        };
        msgError = blEstadi.ConsultaNumeroTurnosHora(ref dsTurnos, "CERRADOS", ddlOficinaId.SelectedValue, HoraInicio, HoraFin);
        if (msgError == "")
        {
            JSonData = "[";
            gvCerrados.DataSource = dsTurnos;
            gvCerrados.DataBind();
            if (gvCerrados.Rows.Count > 0)
            {
                gvCerrados.HeaderRow.TableSection = TableRowSection.TableHeader;

                notificacion.Visible = false;
                TablaInfoTurnos.Visible = true;
                gvTurnosTotales.HeaderRow.TableSection = TableRowSection.TableHeader;
                for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
                {
                    String Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                    Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();
                    horasTurno info = new horasTurno();

                    int hora = Convert.ToInt32(dsTurnos.Tables[0].Rows[i]["hora"].ToString());

                    turnosSede ts = new turnosSede()
                    {
                        sede = dsTurnos.Tables[0].Rows[i]["Oficina"].ToString(),
                        cantTurnos = Convert.ToInt32(dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString())
                    };
                    if (!listaOficinas.Contains(ts.sede))
                        listaOficinas.Add(ts.sede);

                    horasTurno t = turnosHora.Select(x => x).Where(y => y.hora == hora).FirstOrDefault();
                    if (t == null)
                    {
                        t = new horasTurno();
                        t.hora = hora;
                        t.horaTexto = $"{hora}-{hora + 1}";
                    }
                    else
                    {
                        turnosHora.Remove(t);
                    }
                    t.listSedes.Add(ts);
                    turnosHora.Add(t);
                }

                turnosHora = turnosHora.OrderBy(x => x.hora).ToList();
                foreach (var th in turnosHora)
                {
                    JSonData += "{'hora': '" + th.horaTexto + "'";
                    foreach (var hs in th.listSedes)
                    {
                        JSonData += ", '" + hs.sede + "': " + hs.cantTurnos;
                    }
                    JSonData += "}, ";
                }

                string Oficinas = "";
                foreach (var o in listaOficinas)
                {
                    Oficinas += $"{o},";
                }
                Oficinas = Oficinas.Substring(0, Oficinas.Length - 1);
                Oficinas += "";

                listaOficinas.Clear();
                turnosHora.Clear();

                JSonData += "];";
                script += " \n var data2=" + JSonData + $"\n countChart(data2, 'chartdivCerrados', 'Turnos Cerrados por hora', '{Oficinas}'); \n";
            }
        };
        msgError = blEstadi.ConsultaNumeroTurnosHora(ref dsTurnos, "ABANDONADOS", ddlOficinaId.SelectedValue, HoraInicio, HoraFin);
        if (msgError == "")
        {
            JSonData = "[";
            gvAbandonados.DataSource = dsTurnos;
            gvAbandonados.DataBind();
            if (gvAbandonados.Rows.Count > 0)
            {
                gvAbandonados.HeaderRow.TableSection = TableRowSection.TableHeader;
                notificacion.Visible = false;
                TablaInfoTurnos.Visible = true;
                gvTurnosTotales.HeaderRow.TableSection = TableRowSection.TableHeader;
                for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
                {
                    String Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                    Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();
                    horasTurno info = new horasTurno();

                    int hora = Convert.ToInt32(dsTurnos.Tables[0].Rows[i]["hora"].ToString());

                    turnosSede ts = new turnosSede()
                    {
                        sede = dsTurnos.Tables[0].Rows[i]["Oficina"].ToString(),
                        cantTurnos = Convert.ToInt32(dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString())
                    };
                    if (!listaOficinas.Contains(ts.sede))
                        listaOficinas.Add(ts.sede);

                    horasTurno t = turnosHora.Select(x => x).Where(y => y.hora == hora).FirstOrDefault();
                    if (t == null)
                    {
                        t = new horasTurno();
                        t.hora = hora;
                        t.horaTexto = $"{hora}-{hora + 1}";
                    }
                    else
                    {
                        turnosHora.Remove(t);
                    }
                    t.listSedes.Add(ts);
                    turnosHora.Add(t);
                }

                turnosHora = turnosHora.OrderBy(x => x.hora).ToList();
                foreach (var th in turnosHora)
                {
                    JSonData += "{'hora': '" + th.horaTexto + "'";
                    foreach (var hs in th.listSedes)
                    {
                        JSonData += ", '" + hs.sede + "': " + hs.cantTurnos;
                    }
                    JSonData += "}, ";
                }

                string Oficinas = "";
                foreach (var o in listaOficinas)
                {
                    Oficinas += $"{o},";
                }
                Oficinas = Oficinas.Substring(0, Oficinas.Length - 1);
                Oficinas += "";

                listaOficinas.Clear();
                turnosHora.Clear();

                JSonData += "];";
                script += " \n var data3=" + JSonData + $"\n countChart(data3, 'chartdivAbandonados', 'Turnos abandonados por hora', '{Oficinas}'); \n";
            }
        };
        if (script!="")
            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
    }


    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        Filtrar();
    }
}
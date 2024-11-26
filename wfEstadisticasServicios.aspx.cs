using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;


public class Turn
{
    public string id { get; set; }
    public string servicioTipoAtencion { get; set; }
    public string oficina { get; set; }
    public string canal { get; set; }
    public string estado { get; set; }
    public string consecutivo { get; set; }
    public string espera { get; set; }
    public string desplazamiento { get; set; }
    public string atencion { get; set; }
    public string generado { get; set; }
    public string userId { get; set; }
}

public class TurnosSincronizar
{
    public Turn[] turns { get; set; }
    public Turn[] llamados { get; set; }
    public Turn[] enAtencion { get; set; }
    public Turn[] cerrados { get; set; }
    public Turn[] abandonados { get; set; }
}

public class TurnosSincronizados
{
    public string[] turns { get; set; }
    public string[] llamados { get; set; }
    public string[] enAtencion { get; set; }
    public string[] cerrados { get; set; }
    public string[] abandonados { get; set; }
}


public partial class wfEstadisticasServicios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUtiles blU = new clsblUtiles();

        if (!Page.IsPostBack)
        {
            blU.LlenaDDLObligatorio(ddlServicioId, "Servicios", "Id", "Nombre", "", "", "Nombre");
            blU.LlenaDDLObligatorio(ddlOficinaId, "Oficinas", "Id", "Nombre", "", "", "Nombre");
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
            if (tbFechaIni.Text != "")
                fecha = blU.FechaDeString(tbFechaIni.Text);
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
        msgError = blEstadi.ConsultaNumeroTurnosServicios(ref dsTurnos, "SOLICITADOS", ddlOficinaId.SelectedValue, ddlServicioId.SelectedValue, tbFechaIni.Text, tbFechaFin.Text);
        if (msgError == "")
        {
            gvTurnosTotales.DataSource = dsTurnos;
            gvTurnosTotales.DataBind();
            if (gvTurnosTotales.Rows.Count > 0)
            {
                gvTurnosTotales.HeaderRow.TableSection = TableRowSection.TableHeader;
                for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
                {
                    String Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                    Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();

                    JSonData += "{'groupname': '" + dsTurnos.Tables[0].Rows[i]["Abreviatura"].ToString() + "', 'count': " + dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
                    if (i < dsTurnos.Tables[0].Rows.Count - 1)
                        JSonData += ",";
                }
                JSonData += "];";
                script += " \n var data=" + JSonData + "\n countChart(data, 'chartdiv', 'Número de turnos solicitados por servicio'); \n";
            }
            else
            {

            }
        };
        msgError = blEstadi.ConsultaNumeroTurnosServicios(ref dsTurnos, "LLAMADOS", ddlOficinaId.SelectedValue, ddlServicioId.SelectedValue, tbFechaIni.Text, tbFechaFin.Text);
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

                    JSonData += "{'groupname': '" + dsTurnos.Tables[0].Rows[i]["Abreviatura"].ToString() + "', 'count': " + dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
                    if (i < dsTurnos.Tables[0].Rows.Count - 1)
                        JSonData += ",";
                }
                JSonData += "];";
                script += " \n var data1=" + JSonData + "\n countChart(data1, 'chartdivLlamados', 'Número de turnos llamados por servicio'); \n";
            }
        };
        msgError = blEstadi.ConsultaNumeroTurnosServicios(ref dsTurnos, "CERRADOS", ddlOficinaId.SelectedValue, ddlServicioId.SelectedValue, tbFechaIni.Text, tbFechaFin.Text);
        if (msgError == "")
        {
            JSonData = "[";
            gvCerrados.DataSource = dsTurnos;
            gvCerrados.DataBind();
            if (gvCerrados.Rows.Count > 0)
            {
                gvCerrados.HeaderRow.TableSection = TableRowSection.TableHeader;
                for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
                {
                    String Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                    Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();

                    JSonData += "{'groupname': '" + dsTurnos.Tables[0].Rows[i]["Abreviatura"].ToString() + "', 'count': " + dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
                    if (i < dsTurnos.Tables[0].Rows.Count - 1)
                        JSonData += ",";
                }
                JSonData += "];";
                script += " \n var data2=" + JSonData + "\n countChart(data2, 'chartdivCerrados', 'Número de turnos cerrados por servicio'); \n";
            }
        };
        msgError = blEstadi.ConsultaNumeroTurnosServicios(ref dsTurnos, "ABANDONADOS", ddlOficinaId.SelectedValue, ddlServicioId.SelectedValue, tbFechaIni.Text, tbFechaFin.Text);
        if (msgError == "")
        {
            JSonData = "[";
            gvAbandonados.DataSource = dsTurnos;
            gvAbandonados.DataBind();
            if (gvAbandonados.Rows.Count > 0)
            {
                gvAbandonados.HeaderRow.TableSection = TableRowSection.TableHeader;
                for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
                {
                    String Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                    Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();

                    JSonData += "{'groupname': '" + dsTurnos.Tables[0].Rows[i]["Abreviatura"].ToString() + "', 'count': " + dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
                    if (i < dsTurnos.Tables[0].Rows.Count - 1)
                        JSonData += ",";
                }
                JSonData += "];";
                script += " \n var data3=" + JSonData + "\n countChart(data3, 'chartdivAbandonados', 'Número de turnos abandonados por servicio'); \n";
            }
        };

        /* 2021 - TURNOS AGENDADOS */
        /*     msgError = blEstadi.ConsultaNumeroTurnosAgendadosServicio(ref dsTurnos, "AGENDADOS", ddlOficinaId.SelectedValue, ddlServicioId.SelectedValue, tbFechaIni.Text, tbFechaFin.Text);
             if (msgError == "")
             {
                 JSonData = "[";
                 gvAgendados.DataSource = dsTurnos;
                 gvAgendados.DataBind();
                 if (gvAgendados.Rows.Count > 0)
                 {
                     gvAgendados.HeaderRow.TableSection = TableRowSection.TableHeader;
                     for (i = 0; i < dsTurnos.Tables[0].Rows.Count; i++)
                     {
                         String Mean = dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString();
                         Mean = Convert.ToInt32(decimal.Parse(Mean) * 100).ToString();

                         JSonData += "{'groupname': '" + dsTurnos.Tables[0].Rows[i]["Abreviatura"].ToString() + "', 'count': " + dsTurnos.Tables[0].Rows[i]["NoTurnos"].ToString() + " }";
                         if (i < dsTurnos.Tables[0].Rows.Count - 1)
                             JSonData += ",";
                     }
                     JSonData += "];";
                     script += " \n var data4=" + JSonData + "\n countChart(data4, 'chartdivAgendados', 'Número de turnos agendados por servicio'); \n";
                 }
             };*/

        if (script != "")
            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
    }

    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        Filtrar();
    }

    protected void btnFiltrar0_Click(object sender, EventArgs e)
    {
        string Json = "{\"turns\":[{\"id\":1,\"servicioTipoAtencion\":1,\"oficina\":101,\"canal\":1,\"estado\":6,\"consecutivo\":1,\"espera\":87,\"desplazamiento\":53,\"atencion\":2375,\"generado\":\"08/11/2019 21:08:46\",\"userId\":\"1019077168\"},{ \"id\":2,\"servicioTipoAtencion\":1,\"oficina\":101,\"canal\":1,\"estado\":2,\"consecutivo\":2,\"espera\":0,\"desplazamiento\":0,\"atencion\":0,\"generado\":\"08/11/2019 21:09:27\",\"userId\":\"1019077168\"}],\"llamados\":[],\"enAtencion\":[],\"cerrados\":[],\"abandonados\":[]}";
        string JSonReturn = "";
//        SincronizarDatos(Json);
    }

    public TurnosSincronizados SincronizarDatos(TurnosSincronizar JSonASincronizar)
    {
        DataSet dsInterno = null;
        NSSSqlUtil objNSS = new NSSSqlUtil();
        clsblUtiles blU = new clsblUtiles();
        string msgError = "", Id = "", fechaJSon = "", IdInsertado = "";
        DateTime fechaTurno;
        int i = 0;
        //string Json = "{\"turns\":[{\"id\":1,\"servicioTipoAtencion\":1,\"oficina\":101,\"canal\":1,\"estado\":6,\"consecutivo\":1,\"espera\":87,\"desplazamiento\":53,\"atencion\":2375,\"generado\":\"08/11/2019 21:08:46\",\"userId\":\"1019077168\"},{ \"id\":2,\"servicioTipoAtencion\":1,\"oficina\":101,\"canal\":1,\"estado\":2,\"consecutivo\":2,\"espera\":0,\"desplazamiento\":0,\"atencion\":0,\"generado\":\"08/11/2019 21:09:27\",\"userId\":\"1019077168\"}],\"llamados\":[],\"enAtencion\":[],\"cerrados\":[],\"abandonados\":[]}";
        int countVa = 0;
        clsblEstadisticas blEsta = new clsblEstadisticas();
        TurnosSincronizados turnosRetornar = new TurnosSincronizados();
        string[] TurnosTemp = new string[1000];
        string[] LlamdosTemp = new string[1000];
        string[] enAtencionTemp = new string[1000];
        string[] cerradosTemp = new string[1000];
        string[] abandonadosTemp = new string[1000];


        try
        {
            // Inserta en la tabla turnos
            if (JSonASincronizar.turns.Count() > 0)
            {
                for (i = 0; i < JSonASincronizar.turns.Count(); i++)
                {
                    string TurnoId = "";
                    fechaJSon = JSonASincronizar.turns[0].generado;
                    string[] PartesFecha = fechaJSon.Split(' ');
                    string[] DateParts = PartesFecha[0].Split('/');
                    string[] TimeParts = PartesFecha[1].Split(':');
                    fechaTurno = new DateTime(Convert.ToInt32(DateParts[2]), Convert.ToInt32(DateParts[1]), Convert.ToInt32(DateParts[0]), Convert.ToInt32(TimeParts[0]), Convert.ToInt32(TimeParts[1]), Convert.ToInt32(TimeParts[2]));
                    objNSS = new NSSSqlUtil();
                    objNSS.NombreTabla = "Turnos";
                    objNSS.LlavePrimaria = "Id";
                    objNSS.IsIdentity = true;
                    objNSS.Add("TurnoOficinaId", JSonASincronizar.turns[i].id);
                    objNSS.Add("ServicioTipoAtencionId", JSonASincronizar.turns[i].servicioTipoAtencion);
                    objNSS.Add("OficinaId", JSonASincronizar.turns[i].oficina);
                    objNSS.Add("CanalId", JSonASincronizar.turns[i].canal);
                    objNSS.Add("Estado", JSonASincronizar.turns[i].estado);
                    objNSS.Add("Consecutivo", JSonASincronizar.turns[i].consecutivo);
                    objNSS.Add("Tiempo_Espera", JSonASincronizar.turns[i].espera);
                    objNSS.Add("Tiempo_Desplazamiento", JSonASincronizar.turns[i].desplazamiento);
                    objNSS.Add("Tiempo_Atencion", JSonASincronizar.turns[i].atencion);
                    objNSS.Add("Hora_Generado", blU.FechaAString(fechaTurno));
                    objNSS.Add("UserId", JSonASincronizar.turns[i].userId);
                    objNSS.Add("Sincronizado", "1");
                    msgError = blEsta.ConsultaExistenciaTurnoOficina(ref TurnoId, JSonASincronizar.turns[i].id, JSonASincronizar.turns[i].oficina);
                    if (TurnoId == "")
                    {
                        objNSS.NssEjecutarSQL("INSERT", ref dsInterno, ref IdInsertado, "", "", null, null);
                        TurnosTemp[countVa] = JSonASincronizar.turns[i].id;
                        countVa++;
                    }
                    else
                    {
                        objNSS.Add("Id", TurnoId);
                        objNSS.NssEjecutarSQL("UPDATE", ref dsInterno, ref IdInsertado, "", "", null, null);
                        IdInsertado = TurnoId;
                    }
                }
            }
            Array.Resize(ref TurnosTemp, countVa);

            // Inserta en la tabla turnosllamados
            countVa = 0;
            if (JSonASincronizar.llamados.Count() > 0)
            {
            }
            Array.Resize(ref LlamdosTemp, countVa);

            // Inserta en la tabla enAtencion
            countVa = 0;
            if (JSonASincronizar.enAtencion.Count() > 0)
            {
            }
            Array.Resize(ref enAtencionTemp, countVa);

            // Inserta en la tabla turnoscerrados
            countVa = 0;
            if (JSonASincronizar.cerrados.Count() > 0)
            {
            }
            Array.Resize(ref cerradosTemp, countVa);

            // Inserta en la tabla turnosabandonados
            countVa = 0;
            if (JSonASincronizar.abandonados.Count() > 0)
            {
            }
            Array.Resize(ref abandonadosTemp, countVa);

            turnosRetornar.turns = TurnosTemp;
            turnosRetornar.llamados = LlamdosTemp;
            turnosRetornar.enAtencion = enAtencionTemp;
            turnosRetornar.cerrados = cerradosTemp;
            turnosRetornar.abandonados = abandonadosTemp;
            //            JSonInsertados = JsonConvert.SerializeObject(TurnosIds);
        }
        catch (Exception ex)
        {
            msgError = ex.ToString();
        }
        return turnosRetornar;
    }

}
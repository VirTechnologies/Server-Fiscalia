using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public class Turn
{
    public int id { get; set; }
    public int servicioTipoAtencion { get; set; }
    public int oficina { get; set; }
    public int canal { get; set; }
    public int estado { get; set; }
    public int consecutivo { get; set; }
    public int espera { get; set; }
    public int desplazamiento { get; set; }
    public int atencion { get; set; }
    public string generado { get; set; }
    public string userId { get; set; }
}

public class OthersTurns
{
    public int id { get; set; }
    public int turno { get; set; }
    public int prioridad { get; set; }
    public int puesto { get; set; }
    public string hora { get; set; }
}

public class ClosedTurns
{
    public int id { get; set; }
    public int turno { get; set; }
    public int prioridad { get; set; }
    public int puesto { get; set; }
    public string hora { get; set; }
    public string observaciones { get; set; }
}

public class TurnosSincronizar
{
    public Turn[] turns { get; set; }
    public OthersTurns[] llamados { get; set; }
    public OthersTurns[] enAtencion { get; set; }
    public ClosedTurns[] cerrados { get; set; }
    public OthersTurns[] abandonados { get; set; }
}

public class TurnosSincronizados
{
    public string[] turns { get; set; }
    public string[] llamados { get; set; }
    public string[] enAtencion { get; set; }
    public string[] cerrados { get; set; }
    public string[] abandonados { get; set; }
}



public partial class wfServicios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //Configura los botones de acuerdo a los permisos
        if (!(objUsuario.PermisoModulo("50", blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
            btnAgregar.Visible = false;
        //if (txSQL.Text != "")
            Filtrar();
        if (blU.ValorObjetoString(Session["Volver"]) == "S")
        {
            tbNombre.Text = Session["tbnombre"].ToString();
            if (Session["txSQL"].ToString() != "")
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["tbnombre"] = "";
        }
    }

    private void Filtrar()
    {
        DataSet dsServicios = new DataSet();
        clsblParametricas blParam = new clsblParametricas();
        String msgError;
        String strSQL = "";

        msgError = blParam.ConsultaServicios(ref dsServicios, "", tbNombre.Text);
        if (msgError == "")
        {
            gvServicios.DataSource = dsServicios;

            //cbVisibleKiosco.Checked = false;
            gvServicios.DataBind();

            if (gvServicios.Rows.Count <= 0)
            {
                lblNoRegistros.Visible = tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
            }
            else
            {
                tbNoRegistros.Text = gvServicios.Rows.Count.ToString();
                lblNoRegistros.Visible = tbNoRegistros.Visible = true;
                lblSinRegistros.Visible = false;
            }

            txSQL.Text = strSQL;
            if (gvServicios.Rows.Count > 0)
                gvServicios.HeaderRow.TableSection = TableRowSection.TableHeader;
            CrearTablaServicios();
        }
        else
        {
            txSQL.Text = "";
            return;
        }
    }

    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        Filtrar();

    }

    public TurnosSincronizados SincronizarDatos(TurnosSincronizar JSonASincronizar)
    {
        DataSet dsInterno = null;
        NSSSqlUtil objNSS = new NSSSqlUtil();
        clsblUtiles blU = new clsblUtiles();
        string msgError = "", OficinaId = "", fechaJSon = "", IdInsertado = "";
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
            if (JSonASincronizar.turns != null)
            {
                // Inserta en la tabla turnos
                if (JSonASincronizar.turns.Count() > 0)
                {
                    OficinaId = JSonASincronizar.turns[0].oficina.ToString();
                    for (i = 0; i < JSonASincronizar.turns.Count(); i++)
                    {
                        string TurnoId = "";
                        fechaJSon = JSonASincronizar.turns[i].generado;
                        string[] PartesFecha = fechaJSon.Split(' ');
                        string[] DateParts = PartesFecha[0].Split('/');
                        string[] TimeParts = PartesFecha[1].Split(':');
                        fechaTurno = new DateTime(Convert.ToInt32(DateParts[2]), Convert.ToInt32(DateParts[1]), Convert.ToInt32(DateParts[0]), Convert.ToInt32(TimeParts[0]), Convert.ToInt32(TimeParts[1]), Convert.ToInt32(TimeParts[2]));
                        objNSS = new NSSSqlUtil();
                        objNSS.NombreTabla = "Turnos";
                        objNSS.LlavePrimaria = "Id";
                        objNSS.IsIdentity = true;
                        objNSS.Add("TurnoOficinaId", JSonASincronizar.turns[i].id.ToString());
                        objNSS.Add("ServicioTipoAtencionId", JSonASincronizar.turns[i].servicioTipoAtencion.ToString());
                        objNSS.Add("OficinaId", JSonASincronizar.turns[i].oficina.ToString());
                        objNSS.Add("CanalId", JSonASincronizar.turns[i].canal.ToString());
                        objNSS.Add("Estado", JSonASincronizar.turns[i].estado.ToString());
                        objNSS.Add("Consecutivo", JSonASincronizar.turns[i].consecutivo.ToString());
                        objNSS.Add("Tiempo_Espera", JSonASincronizar.turns[i].espera.ToString());
                        objNSS.Add("Tiempo_Desplazamiento", JSonASincronizar.turns[i].desplazamiento.ToString());
                        objNSS.Add("Tiempo_Atencion", JSonASincronizar.turns[i].atencion.ToString());
                        objNSS.Add("Hora_Generado", blU.FechaHoraAString(fechaTurno));
                        objNSS.Add("UserId", JSonASincronizar.turns[i].userId);
                        objNSS.Add("Sincronizado", "1");
                        msgError = blEsta.ConsultaExistenciaTurnoOficina(ref TurnoId, JSonASincronizar.turns[i].id.ToString(), JSonASincronizar.turns[i].oficina.ToString());
                        if (TurnoId == "")
                        {
                            objNSS.NssEjecutarSQL("INSERT", ref dsInterno, ref IdInsertado, "", "", null, null);
                            TurnosTemp[countVa] = JSonASincronizar.turns[i].id.ToString();
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
                if (OficinaId != "")
                {
                    // Inserta en la tabla turnosllamados
                    countVa = 0;
                    if (JSonASincronizar.llamados.Count() > 0)
                    {
                        for (i = 0; i < JSonASincronizar.llamados.Count(); i++)
                        {
                            string TurnoId = "";
                            string TurnoTablaId = "";

                            msgError = blEsta.ConsultaExistenciaTurnoOficina(ref TurnoId, JSonASincronizar.llamados[i].turno.ToString(), OficinaId);
                            if (msgError == "" && TurnoId != "")
                            {
                                msgError = blEsta.ConsultaExistenciaTurnosTabla(ref TurnoTablaId, TurnoId, "TurnosLlamados");
                                fechaJSon = JSonASincronizar.llamados[i].hora;
                                string[] PartesFecha = fechaJSon.Split(' ');
                                string[] DateParts = PartesFecha[0].Split('/');
                                string[] TimeParts = PartesFecha[1].Split(':');
                                fechaTurno = new DateTime(Convert.ToInt32(DateParts[2]), Convert.ToInt32(DateParts[1]), Convert.ToInt32(DateParts[0]), Convert.ToInt32(TimeParts[0]), Convert.ToInt32(TimeParts[1]), Convert.ToInt32(TimeParts[2]));
                                objNSS = new NSSSqlUtil();
                                objNSS.NombreTabla = "TurnosLlamados";
                                objNSS.LlavePrimaria = "Id";
                                objNSS.IsIdentity = true;
                                objNSS.Add("TurnoId", TurnoId);
                                objNSS.Add("PrioridadDeAsesorId", JSonASincronizar.llamados[i].prioridad.ToString());
                                objNSS.Add("PuestoDeAtencionId", JSonASincronizar.llamados[i].puesto.ToString());
                                objNSS.Add("Hora", blU.FechaHoraAString(fechaTurno));
                                objNSS.Add("Sincronizado", "1");
                                if (TurnoTablaId == "")
                                {
                                    objNSS.NssEjecutarSQL("INSERT", ref dsInterno, ref IdInsertado, "", "", null, null);
                                    LlamdosTemp[countVa] = JSonASincronizar.llamados[i].id.ToString();
                                    countVa++;
                                }
                                else
                                {
                                    objNSS.Add("Id", TurnoTablaId);
                                    objNSS.NssEjecutarSQL("UPDATE", ref dsInterno, ref IdInsertado, "", "", null, null);
                                    IdInsertado = TurnoId;
                                }
                            }
                        }
                    }
                    Array.Resize(ref LlamdosTemp, countVa);

                    // Inserta en la tabla enAtencion
                    countVa = 0;
                    if (JSonASincronizar.enAtencion.Count() > 0)
                    {
                        for (i = 0; i < JSonASincronizar.enAtencion.Count(); i++)
                        {
                            string TurnoId = "";
                            string TurnoTablaId = "";

                            msgError = blEsta.ConsultaExistenciaTurnoOficina(ref TurnoId, JSonASincronizar.enAtencion[i].turno.ToString(), OficinaId);
                            if (msgError == "" && TurnoId != "")
                            {
                                msgError = blEsta.ConsultaExistenciaTurnosTabla(ref TurnoTablaId, TurnoId, "TurnosAtendidos");
                                fechaJSon = JSonASincronizar.enAtencion[i].hora;
                                string[] PartesFecha = fechaJSon.Split(' ');
                                string[] DateParts = PartesFecha[0].Split('/');
                                string[] TimeParts = PartesFecha[1].Split(':');
                                fechaTurno = new DateTime(Convert.ToInt32(DateParts[2]), Convert.ToInt32(DateParts[1]), Convert.ToInt32(DateParts[0]), Convert.ToInt32(TimeParts[0]), Convert.ToInt32(TimeParts[1]), Convert.ToInt32(TimeParts[2]));
                                objNSS = new NSSSqlUtil();
                                objNSS.NombreTabla = "TurnosAtendidos";
                                objNSS.LlavePrimaria = "Id";
                                objNSS.IsIdentity = true;
                                objNSS.Add("TurnoId", TurnoId);
                                objNSS.Add("PrioridadDeAsesorId", JSonASincronizar.enAtencion[i].prioridad.ToString());
                                objNSS.Add("PuestoDeAtencionId", JSonASincronizar.enAtencion[i].puesto.ToString());
                                objNSS.Add("Hora", blU.FechaHoraAString(fechaTurno));
                                objNSS.Add("Sincronizado", "1");
                                if (TurnoTablaId == "")
                                {
                                    objNSS.NssEjecutarSQL("INSERT", ref dsInterno, ref IdInsertado, "", "", null, null);
                                    enAtencionTemp[countVa] = JSonASincronizar.enAtencion[i].id.ToString();
                                    countVa++;
                                }
                                else
                                {
                                    objNSS.Add("Id", TurnoTablaId);
                                    objNSS.NssEjecutarSQL("UPDATE", ref dsInterno, ref IdInsertado, "", "", null, null);
                                    IdInsertado = TurnoId;
                                }
                            }
                        }
                    }
                    Array.Resize(ref enAtencionTemp, countVa);

                    // Inserta en la tabla turnoscerrados
                    countVa = 0;
                    if (JSonASincronizar.cerrados.Count() > 0)
                    {
                        for (i = 0; i < JSonASincronizar.cerrados.Count(); i++)
                        {
                            string TurnoId = "";
                            string TurnoTablaId = "";

                            msgError = blEsta.ConsultaExistenciaTurnoOficina(ref TurnoId, JSonASincronizar.cerrados[i].turno.ToString(), OficinaId);
                            if (msgError == "" && TurnoId != "")
                            {
                                msgError = blEsta.ConsultaExistenciaTurnosTabla(ref TurnoTablaId, TurnoId, "TurnosCerrados");
                                fechaJSon = JSonASincronizar.cerrados[i].hora;
                                string[] PartesFecha = fechaJSon.Split(' ');
                                string[] DateParts = PartesFecha[0].Split('/');
                                string[] TimeParts = PartesFecha[1].Split(':');
                                fechaTurno = new DateTime(Convert.ToInt32(DateParts[2]), Convert.ToInt32(DateParts[1]), Convert.ToInt32(DateParts[0]), Convert.ToInt32(TimeParts[0]), Convert.ToInt32(TimeParts[1]), Convert.ToInt32(TimeParts[2]));
                                objNSS = new NSSSqlUtil();
                                objNSS.NombreTabla = "TurnosCerrados";
                                objNSS.LlavePrimaria = "Id";
                                objNSS.IsIdentity = true;
                                objNSS.Add("TurnoId", TurnoId);
                                objNSS.Add("PrioridadDeAsesorId", JSonASincronizar.cerrados[i].prioridad.ToString());
                                objNSS.Add("PuestoDeAtencionId", JSonASincronizar.cerrados[i].puesto.ToString());
                                objNSS.Add("Hora", blU.FechaHoraAString(fechaTurno));
                                objNSS.Add("Observations", JSonASincronizar.cerrados[i].observaciones);
                                objNSS.Add("Sincronizado", "1");
                                if (TurnoTablaId == "")
                                {
                                    objNSS.NssEjecutarSQL("INSERT", ref dsInterno, ref IdInsertado, "", "", null, null);
                                    cerradosTemp[countVa] = JSonASincronizar.cerrados[i].id.ToString();
                                    countVa++;
                                }
                                else
                                {
                                    objNSS.Add("Id", TurnoTablaId);
                                    objNSS.NssEjecutarSQL("UPDATE", ref dsInterno, ref IdInsertado, "", "", null, null);
                                    IdInsertado = TurnoId;
                                }
                            }
                        }
                    }
                    Array.Resize(ref cerradosTemp, countVa);

                    // Inserta en la tabla turnosabandonados
                    countVa = 0;
                    if (JSonASincronizar.abandonados.Count() > 0)
                    {
                        for (i = 0; i < JSonASincronizar.abandonados.Count(); i++)
                        {
                            string TurnoId = "";
                            string TurnoTablaId = "";

                            msgError = blEsta.ConsultaExistenciaTurnoOficina(ref TurnoId, JSonASincronizar.abandonados[i].turno.ToString(), OficinaId);
                            if (msgError == "" && TurnoId != "")
                            {
                                msgError = blEsta.ConsultaExistenciaTurnosTabla(ref TurnoTablaId, TurnoId, "TurnosAbandonados");
                                fechaJSon = JSonASincronizar.abandonados[i].hora;
                                string[] PartesFecha = fechaJSon.Split(' ');
                                string[] DateParts = PartesFecha[0].Split('/');
                                string[] TimeParts = PartesFecha[1].Split(':');
                                fechaTurno = new DateTime(Convert.ToInt32(DateParts[2]), Convert.ToInt32(DateParts[1]), Convert.ToInt32(DateParts[0]), Convert.ToInt32(TimeParts[0]), Convert.ToInt32(TimeParts[1]), Convert.ToInt32(TimeParts[2]));
                                objNSS = new NSSSqlUtil();
                                objNSS.NombreTabla = "TurnosAbandonados";
                                objNSS.LlavePrimaria = "Id";
                                objNSS.IsIdentity = true;
                                objNSS.Add("TurnoId", TurnoId);
                                objNSS.Add("PrioridadDeAsesorId", JSonASincronizar.abandonados[i].prioridad.ToString());
                                objNSS.Add("PuestoDeAtencionId", JSonASincronizar.abandonados[i].puesto.ToString());
                                objNSS.Add("Hora", blU.FechaHoraAString(fechaTurno));
                                objNSS.Add("Sincronizado", "1");
                                if (TurnoTablaId == "")
                                {
                                    objNSS.NssEjecutarSQL("INSERT", ref dsInterno, ref IdInsertado, "", "", null, null);
                                    abandonadosTemp[countVa] = JSonASincronizar.abandonados[i].id.ToString();
                                    countVa++;
                                }
                                else
                                {
                                    objNSS.Add("Id", TurnoTablaId);
                                    objNSS.NssEjecutarSQL("UPDATE", ref dsInterno, ref IdInsertado, "", "", null, null);
                                    IdInsertado = TurnoId;
                                }
                            }
                        }
                    }
                    Array.Resize(ref abandonadosTemp, countVa);
                }
                turnosRetornar.turns = TurnosTemp;
                turnosRetornar.llamados = LlamdosTemp;
                turnosRetornar.enAtencion = enAtencionTemp;
                turnosRetornar.cerrados = cerradosTemp;
                turnosRetornar.abandonados = abandonadosTemp;
                //            JSonInsertados = JsonConvert.SerializeObject(TurnosIds);
            }
        }
        catch (Exception ex)
        {
            msgError = ex.ToString();
        }
        return turnosRetornar;
    }


    protected void gvServicios_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsblUsuarios obj = new clsblUsuarios();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int cellIndex = -1;
            for (int i = 0; i < gvServicios.Columns.Count; i++)
            {
                if (gvServicios.Columns[i].HeaderText == "Visible en el Kiosco")
                {
                    cellIndex = i;
                    break;
                }
            }

            if (cellIndex != -1)
            {

                if (e.Row.Cells[cellIndex].Text == "True")
                {
                    e.Row.Cells[cellIndex].Text = "Sí";
                }
                else if (e.Row.Cells[cellIndex].Text == "False")
                {
                    e.Row.Cells[cellIndex].Text = "No";
                }
            }


            String idS = "";
            DataSet dsR = new DataSet();

            idS = e.Row.Cells[0].Text;
            LinkButton AdministrarButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 1].Controls[0];
            LinkButton queryButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 2].Controls[0];

            AdministrarButton.CommandName = "ADMINISTRAR";
            AdministrarButton.CommandArgument = idS;
            queryButton.CommandName = "CONSULTAR";
            queryButton.CommandArgument = idS;
            if (!obj.PermisoModulo("50", Session["IDUSUARIO"].ToString(), "U"))
                e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
            e.Row.Cells[0].Visible = false;
        }
        else
        {
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[0].Visible = false;
        }
        if (!obj.PermisoModulo("50", Session["IDUSUARIO"].ToString(), "U"))
        {
            e.Row.Cells[e.Row.Cells.Count - 1].Visible = false;
        }
    }

    protected void gvServicios_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idS = "";

        if (e.CommandName != "")
        {
            idS = e.CommandArgument.ToString();
            Session["tbnombre"] = tbNombre.Text;
            Session["txSQL"] = txSQL.Text;
        }
        if (e.CommandName.ToString() == "ADMINISTRAR")
            Response.Redirect("wfServicio.aspx?id=" + idS);
        if (e.CommandName.ToString() == "CONSULTAR")
            Response.Redirect("wfServicio.aspx?consulta=si&id=" + idS);
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        Session["tbnombre"] = tbNombre.Text;
        Session["txSQL"] = txSQL.Text;
        Response.Redirect("wfServicio.aspx");
    }

    protected void CrearTablaServicios()
    {
        clsblParametricas blParam = new clsblParametricas();
        int i = 0, j = 0, k = 0;
        clsblParametricas.Niveles[,] Levels;
        TableCell cell = null;
        TableRow row = null;
        string msgError = "", style = "";
        int nLevels = 0, nLevelsId = 0, sublevels = 0;

        Levels = new clsblParametricas.Niveles[20, 50];
        for (i = 0; i < 20; i++)
        {
            for (j = 0; j < 50; j++)
            {
                Levels[i, j].Id = "";
            }
        }
        msgError = blParam.CreaArbolServicios1(Levels, 0, "", "");
        while (Levels[nLevels, 0].Id != "")
            nLevels++;
        for (i = 0; i < nLevels; i++)
        {
            if (i == 0)
                style = "table-primary";
            if (i == 1)
                style = "table-secondary";
            if (i == 2)
                style = "table-success";
            row = new TableRow();
            row.BackColor = System.Drawing.Color.LightGray;
            nLevelsId = 0;
            while (Levels[i, nLevelsId].Id != "")
                nLevelsId++;
            for (j = 0; j < nLevelsId; j++)
            {
                sublevels = 0;
                if (i < nLevels - 1)
                {
                    k = 0;
                    while (Levels[i + 1, k].Id != "")
                    {
                        if (Levels[i + 1, k].IdPadre == Levels[i, j].Id)
                            sublevels++;
                        k++;
                    }
                }
                row.Cells.Add(CrearCelda(Levels[i, j].Nombre, true, -1, null, sublevels, style));
            }
            tablaServicios.Rows.Add(row);
            row = null;
        }
    }

    protected TableCell CrearCelda(String Texto, bool Titulo, int Width, Control ctl, int colSpan, string style)
    {
        TableCell cell = new TableCell();
        cell.Text = Texto;
        if (Width != -1)
            cell.Width = Unit.Percentage(Width);
        cell.ForeColor = System.Drawing.Color.Black;
        cell.HorizontalAlign = HorizontalAlign.Center;
        if (style != "")
            cell.CssClass = style;
        if (Titulo)
        {
            cell.Font.Bold = true;
        }
        else
        {
            cell.Font.Bold = false;
        }
        if (ctl != null)
        {
            cell.Controls.Add(ctl);
        }
        if (colSpan != -1)
            cell.ColumnSpan = colSpan;
        return cell;
    }

}
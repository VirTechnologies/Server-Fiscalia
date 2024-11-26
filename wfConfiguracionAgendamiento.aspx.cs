using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class wfPrioridades : System.Web.UI.Page
{
    
    public string url = ConfigurationManager.AppSettings["Agendamiento"];
    protected void Page_Load(object sender, EventArgs e)
    {
        clsblUsuarios objUsuario = new clsblUsuarios();
        clsblUtiles blU = new clsblUtiles();

        if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
            Response.Redirect("wfSesionTimeOut.aspx?modal=0");
        //
        //if (!(objUsuario.PermisoModulo("49", blU.ValorObjetoString(Session["IDUSUARIO"]), "I")))
            //  btnAgregar.Visible = false;
            //if (txSQL.Text != "")
                Filtrar();
        if (!Page.IsPostBack)
        {

            string respuestaAPI = ObtenerRespuestaDeAPI(url);
            if (!string.IsNullOrEmpty(respuestaAPI))
            {
                DataTable seccionales = ProcesarRespuestaSOAP(respuestaAPI);

                if (seccionales.Rows.Count > 0)
                {
                    ddlSeccionalId.DataSource = seccionales;
                    ddlSeccionalId.DataTextField = "Seccional";
                    ddlSeccionalId.DataValueField = "Id";
                    ddlSeccionalId.DataBind();
                    ddlSeccionalId.Items.Insert(0, new ListItem("Seleccione una opción", "0"));
                }
                else
                {
                    ddlSeccionalId.Items.Clear();
                    ddlSeccionalId.Items.Add(new ListItem("No se encontraron seccionales", "-1"));
                }
            }
            else
            {
                ddlSeccionalId.Items.Clear();
                ddlSeccionalId.Items.Add(new ListItem("Error al obtener datos", "-1"));
            }
        }
        if (blU.ValorObjetoString(Session["Volver"]) == "S")
        {
            if (Session["txSQL"].ToString() != "")
                Filtrar();
            Session["txSQL"] = "";
            Session["Volver"] = "";
            Session["OficinaId"] = "";
        }
    }


    private string ObtenerRespuestaDeAPI(string url)
    {
        string soapEnvelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
        <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
          <soap:Body>
            <ObtenerTodosLosSeccionales xmlns=""http://tempuri.org/"" />
          </soap:Body>
        </soap:Envelope>";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.ContentType = "text/xml; charset=utf-8";
        request.Accept = "text/xml";
        request.Method = "POST";
        request.Headers.Add("SOAPAction", "\"http://tempuri.org/ObtenerTodosLosSeccionales\"");
        using (Stream stream = request.GetRequestStream())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(soapEnvelope);
            stream.Write(bytes, 0, bytes.Length);
        }
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedSedeId = DropDownList1.SelectedValue;
        //lblSelectedSedeId.Text = selectedSedeId;

        if (!string.IsNullOrEmpty(selectedSedeId) && selectedSedeId != "0")
        {


            string respuestaAPI = ObtenerDisponibilidadAgendamientosPorIDSede(url, selectedSedeId);

            if (!string.IsNullOrEmpty(respuestaAPI))
            {
                DataTable disponibilidad = ProcesarRespuestaSOAP_Disponibilidad(respuestaAPI);
                if (disponibilidad != null && disponibilidad.Rows.Count > 0)
                {
                    gvPrioridades.DataSource = disponibilidad;
                    gvPrioridades.DataBind();
                    lblNoRegistros.Visible = true;
                    tbNoRegistros.Visible = true;
                    tbNoRegistros.Text = disponibilidad.Rows.Count.ToString();
                    lblSinRegistros.Visible = false;
                    btnFiltrarCero.Style["display"] = "flex";
                    btnFiltrarCero.Style["margin-right"] = "30px";
                    LabelSetGlobal1.Visible = true;
                    LabelSetGlobal.Visible = true;
                    labelSede.Visible = true;
                    if (Session["Nombre_Sede"] != null && !string.IsNullOrEmpty(Session["Nombre_Sede"].ToString()))
                    { labelSede.Text = Session["Nombre_Sede"].ToString(); }
                    else
                    { labelSede.Text = "Nombre de sede no disponible"; }


                }
                else
                {
                    labelSede.Visible = false;
                    LabelSetGlobal.Visible = false;
                    btnFiltrarCero.Style["display"] = "none";
                    LabelSetGlobal1.Visible = false;
                    gvPrioridades.DataSource = null;
                    gvPrioridades.DataBind();
                    lblNoRegistros.Visible = false;
                    tbNoRegistros.Visible = false;
                    lblSinRegistros.Visible = true;



                }
            }
            else
            {
                gvPrioridades.DataSource = null;
                gvPrioridades.DataBind();
                lblNoRegistros.Visible = false;
                tbNoRegistros.Visible = false;
                lblSinRegistros.Visible = true;
            }
        }
        else
        {
            gvPrioridades.DataSource = null;
            gvPrioridades.DataBind();
            lblNoRegistros.Visible = false;
            tbNoRegistros.Visible = false;
            lblSinRegistros.Visible = true;
        }
    }

    /*
    protected void btnFiltrarCinco_Click(object sender, EventArgs e)
    {
        string url = "http://localhost:62748/WSAgenda.asmx";

        int idSede = Convert.ToInt32(DropDownList1.SelectedValue);
        string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <soap:Body>
                                    <ActualizarTurnosPorFranja xmlns=""http://tempuri.org/"">
                                        <numeroTurnos>{5}</numeroTurnos>
                                        <idSede>{idSede}</idSede>
                                    </ActualizarTurnosPorFranja>
                                </soap:Body>
                            </soap:Envelope>";

        string response = SendSoapRequest5(soapRequest, url);
    }

    private string SendSoapRequest5(string soapRequest, string url)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        httpWebRequest.ContentType = "text/xml; charset=utf-8";
        httpWebRequest.Method = "POST";
        httpWebRequest.Headers.Add("SOAPAction", "\"http://tempuri.org/ActualizarTurnosPorFranja\"");

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(soapRequest);
        }
        using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
        {
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
    */

    protected void btnFiltrarCero_Click(object sender, EventArgs e)
    {
        //  string url = "http://localhost:62748/WSAgenda.asmx"; 

        int idSede = Convert.ToInt32(DropDownList1.SelectedValue);
        string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <soap:Body>
                                        <ActualizarTurnosPorFranja xmlns=""http://tempuri.org/"">
                                            <numeroTurnos>{0}</numeroTurnos>
                                            <idSede>{idSede}</idSede>
                                        </ActualizarTurnosPorFranja>
                                    </soap:Body>
                                </soap:Envelope>";
        string response = SendSoapRequest0(soapRequest, url);
    }
    private string SendSoapRequest0(string soapRequest, string url)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        httpWebRequest.ContentType = "text/xml; charset=utf-8";
        httpWebRequest.Method = "POST";
        httpWebRequest.Headers.Add("SOAPAction", "\"http://tempuri.org/ActualizarTurnosPorFranja\"");
        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(soapRequest);
        }
        using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
        {
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }
    }

    private string ObtenerDisponibilidadAgendamientosPorIDSede(string url, string idSede)
    {
        string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                  <soap:Body>
                                    <ObtenerDisponibilidadAgendamientosPorIDSede xmlns=""http://tempuri.org/"">
                                      <ID_Sede>{idSede}</ID_Sede>
                                    </ObtenerDisponibilidadAgendamientosPorIDSede>
                                  </soap:Body>
                                </soap:Envelope>";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.ContentType = "text/xml; charset=utf-8";
        request.Accept = "text/xml";
        request.Method = "POST";
        request.Headers.Add("SOAPAction", "\"http://tempuri.org/ObtenerDisponibilidadAgendamientosPorIDSede\"");
        using (Stream stream = request.GetRequestStream())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(soapEnvelope);
            stream.Write(bytes, 0, bytes.Length);
        }
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
    }

    private DataTable ProcesarRespuestaSOAP_Disponibilidad(string respuestaXML)
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("ID_Agendamiento", typeof(int));
        dataTable.Columns.Add("ID_Sede", typeof(int));

        dataTable.Columns.Add("Nombre_Sede", typeof(string));
        dataTable.Columns.Add("ID_ZonaHoraria", typeof(int));
        dataTable.Columns.Add("ID_Dia", typeof(int));
        dataTable.Columns.Add("Nombre_Dia", typeof(string));
        dataTable.Columns.Add("TurnosPorFranja", typeof(int));
        dataTable.Columns.Add("HorarioFranja", typeof(string));

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(respuestaXML);

        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
        nsmgr.AddNamespace("diffgr", "urn:schemas-microsoft-com:xml-diffgram-v1");
        nsmgr.AddNamespace("msdata", "urn:schemas-microsoft-com:xml-msdata");

        XmlNodeList nodes = doc.SelectNodes("//diffgr:diffgram/NewDataSet/Table", nsmgr);
        foreach (XmlNode node in nodes)
        {
            DataRow row = dataTable.NewRow();
            row["ID_Agendamiento"] = int.Parse(node["ID_Agendamiento"].InnerText);
            row["ID_Sede"] = int.Parse(node["ID_Sede"].InnerText);
            row["Nombre_Sede"] = node["Nombre_Sede"].InnerText;
            row["ID_ZonaHoraria"] = int.Parse(node["ID_ZonaHoraria"].InnerText);
            row["ID_Dia"] = int.Parse(node["ID_Dia"].InnerText);
            row["Nombre_Dia"] = node["Nombre_Dia"].InnerText;
            row["TurnosPorFranja"] = int.Parse(node["TurnosPorFranja"].InnerText);
            row["HorarioFranja"] = node["HorarioFranja"].InnerText;

            Session["Nombre_Sede"] = node["Nombre_Sede"].InnerText;

            dataTable.Rows.Add(row);
        }
        return dataTable;
    }

    private DataTable ProcesarRespuestaSOAP(string respuestaXML)
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("Id", typeof(int));
        dataTable.Columns.Add("Seccional", typeof(string));

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(respuestaXML);

        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
        nsmgr.AddNamespace("diffgr", "urn:schemas-microsoft-com:xml-diffgram-v1");
        nsmgr.AddNamespace("msdata", "urn:schemas-microsoft-com:xml-msdata");
        nsmgr.AddNamespace("", "http://tempuri.org/");

        XmlNodeList nodes = doc.SelectNodes("//diffgr:diffgram/NewDataSet/Table", nsmgr);
        foreach (XmlNode node in nodes)
        {
            DataRow row = dataTable.NewRow();
            row["Id"] = int.Parse(node["Id"].InnerText);
            row["Seccional"] = node["Seccional"].InnerText;
            dataTable.Rows.Add(row);
        }
        return dataTable;
    }

    private void Filtrar()
    {
        // string url = "http://localhost:62748/WSAgenda.asmx";
        string idSeccional = ddlSeccionalId.SelectedValue;
        string respuestaAPI = ObtenerSedesPorIDSeccional(url, idSeccional);

        if (!string.IsNullOrEmpty(respuestaAPI))
        {
            List<Sede> sedes = ProcesarRespuestaSOAP_Sedes(respuestaAPI);
            if (sedes != null)
            {
                gvPrioridades.DataSource = sedes;
                gvPrioridades.DataBind();
            }
            else
            {
                gvPrioridades.DataSource = null;
                gvPrioridades.DataBind();
                lblSinRegistros.Visible = true;
            }
        }
        else
        {
            gvPrioridades.DataSource = null;
            gvPrioridades.DataBind();
            lblSinRegistros.Visible = true;
        }
    }

    protected void ddlSeccionalId_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string url = "http://localhost:62748/WSAgenda.asmx";
        string idSeccional = ddlSeccionalId.SelectedValue;

        string respuestaAPI = ObtenerSedesPorIDSeccional(url, idSeccional);

        if (!string.IsNullOrEmpty(respuestaAPI))
        {
            List<Sede> sedes = ProcesarRespuestaSOAP_Sedes(respuestaAPI);
            if (sedes != null)
            {
                DropDownList1.DataSource = sedes;
                DropDownList1.DataTextField = "NombreSede";
                DropDownList1.DataValueField = "Id";
                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, new ListItem("Seleccione una opción", "0"));
            }
            else
            {
                DropDownList1.Items.Clear();
                DropDownList1.Items.Add(new ListItem("No se encontraron sedes", "-1"));
            }
        }
        else
        {
            DropDownList1.Items.Clear();
            DropDownList1.Items.Add(new ListItem("Error al obtener datos", "-1"));
        }
    }


    /*
    protected void ddlSeccionalId_SelectedIndexChanged(object sender, EventArgs e)
    {
        Filtrar();
    }*/
    private string ObtenerSedesPorIDSeccional(string url, string idSeccional)
    {
        string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <ObtenerSedesPorIDSeccional xmlns=""http://tempuri.org/"">
                  <Id_Seccional>{idSeccional}</Id_Seccional>
                </ObtenerSedesPorIDSeccional>
              </soap:Body>
            </soap:Envelope>";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.ContentType = "text/xml; charset=utf-8";
        request.Accept = "text/xml";
        request.Method = "POST";
        request.Headers.Add("SOAPAction", "\"http://tempuri.org/ObtenerSedesPorIDSeccional\"");
        using (Stream stream = request.GetRequestStream())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(soapEnvelope);
            stream.Write(bytes, 0, bytes.Length);
        }
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
    }
    private List<Sede> ProcesarRespuestaSOAP_Sedes(string respuestaXML)
    {
        List<Sede> sedes = new List<Sede>();
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(respuestaXML);
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
        nsmgr.AddNamespace("diffgr", "urn:schemas-microsoft-com:xml-diffgram-v1");
        nsmgr.AddNamespace("msdata", "urn:schemas-microsoft-com:xml-msdata");
        XmlNodeList nodes = doc.SelectNodes("//diffgr:diffgram/NewDataSet/Table", nsmgr);
        foreach (XmlNode node in nodes)
        {
            Sede sede = new Sede();

            if (node["id"] != null && !string.IsNullOrEmpty(node["id"].InnerText))
            {
                int id;
                if (int.TryParse(node["id"].InnerText, out id))
                {
                    sede.Id = id;
                }
                else
                {
                    sede.Id = 0;
                }
            }

            if (node["Nombre_Sede"] != null && !string.IsNullOrEmpty(node["Nombre_Sede"].InnerText))
            {
                sede.NombreSede = node["Nombre_Sede"].InnerText;
            }
            else
            {
                sede.NombreSede = string.Empty;
            }

            if (node["Direccion"] != null && !string.IsNullOrEmpty(node["Direccion"].InnerText))
            {
                sede.Direccion = node["Direccion"].InnerText;
            }
            else
            {
                sede.Direccion = string.Empty;
            }
            sedes.Add(sede);
        }
        return sedes;
    }




    protected void gvPrioridades_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String idS = e.Row.RowIndex.ToString();
            LinkButton queryButton = (LinkButton)e.Row.Cells[e.Row.Cells.Count - 1].Controls[0];
            queryButton.CommandName = "EDITARTURNOS";
            queryButton.CommandArgument = idS;
        }
    }


    protected void gvPrioridades_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EDITARTURNOS")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvPrioridades.Rows[rowIndex];

            HiddenField hiddenIdAgendamiento = (HiddenField)row.FindControl("HiddenID_Agendamiento");
            string idAgendamiento = hiddenIdAgendamiento.Value;
            string nombreDia = row.Cells[2].Text;
            string nombreSede = row.Cells[1].Text;
            string turnosEnFranja = row.Cells[3].Text;
            string horarioFranja = row.Cells[4].Text;

            Session["ID_Agendamiento"] = idAgendamiento;
            Session["Nombre_Dia"] = nombreDia;
            Session["Nombre_Sede"] = nombreSede;
            Session["TurnosPorFranja"] = turnosEnFranja;
            Session["HorarioFranja"] = horarioFranja;

            Response.Redirect("wfEditarFranjaTurnos.aspx");
        }
    }


    /*
    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        Session["txSQL"] = txSQL.Text;
        Response.Redirect("wfCrearSedeAgendamiento.aspx");
    }
    */

    public class Sede
    {
        public int Id { get; set; }
        public string NombreSede { get; set; }
        public string Direccion { get; set; }
    }

}
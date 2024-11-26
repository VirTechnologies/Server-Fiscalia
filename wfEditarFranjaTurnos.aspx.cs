using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;

public partial class wfVerAgendamiento : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           if (Session["ID_Agendamiento"] != null)
            {
               // LabelIDAgendamiento.Text = Session["ID_Agendamiento"].ToString();
            }
            if (Session["Nombre_Sede"] != null)
            {
                LabelNombreSede.Text = Session["Nombre_Sede"].ToString();
            }
            if (Session["Nombre_Dia"] != null)
            {
                LabelNombreDia.Text = Session["Nombre_Dia"].ToString();
            }
            if (Session["HorarioFranja"] != null)
            {
                LabelHorarioFranja.Text = Session["HorarioFranja"].ToString();
            }            
            if (Session["TurnosPorFranja"] != null)
            {
                LabelTurnosFranja.Text = Session["TurnosPorFranja"].ToString();
            }
        }
    }

    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        int idAgendamiento = int.Parse(Session["ID_Agendamiento"].ToString());
        int turnosPorFranja = int.Parse(tbNombre.Text);
        bool ok = ActualizaDisp_AgendamientoTurnos(idAgendamiento, turnosPorFranja);

        if (ok)
        {
            lblSinRegistros.Text = "La edición fue exitosa.";
            lblSinRegistros.CssClass = "badge badge-success";
            lblSinRegistros.Visible = true;
        }
        else
        {
            lblSinRegistros.Text = "La edición falló la cantidad debe ser 9 o menos.";
            lblSinRegistros.CssClass = "badge badge-danger";
            lblSinRegistros.Visible = true;
        }
    }



    private bool ActualizaDisp_AgendamientoTurnos(int idAgendamiento, int turnosPorFranja)
    {
        string url = "http://localhost:62748/WSAgenda.asmx";
        string soapAction = "http://tempuri.org/ActualizaDisp_AgendamientoTurnos";
        bool resultado = false; 

        string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
        <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
          <soap:Body>
            <ActualizaDisp_AgendamientoTurnos xmlns=""http://tempuri.org/"">
              <ID_Agendamiento>{idAgendamiento}</ID_Agendamiento>
              <TurnosPorFranja>{turnosPorFranja}</TurnosPorFranja>
            </ActualizaDisp_AgendamientoTurnos>
          </soap:Body>
        </soap:Envelope>";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Headers.Add("SOAPAction", soapAction);
        request.ContentType = "text/xml; charset=utf-8";
        request.Method = "POST";
        using (Stream stream = request.GetRequestStream())
        {
            byte[] content = Encoding.UTF8.GetBytes(soapEnvelope);
            stream.Write(content, 0, content.Length);
        }
        try
        {
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();
                   
                    resultado = soapResult.Contains("true"); 
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return resultado;
    }
    
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("wfConfiguracionAgendamiento.aspx");
    }
    

}

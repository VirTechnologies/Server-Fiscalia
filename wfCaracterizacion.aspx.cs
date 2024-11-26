using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

public partial class wfUsuarioCaja : System.Web.UI.Page
{
	clsblUsuarios objUsuario = new clsblUsuarios();
	clsblUtiles blU = new clsblUtiles();
	String msgError;
	DataSet dsAsesores = new DataSet();
	clsblParametricas blPara = new clsblParametricas();

	protected void Page_Load(object sender, EventArgs e)
	{
		//=============== ESTAS 4 LINEAS SON PARA LAS FECHAS ===============================
		DateTimeFormatInfo myDTF = new DateTimeFormatInfo();
		myDTF.ShortDatePattern = "dd/MM/yyyy";
		Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CO");
		Thread.CurrentThread.CurrentCulture.DateTimeFormat = myDTF;
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
		if (blU.ValorObjetoString(Session["IDUSUARIO"]) == "")
			Response.Redirect("wfSesionTimeOut.aspx?modal=0");

		// Configura los botones de acuerdo a los permisos
		if (!((objUsuario.PermisoModulo("58", Session["IDUSUARIO"].ToString(), "I")) || (objUsuario.PermisoModulo("58", Session["IDUSUARIO"].ToString(), "U"))))
		{
			btnGrabar.Visible = false;
			objUsuario.SoloLectura(this);
		}
		if (!objUsuario.PermisoModulo("58", Session["IDUSUARIO"].ToString(), "D"))
			btnEliminar.Visible = false;

        if (!Page.IsPostBack)
        {
            hfConsulta.Value = blU.ValorObjetoString(Request.QueryString["consulta"]);
            hfid.Value = blU.ValorObjetoString(Request.QueryString["id"]);

            if (!string.IsNullOrEmpty(hfid.Value))
            {
                msgError = blPara.ConsultaCaracterizacion(ref dsAsesores, hfid.Value);

                if (string.IsNullOrEmpty(msgError) && dsAsesores.Tables[0].Rows.Count > 0)
                {
                    var row = dsAsesores.Tables[0].Rows[0];

                    tbNombre.Text = row["Descripcion"].ToString();
                    cbHabilitado.Checked = (bool)row["Habilitado"];
                    int? idPadre = row["IdPadre"] != DBNull.Value ? (int?)row["IdPadre"] : null;

                    msgError = blPara.ConsultaCaracterizaciones(ref dsAsesores, true);

                    if (string.IsNullOrEmpty(msgError) && idPadre.HasValue)
                    {
                        var rowToRemove = dsAsesores.Tables[0].AsEnumerable()
                            .FirstOrDefault(r => r.Field<string>("Descripcion") == tbNombre.Text);

                        if (rowToRemove != null)
                            dsAsesores.Tables[0].Rows.Remove(rowToRemove);

                        CargarCaracterizacionPadre(dsAsesores, idPadre);
                    }
                }
            }
            else
            {
                btnEliminar.Visible = false;

                msgError = blPara.ConsultaCaracterizaciones(ref dsAsesores, true);

                if (string.IsNullOrEmpty(msgError))
                    CargarCaracterizacionPadre(dsAsesores, null);
            }
        }
        if (hfConsulta.Value == "si")
		{
			btnGrabar.Visible = false;
			btnEliminar.Visible = false;
			objUsuario.SoloLectura(this);
		}

	}

    private void CargarCaracterizacionPadre(DataSet dsAsesores, int? idPadre)
    {
        if (dsAsesores.Tables[0].Rows.Count > 0)
        {
            ddlCaracterizacionPadre.DataSource = dsAsesores.Tables[0];
            ddlCaracterizacionPadre.DataTextField = "Descripcion";
            ddlCaracterizacionPadre.DataValueField = "Id";
            ddlCaracterizacionPadre.DataBind();
            ddlCaracterizacionPadre.Items.Insert(0, new ListItem("No Aplica", "0"));
            Console.WriteLine(ddlCaracterizacionPadre.Items);

            if (idPadre.HasValue && ddlCaracterizacionPadre.Items.FindByValue(idPadre.ToString()) != null)
                ddlCaracterizacionPadre.SelectedValue = idPadre.ToString();
            else
                ddlCaracterizacionPadre.SelectedIndex = 0;
            Console.WriteLine(ddlCaracterizacionPadre.Items);
        }
    }

    protected void btnGrabar_Click(object sender, EventArgs e)
	{
		try
		{
			int idPadre = int.Parse(ddlCaracterizacionPadre.SelectedValue);
			int habilitado = cbHabilitado.Checked ? 1 : 0;
			bool grabado;

            grabado = blPara.GrabarCaracterizacion(ref dsAsesores, hfid.Value, tbNombre.Text, idPadre, habilitado);

			if (grabado)
            {
                lbConfirmacion.Text = "¡Registro grabado correctamente!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-success";
                notificacion.Visible = true;
            }
            else
            {
                lbConfirmacion.Text = "¡Error al grabar el registro!";
                notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
                notificacion.Visible = true;
            }
		}
		catch (Exception ex)
		{
			lbConfirmacion.Text = "¡Error al grabar el registro! ";
			notificacion.Attributes["Class"] = "alert alert-dismissible alert-danger";
			notificacion.Visible = true;
		}
	}

	protected void btnSalir_Click(object sender, EventArgs e)
	{
        Response.Redirect("wfCaracterizaciones.aspx", false);
    }
}
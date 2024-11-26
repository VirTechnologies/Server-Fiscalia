using Newtonsoft.Json;
using RabbitMQconector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;


public class Variables
{
    public string Variable;
    public string tematicaid;
    public Variables()
    {

    }
}

/// <summary>
/// Summary description for blUtiles
/// </summary>
public class clsblUtiles
{
    public clsblUtiles()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /*
      Función cadenaConexion
      Propósito:  Retorna los valores de la especificación de la base de datos, servidor y usuario.
                  para ejecutar transacciones sobre la base de datos
    */
    public String cadenaConexion()
    {
        String cadena, servidor, database, usuario, clave, sid, puerto;

        cadena = "";
        if (BaseDeDatos() == "SQLSERVER")
        {
            servidor = ConfigurationManager.AppSettings["ServerName"];
            database = ConfigurationManager.AppSettings["DatabaseName"];
            usuario = ConfigurationManager.AppSettings["UserID"];
            clave = ConfigurationManager.AppSettings["Password"];
            cadena = "Provider=SQLNCLI11;Persist Security Info=False;User ID=" + usuario + "; Password=" + clave + "; Initial Catalog=" + database + ";Data Source=" + servidor + ";Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Use Encryption for Data=False;Tag with column collation when possible=False";
        }
        if (BaseDeDatos() == "ORACLE")
        {
            servidor = ConfigurationManager.AppSettings["Servidor"];
            usuario = ConfigurationManager.AppSettings["UserID"];
            clave = ConfigurationManager.AppSettings["Password"];
            sid = ConfigurationManager.AppSettings["Sid"];
            puerto = ConfigurationManager.AppSettings["Puerto"];
            cadena = "Data Source=(DESCRIPTION="
                    + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + servidor + ")(PORT=" + puerto + ")))"
                    + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + sid + ")));"
                    + "User Id=" + usuario + ";Password=" + clave + ";";
        }
        return cadena;
    }

    public String cadenaConexionLog()
    {
        String cadena, servidor, database, usuario, clave, sid, puerto;

        cadena = "";
        if (BaseDeDatos() == "SQLSERVER")
        {
            servidor = ConfigurationManager.AppSettings["ServerName"];
            database = ConfigurationManager.AppSettings["DatabaseName"];
            usuario = ConfigurationManager.AppSettings["UserID"];
            clave = ConfigurationManager.AppSettings["Password"];
            cadena = "Provider=SQLOLEDB;Persist Security Info=False;User ID=" + usuario + "; Password=" + clave + "; Initial Catalog=" + database + ";Data Source=" + servidor + ";Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Use Encryption for Data=False;Tag with column collation when possible=False";
        }
        if (BaseDeDatos() == "ORACLE")
        {
            servidor = ConfigurationManager.AppSettings["ServerName"];
            usuario = ConfigurationManager.AppSettings["UserID"];
            clave = ConfigurationManager.AppSettings["Password"];
            sid = ConfigurationManager.AppSettings["Sid"];
            puerto = ConfigurationManager.AppSettings["Puerto"];
            cadena = "Data Source=(DESCRIPTION="
                    + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + servidor + ")(PORT=" + puerto + ")))"
                    + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + sid + ")));"
                    + "User Id=" + usuario + ";Password=" + clave + ";";
        }
        return cadena;
    }

    public String BaseDeDatos()
    {
        String cualBase, cadena = ""; //0:SQLSERVER 1:ORACLE 2:MYSQL

        cualBase = ConfigurationManager.AppSettings["database"];
        if (cualBase == "1")
            cadena = "SQLSERVER";
        if (cualBase == "2")
            cadena = "ORACLE";
        if (cualBase == "3")
            cadena = "MYSQL";
        cadena = "SQLSERVER";
        return cadena;
    }


    // ****************************************************************************************
    // LlenaDDLObligatorio: Llenar un dropdownlist con un valor inicial no seleccionado 
    //
    public void LlenaDDLObligatorio(DropDownList DDLn, String tblN, String DVF,
                            String DTF, String Condicion, String sRS, String CampoOrden)
    {
        //Const SinEscoger As String = "No seleccionado..."

        String msgError;
        DataSet dsP = new DataSet();
        ListItem Item = new ListItem();

        if (CampoOrden == "")
            CampoOrden = DTF;
        msgError = ConsultaTablaParametro(ref dsP, tblN, Condicion, sRS, CampoOrden);
        if (msgError == "")
        {
            DDLn.DataSource = dsP;
            DDLn.DataValueField = dsP.Tables[0].Columns[DVF].ToString();
            DDLn.DataTextField = dsP.Tables[0].Columns[DTF].ToString();
            DDLn.DataBind();
            //            If AgregaFilaNoSeleccionada Then
            //If TextoAgregar = Nothing Then
            Item.Text = "";
            //Else
            //Item.Text = TextoAgregar
            //End If
            Item.Selected = true;
            DDLn.Items.Insert(0, Item);
            //End If
        }
    }
    public void LlenaDDLObligatorioUnico(DropDownList DDLn, String tblN, String DVF,
                            String DTF, String Condicion, String sRS, String CampoOrden)
    {
        //Const SinEscoger As String = "No seleccionado..."

        String msgError;
        DataSet dsP = new DataSet();
        ListItem Item = new ListItem();

        if (CampoOrden == "")
            CampoOrden = DTF;
        msgError = ConsultaTablaParametroUnico(ref dsP, tblN, Condicion, sRS, CampoOrden);
        if (msgError == "")
        {
            DDLn.DataSource = dsP;
            DDLn.DataValueField = dsP.Tables[0].Columns[DVF].ToString();
            DDLn.DataTextField = dsP.Tables[0].Columns[DTF].ToString();
            DDLn.DataBind();
            //            If AgregaFilaNoSeleccionada Then
            //If TextoAgregar = Nothing Then
            Item.Text = "";
            //Else
            //Item.Text = TextoAgregar
            //End If
            Item.Selected = true;
            DDLn.Items.Insert(0, Item);
            //End If
        }
    }

    public void LlenaDDLObligatorioTematicas(DropDownList DDLn, String tblN, String DVF,
                            String DTF, String Condicion, String sRS, String CampoOrden)
    {
        //Const SinEscoger As String = "No seleccionado..."

        String msgError;
        DataSet dsP = new DataSet();
        ListItem Item = new ListItem();

        if (CampoOrden == "")
            CampoOrden = DTF;
        msgError = ConsultaTablaParametroTematicas(ref dsP, tblN, Condicion, sRS, CampoOrden);
        if (msgError == "")
        {
            DDLn.DataSource = dsP;
            DDLn.DataValueField = dsP.Tables[0].Columns[DVF].ToString();
            DDLn.DataTextField = dsP.Tables[0].Columns[DTF].ToString();
            DDLn.DataBind();
            //            If AgregaFilaNoSeleccionada Then
            //If TextoAgregar = Nothing Then
            Item.Text = "";
            //Else
            //Item.Text = TextoAgregar
            //End If
            Item.Selected = true;
            DDLn.Items.Insert(0, Item);
            //End If
        }
    }

    //Ejecuta una consulta sobre una tabla especificada.
    //El resultado se retorna en el parámetro dsReferido (Data.DataSet)
    //Parámetros
    //tblN --> Nombre de la tabla sobre la que se ejecuta la consulta.
    //Condicion --> Para una sola tabla se escribe la condición, SIN LA PALABRA WHERE
    //sRS --> Si se requieren condiciones espaciales (más de una tabla involucrada), 
    //se ejecuta este query y no el estándar
    public String ConsultaTablaParametro(ref DataSet dsReferido, String tblN, String Condicion,
                                         String sRS, String CampoOrden)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        if (sRS != "" || tblN == "")
            strSQL = sRS;
        else
        {
            strSQL = "select * from " + tblN;
            if (Condicion != "")
                strSQL += " where " + Condicion;
            if (CampoOrden != "")
                strSQL += " ORDER BY " + CampoOrden;
        }
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }
    public String ConsultaTablaParametroUnico(ref DataSet dsReferido, String tblN, String Condicion,
                                         String sRS, String CampoOrden)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        if (sRS != "" || tblN == "")
            strSQL = sRS;
        else
        {
            strSQL = "select *, s.Nombre + ' ' + tp.Nombre as servicios from ServicioTipoAtencion st inner join Servicios s on st.ServicioId = s.Id  inner join TipoDeAtencion tp on st.TipoAtencionId = tp.Id ORDER BY st.Abreviatura";
        }
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }
    public String ConsultaTablaParametroTematicas(ref DataSet dsReferido, String tblN, String Condicion,
                                         String sRS, String CampoOrden)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        if (sRS != "" || tblN == "")
            strSQL = sRS;
        else
        {
            strSQL = "  select * from Oficinas o inner join Grupo_oficinas gpo on o.Id = gpo.OficinaId WHERE " + Condicion;
        }
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }


    //==============================================================================
    //Nombre de la función : FiltraParametros
    //Objetivo : Retornar un dataset con la información de la tabla PARAMETROS
    public String FiltraParametros(ref DataSet dsReferido, string parametro, string id_parametro)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT * FROM Parametros_Generales WHERE Id IS NOT NULL ";
        if (id_parametro != "")
        {
            strSQL += " AND Id=" + id_parametro;
        }
        if (parametro != "")
        {
            strSQL += " AND UPPER(Parametro)='" + parametro.ToUpper() + "' ";
        }
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //===================================================
    //ValorString: Retornal el valor del objeto si no es null, "" en caso contrario
    public String ValorObjetoString(Object objeto)
    {
        if (objeto == null)
            return "";
        else
            return objeto.ToString();
    }

    //===================================================
    //Función: FechaDeString
    //Retorna una fecha dado su valer en cadena de caracteres
    public DateTime FechaDeString(String fS)
    {
        //"dd/MM/yyyy"
        return new DateTime(Convert.ToInt32(fS.Substring(6, 4)), Convert.ToInt32(fS.Substring(3, 2)), Convert.ToInt32(fS.Substring(0, 2)));
    }

    //===================================================
    //Función: FechaHoraDeString
    //Retorna una fecha dado su valoer en cadena de caracteres
    public DateTime FechaHoraDeString(String fS)
    {
        //"dd/MM/yyyy hh:mm:ss"
        return new DateTime(Convert.ToInt32(fS.Substring(6, 4)), Convert.ToInt32(fS.Substring(3, 2)), Convert.ToInt32(fS.Substring(0, 2)),
                            Convert.ToInt32(fS.Substring(11, 2)), Convert.ToInt32(fS.Substring(14, 2)), Convert.ToInt32(fS.Substring(17, 2)));
    }

    //===================================================
    //Función: FechaAString
    //Retorna un string dado su valor en datetime
    public string FechaAString(DateTime fS)
    {
        return fS.Day.ToString() + "/" + fS.Month.ToString() + "/" + fS.Year.ToString();
    }

    //===================================================
    //Función: FechaHoraAString
    //Retorna un string dado su valer en datetime con hora incluido
    public string FechaHoraAString(DateTime fS)
    {
        return fS.Day.ToString() + "/" + fS.Month.ToString() + "/" + fS.Year.ToString() + " " + fS.Hour + ":" + fS.Minute + ":" + fS.Second;
    }

    public String Blanco(String sR)
    {
        if (sR == "&nbsp;")
            return "";
        else
            return sR;
    }

    //==============================================================================
    //Nombre de la función : ExisteSupermarketCheckout
    //Objetivo : Verifica si existe un checkout para el IdSupermarket y la fecha
    //           
    public String ExisteSupermarketCheckout(ref String Id, String IdSupermarket, String fecha)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = new DataSet();

        Id = "";
        strSQL = "SELECT SupermarketCheckout.IdSupermarketCheckout " +
                 "FROM SupermarketCheckout " +
                 "WHERE  SupermarketCheckout.IdSupermarket=" + IdSupermarket +
                 " AND Convert(date,SupermarketCheckout.Date)=CONVERT(DATETIME,'" + fecha + "',103)";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (Mensaje == "")
        {
            if (dsInterno.Tables[0].Rows.Count > 0)
                Id = dsInterno.Tables[0].Rows[0]["IdSupermarketCheckout"].ToString();
        }
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ExisteCheckout
    //Objetivo : Verifica si existe un checkout para el IdSupermarketCheckout y el Checkout
    //           
    public String ExisteCheckout(ref String Id, String IdSupermarketCheckout, String Checkout, String Count, String MeanInMinutes)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = new DataSet();

        Id = "";
        strSQL = "SELECT Checkout.IdSupermarketCheckout " +
                 "FROM Checkout " +
                 "WHERE  IdSupermarketCheckout=" + IdSupermarketCheckout +
                 " AND UPPER(Checkout)='" + Checkout.ToUpper() + "'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (Mensaje == "")
        {
            if (dsInterno.Tables[0].Rows.Count > 0)
                Id = dsInterno.Tables[0].Rows[0]["IdSupermarketCheckout"].ToString();
            if (Id != "")
            {
                strSQL = "UPDATE Checkout SET Count=" + Count + ", MeanInMinutes= " + MeanInMinutes + " " +
                         "WHERE  IdSupermarketCheckout=" + IdSupermarketCheckout + " AND Checkout='" + Checkout + "'";
                ConexionDataObject = new clsDataLayer();
                ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
            }
        }
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ExisteInterval
    //Objetivo : Verifica si existe un Hit para el IdSupermarketCheckout, el Checkout y el Hour
    //           
    public String ExisteInterval(ref String Id, String IdSupermarketCheckout, String Checkout, String Hour, String Count)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = new DataSet();

        Id = "";
        strSQL = "SELECT IntervalHits.IdSupermarketCheckout " +
                 "FROM IntervalHits " +
                 "WHERE  IdSupermarketCheckout=" + IdSupermarketCheckout +
                 " AND Hour=" + Hour +
                 " AND UPPER(Checkout)='" + Checkout.ToUpper() + "'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (Mensaje == "")
        {
            if (dsInterno.Tables[0].Rows.Count > 0)
                Id = dsInterno.Tables[0].Rows[0]["IdSupermarketCheckout"].ToString();
            if (Id != "")
            {
                strSQL = "UPDATE IntervalHits SET Count=" + Count + " " +
                         "WHERE  IdSupermarketCheckout=" + IdSupermarketCheckout + " AND Checkout='" + Checkout + "' AND Hour=" + Hour;
                ConexionDataObject = new clsDataLayer();
                ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
            }
        }
        return Mensaje;
    }

    //Función: ExisteValor
    //Propósito: Informa si un valor existe, para un campo de condición y una tabla especificadas.
    //Parámetros
    //           tblN  --> Nombre de la tabla donde se buscará el valor.
    //           CampoN --> Nombre del campo a buscar.
    //           ValorC --> Valor buscado
    //Si encuentra el valor, retorna verdadero, falso en caso contrario.

    public bool ExisteValor(String tblN, String campoN, String ValorC, OleDbConnection Conexion, OleDbTransaction myTrans)
    {
        int intRetVal = 0;
        clsDataLayer ConexionDataObject;
        String Mensaje = "", strSQL = "";
        DataSet dsP = new DataSet();

        strSQL = "select " + campoN + " as Val from " + tblN + " where " + campoN + "='" + ValorC + "'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsP, Conexion, myTrans, true);
        if (Mensaje == "")
        {
            if (dsP.Tables[0].Rows.Count <= 0)
                return false;
            else
            {
                if (dsP.Tables[0].Rows[0]["Val"].ToString() == "")
                    return false;
                else
                    return true;
            }
        }
        else
        {
            return true;
        }
    }

    //==============================================================================
    //Nombre de la función : EncolarMensajesRabbit
    //Objetivo : Pone en la cola de Rabbit los mensajes 
    //           
    public String EncolarMensajesRabbit(List<string> MensajesEncolar, string OfficeId, bool TodasOficinas)
    {
        String Mensaje = "", msgError = "";
        string elJson = "";
        Variables[] Pares;
        int i = 0;
        DataSet dsOficinas = new DataSet();
        DataSet dsParametros = new DataSet();
        clsblParametricas blParam = new clsblParametricas();

        try
        {
            msgError = FiltraParametros(ref dsParametros, "ENCOLAR_RABBIT", "");
            if (msgError == "")
            {
                if (dsParametros.Tables[0].Rows[0]["Valor"].ToString().ToUpper() == "SI")
                {
                    Pares = new Variables[MensajesEncolar.Count + 1];
                    Pares[0] = new Variables();
                    Pares[0].Variable = "action : SQLAction";
                    for (i = 0; i < MensajesEncolar.Count; i++)
                    {
                        Pares[i + 1] = new Variables();
                        MensajesEncolar[i] = MensajesEncolar[i].Replace("\r\n", " ");
                        Pares[i + 1].Variable = "SQLStatement : " + MensajesEncolar[i];
                    }
                    elJson = JsonConvert.SerializeObject(Pares);



                    if (TodasOficinas)
                    {
                        Mensaje = blParam.ConsultaOficinas(ref dsOficinas, "", "");
                        for (i = 0; i < dsOficinas.Tables[0].Rows.Count; i++)

                            Sender.sendMessage("office" + dsOficinas.Tables[0].Rows[i]["Id"].ToString(), elJson, "localhost", 5672, "guest", "guest", 20000);
                    }
                    else
                    {
                        Sender.sendMessage("office" + OfficeId, elJson, "localhost", 5672, "guest", "guest");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
        return Mensaje;
    }
}

using System;
using System.Data;
using System.Globalization;


/// <summary>
/// Summary description for NSSSqlUtil
/// </summary>
public class NSSSqlUtil
{
    public struct SCampos
    {
        public String Nombre;
        public String Valor;
    };

    public SCampos[] Campos = new SCampos[100];
    public String NombreTabla;

    public String LlavePrimaria;
    public bool IsIdentity;
    public bool LlaveTexto;
    public string strSQLExecuted;

    int IndiceVa;
    String SentenciaSelect;

    public NSSSqlUtil()
    {
        for (int i = 0; i < 100; i++)
        {
            Campos[i].Nombre = "";
            Campos[i].Valor = "";
        }
        IndiceVa = 0;
        IsIdentity = false;
        LlaveTexto = false;
    }

    public void NuevoRegistro()
    {
        for (int i = 0; i < 100; i++)
        {
            Campos[i].Nombre = "";
            Campos[i].Valor = "";
        }
        IndiceVa = 0;
    }

    public void Add(String Nombre, String Valor)
    {
        Campos[IndiceVa].Nombre = Nombre;
        Campos[IndiceVa].Valor = Valor;
        IndiceVa = IndiceVa + 1;
    }

    public String ObtieneLlave(ref int ValorLlave, String Tabla, String NombreLlave,
                               Object Conexion, Object Transaccion)
    {
        DataSet dsInterno = new DataSet();
        clsDataLayer ConexionDataObject;
        int intRetVal = 0;
        String Mensaje = "";

        SentenciaSelect = "SELECT MAX(" + NombreLlave + ")+1 AS MAXIMO FROM " + Tabla;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(SentenciaSelect, ref intRetVal, ref Mensaje, ref dsInterno, Conexion, Transaccion, true);
        if (!dsInterno.Tables[0].Rows[0].IsNull(0))
            ValorLlave = Convert.ToInt32(dsInterno.Tables[0].Rows[0]["MAXIMO"].ToString());
        else
            ValorLlave = 1;
        return Mensaje;
    }

    public string ObtenerInsert(ref String idInsertado, Object Conexion, Object Transaccion)
    {
        String strSQL;
        DataSet dsInterno = new DataSet();
        clsDataLayer ConexionDataObject;
        int intRetVal = 0;
        String Mensaje = "";
        int i, k;
        bool HayLlave = false;
        int ValorLlavePrimaria = -1;
        clsblUtiles blU = new clsblUtiles();

        k = 0;
        while (Campos[k].Nombre != "")
        {
            if (Campos[k].Nombre.ToUpper() == LlavePrimaria.ToUpper())
                if (Campos[k].Valor != "")
                    HayLlave = true;
            k = k + 1;
        }
        if (!HayLlave)
        {
            SentenciaSelect = "SELECT MAX(" + LlavePrimaria + ")+1 AS MAXIMO FROM " + NombreTabla;
            ConexionDataObject = new clsDataLayer();
            ConexionDataObject.ExecSQL(SentenciaSelect, ref intRetVal, ref Mensaje, ref dsInterno, Conexion, Transaccion, true);
            if (!dsInterno.Tables[0].Rows[0].IsNull(0))
                ValorLlavePrimaria = Convert.ToInt32(dsInterno.Tables[0].Rows[0]["MAXIMO"]);
            else
                ValorLlavePrimaria = 1;
        }
        SentenciaSelect = "SELECT * FROM " + NombreTabla + " WHERE " + LlavePrimaria + " = -1";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(SentenciaSelect, ref intRetVal, ref Mensaje, ref dsInterno, Conexion, Transaccion, true);
        strSQL = "INSERT INTO " + NombreTabla + " (";
        if (!HayLlave && !IsIdentity)
            strSQL = strSQL + LlavePrimaria + ",";
        k = 0;
        while (Campos[k].Nombre != "")
        {
            for (i = 0; i < dsInterno.Tables[0].Columns.Count; i++)
                if (Campos[k].Nombre.ToUpper() == dsInterno.Tables[0].Columns[i].ColumnName.ToUpper())
                    strSQL = strSQL + dsInterno.Tables[0].Columns[i].ColumnName + " , ";
            k = k + 1;
        }
        strSQL = strSQL.Substring(0, strSQL.Length - 3) + ") VALUES(";
        if (!HayLlave && !IsIdentity)
        {
            strSQL = strSQL + ValorLlavePrimaria.ToString() + ",";
            idInsertado = ValorLlavePrimaria.ToString();
        }
        k = 0;
        while (Campos[k].Nombre != "")
        {
            for (i = 0; i < dsInterno.Tables[0].Columns.Count; i++)
            {
                if (Campos[k].Nombre.ToUpper() == dsInterno.Tables[0].Columns[i].ColumnName.ToUpper())
                {
                    if (dsInterno.Tables[0].Columns[i].DataType.Name.ToUpper() == "DATETIME")
                    {
                        if (Campos[k].Valor != "")
                        {
                            if (blU.BaseDeDatos() == "SQLSERVER")
                                strSQL = strSQL + "CONVERT(DATETIME,'" + Campos[k].Valor + "',103)";
                            if (blU.BaseDeDatos() == "ORACLE")
                                strSQL = strSQL + "TO_DATE('" + Campos[k].Valor + "','DD/MM/YYYY HH24:MI:SS')";
                        }
                        else
                            strSQL = strSQL + "NULL";
                    }
                    else
                    {
                        if (dsInterno.Tables[0].Columns[i].DataType.Name.ToUpper() == "STRING")
                        {
                            strSQL = strSQL + "'";
                            strSQL = strSQL + Campos[k].Valor;
                            strSQL = strSQL + "'";
                        }
                        else
                        {
                            if (dsInterno.Tables[0].Columns[i].DataType.Name.ToUpper() == "OBJECT")
                            {
                                strSQL = strSQL + "'";
                                strSQL = strSQL + Campos[k].Valor;
                                strSQL = strSQL + "'";
                            }
                            else
                            {
                                if ((dsInterno.Tables[0].Columns[i].DataType.Name.ToUpper() == "DOUBLE") ||
                                    (dsInterno.Tables[0].Columns[i].DataType.Name.ToUpper() == "DECIMAL"))
                                    Campos[k].Valor = Campos[k].Valor.Replace(",", ".");
                                if (Campos[k].Valor != "")
                                    strSQL = strSQL + Campos[k].Valor;
                                else
                                    strSQL = strSQL + "NULL";
                            }
                        }
                    }
                    strSQL = strSQL + " , ";
                }
            }
            k = k + 1;
        }
        strSQL = strSQL.Substring(0, strSQL.Length - 2) + ")";
        if (IsIdentity)
            strSQL += ";SELECT SCOPE_IDENTITY();";
        return strSQL;
    }

    public String ObtenerUpdate(Object Conexion, Object Transaccion)
    {
        String strSQL;
        DataSet dsInterno = new DataSet();
        clsDataLayer ConexionDataObject;
        int intRetVal = 0;
        String Mensaje = "";
        int i, k;
        bool HayLlave = false;
        int ValorLlavePrimaria = -1;
        clsblUtiles blU = new clsblUtiles();

        k = 0;
        while (Campos[k].Nombre != "")
        {
            if (Campos[k].Nombre.ToUpper() == LlavePrimaria.ToUpper())
                if (Campos[k].Valor != "")
                {
                    HayLlave = true;
                    ValorLlavePrimaria = Convert.ToInt32(Campos[k].Valor);
                }
            k = k + 1;
        }
        if (HayLlave)
        {
            SentenciaSelect = "SELECT * FROM " + NombreTabla + " WHERE " + LlavePrimaria + " = -1";
            ConexionDataObject = new clsDataLayer();
            ConexionDataObject.ExecSQL(SentenciaSelect, ref intRetVal, ref Mensaje, ref dsInterno, Conexion, Transaccion, true);
            strSQL = "UPDATE " + NombreTabla + " SET " + Environment.NewLine;
            k = 0;
            while (Campos[k].Nombre != "")
            {
                for (i = 0; i < dsInterno.Tables[0].Columns.Count; i++)
                {
                    if (Campos[k].Nombre.ToUpper() == dsInterno.Tables[0].Columns[i].ColumnName.ToUpper())
                    {
                        if (!(/*IsIdentity &&*/ Campos[k].Nombre.ToUpper() == LlavePrimaria.ToUpper()))
                        {
                            if (dsInterno.Tables[0].Columns[i].DataType.Name.ToUpper() == "DATETIME" || dsInterno.Tables[0].Columns[i].DataType.Name.ToUpper() == "TIME")
                            {
                                if (Campos[k].Valor != "")
                                {
                                    if (blU.BaseDeDatos() == "SQLSERVER")
                                        strSQL = strSQL + " " + dsInterno.Tables[0].Columns[i].ColumnName + " = " + "CONVERT(DATETIME,'" + Campos[k].Valor + "',103)";
                                    if (blU.BaseDeDatos() == "ORACLE")
                                        strSQL = strSQL + " " + dsInterno.Tables[0].Columns[i].ColumnName + " = " + "TO_DATE('" + Campos[k].Valor + "','DD/MM/YYYY HH24:MI:SS')";
                                }
                                else
                                {
                                    strSQL = strSQL + " " + dsInterno.Tables[0].Columns[i].ColumnName + " = NULL";
                                }
                            }
                            else
                            {
                                if (dsInterno.Tables[0].Columns[i].DataType.Name.ToUpper() == "STRING")
                                {
                                    strSQL = strSQL + " " + dsInterno.Tables[0].Columns[i].ColumnName + " = " + "'";
                                    strSQL = strSQL + Campos[k].Valor;
                                    strSQL = strSQL + "'";
                                    k = k + 1;
                                }
                                else
                                {
                                    if ((dsInterno.Tables[0].Columns[i].DataType.Name.ToUpper() == "DOUBLE") ||
                                        (dsInterno.Tables[0].Columns[i].DataType.Name.ToUpper() == "DECIMAL"))
                                        Campos[k].Valor = Campos[k].Valor.Replace(",", ".");
                                    if (Campos[k].Valor != "")
                                        strSQL = strSQL + " " + dsInterno.Tables[0].Columns[i].ColumnName + " = " + Campos[k].Valor;
                                    else
                                        strSQL = strSQL + " " + dsInterno.Tables[0].Columns[i].ColumnName + " = NULL";
                                }
                            }
                            strSQL = strSQL + " , " + Environment.NewLine;
                        }
                    }

                }
                k = k + 1;
            }
            strSQL = strSQL.Substring(0, strSQL.Length - 4) + Environment.NewLine;
            strSQL = strSQL + " WHERE " + LlavePrimaria + "=" + ValorLlavePrimaria.ToString();
        }
        else
            return "La llave primaria no está incluida en los campos";
        return strSQL;
    }

    public String ObtenerDelete()
    {
        /// 22/11/2021 OERD MODIFICA PARA QUE LAS LLAVES DE TIPO VARCHAR SE PUEDAN INCLUIR EN EL DELETE

        String strSQL;
        int k;
        bool HayLlave = false;
        string ValorLlavePrimaria = "-1";
        int llave;

        k = 0;
        while (Campos[k].Nombre != "")
        {
            if (Campos[k].Nombre.ToUpper() == LlavePrimaria.ToUpper())
            {
                if (Campos[k].Nombre.ToUpper() == LlavePrimaria.ToUpper())
                    if (Campos[k].Valor != "")
                        HayLlave = true;

                if (int.TryParse(Campos[k].Valor, out llave))
                {
                    ValorLlavePrimaria = llave.ToString();
                }
                else
                {
                    ValorLlavePrimaria = "'" + Campos[k].Valor + "'";
                }
            }
            k = k + 1;
        }
        if (HayLlave)
            strSQL = "DELETE FROM " + NombreTabla + " WHERE " + LlavePrimaria + " = " + ValorLlavePrimaria;
        else
            return "La llave primaria no está incluida en los campos";
        return strSQL;
    }

    public String ObtenerDeleteCAJL()
    {
        String strSQL;
        int k;
        bool HayLlave = false;
        int ValorLlavePrimaria = -1;

        k = 0;
        while (Campos[k].Nombre != "")
        {
            if (Campos[k].Nombre.ToUpper() == LlavePrimaria.ToUpper())
            {
                if (Campos[k].Nombre.ToUpper() == LlavePrimaria.ToUpper())
                    if (Campos[k].Valor != "")
                        HayLlave = true;

                ValorLlavePrimaria = Convert.ToInt32(Campos[k].Valor);
            }
            k = k + 1;
        }
        if (HayLlave)
            strSQL = "DELETE FROM " + NombreTabla + " WHERE " + LlavePrimaria +
                     " = " + ValorLlavePrimaria.ToString();
        else
            return "La llave primaria no está incluida en los campos";
        return strSQL;
    }



    public String ObtenerSelect(String campoCondicion, String valorCondicion)
    {
        String strSQL;

        strSQL = "SELECT * FROM " + NombreTabla;
        if (campoCondicion != "")
            strSQL = strSQL + " WHERE upper(" + campoCondicion + ") = '" + valorCondicion.ToUpper() + "'";
        return strSQL;
    }



    public String NssEjecutarSQL(String Tipo, ref DataSet dsReferido, ref String idInsertado,
                                String campoCondicion, String valorCondicion, Object Conexion, Object Transaccion)
    {
        String strSQL = "";
        String Mensaje = "";
        clsDataLayer ConexionDataObject;
        int intRetVal = 0;
        DataSet dsResult = null;

        if (Tipo.ToUpper() == "INSERT")
            strSQL = ObtenerInsert(ref idInsertado, Conexion, Transaccion);
        if (Tipo.ToUpper() == "UPDATE")
            strSQL = ObtenerUpdate(Conexion, Transaccion);
        if (Tipo.ToUpper() == "DELETE")
            strSQL = ObtenerDelete();
        if (Tipo.ToUpper() == "SELECT")
        {
            strSQL = ObtenerSelect(campoCondicion, valorCondicion);
            ConexionDataObject = new clsDataLayer();
            ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, Conexion, Transaccion, true);
        }
        else
        {
            try
            {
                ConexionDataObject = new clsDataLayer();
                ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsResult, Conexion, Transaccion, true);
                if (Tipo.ToUpper() == "INSERT" && IsIdentity)
                {
                    idInsertado = "";
                    ObtenerIdentity(ref idInsertado, NombreTabla, LlavePrimaria, Conexion, Transaccion);
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.ToString();
                if (!((Tipo.ToUpper() == "DELETE") && (Mensaje.Contains("REFERENCE"))))
                    throw ex;
            }
        }
        strSQLExecuted = strSQL;
        return Mensaje;
    }

    public string ObtenerIdentity(ref string Identity, string Tabla, string Llave, Object Conexion, Object Transaccion)
    {
        String Mensaje = "";
        clsDataLayer ConexionDataObject;
        int intRetVal = 0;
        DataSet dsResult = new DataSet();

        Identity = "";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL("SELECT " + Llave + " FROM " + Tabla + " WHERE " + Llave + " = SCOPE_IDENTITY();", ref intRetVal, ref Mensaje, ref dsResult, Conexion, Transaccion, true);
        if (Mensaje == "")
        {
            if (dsResult.Tables[0].Rows.Count > 0)
                if (dsResult.Tables[0].Rows[0][Llave].ToString() != "")
                    Identity = dsResult.Tables[0].Rows[0][Llave].ToString();
        }
        return Mensaje;
    }

    public bool Duplicado(String campo, String valor)
    {
        DataSet dsReferido = new DataSet();
        String strSQL;
        String Mensaje = "";
        clsDataLayer ConexionDataObject;
        int intRetVal = -1;

        strSQL = ObtenerSelect(campo, valor.ToUpper());
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        if (Mensaje == "")
        {
            if (dsReferido.Tables.Count > 0)
                if (dsReferido.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
            else
                return false;
        }
        else
            return false;
    }

    public String ConvierteFechaGrabar(String fecha)
    {
        String FechaConFormato = "";
        DateTime fechaN;

        fechaN = new DateTime(Convert.ToInt32(fecha.Substring(6, 4)), Convert.ToInt32(fecha.Substring(0, 2)), Convert.ToInt32(fecha.Substring(3, 2)));
        FechaConFormato = fechaN.ToString("dd/MM/yyyy h:mm:ss tt", DateTimeFormatInfo.InvariantInfo);
        return FechaConFormato;
    }

    public String ConsultaSQL(ref DataSet dsReferido, String strSQL)
    {
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }


}

using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
//using Oracle.DataAccess.Client;

/// <summary>
/// Summary description for ObjDataLayer
/// </summary>
public class clsDataLayer
{
    public clsDataLayer()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public void ExecSQL(String strSQLCmd, ref int intReturnVal, ref String MsgError,
                        ref DataSet dsResults, Object Conexion, Object Transaccion,
                        bool LOG)
    {
        clsblUtiles blUtiles = new clsblUtiles();
        OleDbConnection cn = null, cn1;
        OleDbCommand cmd, cmd1;
        OleDbDataAdapter adt;
        String strSQL, msgError1, usuario;

        try
        {
            if (Conexion != null)
            {
                cmd = new OleDbCommand(strSQLCmd, (OleDbConnection)Conexion);
                LOG = false;
                cmd.Transaction = (OleDbTransaction)Transaccion;
            }
            else
            {
                cn = new OleDbConnection();
                cn.ConnectionString = blUtiles.cadenaConexion();
                cn.Open();
                cmd = new OleDbCommand(strSQLCmd, cn);
                cmd.CommandTimeout = 300;
            }
            strSQL = strSQLCmd.ToUpper();
            if (strSQL.IndexOf("SELECT") != -1)
            {
                dsResults = new DataSet();
                adt = new OleDbDataAdapter();
                adt.SelectCommand = cmd;
                intReturnVal = adt.Fill(dsResults);
            }
            else
            {
                intReturnVal = cmd.ExecuteNonQuery();
                try
                {
                    if (LOG)
                    {
                        if (strSQL.IndexOf("SELECT") != -1)
                        {
                            usuario = System.Web.HttpContext.Current.Session["IDUSUARIO"].ToString();
                            strSQL = "INSERT INTO LOG_CIPWIN(id_usuario,fecha_hora,comando)"
                                   + " VALUES(" + usuario + ",CONVERT(DATETIME,'" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "',110),'" + ModificaCadena(strSQLCmd) + "')";
                            cn1 = new OleDbConnection();
                            cn1.ConnectionString = blUtiles.cadenaConexionLog();
                            cn1.Open();
                            cmd1 = new OleDbCommand(strSQL, cn1);
                            cmd1.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    msgError1 = ex.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            MsgError = ex.ToString();
            if (MsgError.IndexOf("UNIQUE KEY") != -1)
                throw new SystemException("El servidor está ocupado. Por favor reintente!<br><br><br>", null);
            else
                throw ex;
        }
        finally
        {
            if (cn != null)
                cn.Close();
        }

    }

    /*
        public void ExecSQL(String strSQLCmd, ref int intReturnVal, ref String MsgError,
                            ref DataSet dsResults, OleDbConnection Conexion, OleDbTransaction Transaccion,
                            bool LOG)
        {
            OleDbConnection cn, cn1;
            OleDbCommand cmd,cmd1;
            OleDbDataAdapter adt;
            String strSQL, msgError1, usuario;
            clsblUtiles blUtiles = new clsblUtiles();

            try
            {
                if (Conexion!=null)
                {
                    cmd = new OleDbCommand(strSQLCmd, Conexion);
                    LOG = false;
                    cmd.Transaction = Transaccion;
                }
                else
                {
                    cn = new OleDbConnection();
                    //cn.ConnectionString = "Data Source=ORCL;User ID=ckmserver;Password=manager;Unicode=True;Provider=SQLOLEDB"; 
                    cn.ConnectionString = blUtiles.cadenaConexion();
                    cn.Open();
                    cmd = new OleDbCommand(strSQLCmd, cn);
                    cmd.CommandTimeout = 300;
                }
                strSQL=strSQLCmd.ToUpper();
                if (strSQL.IndexOf("SELECT") != -1)
                {
                    dsResults = new DataSet();
                    adt = new OleDbDataAdapter();
                    adt.SelectCommand = cmd;
                    intReturnVal = adt.Fill(dsResults);
                }
                else
                {
                    intReturnVal = cmd.ExecuteNonQuery();
                    try
                    {
                        if (LOG)
                        {
                            if (strSQL.IndexOf("SELECT") != -1)
                            {
                              usuario = System.Web.HttpContext.Current.Session["IDUSUARIO"].ToString();
                            strSQL = "INSERT INTO LOG_CIPWIN(id_usuario,fecha_hora,comando)" 
                                   + " VALUES(" + usuario + ",CONVERT(DATETIME,'" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "',110),'" + ModificaCadena(strSQLCmd) + "')";
                              cn1 = new OleDbConnection();
                              cn1.ConnectionString = blUtiles.cadenaConexionLog();
                              cn1.Open();
                              cmd1 = new OleDbCommand(strSQL, cn1);
                              cmd1.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        msgError1 = ex.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgError = ex.ToString();
                if (MsgError.IndexOf("UNIQUE KEY") != -1) 
                    throw new SystemException("El servidor está ocupado. Por favor reintente!<br><br><br>", null);
                else
                    throw ex;
            }

        }
    */

    public String ModificaCadena(String strSQLCmd)
    {
        String Retornar;
        int i;
        char caracter;

        Retornar = "";
        for (i = 0; i < strSQLCmd.Length; i++)
        {
            caracter = strSQLCmd[i];
            switch (caracter)
            {
                case '\'':
                    Retornar = Retornar + "'" + caracter;
                    break;
                default:
                    Retornar = Retornar + caracter;
                    break;
            }
        }
        Retornar = Retornar.ToUpper();
        return Retornar;
    }

    internal object ExecSQLScalar(string strSQL, ref string mensaje, ref DataSet dsReferido, SqlParameter[] parametros)
    {
        throw new NotImplementedException();
    }
}

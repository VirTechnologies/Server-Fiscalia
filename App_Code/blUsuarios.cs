using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for blUsuarios
/// </summary>
public class clsblUsuarios
{

    public clsblUsuarios()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //==============================================================================
    //Nombre de la función : NombreUsuario
    //Objetivo : Retornar el nombre del usuario identificado por id_perfil
    public String NombreUsuario(String id_usuario)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = new DataSet();

        strSQL = "SELECT nombre FROM UsuariosAdmin WHERE id_usuario=" + id_usuario.ToString();
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (dsInterno.Tables.Count > 0)
        {
            if (dsInterno.Tables[0].Rows.Count > 0)
                return dsInterno.Tables[0].Rows[0]["nombre"].ToString();
            else
                return "";
        }
        else
            return "";
    }

    //==============================================================================
    //Nombre de la función : VerificaLogin
    //Objetivo : Retornar un dataset con los datos del usuario
    //           después de consultar la tabla de usuarios
    public String VerificaLogin(ref DataSet dsReferido, String login, String clave)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        try
        {
            strSQL = "SELECT * FROM UsuariosAdmin WHERE upper(login)='" + login.ToUpper() + "' AND upper(password)='" + clave.ToUpper() + "'" + " AND upper(estado)='ACTIVO'";
            ConexionDataObject = new clsDataLayer();
            ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        }
        catch (Exception ex)
        {
            Mensaje = ex.ToString();
        }
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : PerfilUsuario
    //Objetivo : Retornar el nombre del perfil del usuario identificado por id_usuario
    public String PerfilUsuario(String id_usuario)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = new DataSet();

        strSQL = "SELECT PERFILES.perfil FROM UsuariosAdmin INNER JOIN PERFILES ON UsuariosAdmin.id_perfil=PERFILES.id_perfil WHERE UsuariosAdmin.id_usuario=" + id_usuario;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (dsInterno.Tables.Count > 0)
        {
            if (dsInterno.Tables[0].Rows.Count > 0)
                return dsInterno.Tables[0].Rows[0]["perfil"].ToString();
            else
                return "";
        }
        else
            return "";
    }

    //==============================================================================
    //Nombre de la función : RetornarModulosUsuario
    //Objetivo : Retornar un dataset con todos los módulos a los que el usuario id_usuario
    //           tiene acceso 
    public String RetornarModulosUsuario(ref DataSet dsReferido, String id_usuario)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT MODULOS.id_modulo, " +
                 "       MODULOS.modulo," +
                 "       MODULOS.pagina," +
                 "       MODULOS.id_modulo_padre " +
                 "FROM   UsuariosAdmin" +
                 "  INNER JOIN PERFILES ON(UsuariosAdmin.id_perfil=PERFILES.id_perfil)" +
                 "  INNER JOIN PERFILES_MODULOS ON(PERFILES.id_perfil=PERFILES_MODULOS.id_perfil)" +
                 "  INNER JOIN MODULOS ON(PERFILES_MODULOS.id_modulo=MODULOS.id_modulo) " +
                 "WHERE UsuariosAdmin.id_usuario='" + id_usuario + "' " +
                 "AND   MODULOS.visible=1 " +
                 "AND   MODULOS.id_modulo_padre='0'" +
                 "AND   PERFILES_MODULOS.consulta=1 " +
                 "ORDER BY MODULOS.orden ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaUsuarios
    //Objetivo : Retornar un dataset los usuarios registrados en el sistemas
    //           
    public String ConsultaUsuarios(ref DataSet dsReferido, String id_usuario)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT UsuariosAdmin.*, PERFILES.* " +
                 "FROM   UsuariosAdmin INNER JOIN PERFILES ON UsuariosAdmin.id_perfil=PERFILES.id_perfil ";
        if (id_usuario != "")
            strSQL += "WHERE UsuariosAdmin.id_usuario=" + id_usuario;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : RetornarSubModulosUsuario
    //Objetivo : Retornar un dataset con todos los submódulos a los que el usuario id_usuario
    //           tiene acceso y que pertenencen al modulo id_modulo
    public String RetornarSubModulosUsuario(ref DataSet dsReferido, String id_usuario, String id_modulo)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT MODULOS.id_modulo, " +
                 "       MODULOS.modulo," +
                 "       MODULOS.pagina," +
                 "       MODULOS.id_modulo_padre " +
                 "FROM   UsuariosAdmin " +
                 "  INNER JOIN PERFILES ON(UsuariosAdmin.id_perfil=PERFILES.id_perfil)" +
                 "  INNER JOIN PERFILES_MODULOS ON(PERFILES.id_perfil=PERFILES_MODULOS.id_perfil)" +
                 "  INNER JOIN MODULOS ON(PERFILES_MODULOS.id_modulo=MODULOS.id_modulo) " +
                 "WHERE UsuariosAdmin.id_usuario='" + id_usuario + "' " +
                 "AND   MODULOS.visible=1 " +
                 "AND   MODULOS.id_modulo_padre='" + id_modulo + "' " +
                 "AND   PERFILES_MODULOS.consulta=1 " +
                 "ORDER BY MODULOS.orden ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : RetornarModulos
    //Objetivo : Retornar un dataset con todos los módulos de la aplicación
    //           consultando la tabla módulos
    public String RetornarModulos(ref DataSet dsReferido)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT MODULOS.id_modulo, " +
                 "       MODULOS.modulo, " +
                 "       MODULOS.id_modulo_padre " +
                 "FROM   MODULOS " +
                 "ORDER BY MODULOS.orden ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : RetornarModulosPadre
    //Objetivo : Retornar un dataset con todos los módulos de la aplicación
    //           que son padres de otros módulos
    public String RetornarModulosPadre(ref DataSet dsReferido)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT MODULOS.id_modulo, " +
                 "       MODULOS.modulo, " +
                 "       MODULOS.id_modulo_padre " +
                 "FROM   MODULOS " +
                 "WHERE   MODULOS.id_modulo_padre='0' " +
                 "ORDER BY MODULOS.orden ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : RetornarSubModulos
    //Objetivo : Retornar un dataset con todos los módulos de la aplicación
    //           que son hijos del id_modulo
    public String RetornarSubModulos(ref DataSet dsReferido, String id_modulo)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT MODULOS.id_modulo, " +
                 "       MODULOS.modulo, " +
                 "       MODULOS.id_modulo_padre " +
                 "FROM   MODULOS " +
                 "WHERE   MODULOS.id_modulo_padre='" + id_modulo + "' " +
                 "ORDER BY MODULOS.orden ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : RetornarPermisosModulo
    //Objetivo : Retornar un dataset con todos los permisos sobre el módulo id_modulo
    //           para el perfil id_perfil
    public String RetornarPermisosModulo(ref DataSet dsReferido, String id_modulo, String id_perfil)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT PERFILES_MODULOS.* " +
                 "FROM   PERFILES_MODULOS " +
                 "WHERE  PERFILES_MODULOS.id_modulo='" + id_modulo + "' " +
                 "AND    PERFILES_MODULOS.id_perfil='" + id_perfil + "' ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : EliminaPermisosPerfil
    //Objetivo : Elimina todos los permisos del perfil id_perfil de la
    //           tabla PERFILES_MODULOS
    public String EliminaPermisosPerfil(String id_perfil, object Conexion, object Transaccion)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = null;

        strSQL = "DELETE FROM PERFILES_MODULOS WHERE id_perfil='" + id_perfil + "'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, Conexion, Transaccion, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : GrabaPermisosPerfilModulo
    //Objetivo : Graba los permisos del perfil id_perfil para el modulo id_modulo
    //           en la tabla PERFILES_MODULOS
    public String GrabaPermisosPerfilModulo(String id_perfil, String id_modulo,
                                              int insert, int update,
                                              int delete, int query,
                                              Object Conexion,
                                              Object Transaccion)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = null;

        strSQL = "INSERT INTO PERFILES_MODULOS(id_modulo,id_perfil,insercion," +
                 "                             modificacion,borrado, consulta)" +
                 "            VALUES('" + id_modulo + "','" + id_perfil + "'," + insert.ToString() + "," + update.ToString() + "," + delete.ToString() +
                 "," + query.ToString() + ")";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, Conexion, Transaccion, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaPerfiles
    //Objetivo : Consulta los perfiles de la tabla PERFILES filtrando por el perfil
    //
    public String ConsultaPerfiles(ref DataSet dsReferido, String perfil)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT PERFILES.* " +
                 "FROM   PERFILES ";
        if (perfil != "")
            strSQL += "WHERE  upper(PERFILES.perfil) like '%" + perfil.ToUpper() + "%' ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaPerfil
    //Objetivo : Consulta el perfil de la tabla PERFILES identificado por id_perfil
    //
    public String ConsultaPerfil(ref DataSet dsReferido, String id_perfil)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT PERFILES.* " +
                 "FROM   PERFILES " +
                 "WHERE  PERFILES.id_perfil ='" + id_perfil + "' ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : EliminaPermisosPerfil
    //Objetivo : Elimina los permisos de la tabla PERFILES_MODULOS para el perfil
    //           identificado por id_perfil
    //
    public String EliminaPermisosPerfil(String id_perfil)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = null;

        strSQL = "DELETE FROM PERFILES_MODULOS " +
                 "WHERE  PERFILES_MODULOS.id_perfil ='" + id_perfil + "' ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : PermisoModulo
    //Objetivo : Verifica si el usuario id_usuario tiene permiso sobre el módulo id_modulo
    //           y en caso de que tenga permiso retorna true
    //           El tipo de permiso es : 
    //           I : Insert
    //           U : Update
    //           D : Delete
    //           Q : Query
    //
    public bool PermisoModulo(String id_modulo, String id_usuario, String tipo)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = new DataSet();

        tipo = tipo.ToUpper();
        strSQL = "SELECT PERFILES_MODULOS.* " +
                 "FROM   PERFILES_MODULOS " +
                 "INNER JOIN PERFILES ON(PERFILES_MODULOS.id_perfil=PERFILES.id_perfil) " +
                 "INNER JOIN UsuariosAdmin ON(PERFILES.id_perfil=UsuariosAdmin.id_perfil) " +
                 "WHERE  PERFILES_MODULOS.id_modulo='" + id_modulo + "' " +
                 "AND    UsuariosAdmin.id_usuario='" + id_usuario + "' ";
        switch (tipo)
        {
            case "I":
                strSQL += " AND insercion=1";
                break;
            case "U":
                strSQL += " AND modificacion=1";
                break;
            case "D":
                strSQL += " AND borrado=1";
                break;
            case "Q":
                strSQL += " AND consulta=1";
                break;
        }
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (Mensaje == "")
        {
            if (dsInterno.Tables.Count > 0)
            {
                if (dsInterno.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        else
            return false;

    }

    //==============================================================================
    //Nombre de la función : SoloLectura
    //Objetivo : Ubica todos los controles de la pagina Pagina y les cambia el estilo
    //           a campos de solo lectura. Se usa en caso de que el formulario quede en
    //           consulta
    public void SoloLectura(Page Pagina)
    {
        Control e, f;
        TextBox tb;
        DropDownList ddl;
        RadioButtonList rbl;
        Type t;
        HtmlTable table, table1;
        HtmlTableRow tablerow;
        HtmlTableCell tablecol;
        int j, k;

        foreach (Control c in Pagina.FindControl("Form1").Controls)
        {
            t = c.GetType();
            if (t.Name.ToUpper() == "HTMLTABLE")
            {
                table = (HtmlTable)c;
                DeshabilitaTabla(table);
            }
            if (t.Name.ToUpper() == "HTMLTABLEROW")
            {
                tablerow = (HtmlTableRow)c;
                for (j = 0; j < tablerow.Controls.Count; j++)
                {
                    e = tablerow.Controls[j];
                    t = e.GetType();
                    if (t.Name.ToUpper() == "HTMLTABLECELL")
                    {
                        tablecol = (HtmlTableCell)e;
                        for (k = 0; k < tablecol.Controls.Count; k++)
                        {
                            f = tablecol.Controls[k];
                            t = f.GetType();
                            if (t.Name.ToUpper() == "HTMLTABLE")
                            {
                                table1 = (HtmlTable)f;
                                DeshabilitaTabla(table1);
                            }
                            if (t.Namespace == "System.Web.UI.WebControls")
                            {
                                if (t.Name.ToUpper() == "TEXTBOX")
                                {
                                    tb = (TextBox)f;
                                    tb.ReadOnly = true;
                                    tb.Style.Add("disabled", "''");
                                    //tb.CssClass = "NoDigitar";
                                }
                                if (t.Name.ToUpper() == "DROPDOWNLIST")
                                {
                                    ddl = (DropDownList)f;
                                    ddl.Enabled = false;
                                    //ddl.CssClass = "NoDigitar";
                                    if (ddl.ID == "ddl_id_tipo_contenido")
                                        ddl.ID = "ddl_id_tipo_contenido";
                                }
                                if (t.Name.ToUpper() == "RADIOBUTTONLIST")
                                {
                                    rbl = (RadioButtonList)f;
                                    rbl.Enabled = false;
                                    //rbl.CssClass = "NoDigitar";
                                }
                            }
                        }
                    }
                }
            }
            if (t.Name.ToUpper() == "HTMLTABLECELL")
            {
                tablecol = (HtmlTableCell)c;
                for (k = 0; k < tablecol.Controls.Count; k++)
                {
                    f = tablecol.Controls[k];
                    t = f.GetType();
                    if (t.Name.ToUpper() == "HTMLTABLE")
                    {
                        table1 = (HtmlTable)f;
                        DeshabilitaTabla(table1);
                    }
                    if (t.Namespace == "System.Web.UI.WebControls")
                    {
                        if (t.Name.ToUpper() == "TEXTBOX")
                        {
                            tb = (TextBox)f;
                            tb.ReadOnly = true;
                            tb.Style.Add("disabled", "''");
                            //tb.CssClass = "NoDigitar";
                        }
                        if (t.Name.ToUpper() == "DROPDOWNLIST")
                        {
                            ddl = (DropDownList)f;
                            ddl.Enabled = false;
                            ddl.CssClass = "NoDigitar";
                            if (ddl.ID == "ddl_id_tipo_contenido")
                                ddl.ID = "ddl_id_tipo_contenido";
                        }
                        if (t.Name.ToUpper() == "RADIOBUTTONLIST")
                        {
                            rbl = (RadioButtonList)f;
                            rbl.Enabled = false;
                            rbl.CssClass = "NoDigitar";
                        }
                    }
                }
            }
            t = c.GetType();
            if (t.Namespace == "System.Web.UI.WebControls")
            {
                if (t.Name.ToUpper() == "TEXTBOX")
                {
                    tb = (TextBox)c;
                    tb.ReadOnly = true;
                    tb.Style.Add("disabled", "''");
                    //tb.CssClass = "NoDigitar";
                }
                if (t.Name.ToUpper() == "DROPDOWNLIST")
                {
                    ddl = (DropDownList)c;
                    ddl.Enabled = false;
                    ddl.CssClass = "NoDigitar";
                }
                if (t.Name.ToUpper() == "RADIOBUTTONLIST")
                {
                    rbl = (RadioButtonList)c;
                    rbl.Enabled = false;
                    rbl.CssClass = "NoDigitar";
                }
            }
        }
    }

    public void DeshabilitaTabla(HtmlTable table)
    {
        Control d, e, f;
        TextBox tb;
        DropDownList ddl;
        RadioButtonList rbl;
        CheckBox chk;
        Type t;
        HtmlTableRow tablerow;
        HtmlTableCell tablecol;
        int cuantos, i, j, k;
        HtmlTable table1;

        cuantos = table.Controls.Count;
        for (i = 0; i < cuantos; i++)
        {
            d = table.Controls[i];
            t = d.GetType();
            if (t.Name.ToUpper() == "HTMLTABLEROW")
            {
                tablerow = (HtmlTableRow)d;
                for (j = 0; j < tablerow.Controls.Count; j++)
                {
                    e = tablerow.Controls[j];
                    t = e.GetType();
                    if (t.Name.ToUpper() == "HTMLTABLECELL")
                    {
                        tablecol = (HtmlTableCell)e;
                        for (k = 0; k < tablecol.Controls.Count; k++)
                        {
                            f = tablecol.Controls[k];
                            t = f.GetType();
                            if (t.Name.ToUpper() == "HTMLTABLE")
                            {
                                table1 = (HtmlTable)f;
                                DeshabilitaTabla(table1);
                            }
                            if (t.Namespace == "System.Web.UI.WebControls")
                            {
                                if (t.Name.ToUpper() == "TEXTBOX")
                                {
                                    tb = (TextBox)f;
                                    tb.ReadOnly = true;
                                    tb.Style.Add("disabled", "''");
                                    //tb.CssClass = "NoDigitar";
                                }
                                if (t.Name.ToUpper() == "DROPDOWNLIST")
                                {
                                    ddl = (DropDownList)f;
                                    ddl.Enabled = false;
                                    ddl.CssClass = "NoDigitar";
                                    if (ddl.ID == "ddl_id_tipo_contenido")
                                        ddl.ID = "ddl_id_tipo_contenido";
                                }
                                if (t.Name.ToUpper() == "RADIOBUTTONLIST")
                                {
                                    rbl = (RadioButtonList)f;
                                    rbl.Enabled = false;
                                    rbl.CssClass = "NoDigitar";
                                }
                                if (t.Name.ToUpper() == "CHECKBOX")
                                {
                                    chk = (CheckBox)f;
                                    chk.Enabled = false;
                                    chk.CssClass = "NoDigitar";
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //==============================================================================
    //Nombre de la función : FiltraUsuarios
    //Objetivo : Filtra los usuarios de la tabla usuarios teniendo en cuenta los filtros
    //           en nombre, login y perfil
    //
    public String FiltraUsuarios(ref DataSet dsReferido, String nombre,
                                 String login, String perfil)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT UsuariosAdmin.*, " +
                 "       PERFILES.PERFIL " +
                 "FROM   UsuariosAdmin " +
                 "  INNER JOIN PERFILES ON(UsuariosAdmin.id_perfil=PERFILES.id_perfil) " +
                 "WHERE UsuariosAdmin.id_usuario is not null";
        if (nombre != "")
            strSQL += " AND upper(UsuariosAdmin.nombre) like '%" + nombre.ToUpper() + "%'";
        if (login != "")
            strSQL += " AND upper(UsuariosAdmin.login) like '%" + login.ToUpper() + "%'";
        if (perfil != "")
            strSQL += " AND PERFILES.id_perfil = " + perfil.ToUpper();
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    public Control FindControlRecursive(Control root)
    {
        Control d;
        TextBox tb;
        DropDownList ddl;
        RadioButtonList rbl;
        Type t;

        if (root != null)
        {
            t = root.GetType();
            if (t.Namespace == "System.Web.UI.WebControls")
            {
                if (t.Name.ToUpper() == "TEXTBOX")
                {
                    tb = (TextBox)root;
                    tb.ReadOnly = true;
                    tb.Style.Add("disabled", "''");
                    //tb.CssClass = "NoDigitar";
                }
                if (t.Name.ToUpper() == "DROPDOWNLIST")
                {
                    ddl = (DropDownList)root;
                    ddl.Enabled = false;
                    ddl.CssClass = "NoDigitar";
                }
                if (t.Name.ToUpper() == "RADIOBUTTONLIST")
                {
                    rbl = (RadioButtonList)root;
                    rbl.Enabled = false;
                    rbl.CssClass = "NoDigitar";
                }
            }
        }
        foreach (Control c in root.Controls)
        {
            d = FindControlRecursive(c);
            if (d != null)
            {
                t = d.GetType();
                if (t.Namespace == "System.Web.UI.WebControls")
                {
                    if (t.Name.ToUpper() == "TEXTBOX")
                    {
                        tb = (TextBox)d;
                        tb.ReadOnly = true;
                        tb.Style.Add("disabled", "''");
                        //tb.CssClass = "NoDigitar";
                    }
                    if (t.Name.ToUpper() == "DROPDOWNLIST")
                    {
                        ddl = (DropDownList)d;
                        ddl.Enabled = false;
                        ddl.CssClass = "NoDigitar";
                    }
                    if (t.Name.ToUpper() == "RADIOBUTTONLIST")
                    {
                        rbl = (RadioButtonList)d;
                        rbl.Enabled = false;
                        rbl.CssClass = "NoDigitar";
                    }
                }
                return d;
            }
        }
        return null;
    }

    //==============================================================================
    //Nombre de la función : DeshabilitaTD
    //Objetivo : Ubica todos los controles del td y les cambia el estilo
    //           a campos de solo lectura. Se usa en caso de que el td deba quedar en
    //          consulta
    public void DeshabilitaTD(HtmlTableCell td)
    {
        Control e, f;
        TextBox tb;
        DropDownList ddl;
        RadioButtonList rbl;
        Type t;
        HtmlTable table, table1;
        HtmlTableRow tablerow;
        HtmlTableCell tablecol;
        int j, k;

        foreach (Control c in td.Controls)
        {
            t = c.GetType();
            if (t.Name.ToUpper() == "HTMLTABLE")
            {
                table = (HtmlTable)c;
                DeshabilitaTabla(table);
            }
            if (t.Name.ToUpper() == "HTMLTABLEROW")
            {
                tablerow = (HtmlTableRow)c;
                for (j = 0; j < tablerow.Controls.Count; j++)
                {
                    e = tablerow.Controls[j];
                    t = e.GetType();
                    if (t.Name.ToUpper() == "HTMLTABLECELL")
                    {
                        tablecol = (HtmlTableCell)e;
                        for (k = 0; k < tablecol.Controls.Count; k++)
                        {
                            f = tablecol.Controls[k];
                            t = f.GetType();
                            if (t.Name.ToUpper() == "HTMLTABLE")
                            {
                                table1 = (HtmlTable)f;
                                DeshabilitaTabla(table1);
                            }
                            if (t.Namespace == "System.Web.UI.WebControls")
                            {
                                if (t.Name.ToUpper() == "TEXTBOX")
                                {
                                    tb = (TextBox)f;
                                    tb.ReadOnly = true;
                                    tb.Style.Add("disabled", "''");
                                }
                                if (t.Name.ToUpper() == "DROPDOWNLIST")
                                {
                                    ddl = (DropDownList)f;
                                    ddl.Enabled = false;
                                    ddl.CssClass = "NoDigitar";
                                    if (ddl.ID == "ddl_id_tipo_contenido")
                                        ddl.ID = "ddl_id_tipo_contenido";
                                }
                                if (t.Name.ToUpper() == "RADIOBUTTONLIST")
                                {
                                    rbl = (RadioButtonList)f;
                                    rbl.Enabled = false;
                                    rbl.CssClass = "NoDigitar";
                                }
                            }
                        }
                    }
                }
            }
            if (t.Name.ToUpper() == "HTMLTABLECELL")
            {
                tablecol = (HtmlTableCell)c;
                for (k = 0; k < tablecol.Controls.Count; k++)
                {
                    f = tablecol.Controls[k];
                    t = f.GetType();
                    if (t.Name.ToUpper() == "HTMLTABLE")
                    {
                        table1 = (HtmlTable)f;
                        DeshabilitaTabla(table1);
                    }
                    if (t.Namespace == "System.Web.UI.WebControls")
                    {
                        if (t.Name.ToUpper() == "TEXTBOX")
                        {
                            tb = (TextBox)f;
                            tb.ReadOnly = true;
                            //tb.CssClass = "NoDigitar";
                            tb.Style.Add("disabled", "''");
                        }
                        if (t.Name.ToUpper() == "DROPDOWNLIST")
                        {
                            ddl = (DropDownList)f;
                            ddl.Enabled = false;
                            ddl.CssClass = "NoDigitar";
                            if (ddl.ID == "ddl_id_tipo_contenido")
                                ddl.ID = "ddl_id_tipo_contenido";
                        }
                        if (t.Name.ToUpper() == "RADIOBUTTONLIST")
                        {
                            rbl = (RadioButtonList)f;
                            rbl.Enabled = false;
                            rbl.CssClass = "NoDigitar";
                        }
                    }
                }
            }
            t = c.GetType();
            if (t.Namespace == "System.Web.UI.WebControls")
            {
                if (t.Name.ToUpper() == "TEXTBOX")
                {
                    tb = (TextBox)c;
                    tb.ReadOnly = true;
                    //tb.CssClass = "NoDigitar";
                    tb.Style.Add("disabled", "''");
                }
                if (t.Name.ToUpper() == "DROPDOWNLIST")
                {
                    ddl = (DropDownList)c;
                    ddl.Enabled = false;
                    ddl.CssClass = "NoDigitar";
                }
                if (t.Name.ToUpper() == "RADIOBUTTONLIST")
                {
                    rbl = (RadioButtonList)c;
                    rbl.Enabled = false;
                    rbl.CssClass = "NoDigitar";
                }
            }
        }
    }

    public void HabilitaTabla(HtmlTable table)
    {
        Control d, e, f;
        TextBox tb;
        DropDownList ddl;
        RadioButtonList rbl;
        CheckBox chk;
        Type t;
        HtmlTable table1;
        HtmlTableRow tablerow;
        HtmlTableCell tablecol;
        int cuantos, i, j, k;

        cuantos = table.Controls.Count;
        for (i = 0; i < cuantos; i++)
        {
            d = table.Controls[i];
            t = d.GetType();
            if (t.Name.ToUpper() == "HTMLTABLEROW")
            {
                tablerow = (HtmlTableRow)d;
                for (j = 0; j < tablerow.Controls.Count; j++)
                {
                    e = tablerow.Controls[j];
                    t = e.GetType();
                    if (t.Name.ToUpper() == "HTMLTABLECELL")
                    {
                        tablecol = (HtmlTableCell)e;
                        for (k = 0; k < tablecol.Controls.Count; k++)
                        {
                            f = tablecol.Controls[k];
                            t = f.GetType();
                            if (t.Name.ToUpper() == "HTMLTABLE")
                            {
                                table1 = (HtmlTable)f;
                                HabilitaTabla(table1);
                            }
                            if (t.Namespace == "System.Web.UI.WebControls")
                            {
                                if (t.Name.ToUpper() == "TEXTBOX")
                                {
                                    tb = (TextBox)f;
                                    if (tb.ReadOnly)
                                    {
                                        //tb.CssClass = "NoDigitar";
                                        tb.Style.Add("disabled", "''");
                                    }
                                    else
                                    {
                                        tb.ReadOnly = false;
                                        tb.CssClass = "form-control";
                                    }
                                }
                                if (t.Name.ToUpper() == "DROPDOWNLIST")
                                {
                                    ddl = (DropDownList)f;
                                    ddl.Enabled = false;
                                    if (ddl.ID == "ddl_id_tipo_contenido")
                                        ddl.ID = "ddl_id_tipo_contenido";
                                }
                                if (t.Name.ToUpper() == "RADIOBUTTONLIST")
                                {
                                    rbl = (RadioButtonList)f;
                                    rbl.Enabled = true;
                                }
                                if (t.Name.ToUpper() == "CHECKBOX")
                                {
                                    chk = (CheckBox)f;
                                    chk.Enabled = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void DeshabilitaTablaSinReadOnly(HtmlTable table)
    {
        Control d, e, f;
        TextBox tb;
        DropDownList ddl;
        RadioButtonList rbl;
        CheckBox chk;
        Type t;
        HtmlTable table1;
        HtmlTableRow tablerow;
        HtmlTableCell tablecol;
        int cuantos, i, j, k;

        cuantos = table.Controls.Count;
        for (i = 0; i < cuantos; i++)
        {
            d = table.Controls[i];
            t = d.GetType();
            if (t.Name.ToUpper() == "HTMLTABLEROW")
            {
                tablerow = (HtmlTableRow)d;
                for (j = 0; j < tablerow.Controls.Count; j++)
                {
                    e = tablerow.Controls[j];
                    t = e.GetType();
                    if (t.Name.ToUpper() == "HTMLTABLECELL")
                    {
                        tablecol = (HtmlTableCell)e;
                        for (k = 0; k < tablecol.Controls.Count; k++)
                        {
                            f = tablecol.Controls[k];
                            t = f.GetType();
                            if (t.Name.ToUpper() == "HTMLTABLE")
                            {
                                table1 = (HtmlTable)f;
                                DeshabilitaTabla(table1);
                            }
                            if (t.Namespace == "System.Web.UI.WebControls")
                            {
                                if (t.Name.ToUpper() == "TEXTBOX")
                                {
                                    tb = (TextBox)f;
                                    tb.Style.Add("disabled", "''");
                                    //tb.CssClass = "NoDigitar";
                                }
                                if (t.Name.ToUpper() == "DROPDOWNLIST")
                                {
                                    ddl = (DropDownList)f;
                                    ddl.Enabled = false;
                                    ddl.CssClass = "NoDigitar";
                                    if (ddl.ID == "ddl_id_tipo_contenido")
                                        ddl.ID = "ddl_id_tipo_contenido";
                                }
                                if (t.Name.ToUpper() == "RADIOBUTTONLIST")
                                {
                                    rbl = (RadioButtonList)f;
                                    rbl.Enabled = false;
                                    rbl.CssClass = "NoDigitar";
                                }
                                if (t.Name.ToUpper() == "CHECKBOX")
                                {
                                    chk = (CheckBox)f;
                                    chk.Enabled = false;
                                    chk.CssClass = "NoDigitar";
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public bool VerificaPwd(ref DataSet dsReferido, String id_usuario, String clave)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        bool esCorrecto = false;

        try
        {
            strSQL = "SELECT * FROM UsuariosAdmin WHERE id_usuario=" + id_usuario +
                     " AND upper(password)='" + clave.ToUpper() + "'";
            ConexionDataObject = new clsDataLayer();
            ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        }
        catch (Exception ex)
        {
            Mensaje = ex.ToString();
        }
        if (dsReferido.Tables[0] == null)
        {
            return false;
        }
        if (dsReferido.Tables[0].Rows.Count != 0)
            esCorrecto = true;
        else
            esCorrecto = false;
        return esCorrecto;
    }

    //==============================================================================
    //Nombre de la función : FiltraModulos
    //Objetivo : Consulta los módulos de la tabla MODULOS filtrando por nombre del módulo(opcional)
    //
    public String FiltraModulos(ref DataSet dsReferido, String sRSret, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT MODULOS.*, " +
                 "       MODULO_PADRE.modulo AS modulo_padre " +
                 "FROM   MODULOS " +
                 "  LEFT OUTER JOIN MODULOS MODULO_PADRE ON MODULOS.id_modulo_padre=MODULO_PADRE.id_modulo ";
        if (nombre != "")
            strSQL += "WHERE upper(MODULOS.modulo) like '%" + nombre.ToUpper() + "%' ";
        strSQL += "ORDER BY MODULOS.id_modulo_padre, MODULOS.orden ";
        sRSret = strSQL;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaModulos
    //Objetivo : Consulta los módulos de la tabla MODULOS filtrando por el id_modulo(opcional)
    //
    public string ConsultaModulos(ref DataSet dsReferido, String id_modulo)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT MODULOS.*, " +
                 "       MODULO_PADRE.modulo AS modulo_padre " +
                 "FROM   MODULOS " +
                 "  LEFT OUTER JOIN MODULOS MODULO_PADRE ON MODULOS.id_modulo_padre=MODULO_PADRE.id_modulo " +
                 "WHERE MODULOS.id_modulo IS NOT NULL ";
        if (id_modulo != "")
            strSQL += "AND MODULOS.id_modulo=" + id_modulo;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

}

using System;
using System.Data;

/// <summary>
/// Descripción breve de blParametricas
/// </summary>
public class clsblParametricas
{


    public struct Niveles
    {
        public string Id;
        public string Nombre;
        public string IdPadre;
    }

    public clsblParametricas()
    {
    }

    //==============================================================================
    //Nombre de la función : ConsultaCausasAbandono
    //Objetivo : Retornar un dataset los registros de la tabla CausaAbandono
    //           
    public String ConsultaCausasAbandono(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT CausasAbandono.* " +
                 "FROM CausasAbandono WHERE Id IS NOT NULL";
        if (id != "")
            strSQL += " AND CausasAbandono.Id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(CausasAbandono.Nombre) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaOficinas
    //Objetivo : Retornar un dataset los registros de la tabla Oficinas
    //           
    public String ConsultaOficinas(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT Oficinas.* " +
                 "FROM Oficinas WHERE Id IS NOT NULL";
        if (id != "")
            strSQL += " AND Oficinas.Id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(Oficinas.Nombre) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }
    public String ConsultaProcedencia(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "select * from Procedencias P  ";
        if (id != "")
            strSQL += "WHERE  P.Id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(P.Nombre) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }
    public String ConsultaTiposSede(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "select * from TipoSedes P  ";
        if (id != "")
            strSQL += "WHERE  P.Id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(P.Nombre) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    public String ConsultarTematicas(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = " Select ofi.Nombre as 'Seccional', gpo.Nombre as 'Tematica',gpo.Abreviatura " +
                 "from Oficinas ofi " +
                 "  inner join Grupo_oficinas gp on ofi.Id = gp.OficinaId " +
                 "  inner join Grupos gpo on gp.Id_Grupo = gpo.id ";
        if (id != "")
            strSQL += " AND ofi.Id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(ofi.Nombre) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    public String ConsultarSedesUsuarios(ref DataSet dsReferido, String id)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = $@"SELECT DISTINCT s.Descripcion, o.Nombre
                    FROM Usuarios u
                    INNER JOIN PrioridadDeAsesor pa ON pa.UsuarioId = u.Id
                    INNER JOIN Prioridad p ON pa.PrioridadId = p.Id
                    INNER JOIN Sala s ON p.SalaId = s.Id
                    INNER JOIN Oficinas o ON s.OficinaId = o.Id
                    WHERE u.Id = '{id}'";

        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    public String ConsultarSalas(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT o.Nombre,sa.Id,sa.TipoId,sa.Descripcion,sa.OficinaId,g.id as 'TematicaId',mn.id as idmn, dp.id as iddep,dp.Dept_Id_Dane as IdDperatamento,  dp.Nombre as 'Departamento', mn.Nombre as 'Municipio'" +
                 "FROM Sala sa " +
                 "INNER JOIN Oficinas o on sa.OficinaId = o.Id  " +
                 "inner join Grupo_oficinas gp on o.Id =  gp.OficinaId " +
                 "inner join Grupos g on gp.Id_Grupo = g.id  " +
                 "inner join Municipios mn on sa.MunId = mn.iD " +
                 "inner join Departamentos dp on mn.Dept_Id_Dane = dp.Dept_Id_Dane ";

        ;
        if (id != "")
            strSQL += " AND sa.Id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(Sala.Nombre) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }
    public String ConsultarDepartamentos(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "select Id,Nombre,Dept_Id_Dane from Departamentos dp WHERE  "

                 ;
        if (id != "")
            strSQL += "  id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(dp.Nombre) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    public String ConsltaSeccionalesPortematica(ref DataSet dsReferido, String id)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = $@"select o.id, o.Nombre from Oficinas o
                    inner join Grupo_oficinas gof on gof.OficinaId = o.Id
                    inner join Grupos t on gof.Id_Grupo = t.id
                    where t.Id = {int.Parse(id)}";

        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);

        return Mensaje;
    }

    public String ConsultarMunicipios(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "select Id,Nombre from Municipios mn WHERE  "

                 ;
        if (id != "")
            strSQL += "  Dept_Id_Dane =" + id;
        if (nombre != "")
            strSQL += " AND UPPER(mn.Nombre) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }



    public String ConsultaServiciosAtencion(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT Servicios_Autoaten.* " +
                 "FROM Servicios_Autoaten WHERE Id IS NOT NULL";
        if (id != "")
            strSQL += " AND Servicios_Autoaten.Id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(Servicios.Nombre) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    public String ConsultaServiciosConsultaKiosco(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT sc.*  from ServiciosConsultas sc " +
                 "where Activo = 1 and Id is not null";
        if (id != "")
            strSQL += " AND sc.Id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(sc.Nombre) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }



    //==============================================================================
    //Nombre de la función : ConsultaPerfilesAtencion
    //Objetivo : Retornar un dataset los registros de la tabla PerfilesDeAtencion
    //           
    public String ConsultaPerfilesAtencion(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT PerfilesDeAtencion.* " +
                 "FROM PerfilesDeAtencion WHERE Id IS NOT NULL";
        if (id != "")
            strSQL += " AND PerfilesDeAtencion.Id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(PerfilesDeAtencion.Nombre) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaPrioridades
    //Objetivo : Retornar un dataset los registros de la tabla Prioridad
    //           
    public String ConsultaPrioridades(ref DataSet dsReferido, String id, string OficinaId, string SalaId, string TematicaId)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT Prioridad.*,sa.Id as IdSala, Prioridad.CantMaxTurnos as CantMaxTurnos, sa.Descripcion ,gp.Nombre as tematica , gp.id as TematicaId,Servicios.Nombre + ' ' +TipoDeAtencion.Nombre as Servicios, PerfilesDeAtencion.Nombre as perfildeatencion, Servicios.Nombre as servicio, TipoDeAtencion.Nombre as tipodeatencion, Oficinas.Nombre as oficina, ServicioTipoAtencion.Abreviatura " +
                 "FROM Prioridad " +
                 "  INNER JOIN Oficinas ON Prioridad.OficinaId=Oficinas.Id " +
                 "  INNER JOIN Sala sa ON Prioridad.SalaId=sa.Id" +
                 "  INNER JOIN PerfilesDeAtencion ON Prioridad.PerfilDeAtencionId=PerfilesDeAtencion.Id " +
                 "  INNER JOIN ServicioTipoAtencion ON Prioridad.ServicioTipoAtencionId=ServicioTipoAtencion.Id " +
                 "  INNER JOIN Servicios ON ServicioTipoAtencion.ServicioId=Servicios.Id " +
                 "  INNER JOIN TipoDeAtencion ON ServicioTipoAtencion.TipoAtencionId=TipoDeAtencion.Id " +
                 "  INNER JOIN Grupos GP on Prioridad.GrupoId = GP.id " +
                 "WHERE Prioridad.Id IS NOT NULL";
        if (id != "")
            strSQL += " AND Prioridad.Id=" + id;
        if (OficinaId != "")
            strSQL += " AND Prioridad.OficinaId=" + OficinaId;
        if (SalaId != "")
            strSQL += " AND sa.Id=" + SalaId;
        if (TematicaId != "")
            strSQL += " AND gp.Id=" + TematicaId;
        strSQL += " ORDER BY oficina, perfildeatencion, servicio, tipodeatencion desc";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    public String ConsultaPrioridadesHabilitadas(ref DataSet dsReferido, String id, string OficinaId, string SalaId)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT Prioridad.*,sa.Id as IdSala, sa.Descripcion ,PerfilesDeAtencion.Nombre as perfildeatencion, Servicios.Nombre as servicio, TipoDeAtencion.Nombre as tipodeatencion, Oficinas.Nombre as oficina, ServicioTipoAtencion.Abreviatura " +
                 "FROM Prioridad " +
                 "  INNER JOIN Oficinas ON Prioridad.OficinaId=Oficinas.Id " +
                 "  INNER JOIN Sala sa ON Prioridad.SalaId=sa.Id" +
                 "  INNER JOIN PerfilesDeAtencion ON Prioridad.PerfilDeAtencionId=PerfilesDeAtencion.Id " +
                 "  INNER JOIN ServicioTipoAtencion ON Prioridad.ServicioTipoAtencionId=ServicioTipoAtencion.Id " +
                 "  INNER JOIN Servicios ON ServicioTipoAtencion.ServicioId=Servicios.Id " +
                 "  INNER JOIN TipoDeAtencion ON ServicioTipoAtencion.TipoAtencionId=TipoDeAtencion.Id " +
                 "WHERE Prioridad.Id IS NOT NULL AND Prioridad.Habilitado = '1'";
        if (id != "")
            strSQL += " AND Prioridad.Id=" + id;
        if (OficinaId != "")
            strSQL += " AND Prioridad.OficinaId=" + OficinaId;
        if (SalaId != "")
            strSQL += " AND sa.Id=" + SalaId;
        strSQL += " ORDER BY oficina, perfildeatencion, servicio, tipodeatencion desc";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaPuestos
    //Objetivo : Retornar un dataset los registros de la tabla PuestosDeAtencion
    //           
    public String ConsultaPuestos(ref DataSet dsReferido, String id, String NombreOficina)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT PuestosDeAtencion.*, Oficinas.nombre AS nombreOficina, sl.Descripcion " +
                 "FROM PuestosDeAtencion " +
                 "inner join Sala sl on PuestosDeAtencion.SalaId = sl.Id" +
                 "  INNER JOIN Oficinas ON PuestosDeAtencion.OficinaId=Oficinas.Id  " +
                 "WHERE PuestosDeAtencion.Id IS NOT NULL";
        if (id != "")
            strSQL += " AND PuestosDeAtencion.Id=" + id;
        if (NombreOficina != "")
            strSQL += " AND UPPER(PuestosDeAtencion.nombre) like '" + NombreOficina.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaServicios
    //Objetivo : Retornar un dataset los registros de la tabla Servicios
    //           
    public String ConsultaServicios(ref DataSet dsReferido, String id, String NombreServicio)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT Servicios.*, ServPadre.nombre as serviciopadre " +
                 "FROM Servicios " +
                 "  LEFT OUTER JOIN Servicios ServPadre ON Servicios.ServicioPadreId=ServPadre.Id  " +
                 "WHERE Servicios.Id IS NOT NULL";
        if (id != "")
            strSQL += " AND Servicios.Id=" + id;
        if (NombreServicio != "")
            strSQL += " AND UPPER(Servicios.nombre) like '" + NombreServicio.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }
    public String ConsultaSubServicios(ref DataSet dsReferido, String id, String NombreServicio)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "select Id,Descripcion,Habilitado from SubServicio ";
        if (id != "")
            strSQL += " where SubServicio.Id=" + id;
        if (NombreServicio != "")
            strSQL += " AND UPPER(SubServicio.Descripcion) like '" + NombreServicio.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : CreaArbolServicios
    //Objetivo : Retornar un dataset los registros de la tabla Servicios
    //           
    public String CreaArbolServicios(Niveles[,] Levels, int CurrentLevel, string IdServicio)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = new DataSet();

        strSQL = "SELECT Servicios.* " +
                 "FROM Servicios ";
        if (IdServicio == "")
            strSQL += " WHERE Servicios.ServicioPadreId IS NULL ";
        else
            strSQL += " WHERE Servicios.ServicioPadreId=" + IdServicio;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (Mensaje == "")
        {
            if (dsInterno.Tables[0].Rows.Count > 0)
            {
                int j = 0;
                for (int i = 0; i < dsInterno.Tables[0].Rows.Count; i++)
                {
                    while (Levels[CurrentLevel, j].Id != "")
                        j++;
                    Levels[CurrentLevel, j].Id = dsInterno.Tables[0].Rows[i]["Id"].ToString();
                    Levels[CurrentLevel, j].Nombre = dsInterno.Tables[0].Rows[i]["Nombre"].ToString();
                    Levels[CurrentLevel, j].IdPadre = dsInterno.Tables[0].Rows[i]["ServicioPadreId"].ToString();
                    CreaArbolServicios(Levels, CurrentLevel + 1, dsInterno.Tables[0].Rows[i]["Id"].ToString());
                }
            }
        }
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaServiciosPerfilDeAtencion
    //Objetivo : Retornar un dataset los registros de la tabla ServiciosEnPerfil para el ServicioId y PerfilDeAtencionId
    //           
    public String ConsultaServiciosPerfilDeAtencion(ref DataSet dsReferido, String ServicioId, String PerfilDeAtencionId)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT * " +
                 "FROM ServiciosEnPerfil " +
                 "WHERE ServiciosEnPerfil.ServicioId = " + ServicioId + " AND ServiciosEnPerfil.PerfilDeAtencionId=" + PerfilDeAtencionId;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : BorraServiciosPerfilDeAtencion
    //Objetivo : Borra los registros de la tabla ServiciosEnPerfil para el PerfilDeAtencionId
    //           
    public String BorraServiciosPerfilDeAtencion(String PerfilDeAtencionId, object Conexion, object Transaccion)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = null;

        strSQL = "DELETE FROM ServiciosEnPerfil " +
                 "WHERE ServiciosEnPerfil.PerfilDeAtencionId=" + PerfilDeAtencionId;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, Conexion, Transaccion, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaServiciosOficina
    //Objetivo : Retornar un dataset los registros de la tabla ServiciosEnOficina para el ServicioId y OficinaId
    //           
    public String ConsultaServiciosOficina(ref DataSet dsReferido, String Id, String ServicioId, String OficinaId, object Conexion, object Transaccion)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        if (Id == "")
        {
            strSQL = "SELECT * " +
                     "FROM ServiciosEnOficina " +
                     "WHERE ServiciosEnOficina.ServicioId = " + ServicioId + " AND ServiciosEnOficina.OficinaId=" + OficinaId;
        }
        else
        {
            strSQL = "SELECT ServiciosEnOficina.*, Oficinas.Nombre as NombreOficina, Servicios.Nombre as NombreServicio  " +
                     "FROM ServiciosEnOficina " +
                     "  INNER JOIN Oficinas ON ServiciosEnOficina.OficinaId=Oficinas.Id  " +
                     "  INNER JOIN Servicios ON ServiciosEnOficina.ServicioId=Servicios.Id  " +
                     "WHERE ServiciosEnOficina.Id = " + Id;
        }
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, Conexion, Transaccion, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : BorraServiciosEnOficina
    //Objetivo : Borra los registros de la tabla ServiciosEnOficina para el OficinaId
    //           
    public String BorraServiciosEnOficina(String OficinaId, object Conexion, object Transaccion)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = null;

        strSQL = "DELETE FROM ServiciosEnOficina " +
                 "WHERE ServiciosEnOficina.OficinaId=" + OficinaId;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, Conexion, Transaccion, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : BorraServicioEnOficina
    //Objetivo : Borra el registro de la tabla ServiciosEnOficina para el OficinaId y el ServicioId
    //           
    public String BorraServicioEnOficina(String OficinaId, String ServicioId, object Conexion, object Transaccion)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = null;

        strSQL = "DELETE FROM ServiciosEnOficina " +
                 "WHERE ServiciosEnOficina.OficinaId=" + OficinaId + " AND ServiciosEnOficina.ServicioId=" + ServicioId;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, Conexion, Transaccion, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : CreaArbolServicios1
    //Objetivo : Retornar un dataset los registros de la tabla Servicios
    //           
    public String CreaArbolServicios1(Niveles[,] Levels, int CurrentLevel, string IdServicio, string OficinaId)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = new DataSet();

        strSQL = "SELECT Servicios.* " +
                 "FROM Servicios ";
        if (OficinaId != "")
            strSQL += " INNER JOIN ServiciosEnOficina ON Servicios.Id=ServiciosEnOficina.ServicioId ";
        if (IdServicio == "")
            strSQL += " WHERE Servicios.ServicioPadreId IS NULL ";
        else
            strSQL += " WHERE Servicios.ServicioPadreId=" + IdServicio;
        if (OficinaId != "")
            strSQL += " AND ServiciosEnOficina.OficinaId= " + OficinaId;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (Mensaje == "")
        {
            if (dsInterno.Tables[0].Rows.Count > 0)
            {
                int j = 0;
                for (int i = 0; i < dsInterno.Tables[0].Rows.Count; i++)
                {
                    while (Levels[CurrentLevel, j].Id != "")
                        j++;
                    Levels[CurrentLevel, j].Id = dsInterno.Tables[0].Rows[i]["Id"].ToString();
                    Levels[CurrentLevel, j].Nombre = dsInterno.Tables[0].Rows[i]["Nombre"].ToString();
                    Levels[CurrentLevel, j].IdPadre = dsInterno.Tables[0].Rows[i]["ServicioPadreId"].ToString();
                    CreaArbolServicios1(Levels, CurrentLevel + 1, dsInterno.Tables[0].Rows[i]["Id"].ToString(), OficinaId);
                }
            }
        }
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaPerfilesOficina
    //Objetivo : Retornar un dataset los registros de la tabla PerfilesEnOficina para el PerfilDeAtencionId y OficinaId
    //           
    public String ConsultaPerfilesOficina(ref DataSet dsReferido, String Id, String PerfilDeAtencionId, String OficinaId, object Conexion, object Transaccion)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        if (Id == "")
        {
            strSQL = "SELECT * " +
                     "FROM PerfilesEnOficina " +
                     "WHERE PerfilesEnOficina.PerfilDeAtencionId = " + PerfilDeAtencionId + " AND PerfilesEnOficina.OficinaId=" + OficinaId;
        }
        else
        {
            strSQL = "SELECT PerfilesEnOficina.*, Oficinas.Nombre as NombreOficina, PerfilesDeAtencion.Nombre as NombrePerfilAtencion  " +
                     "FROM PerfilesEnOficina " +
                     "  INNER JOIN Oficinas ON PerfilesEnOficina.OficinaId=Oficinas.Id  " +
                     "  INNER JOIN PerfilesDeAtencion ON PerfilesEnOficina.PerfilDeAtencionId=PerfilesDeAtencion.Id  " +
                     "WHERE PerfilesEnOficina.Id = " + Id;
        }
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, Conexion, Transaccion, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : BorraPerfilesEnOficina
    //Objetivo : Borra el registro de la tabla ServiciosEnOficina para el OficinaId y el ServicioId
    //           
    public String BorraPerfilesEnOficina(String OficinaId, object Conexion, object Transaccion)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = null;

        strSQL = "DELETE FROM PerfilesEnOficina " +
                 "WHERE PerfilesEnOficina.OficinaId=" + OficinaId;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, Conexion, Transaccion, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaTurneros
    //Objetivo : Retornar un dataset los registros de la tabla Turnero
    //           
    public String ConsultaTurneros(ref DataSet dsReferido, String id, String OficinaId)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT Turnero.* " +
                 "FROM Turnero WHERE Id IS NOT NULL";
        if (id != "")
            strSQL += " AND Turnero.Id=" + id;
        if (OficinaId != "")
            strSQL += " AND Turnero.OficinaId=" + OficinaId;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaTiposDeAtencion
    //Objetivo : Retornar un dataset los registros de la tabla Turnero
    //           
    public String ConsultaTiposDeAtencion(ref DataSet dsReferido, String id, string nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT TipoDeAtencion.* " +
                 "FROM TipoDeAtencion WHERE Id IS NOT NULL";
        if (id != "")
            strSQL += " AND TipoDeAtencion.Id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(TipoDeAtencion.Nombre) like '" + nombre.ToUpper() + "%' ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaServiciosTiposDeAtencion
    //Objetivo : Retornar un dataset los registros de la tabla Turnero
    //           
    public String ConsultaServiciosTiposDeAtencion(ref DataSet dsReferido, String id, string nombreservicio)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT ServicioTipoAtencion.*, Servicios.Nombre AS Servicio, TipoDeAtencion.Nombre AS TipoAtencion  " +
                 "FROM ServicioTipoAtencion" +
                 "  INNER JOIN  Servicios ON ServicioTipoAtencion.ServicioId=Servicios.Id " +
                 "  INNER JOIN  TipoDeAtencion ON ServicioTipoAtencion.TipoAtencionId=TipoDeAtencion.Id " +
                 "WHERE ServicioTipoAtencion.Id IS NOT NULL";
        if (id != "")
            strSQL += " AND ServicioTipoAtencion.Id=" + id;
        if (nombreservicio != "")
            strSQL += " AND UPPER(Servicios.Nombre) like '" + nombreservicio.ToUpper() + "%' ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaCanales
    //Objetivo : Retornar un dataset los registros de la tabla Turnero
    //           
    public String ConsultaCanales(ref DataSet dsReferido, String id, string nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT Canales.* " +
                 "FROM Canales WHERE Id IS NOT NULL";
        if (id != "")
            strSQL += " AND Canales.Id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(Canales.Nombre) like '" + nombre.ToUpper() + "%' ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaPrioridadesUsuario
    //Objetivo : Retornar un dataset los registros de la tabla Prioridad
    //           
    public bool ConsultaPrioridadesUsuario(ref string Id, string PrioridadId, string UsuarioId)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        bool TienePrioridad = false;
        DataSet dsInterno = new DataSet();

        strSQL = "SELECT * FROM PrioridadDeAsesor WHERE PrioridadId=" + PrioridadId + " AND UsuarioId='" + UsuarioId + "' AND UPPER(Habilitado)='S'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (dsInterno.Tables[0].Rows.Count > 0)
        {
            TienePrioridad = true;
            Id = dsInterno.Tables[0].Rows[0]["Id"].ToString();
        }
        return TienePrioridad;
    }

    //==============================================================================
    //Nombre de la función : ExistePrioridadesUsuario
    //Objetivo : Retornar true si existe un registro en PrioridadDeAsesor para la PrioridadId y UsuarioId
    //           
    public bool ExistePrioridadesUsuario(ref string Id, string PrioridadId, string UsuarioId)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        bool TienePrioridad = false;
        DataSet dsInterno = new DataSet();

        strSQL = "SELECT * FROM PrioridadDeAsesor WHERE PrioridadId=" + PrioridadId + " AND UsuarioId='" + UsuarioId + "'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (dsInterno.Tables[0].Rows.Count > 0)
        {
            TienePrioridad = true;
            Id = dsInterno.Tables[0].Rows[0]["Id"].ToString();
        }
        return TienePrioridad;
    }


    //==============================================================================
    //Nombre de la función : ConsultaUsuariosAsesores
    //Objetivo : Retornar un dataset los registros de la tabla Usuarios
    //           
    public String ConsultaUsuariosAsesores(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT Usuarios.* , ps.Nombre as Pnombre " +
                 "FROM Usuarios " +
                 "inner join Procedencias ps on Usuarios.ProcedenciaId = ps.Id " +
                 "WHERE Usuarios.Id IS NOT NULL ";
        if (id != "")
            strSQL += " AND Usuarios.Id='" + id + "'";
        if (nombre != "")
            strSQL += " AND UPPER(Usuarios.Nombre) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaCaracterizaciones
    //Objetivo : Retornar un dataset los registros de la tabla Usuarios
    //           
    public String ConsultaCaracterizaciones(ref DataSet dsReferido, bool soloPadres)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";


        strSQL = @"select Caracterizacion.*, cp.Descripcion as DescripcionPadre  from Caracterizacion 
                    left join Caracterizacion cp on Caracterizacion.IdPadre = cp.Id";

        if (soloPadres)
            strSQL = "SELECT c.Id, c.Descripcion, c.IdPadre FROM Caracterizacion c WHERE IdPadre IS NULL";

        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaCaracterizacion
    //Objetivo : Retornar un dataset los registros de la tabla Usuarios
    //           
    public String ConsultaCaracterizacion(ref DataSet dsReferido, string id)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = $@"select C.*, cp.Descripcion as DescripcionPadre from Caracterizacion c
                    left join Caracterizacion cp on C.IdPadre = cp.Id 
                    where C.Id = {id}";

        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : GrabarCaracterizacion
    //Objetivo : Actualiza un registro de la tabla Caracterizacion
    //           
    public bool GrabarCaracterizacion(ref DataSet dsReferido, string id, string nombre, int idPadre, int habilitado)
    {
        NSSSqlUtil blObj = new NSSSqlUtil();
        String msgError = "";
        DataSet dsInterno = null;
        String strAux = "";
        clsblUtiles blU = new clsblUtiles();



        blObj.LlavePrimaria = "id";
        blObj.NombreTabla = "Caracterizacion";
        blObj.Add("Descripcion", nombre);

        if (idPadre == 0)
            blObj.Add("idPadre", "");
        else
            blObj.Add("idPadre", idPadre.ToString());

        blObj.Add("Habilitado", habilitado.ToString());

        if (id == "")
        {
            blObj.IsIdentity = true;
            msgError = blObj.NssEjecutarSQL("INSERT", ref dsInterno, ref strAux, "", "", null, null);
            id = strAux;
        }
        else
        {
            blObj.Add("id", id);
            msgError = blObj.NssEjecutarSQL("UPDATE", ref dsInterno, ref strAux, "", "", null, null);
        }

        return true;
    }


    //==============================================================================
    //Nombre de la función : ConsultaParametrosGenerales
    //Objetivo : Retornar un dataset los registros de la tabla Parametros_Generales
    //           
    public String ConsultaParametrosGenerales(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT Parametros_Generales.* " +
                 "FROM Parametros_Generales WHERE Id IS NOT NULL";
        if (id != "")
            strSQL += " AND Parametros_Generales.Id='" + id + "'";
        if (nombre != "")
            strSQL += " AND UPPER(Parametros_Generales.Parametro) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaEncuestas
    //Objetivo : Retornar un dataset los registros de la tabla Encuestas
    //           
    public String ConsultaEncuestasCARLOS(ref DataSet dsReferido, String id, String nombre_encuesta)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT Encuestas.* " +
                 "FROM Encuestas WHERE Id IS NOT NULL";
        if (id != "")
            strSQL += " AND Encuestas.Id='" + id + "'";
        if (nombre_encuesta != "")
            strSQL += " AND UPPER(Encuestas.nombre_encuesta) like '" + nombre_encuesta.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaEncuestasPreguntas
    //Objetivo : Retornar un dataset los registros de la tabla EncuestasPreguntas
    //           
    public String ConsultaEncuestasPreguntas(ref DataSet dsReferido, String id, String descripcion, String EncuestaId)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT EncuestasPreguntas.* " +
                 "FROM EncuestasPreguntas WHERE Id IS NOT NULL";
        if (id != "")
            strSQL += " AND EncuestasPreguntas.Id='" + id + "'";
        if (descripcion != "")
            strSQL += " AND UPPER(EncuestasPreguntas.descripcion) like '" + descripcion.ToUpper() + "%'";
        if (EncuestaId != "")
            strSQL += " AND EncuestasPreguntas.EncuestaId='" + EncuestaId + "'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaEncuestasPreguntasOpciones
    //Objetivo : Retornar un dataset los registros de la tabla EncuestasPreguntasOpciones
    //           
    public String ConsultaEncuestasPreguntasOpciones(ref DataSet dsReferido, String id, String descripcion, String EncuestaPreguntaId)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT EncuestasPreguntasOpciones.* " +
                 "FROM EncuestasPreguntasOpciones WHERE Id IS NOT NULL";
        if (id != "")
            strSQL += " AND EncuestasPreguntasOpciones.Id='" + id + "'";
        if (descripcion != "")
            strSQL += " AND UPPER(EncuestasPreguntasOpciones.descripcion) like '" + descripcion.ToUpper() + "%'";
        if (EncuestaPreguntaId != "")
            strSQL += " AND EncuestasPreguntasOpciones.EncuestaPreguntaId='" + EncuestaPreguntaId + "'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    #region OERD - FUNCIONES NUEVAS INPEC OCTUBRE DE 2020

    //==============================================================================
    //Nombre de la función : ConsultaGrupos
    //Objetivo : Retornar un dataset los registros de la tabla Grupos
    //           
    public string ConsultaGrupos(ref DataSet dsReferido, string id, string NombreGrupo)
    {
        string strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        string Mensaje = "";

        strSQL = "SELECT Grupos.*, GrupoPadre.nombre as grupopadre " +
                 "FROM Grupos " +
                 "  LEFT OUTER JOIN Grupos GrupoPadre ON Grupos.GrupoPadreId=GrupoPadre.Id  " +
                 "WHERE Grupos.Id IS NOT NULL";
        if (id != "")
            strSQL += " AND Grupos.Id=" + id;
        if (NombreGrupo != "")
            strSQL += " AND UPPER(Grupos.nombre) like '%" + NombreGrupo.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : CreaArbolGrupos
    //Objetivo : Retornar un dataset los registros de la tabla Grupos
    //           
    public String CreaArbolGrupos(Niveles[,] Levels, int CurrentLevel, string IdGrupo)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = new DataSet();

        strSQL = "SELECT GRUPOS.* " +
                 "FROM GRUPOS ";
        if (IdGrupo == "")
            strSQL += " WHERE GRUPOS.GrupoPadreId IS NULL ";
        else
            strSQL += " WHERE GRUPOS.GrupoPadreId=" + IdGrupo;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (Mensaje == "")
        {
            if (dsInterno.Tables[0].Rows.Count > 0)
            {
                int j = 0;
                for (int i = 0; i < dsInterno.Tables[0].Rows.Count; i++)
                {
                    while (Levels[CurrentLevel, j].Id != "")
                        j++;
                    Levels[CurrentLevel, j].Id = dsInterno.Tables[0].Rows[i]["Id"].ToString();
                    Levels[CurrentLevel, j].Nombre = dsInterno.Tables[0].Rows[i]["Nombre"].ToString();
                    Levels[CurrentLevel, j].IdPadre = dsInterno.Tables[0].Rows[i]["GrupoPadreId"].ToString();
                    CreaArbolServicios(Levels, CurrentLevel + 1, dsInterno.Tables[0].Rows[i]["Id"].ToString());
                }
            }
        }
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaOficinasGrupo
    //Objetivo : Retornar un dataset los registros de la tabla Grupos
    //           
    public string ConsultaOficinasGrupo(ref DataSet dsReferido, string id, string NombreGrupo)
    {
        string strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        string Mensaje = "";

        strSQL = " SELECT Grupo_oficinas.Id_Grupo, Oficinas.Id, Oficinas.Nombre, Oficinas.Ubicacion " +
                 " FROM Grupo_oficinas RIGHT JOIN " +
                 " Grupos ON dbo.Grupo_oficinas.Id_Grupo = dbo.Grupos.id RIGHT JOIN  " +
                 " Oficinas ON dbo.Grupo_oficinas.OficinaId = dbo.Oficinas.Id";
        if (id != "")
            strSQL += " AND Grupos.Id=" + id;
        if (NombreGrupo != "")
            strSQL += " AND UPPER(Grupos.nombre) like '%" + NombreGrupo.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : BorrarOficinasDelGrupo
    //Objetivo : Retornar un int con la cantidad de registros eliminados
    //           
    public string EliminarGrupoComoPadre(ref DataSet dsReferido, string id, ref int borrados)
    {
        string strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        string Mensaje = "";

        strSQL = " UPDATE Grupos " +
                 " set GrupoPadreId = NULL" +
                 " WHERE GrupoPadreId = " + id;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        borrados = intRetVal;
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : BorrarOficinasDelGrupo
    //Objetivo : Retornar un int con la cantidad de registros eliminados
    //           
    public string BorrarOficinasDelGrupo(ref DataSet dsReferido, string id, ref int borrados)
    {
        string strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        string Mensaje = "";

        strSQL = " DELETE FROM Grupo_oficinas " +
                 " WHERE ID_GRUPO = " + id;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        borrados = intRetVal;
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaEncuestas
    //Objetivo : Retornar un dataset los registros de la tabla Encuestas
    //           
    public String ConsultaEncuestas(ref DataSet dsReferido, String id, String nombre_encuesta)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT E.Id, E.nombre_encuesta, E.Objetivo, E.fecha_encuesta, E.fecha_registro, E.Activa, COUNT(EP.EncuestaId) AS Preguntas " +
                 "FROM dbo.Encuestas AS E  " +
                 "LEFT OUTER JOIN dbo.EncuestasPreguntas AS EP ON E.Id = EP.EncuestaId " +
                 "WHERE E.Id IS NOT NULL ";
        if (id != "")
            strSQL += " AND E.Id='" + id + "'";
        if (nombre_encuesta != "")
            strSQL += " AND UPPER(E.nombre_encuesta) like '" + nombre_encuesta.ToUpper() + "%'";
        strSQL += "GROUP BY E.nombre_encuesta, E.Objetivo, E.fecha_encuesta, E.fecha_registro, E.Activa, E.Id, EP.EncuestaId ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaEncuestasPreguntas
    //Objetivo : Retornar un dataset los registros de la tabla EncuestasPreguntas
    //           
    public String ConsultaRespuestasEncuesta_SIN_CANCELADAS(ref DataSet dsReferido, String EncuestaId, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = " SELECT O.Nombre as Oficina, E.nombre_encuesta as Encuesta, EP.descripcion as Pregunta, EPO.descripcion_opcion as opcion, COUNT(EPR.OpcionId) AS Total " +
            " FROM Encuestas E INNER JOIN " +
            " EncuestasPreguntas EP ON E.Id = EP.EncuestaId INNER JOIN " +
            " EncuestasPreguntasOpciones EPO ON EP.Id = EPO.EncuestaPreguntaId INNER JOIN " +
            " EncuestasPreguntasRespuestas EPR ON EPO.Id = EPR.OpcionId INNER JOIN " +
            " Oficinas O ON O.Id = EPR.OfficeId " +
            " WHERE (E.Id = '" + EncuestaId + "') ";

        if ((fechaIni != "") && (fechaFin != ""))
        {
            strSQL += " AND EPR.Fecha between " +
                      " CONVERT(DATETIME,'" + fechaIni + "',103) " +
                      " AND CONVERT(DATETIME,'" + fechaFin + "',103) ";
        }
        else
        {
            if (fechaIni != "")
            {
                strSQL += " AND EPR.Fecha between CONVERT(DATETIME,'" + fechaIni + "',103) ";
                if (fechaFin == "")
                    strSQL += " AND CONVERT(DATETIME,'" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "',103) ";
                else
                    strSQL += " AND CONVERT(DATETIME,'" + fechaFin + "',103) ";
            }
        }

        strSQL += " GROUP BY EPR.OpcionId, EPO.descripcion_opcion, EP.descripcion, E.nombre_encuesta, O.Nombre " +
            " ORDER BY O.Nombre ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaEncuestasPreguntas
    //Objetivo : Retornar un dataset los registros de la tabla EncuestasPreguntas
    //           
    public String ConsultaRespuestasEncuesta(ref DataSet dsReferido, String EncuestaId, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        string fechaFinNow = "";

        // CONSULTA NOMBRE ENCUESTA ACTIVA
        strSQL = "DECLARE @nombre varchar(100); ";
        strSQL += "set @nombre=(SELECT nombre_encuesta FROM Encuestas where id = '" + EncuestaId + "') ";

        // QUERY ENCUESTAS CONTESTADAS
        strSQL += " SELECT O.Nombre as Oficina, @nombre as Encuesta, EP.descripcion as Pregunta, EPO.descripcion_opcion as opcion, COUNT(EPR.OpcionId) AS Total, EP.Id as idP, 'R' as tipo " +
            " FROM Encuestas E  " +
            " INNER JOIN  EncuestasPreguntas EP ON E.Id = EP.EncuestaId  " +
            " INNER JOIN  EncuestasPreguntasOpciones EPO ON EP.Id = EPO.EncuestaPreguntaId  " +
            " INNER JOIN  EncuestasPreguntasRespuestas EPR ON EPO.Id = EPR.OpcionId  " +
            " INNER JOIN  Oficinas O ON O.Id = EPR.AsesorId " +
            " WHERE (E.Id = '" + EncuestaId + "') ";

        if ((fechaIni != "") && (fechaFin != ""))
        {
            strSQL += " AND EPR.Fecha between " + " CONVERT(DATETIME,'" + fechaIni + "',103) " + " AND CONVERT(DATETIME,'" + fechaFin + "',103) ";
        }
        else
        {
            if (fechaIni != "")
            {
                strSQL += " AND EPR.Fecha between CONVERT(DATETIME,'" + fechaIni + "',103) ";
                if (fechaFin == "")
                {
                    fechaFinNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    strSQL += " AND CONVERT(DATETIME,'" + fechaFinNow + "',103) ";
                }
                else
                    strSQL += " AND CONVERT(DATETIME,'" + fechaFin + "',103) ";
            }
        }

        strSQL += " GROUP BY EP.Id, EPO.descripcion_opcion, EP.descripcion, E.nombre_encuesta, O.Nombre  ";

        // UNION QUERY DE ENCUESTAS CANCELADAS
        strSQL += " union (\r\n";
        strSQL += " SELECT O.Nombre as Oficina, @nombre as Encuesta, 'N/A' as Pregunta, EPR.Valor as opcion, COUNT(EPR.Valor) AS Total, EPR.OpcionId as idP, 'C' as tipo ";
        strSQL += " FROM EncuestasPreguntasRespuestas EPR ";
        strSQL += " JOIN  Oficinas O ON O.Id = EPR.OfficeId ";
        strSQL += " WHERE EPR.OpcionId = 0 AND EPR.PreguntaId = 0 ";

        if ((fechaIni != "") && (fechaFin != ""))
        {
            strSQL += " AND EPR.Fecha between " + " CONVERT(DATETIME,'" + fechaIni + "',103) " + " AND CONVERT(DATETIME,'" + fechaFin + "',103) ";
        }
        else
        {
            if (fechaIni != "")
            {
                strSQL += " AND EPR.Fecha between CONVERT(DATETIME,'" + fechaIni + "',103) ";
                if (fechaFin == "")
                    strSQL += " AND CONVERT(DATETIME,'" + fechaFinNow + "',103) ";
                else
                    strSQL += " AND CONVERT(DATETIME,'" + fechaFin + "',103) ";
            }
        }
        strSQL += " GROUP BY O.Nombre, EPR.Valor, EPR.OpcionId ";

        // FIN QUERY Y ORDER BY
        strSQL += ") ORDER BY O.Nombre, tipo, idp ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }


    //==============================================================================
    //Nombre de la función : ConsultaCamposTicket
    //Objetivo : Retornar un dataset con los campos de la tabla TicketFields qu contiene los campos que se imprimen en el ticket de turnos
    //           
    public String ConsultaCamposTicket(ref DataSet dsReferido)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = " SELECT idCampo, orden, obligatorio,tipo,nombre,contenido,tamanoFuente,tipoFuente, " +
                 " CASE WHEN align = 0 then 'Izquierda' WHEN align = 1 then 'Centro' WHEN align = 2 then 'Derecha' WHEN align = 3 then 'Justificado' else NULL end as align from " +
                 " TicketFields order by orden ASC";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }


    //==============================================================================
    //Nombre de la función : ActualizarCamposTicket
    //Objetivo : Retornar un dataset con los campos de la tabla TicketFields qu contiene los campos que se imprimen en el ticket de turnos
    //           
    public String ActualizarCamposTicket(DataTable tbCampos, ref string query)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet vacio = null;
        int i = 0;
        int align = 0;
        int fijo = 1;

        strSQL = " DELETE FROM TicketFields ";
        strSQL += " INSERT INTO TicketFields VALUES ";
        foreach (DataRow r in tbCampos.Rows)
        {
            i++;
            strSQL += " (";
            switch (r[8])
            {
                case "Izquierda": align = 0; break;
                case "Centro": align = 1; break;
                case "Derecha": align = 2; break;
                case "Justificado": align = 3; break;
            }
            if (r[2].ToString().ToUpper() == "TRUE")
                fijo = 1;
            else
                fijo = 0;
            strSQL += $"{i}, {i}, {fijo}, {r[3]}, '{r[4]}', '{r[5]}', {r[6]}, '{r[7]}', {align}";
            strSQL += "),";
        }
        strSQL = strSQL.Substring(0, strSQL.Length - 1);
        query = strSQL;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref vacio, null, null, true);
        return Mensaje;
    }


    //==============================================================================
    //Nombre de la función : ConsultaPerfilesAtención
    //Objetivo : Retornar un dataset los registros de la tabla PerfilesDeAtencion
    //           
    public String ConsultaReasonsForSuspension(ref DataSet dsReferido, String id, String nombre)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = " SELECT ReasonsForSuspension.* " +
                 " FROM ReasonsForSuspension " +
                 " WHERE Id IS NOT NULL " +
                 " and activity = 3";
        if (id != "")
            strSQL += " AND ReasonsForSuspension.Id=" + id;
        if (nombre != "")
            strSQL += " AND UPPER(ReasonsForSuspension.Description) like '" + nombre.ToUpper() + "%'";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }




    #endregion



}


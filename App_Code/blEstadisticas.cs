using System;
using System.Data;

/// <summary>
/// Descripción breve de blEstadisticas
/// </summary>
public class clsblEstadisticas
{
    public clsblEstadisticas()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    //==============================================================================
    //Nombre de la función : ConsultaGeneralTurnos
    //Objetivo : Retornar un dataset con la información de atención de los Turnos Generados
    //
    //      /// este query saca las fechas con hora militar
    public String ConsultaGeneralTurnos(ref DataSet dsReferido, string OficinaId, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT O.Nombre as Oficina, ta.Nombre as TipoAtencion, CONCAT(st.Abreviatura, tr.Consecutivo) as Turno, " +
                         "tr.Estado as estado, sr.Nombre as servicio, " +
                         "CONVERT(varchar,tr.Hora_Generado,20) as FechaInicio,  " +
                         "CASE WHEN tr.estado = 4 then CONVERT(varchar,trAb.Hora,20)  " +
                         "WHEN tr.estado = 6 then CONVERT(varchar,trC.Hora,20) " +
                         "else NULL " +
                         "end as FechaFin, " +
                         "tr.Tiempo_Espera as TiempoEspera, " +
                         "CASE WHEN tr.Tiempo_Espera=0 THEN Null " +
                         "ELSE CONVERT(varchar,DATEADD(SECOND, tr.Tiempo_Espera, tr.Hora_Generado),20)  " +
                         "END AS FechaLlamado, " +
                         "tr.Tiempo_Atencion as TiempoAtencion,  " +
                         "CASE WHEN CA.Nombre is null then 'N/A' else CA.Nombre end as Causa, " +
                         "us.Nombre as Asesor, tr.UserId as documento " +
                         "FROM Turnos tr " +
                         "INNER JOIN Oficinas O On O.Id = tr.OficinaId " +
                         "INNER JOIN ServicioTipoAtencion st On st.Id = tr.ServicioTipoAtencionId " +
                         "INNER JOIN TipoDeAtencion ta On st.TipoAtencionId = ta.Id " +
                         "INNER JOIN Servicios sr ON sr.Id = st.ServicioId " +
                         "INNER JOIN Prioridad pr On pr.ServicioTipoAtencionId = st.Id " +
                         "FULL OUTER JOIN TurnosLlamados trLL On tr.id = trLL.TurnoId " +
                         "FULL OUTER JOIN TurnosAbandonados trAb On tr.id = trAb.TurnoId " +
                         "FULL OUTER JOIN PrioridadDeAsesor prA On prA.Id = trLL.PrioridadDeAsesorId " +
                         "FULL OUTER JOIN Usuarios us On us.Id = prA.UsuarioId " +
                         "FULL OUTER JOIN TurnosCerrados trC On tr.id = trC.TurnoId " +
                         "FULL OUTER JOIN CausasAbandono CA On trAb.CausaAbandonoId = CA.Id ";
        strSQL += "WHERE tr.Id IS NOT NULL ";
        if (OficinaId != "")
            strSQL += " AND tr.OficinaId=" + OficinaId;

        if ((fechaIni != "") && (fechaFin != ""))
        {
            strSQL += " AND tr.Hora_Generado between " +
                      " CONVERT(DATETIME,'" + fechaIni + "',103) " +
                      " AND CONVERT(DATETIME,'" + fechaFin + "',103) ";
        }
        else
        {
            if (fechaIni != "")
            {
                strSQL += " AND tr.Hora_Generado between CONVERT(DATETIME,'" + fechaIni + "',103) ";
                if (fechaFin == "")
                    strSQL += " AND CONVERT(DATETIME,'" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "',103) ";
                else
                    strSQL += " AND CONVERT(DATETIME,'" + fechaFin + "',103) ";
            }
        }
        strSQL += " GROUP BY " +  //// jose adiciona lo de estado para reporte de movilidad
                  " tr.UserId, us.Nombre, pra.UsuarioId, tr.ServicioTipoAtencionId, " +
                  " trLL.Hora, tr.Tiempo_Atencion, tr.Tiempo_Espera, trC.Hora, trAb.Hora, " +
                  " tr.Hora_Generado, sr.Nombre, tr.Estado, st.Abreviatura, tr.Consecutivo, " +
                  " ta.Nombre, O.Nombre, CA.Nombre ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }


    ///// este query saca el reporte con fechas am pm 
    public String ConsultaGeneralTurnosFechaampm(DataSet dsReferido, string OficinaId, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT O.Nombre as Oficina, ta.Nombre as TipoAtencion, CONCAT(st.Abreviatura, tr.Consecutivo) as Turno, " +
                 "tr.Estado as estado, sr.Nombre as servicio, tr.Hora_Generado as FechaInicio, " +
                 "CASE WHEN tr.estado = 4 then trAb.Hora WHEN tr.estado = 6 then trC.Hora else NULL end as FechaFin, " +
                 "tr.Tiempo_Espera as TiempoEspera, " +
                 "CASE WHEN tr.Tiempo_Espera=0 THEN Null ELSE DATEADD(SECOND, tr.Tiempo_Espera, tr.Hora_Generado) END AS FechaLlamado, " +
                 "tr.Tiempo_Atencion as TiempoAtencion,  " +
                 "CASE WHEN CA.Nombre is null then 'N/A' else CA.Nombre end as Causa, " +
                 "us.Nombre as Asesor, tr.UserId as documento " +
                 "FROM Turnos tr " +
                 "INNER JOIN Oficinas O On O.Id = tr.OficinaId " +
                 "INNER JOIN ServicioTipoAtencion st On st.Id = tr.ServicioTipoAtencionId " +
                 "INNER JOIN TipoDeAtencion ta On st.TipoAtencionId = ta.Id " +
                 "INNER JOIN Servicios sr ON sr.Id = st.ServicioId " +
                 "INNER JOIN Prioridad pr On pr.ServicioTipoAtencionId = st.Id " +
                 "FULL OUTER JOIN TurnosLlamados trLL On tr.id = trLL.TurnoId " +
                 "FULL OUTER JOIN TurnosAbandonados trAb On tr.id = trAb.TurnoId " +
                 "FULL OUTER JOIN PrioridadDeAsesor prA On prA.Id = trLL.PrioridadDeAsesorId " +
                 "FULL OUTER JOIN Usuarios us On us.Id = prA.UsuarioId " +
                 "FULL OUTER JOIN TurnosCerrados trC On tr.id = trC.TurnoId " +
                 "FULL OUTER JOIN CausasAbandono CA On trAb.CausaAbandonoId = CA.Id ";
        strSQL += "WHERE tr.Id IS NOT NULL ";
        if (OficinaId != "")
            strSQL += " AND tr.OficinaId=" + OficinaId;

        if ((fechaIni != "") && (fechaFin != ""))
        {
            strSQL += " AND tr.Hora_Generado between " +
                      " CONVERT(DATETIME,'" + fechaIni + "',103) " +
                      " AND CONVERT(DATETIME,'" + fechaFin + "',103) ";
        }
        else
        {
            if (fechaIni != "")
            {
                strSQL += " AND tr.Hora_Generado between CONVERT(DATETIME,'" + fechaIni + "',103) ";
                if (fechaFin == "")
                    strSQL += " AND CONVERT(DATETIME,'" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "',103) ";
                else
                    strSQL += " AND CONVERT(DATETIME,'" + fechaFin + "',103) ";
            }
        }
        strSQL += " GROUP BY " +  //// jose adiciona lo de estado para reporte de movilidad
                  " tr.UserId, us.Nombre, pra.UsuarioId, tr.ServicioTipoAtencionId, " +
                  " trLL.Hora, tr.Tiempo_Atencion, tr.Tiempo_Espera, trC.Hora, trAb.Hora, " +
                  " tr.Hora_Generado, sr.Nombre, tr.Estado, st.Abreviatura, tr.Consecutivo, " +
                  " ta.Nombre, O.Nombre, CA.Nombre ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }



    //==============================================================================
    //Nombre de la función : ConsultaGeneralTurnosOLD
    //Objetivo : Retornar un dataset con la información de atención de los Turnos Generados
    //           
    public String ConsultaGeneralTurnosOLD(ref DataSet dsReferido, string OficinaId, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT O.Nombre as Oficina, ta.Nombre as TipoAtencion, CONCAT(st.Abreviatura, tr.Consecutivo) as Turno, " +
                 "tr.Estado as estado, sr.Nombre as servicio, tr.Hora_Generado as FechaInicio, " +
                 "CASE WHEN tr.estado = 4 then trAb.Hora WHEN tr.estado = 6 then trC.Hora else NULL end as FechaFin, " +
                 "tr.Tiempo_Espera as TiempoEspera, " +
                 "CASE WHEN tr.Tiempo_Espera=0 THEN Null ELSE DATEADD(SECOND, tr.Tiempo_Espera, tr.Hora_Generado) END AS FechaLlamado, " +
                 "tr.Tiempo_Atencion as TiempoAtencion, us.Nombre as Asesor, tr.UserId as documento " +
                 "FROM Turnos tr " +
                 "INNER JOIN Oficinas O On O.Id = tr.OficinaId " +
                 "INNER JOIN ServicioTipoAtencion st On st.Id = tr.ServicioTipoAtencionId " +
                 "INNER JOIN TipoDeAtencion ta On st.TipoAtencionId = ta.Id " +
                 "INNER JOIN Servicios sr ON sr.Id = st.ServicioId " +
                 "INNER JOIN Prioridad pr On pr.ServicioTipoAtencionId = st.Id " +
                 "FULL OUTER JOIN TurnosLlamados trLL On tr.id = trLL.TurnoId " +
                 "FULL OUTER JOIN TurnosAbandonados trAb On tr.id = trAb.TurnoId " +
                 "FULL OUTER JOIN PrioridadDeAsesor prA On prA.Id = trLL.PrioridadDeAsesorId " +
                 "FULL OUTER JOIN Usuarios us On us.Id = prA.UsuarioId " +
                 "FULL OUTER JOIN TurnosCerrados trC On tr.id = trC.TurnoId ";
        strSQL += "WHERE tr.Id IS NOT NULL ";
        if (OficinaId != "")
            strSQL += " AND tr.OficinaId=" + OficinaId;

        if ((fechaIni != "") && (fechaFin != ""))
        {
            strSQL += " AND tr.Hora_Generado between " +
                      " CONVERT(DATETIME,'" + fechaIni + "',103) " +
                      " AND CONVERT(DATETIME,'" + fechaFin + "',103) ";
        }
        else
        {
            if (fechaIni != "")
            {
                strSQL += " AND tr.Hora_Generado between CONVERT(DATETIME,'" + fechaIni + "',103) ";
                if (fechaFin == "")
                    strSQL += " AND CONVERT(DATETIME,'" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "',103) ";
                else
                    strSQL += " AND CONVERT(DATETIME,'" + fechaFin + "',103) ";
            }
        }
        strSQL += " GROUP BY " +
                  " tr.UserId, us.Nombre, pra.UsuarioId, tr.ServicioTipoAtencionId, " +
                  " trLL.Hora, tr.Tiempo_Atencion, tr.Tiempo_Espera, trC.Hora, trAb.Hora, " +
                  " tr.Hora_Generado, sr.Nombre, tr.Estado, st.Abreviatura, tr.Consecutivo, " +
                  " ta.Nombre, O.Nombre ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }




    //==============================================================================
    //Nombre de la función : ConsultaNumeroTurnos
    //Objetivo : Retornar un dataset los registros de la tabla Turnos
    //           
    public String ConsultaNumeroTurnos(ref DataSet dsReferido, string tipoTurno, string OficinaId, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT COUNT(Turnos.Id) as NoTurnos, Oficinas.Nombre as Oficina " +
                 "FROM Turnos " +
                 " INNER JOIN Oficinas ON Turnos.OficinaId=Oficinas.Id ";
        if (tipoTurno.ToUpper() == "ATENDIDOS")
            strSQL += " INNER JOIN TurnosAtendidos ON Turnos.Id=TurnosAtendidos.TurnoId ";
        if (tipoTurno.ToUpper() == "LLAMADOS")
            strSQL += " INNER JOIN TurnosLlamados ON Turnos.Id=TurnosLlamados.TurnoId ";
        if (tipoTurno.ToUpper() == "CERRADOS")
            strSQL += " INNER JOIN TurnosCerrados ON Turnos.Id=TurnosCerrados.TurnoId ";
        if (tipoTurno.ToUpper() == "ABANDONADOS")
            strSQL += " INNER JOIN TurnosAbandonados ON Turnos.Id=TurnosAbandonados.TurnoId ";
        strSQL += "WHERE Turnos.Id IS NOT NULL ";
        if (OficinaId != "")
            strSQL += " AND Turnos.OficinaId=" + OficinaId;
        if (fechaIni != "")
            strSQL += " AND Convert(date,Turnos.Hora_Generado)>=CONVERT(DATETIME,'" + fechaIni + "',103) ";
        if (fechaFin != "")
            strSQL += " AND Convert(date,Turnos.Hora_Generado)<=CONVERT(DATETIME,'" + fechaFin + "',103) ";
        strSQL += " GROUP BY Oficinas.Nombre ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaNumeroTurnosServicios
    //Objetivo : Retornar un dataset los registros de la tabla Turnos
    //           
    public String ConsultaNumeroTurnosServicios(ref DataSet dsReferido, string tipoTurno, string OficinaId, string ServicioId, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT COUNT(Turnos.Id) as NoTurnos, Servicios.Nombre as Servicio, Servicios.Abreviatura " +
                 "FROM Turnos " +
                 " INNER JOIN Oficinas ON Turnos.OficinaId=Oficinas.Id " +
                 " INNER JOIN ServicioTipoAtencion ON Turnos.ServicioTipoAtencionId=ServicioTipoAtencion.Id " +
                 " INNER JOIN Servicios ON ServicioTipoAtencion.ServicioId=Servicios.Id ";
        if (tipoTurno.ToUpper() == "ATENDIDOS")
            strSQL += " INNER JOIN TurnosAtendidos ON Turnos.Id=TurnosAtendidos.TurnoId ";
        if (tipoTurno.ToUpper() == "LLAMADOS")
            strSQL += " INNER JOIN TurnosLlamados ON Turnos.Id=TurnosLlamados.TurnoId ";
        if (tipoTurno.ToUpper() == "CERRADOS")
            strSQL += " INNER JOIN TurnosCerrados ON Turnos.Id=TurnosCerrados.TurnoId ";
        if (tipoTurno.ToUpper() == "ABANDONADOS")
            strSQL += " INNER JOIN TurnosAbandonados ON Turnos.Id=TurnosAbandonados.TurnoId ";
        strSQL += "WHERE Turnos.Id IS NOT NULL ";
        if (OficinaId != "")
            strSQL += " AND Turnos.OficinaId=" + OficinaId;
        if (fechaIni != "")
            strSQL += " AND Convert(date,Turnos.Hora_Generado)>=CONVERT(DATETIME,'" + fechaIni + "',103) ";
        if (fechaFin != "")
            strSQL += " AND Convert(date,Turnos.Hora_Generado)<=CONVERT(DATETIME,'" + fechaFin + "',103) ";
        if (ServicioId != "")
            strSQL += " AND ServicioTipoAtencion.ServicioId=" + ServicioId;
        strSQL += " GROUP BY Servicios.Nombre, Servicios.Abreviatura ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaExistenciaTurnoOficina
    //Objetivo : Verifica si el Id pertenece a un  turno ya registrado en esa fecha para esa OficinaId  y retorna el Id el turno
    //           
    public String ConsultaExistenciaTurnoOficina(ref string TurnoId, string Id, string OficinaId)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = new DataSet();

        TurnoId = "";
        strSQL = "SELECT Turnos.Id   " +
                 "FROM Turnos WHERE OficinaId=" + OficinaId + " AND TurnoOficinaId=" + Id;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (Mensaje == "")
        {
            if (dsInterno.Tables[0].Rows.Count > 0)
            {
                TurnoId = dsInterno.Tables[0].Rows[0]["Id"].ToString();
            }
        }
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaExistenciaTurnosTabla
    //Objetivo : Verifica si el TurnoId pertenece ya  existe en la tabla tabla
    public String ConsultaExistenciaTurnosTabla(ref string TurnoTablaId, string TurnoId, string tabla)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";
        DataSet dsInterno = new DataSet();

        TurnoTablaId = "";
        strSQL = "SELECT Id   " +
                 "FROM " + tabla + " WHERE TurnoId=" + TurnoId;
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsInterno, null, null, true);
        if (Mensaje == "")
        {
            if (dsInterno.Tables[0].Rows.Count > 0)
            {
                TurnoTablaId = dsInterno.Tables[0].Rows[0]["Id"].ToString();
            }
        }
        return Mensaje;
    }


    #region F U N C I O N E S   OSCAR RODRIGUEZ  -  I N P E C

    //==============================================================================
    //Nombre de la función : ConsultaNumeroTurnosGrupo
    //Objetivo : Retornar un dataset con los Turnos agrupados por grupo y oficina
    //           
    public String ConsultaNumeroTurnosGrupo(ref DataSet dsReferido, string tipoTurno, string GrupoId, string OficinaId, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = " SELECT COUNT(T.Id) as NoTurnos, G.Nombre as Grupo, O.Nombre as Oficina " +
                 " FROM Turnos T " +
                 " INNER JOIN Oficinas O ON T.OficinaId=O.Id " +
                 " INNER JOIN Grupo_oficinas GrOf ON O.Id=GrOf.Id_Oficina " +
                 " INNER JOIN Grupos G ON GrOf.Id_Grupo=G.Id ";
        if (tipoTurno.ToUpper() == "ATENDIDOS")
            strSQL += " INNER JOIN TurnosAtendidos TuAt ON T.Id=TuAt.TurnoId  ";
        if (tipoTurno.ToUpper() == "LLAMADOS")
            strSQL += " INNER JOIN TurnosLlamados TuLl ON T.Id=Tull.TurnoId ";
        if (tipoTurno.ToUpper() == "CERRADOS")
            strSQL += " INNER JOIN TurnosCerrados TuCe ON T.Id=TuCe.TurnoId ";
        if (tipoTurno.ToUpper() == "ABANDONADOS")
            strSQL += " INNER JOIN TurnosAbandonados TuAb ON T.Id=TuAb.TurnoId ";
        strSQL += "WHERE T.Id IS NOT NULL ";
        if (GrupoId != "")
            strSQL += " AND G.Id=" + GrupoId;
        if (OficinaId != "")
            strSQL += " AND T.OficinaId=" + OficinaId;
        if (fechaIni != "")
            strSQL += " AND Convert(date,T.Hora_Generado)>=CONVERT(DATETIME,'" + fechaIni + "',103) ";
        if (fechaFin != "")
            strSQL += " AND Convert(date,T.Hora_Generado)<=CONVERT(DATETIME,'" + fechaFin + "',103) ";
        strSQL += " GROUP BY O.Nombre, G.Nombre ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaNumeroTurnosCliente
    //Objetivo : Retornar un dataset con los Turnos de un usuario
    //           
    public String ConsultaNumeroTurnosCliente(ref DataSet dsReferido, string Identificacion, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT UserId as Identificacion, COUNT(Id) as NoTurnos " +
                 "FROM Turnos ";
        strSQL += "WHERE Id IS NOT NULL ";
        if (Identificacion != "")
            strSQL += " AND UserId='" + Identificacion + "'";
        if (fechaIni != "")
            strSQL += " AND Convert(date,Hora_Generado)>=CONVERT(DATETIME,'" + fechaIni + "',103) ";
        if (fechaFin != "")
            strSQL += " AND Convert(date,Hora_Generado)<=CONVERT(DATETIME,'" + fechaFin + "',103) ";
        strSQL += " GROUP BY UserID ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaNumeroTurnosHora
    //Objetivo : Retornar un dataset los registros de la tabla Turnos Agrupados por hora del día
    //           
    public String ConsultaNumeroTurnosHora(ref DataSet dsReferido, string tipoTurno, string OficinaId, string HoraIni, string HoraFin)
    {
        string strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        string Mensaje = "";

        strSQL = " SELECT DATEPART(HOUR, Turnos.Hora_Generado) as hora, COUNT(Turnos.Id) as NoTurnos, Oficinas.Nombre as Oficina " +
                 " FROM Turnos  INNER JOIN Oficinas ON Turnos.OficinaId=Oficinas.Id ";
        if (tipoTurno.ToUpper() == "ATENDIDOS")
            strSQL += " INNER JOIN TurnosAtendidos ON Turnos.Id=TurnosAtendidos.TurnoId ";
        if (tipoTurno.ToUpper() == "LLAMADOS")
            strSQL += " INNER JOIN TurnosLlamados ON Turnos.Id=TurnosLlamados.TurnoId ";
        if (tipoTurno.ToUpper() == "CERRADOS")
            strSQL += " INNER JOIN TurnosCerrados ON Turnos.Id=TurnosCerrados.TurnoId ";
        if (tipoTurno.ToUpper() == "ABANDONADOS")
            strSQL += " INNER JOIN TurnosAbandonados ON Turnos.Id=TurnosAbandonados.TurnoId ";
        strSQL += "WHERE Turnos.Id IS NOT NULL ";
        if (OficinaId != "")
            strSQL += " AND Turnos.OficinaId=" + OficinaId;
        strSQL += " AND Turnos.Hora_Generado between CONVERT(DATETIME,'" + HoraIni + "',103) ";
        strSQL += " AND CONVERT(DATETIME,'" + HoraFin + "',103) ";
        strSQL += " GROUP BY DATEPART(HOUR, Turnos.Hora_Generado), Oficinas.Nombre ";
        strSQL += " ORDER BY hora ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }


    //==============================================================================
    //Nombre de la función : ConsultaActividadUsuarios
    //Objetivo : Retornar un dataset con los registros de actividad de cada asesor
    //           
    public String ConsultaActividadUsuarios(ref DataSet dsRef, string OficinaId, string UserId, string RazonId, string FechaIni, string FechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = " SELECT O.Nombre as Oficina, U.Nombre, AL.TimeActivity, " +
                 " CASE WHEN RS.Activity=3 THEN 'Suspendido' ELSE 'En atención' END as Estado, " +
                 " CASE WHEN RS.Activity=3 THEN RS.Description ELSE 'N/A' END as Razon" +
                 " FROM ActivityLogs AL " +
                 " inner join Usuarios U on AL.UsuarioId = U.Id " +
                 " inner join ReasonsForSuspension RS on AL.ReasonForSuspensionId = RS.Id " +
                 " inner join Oficinas O on AL.OfficeId = O.Id ";
        strSQL += " WHERE AL.Id IS NOT NULL ";
        if (OficinaId != "")
            strSQL += " AND O.Id = " + OficinaId;
        if (UserId != "")
            strSQL += " AND AL.UsuarioId = '" + UserId + "' ";
        if (RazonId != "")
            strSQL += " AND RS.Id = " + RazonId;
        if (FechaIni != "")
        {
            strSQL += " AND AL.TimeActivity between CONVERT(DATETIME,'" + FechaIni + "',103) ";
            if (FechaFin == "")
                strSQL += " AND CONVERT(DATETIME,'" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + "',103) ";
            else
                strSQL += " AND CONVERT(DATETIME,'" + FechaFin + "',103) ";
        }
        strSQL += " order by O.Id, AL.TimeActivity ASC, U.Nombre ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsRef, null, null, true);
        return Mensaje;
    }


    //==============================================================================
    //Nombre de la función : ConsultaNumeroTurnosAgendadosServicio
    //Objetivo : Retornar un dataset los registros de la tabla Agendamiento
    //           
    public String ConsultaNumeroTurnosAgendadosServicio(ref DataSet dsReferido, string tipoTurno, string OficinaId, string ServicioId, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT COUNT(AG.Id) as NoTurnos, S.Nombre as Servicio, S.Abreviatura " +
                 " FROM  Agendamiento AG " +
                 " INNER JOIN Oficinas O ON AG.Sede = O.Id " +
                 " INNER JOIN HorarioAgenda ON AG.Horario = HorarioAgenda.id  " +
                 " INNER JOIN Servicios S ON AG.TipoServicio=S.Id ";
        if (tipoTurno.ToUpper() == "AGENDADOS")
            strSQL += " ";
        strSQL += "WHERE AG.Id IS NOT NULL ";
        if (OficinaId != "")
            strSQL += " AND AG.Sede=" + OficinaId;
        if (fechaIni != "")
            strSQL += " AND Convert(date,AG.Fecha)>=CONVERT(DATETIME,'" + fechaIni + "',103) ";
        if (fechaFin != "")
            strSQL += " AND Convert(date,AG.FECHA)<=CONVERT(DATETIME,'" + fechaFin + "',103) ";
        if (ServicioId != "")
            strSQL += " AND AG.TipoServicio=" + ServicioId;
        strSQL += " GROUP BY S.Nombre, S.Abreviatura ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaNumeroTurnosAgendados
    //Objetivo : Retornar un dataset los registros de la tabla Agendamiento
    //           
    public String ConsultaNumeroTurnosAgendados(ref DataSet dsReferido, string tipoTurno, string OficinaId, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT COUNT(AG.Id) as NoTurnos, O.Nombre as Oficina " +
                 " FROM  Agendamiento AG " +
                 " INNER JOIN Oficinas O ON AG.Sede = O.Id ";
        if (tipoTurno.ToUpper() == "AGENDADOS")
            strSQL += " ";
        strSQL += "WHERE AG.Id IS NOT NULL ";
        if (OficinaId != "")
            strSQL += " AND AG.Sede=" + OficinaId;
        if (fechaIni != "")
            strSQL += " AND Convert(date,AG.Fecha)>=CONVERT(DATETIME,'" + fechaIni + "',103) ";
        if (fechaFin != "")
            strSQL += " AND Convert(date,AG.FECHA)<=CONVERT(DATETIME,'" + fechaFin + "',103) ";
        strSQL += " GROUP BY O.Nombre ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaNumeroTurnosAgendados
    //Objetivo : Retornar un dataset los registros de la tabla Agendamiento
    //           
    public String ConsultaNumeroTurnosAgendadosGrupo(ref DataSet dsReferido, string tipoTurno, string GrupoId, string OficinaId, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT COUNT(AG.Id) as NoTurnos, G.Nombre as Grupo, O.Nombre as Oficina " +
                 " FROM  Agendamiento AG " +
                 " INNER JOIN Oficinas O ON AG.Sede = O.Id " +
                 " INNER JOIN Grupo_oficinas GrOf ON O.Id=GrOf.Id_Oficina " +
                 " INNER JOIN Grupos G ON GrOf.Id_Grupo=G.Id ";
        if (tipoTurno.ToUpper() == "AGENDADOS")
            strSQL += " ";
        strSQL += "WHERE AG.Id IS NOT NULL ";
        if (GrupoId != "")
            strSQL += " AND G.Id=" + GrupoId;
        if (OficinaId != "")
            if (OficinaId != "")
                strSQL += " AND AG.Sede=" + OficinaId;
        if (fechaIni != "")
            strSQL += " AND Convert(date,AG.Fecha)>=CONVERT(DATETIME,'" + fechaIni + "',103) ";
        if (fechaFin != "")
            strSQL += " AND Convert(date,AG.FECHA)<=CONVERT(DATETIME,'" + fechaFin + "',103) ";
        strSQL += " GROUP BY O.Nombre, G.Nombre ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaNumeroTurnosAgendadosHora
    //Objetivo : Retornar un dataset los registros de la tabla Agendamiento Agrupados por hora del día
    //           
    public String ConsultaNumeroTurnosAgendadosHora(ref DataSet dsReferido, string tipoTurno, string OficinaId, string HoraIni, string HoraFin)
    {
        string strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        string Mensaje = "";

        strSQL = " SELECT DATEPART(HOUR, Turnos.Hora_Generado) as hora, COUNT(Turnos.Id) as NoTurnos, Oficinas.Nombre as Oficina " +
                 " FROM Turnos  INNER JOIN Oficinas ON Turnos.OficinaId=Oficinas.Id ";
        if (tipoTurno.ToUpper() == "ATENDIDOS")
            strSQL += " INNER JOIN TurnosAtendidos ON Turnos.Id=TurnosAtendidos.TurnoId ";
        if (tipoTurno.ToUpper() == "LLAMADOS")
            strSQL += " INNER JOIN TurnosLlamados ON Turnos.Id=TurnosLlamados.TurnoId ";
        if (tipoTurno.ToUpper() == "CERRADOS")
            strSQL += " INNER JOIN TurnosCerrados ON Turnos.Id=TurnosCerrados.TurnoId ";
        if (tipoTurno.ToUpper() == "ABANDONADOS")
            strSQL += " INNER JOIN TurnosAbandonados ON Turnos.Id=TurnosAbandonados.TurnoId ";
        strSQL += "WHERE Turnos.Id IS NOT NULL ";
        if (OficinaId != "")
            strSQL += " AND Turnos.OficinaId=" + OficinaId;
        strSQL += " AND Turnos.Hora_Generado between CONVERT(DATETIME,'" + HoraIni + "',103) ";
        strSQL += " AND CONVERT(DATETIME,'" + HoraFin + "',103) ";
        strSQL += " GROUP BY DATEPART(HOUR, Turnos.Hora_Generado), Oficinas.Nombre ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }

    //==============================================================================
    //Nombre de la función : ConsultaNumeroTurnosAgendados
    //Objetivo : Retornar un dataset los registros de la tabla Agendamiento
    //           
    public String ConsultaTurnosAgendados(ref DataSet dsReferido, string tipoTurno, string OficinaId, string fechaIni, string fechaFin)
    {
        String strSQL = "";
        int intRetVal = -1;
        clsDataLayer ConexionDataObject;
        String Mensaje = "";

        strSQL = "SELECT AG.*, O.Nombre as Oficina, S.Abreviatura, S.Nombre as Servicio, H.Horario as hora" +
                 " FROM  Agendamiento AG " +
                 " INNER JOIN HorarioAgenda H ON AG.Horario = H.id  " +
                 " INNER JOIN Servicios S ON AG.TipoServicio=S.Id " +
                 " INNER JOIN Oficinas O ON AG.Sede = O.Id ";
        if (tipoTurno.ToUpper() == "AGENDADOS")
            strSQL += " ";
        strSQL += "WHERE AG.Id IS NOT NULL ";
        if (OficinaId != "")
            strSQL += " AND AG.Sede=" + OficinaId;
        if (fechaIni != "")
            strSQL += " AND Convert(date,AG.FechaAgendamiento)>=CONVERT(DATETIME,'" + fechaIni + "',103) ";
        if (fechaFin != "")
            strSQL += " AND Convert(date,AG.FechaAgendamiento)<=CONVERT(DATETIME,'" + fechaFin + "',103) ";
        strSQL += " Order BY AG.Fecha ";
        ConexionDataObject = new clsDataLayer();
        ConexionDataObject.ExecSQL(strSQL, ref intRetVal, ref Mensaje, ref dsReferido, null, null, true);
        return Mensaje;
    }



    #endregion
}
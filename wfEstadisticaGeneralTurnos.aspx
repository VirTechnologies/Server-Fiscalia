<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfEstadisticaGeneralTurnos.aspx.cs" Inherits="wfEstadisticaGeneralTurnos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Estadística General de Turnos</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="datetimepicker/bootstrap-datepicker.min.css" />
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css" />

    <script type="text/javascript" src="./datetimepicker/jquery.min.js"></script>
    <script type="text/javascript" src="./datetimepicker/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" src="./datetimepicker/bootstrap-datepicker.es.min.js"></script>
<%--	<link rel="stylesheet" type="text/javascript" href="datetimepicker/bootstrap-datepicker.min.js" />
	<link rel="stylesheet" type="text/javascript" href="datetimepicker/bootstrap-datepicker.min.js" />--%>
<%--    <script src="./js/amcharts4/core.js"></script>
    <script src="./js/amcharts4/charts.js"></script>
    <script src="./js/amcharts4/themes/animated.js"></script>
    <script src="./js/amcharts4/themes/kelly.js"></script>--%>
    <style>
        #chartdiv {
          width: 100%;
          height: 300px;
        }
        #chartdivLlamados {
          width: 100%;
          height: 300px;
        }
        #chartdivCerrados {
          width: 100%;
          height: 300px;
        }
        #chartdivAbandonados {
          width: 100%;
          height: 300px;
        }
    </style>
    <style>
/*
        body {
          font-family: "lato", sans-serif;
        }
*/
        .container {
          max-width: 1000px;
          margin-left: auto;
          margin-right: auto;
          padding-left: 10px;
          padding-right: 10px;
        }

        .table th, .table td {
            padding: 0.5rem;
            vertical-align: middle;
        }

        .c {
          text-align: center;
        }
        .l {
          text-align: left;
        }
        .r {
          text-align: right;
        }

        .responsive-table th {
          border-radius: 3px;
          padding: 10px 5px;
          /*display: flex;*/
          justify-content: space-between;
          margin-bottom: 10px;
          text-align: center;
        }
        .responsive-table .table-header {
          background-color: #0e83c6;
          color: white;
          font-size: 14px;
          text-transform: uppercase;
          letter-spacing: 0.03em;
          text-align: center;
        }
        .responsive-table .table-row {
          background-color: #FFFFFF;
          font-size: 12px;
          /*box-shadow: 0px 0px 9px 0px rgba(0, 0, 0, 0.1);*/
        }
        .responsive-table .table-sorted {
          background-color: #5fc8da;
        }
        .responsive-table .table-header a {
          color: white;
        }

        .responsive-table .col1 {
          flex-basis: 25%;
          padding-left: 5px;
          text-align: left;
        }
        .responsive-table .col2 {
          flex-basis: 3%;
        }
        .responsive-table .col3 {
          flex-basis: 3%;
        }
        .responsive-table .col4 {
          flex-basis: 5%;
        }
        .responsive-table .col5 {
          flex-basis: 14%;
        }
        .responsive-table .col6 {
          flex-basis: 10%;
        }
        .responsive-table .col7 {
          flex-basis: 10%;
        }
        .responsive-table .col8 {
          flex-basis: 5%;
        }
        .responsive-table .col9 {
          flex-basis: 5%;
        }
        .responsive-table .col10 {
          flex-basis: 5%;
        }
        .responsive-table .col11 {
          flex-basis: 10%;
        }
        .responsive-table .col12 {
          flex-basis: 5%;
        }
        @media all and (max-width: 600px) {
          .responsive-table .table-header {
            /*display: none;*/
            font-size: 12px;
          }
          .responsive-table hr {
            display: block;
          }
          .responsive-table .col {
            flex-basis: 100%;
          }
          .responsive-table .col {
            display: flex;
            padding: 10px 0;
          }
          .responsive-table .col:before {
            color: #6C7A89;
            padding-right: 10px;
            content: attr(data-label);
            flex-basis: 50%;
            text-align: right;
          }
        }

        .responsive-table .table-footer {
          background-color: #0e83c6; /* azul oscuro */
          color: white;
          font-size: 14px;
          margin: 10px;
          height: 30px;
        }

        .pag span
        {
          padding: 5px;
          border: solid 1px #0e83c6;
          text-decoration: none;
          white-space: nowrap;
          background: #5fc8da;
        }
        .pag a, 
        .pag a:visited
        {
          text-decoration: none;
          padding: 6px;
          white-space: nowrap;
          color:white;
        }
        .pag a:hover, 
        .pag a:active
        {
          padding: 5px;
          border: solid 1px #9ECDE7;
          text-decoration: none;
          white-space: nowrap;
          background: #0e83c6;
        }

    </style>
    <style>
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 99%">
            <tr>
                <td colspan="3">
                    <h4>
                        <asp:Label ID="lTitulo" runat="server" Text="ESTADÍSTICA GENERAL DE TURNOS"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table style="width: 100%" class="tablas">
                        <tr>
                            <td style="width: 20%" class="fila1" align="right">
                                <asp:Label ID="Label8" runat="server" Text="OFICINA: "></asp:Label></td>
                            <td style="width: 40%" class="fila1">
                                <asp:DropDownList ID="ddlOficinaId" CssClass="custom-select" Width="80%" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td class="fila1" style="width: 40%"></td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label9" runat="server" Text="FECHA INICIAL: "></asp:Label>
                            </td>
                            <td class="auto-style2">
                                <asp:TextBox ID="tbFechaIni" CssClass="form-control" Width="120px" runat="server" style="display:inline;" autocomplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ValidatorFechaInicial" runat="server" CssClass="text-danger" ControlToValidate="tbFechaIni" ErrorMessage="¡Debe suministrar la fecha inicial!!"></asp:RequiredFieldValidator>
                                <div class="container">
                                    <div class="row">
                                        <script type="text/javascript">
                                            $('#tbFechaIni').datepicker({
                                                clearBtn: true,
                                                language: "es",
                                                orientation: "bottom left",
                                                autoclose: true,
                                                todayHighlight: true,
                                                toggleActive: true
                                            });
                                        </script>
                                    </div>
                                </div>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label10" runat="server" Text="FECHA FINAL: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbFechaFin" CssClass="form-control" Width="120px" runat="server" style="display:inline;" autocomplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ValidatorFechaFinal" runat="server" CssClass="text-danger" ControlToValidate="tbFechaFin" ErrorMessage="¡Debe suministrar la fecha final!!"></asp:RequiredFieldValidator>
                                    <div class="container">
                                        <div class="row">
                                            <script type="text/javascript">
                                                $('#tbFechaFin').datepicker({
                                                    clearBtn: true,
                                                    language: "es",
                                                    orientation: "bottom left",
                                                    autoclose: true,
                                                    todayHighlight: true,
                                                    toggleActive: true
                                                });
                                            </script>
                                        </div>
                                    </div>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td align="left" style="width: 60%; height: 30px">
                                <div id="notificacion" style="width: 80%" runat="server" visible="false" class="alert alert-dismissible">
                                    <button type="button" class="close" data-dismiss="alert">&times;</button>
                                    <strong>
                                        <asp:Label ID="lbConfirmacion" runat="server"></asp:Label>
                                    </strong>
                                </div>
                            </td>
                            <td align="left" style="width: 40%">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 125px"></td>
                            <td>
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="GENERAR INFORME" OnClick="btnFiltrar_Click" />&nbsp;
                                <asp:Button ID="btnExportar" runat="server" CssClass="btn btn-outline-primary" Text="EXPORTAR" OnClick="btnExportar_Click" Visible="false" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblNoRegistros" runat="server" CssClass="badge badge-info" Text="No. de registros: " Visible="false"></asp:Label>
                    <asp:Label ID="tbNoRegistros" runat="server" CssClass="badge badge-primary rounded-pill" Visible="false"></asp:Label>
                    <asp:Label ID="lblSinRegistros" runat="server" Text="No se encontraron registros!" CssClass="badge badge-info" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gvTurnosTotales" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" PageSize="30" AllowSorting="True" OnPageIndexChanged="gvTurnosTotales_PageIndexChanged" CssClass="table border-dark table-hover" OnPageIndexChanging="gvTurnosTotales_PageIndexChanging" OnRowDataBound="gvTurnosTotales_RowDataBound" OnSorting="gvTurnosTotales_Sorting" OnSorted="gvTurnosTotales_Sorted" >
                        <Columns>
                            <asp:BoundField DataField="Oficina" SortExpression="Oficina" HeaderText="OFICINA" ItemStyle-CssClass="col1 l"/>
                            <asp:BoundField DataField="TipoAtencion" SortExpression="TipoAtencion" HeaderText="TIPO DE ATENCIÓN" ItemStyle-CssClass="col2 c" />
                            <asp:BoundField DataField="Turno" SortExpression="Turno" HeaderText="TURNO" ItemStyle-CssClass="col3 c" />
                            <asp:BoundField DataField="Estado" SortExpression="Estado" HeaderText="ESTADO" ItemStyle-CssClass="col4 l" />
                            <asp:BoundField DataField="Servicio" SortExpression="Servicio" HeaderText="SERVICIO SOLICITADO" ItemStyle-CssClass="col5 l" />
                            <asp:BoundField DataField="FechaInicio" SortExpression="FechaInicio" HeaderText="FECHA INICIO" ItemStyle-CssClass="col6 l" />
                            <asp:BoundField DataField="FechaFin" SortExpression="FechaFin" HeaderText="FECHA FIN" ItemStyle-CssClass="col7 l" />
                            <asp:BoundField DataField="TiempoEspera" SortExpression="TiempoEspera" HeaderText="TIEMPO DE ESPERA" ItemStyle-CssClass="col8 c" />
                            <asp:BoundField DataField="FechaLlamado" SortExpression="FechaLlamado" HeaderText="FECHA LLAMADO" ItemStyle-CssClass="col9 l" />
                            <asp:BoundField DataField="TiempoAtencion" SortExpression="TiempoAtencion" HeaderText="TIEMPO DE ATENCIÓN" ItemStyle-CssClass="col10 c" />
                            <asp:BoundField DataField="Causa" SortExpression="Causa" HeaderText="CAUSA ABANDONO" ItemStyle-CssClass="col6 l" />
                            <asp:BoundField DataField="Asesor" SortExpression="Asesor" HeaderText="USUARIO QUE ATENDIÓ" ItemStyle-CssClass="col11 l" />
                            <asp:BoundField DataField="Documento" SortExpression="Documento" HeaderText="DOCUMENTO CLIENTE" ItemStyle-CssClass="col12 l" />
                        </Columns>
                        <HeaderStyle CssClass="table-info text-center small" />
                        <RowStyle CssClass="border-dark table-active small"/>
                        <AlternatingRowStyle CssClass="border-dark small" />
                        <SortedAscendingHeaderStyle CssClass="table-sorted" /> 
                        <SortedDescendingHeaderStyle CssClass="table-sorted" />
                        <%--<PagerStyle BackColor="#8ee7e4" ForeColor="Black" HorizontalAlign="Center" CssClass="pagination" />--%>
                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager GridPager-Info" />

                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
            </tr>
        </table>
      </div>
      <asp:TextBox ID="txSQL" runat="server" Visible="False"></asp:TextBox>
    </form>
</body>
</html>

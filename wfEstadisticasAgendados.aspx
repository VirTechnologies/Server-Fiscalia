<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfEstadisticasAgendados.aspx.cs" Inherits="wfEstadisticasAgendados" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Estadísticas Agendamiento</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css" />
	<link rel="stylesheet" type="text/css" href="datetimepicker/bootstrap-datepicker.min.css" />
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css" />

    <script type="text/javascript" src="./datetimepicker/jquery.min.js"></script>
    <script type="text/javascript" src="./datetimepicker/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" src="./datetimepicker/bootstrap-datepicker.es.min.js"></script>

<%--    <script src="./js/amcharts4/core.js"></script>
    <script src="./js/amcharts4/charts.js"></script>
    <script src="./js/amcharts4/themes/animated.js"></script>
    <script src="./js/amcharts4/themes/kelly.js"></script>--%>
    <style>
        .c {
          text-align: center;
        }
        .l {
          text-align: left;
        }
        .r {
          text-align: right;
        }

        #chartdivAgendados {
          width: 100%;
          height: 400px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 99%">
            <tr>
                <td>
                    <h4>
                        <asp:Label ID="lTitulo" runat="server" Text="Estadísticas agendamiento turnos"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 20%" class="fila1" align="right">
                                <asp:Label ID="Label8" runat="server" Text="Seccional: "></asp:Label></td>
                            <td style="width: 40%" class="fila1">
                                <asp:DropDownList ID="ddlOficinaId" CssClass="custom-select" Width="80%" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td class="fila1" style="width: 40%"></td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label9" runat="server" Text="Fecha inicial: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbFechaIni" CssClass="form-control" Width="120px" runat="server" Style="display: inline;" autocomplete="off"></asp:TextBox>
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
                                <asp:Label ID="Label10" runat="server" Text="Fecha final: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbFechaFin" CssClass="form-control" Width="120px" runat="server" Style="display: inline;" autocomplete="off"></asp:TextBox>
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
                            <td align="left"></td>
                            <td align="left" style="width: 60%; height: 30px">
                                <div id="notificacion" style="width: 80%" runat="server" visible="false" class="alert alert-dismissible">
                                    <button type="button" class="close" data-dismiss="alert">&times;</button>
                                    <strong>
                                        <asp:Label ID="lbConfirmacion" runat="server"></asp:Label>
                                    </strong>
                                </div>
                            </td>
                            <td align="left" style="width: 40%"></td>
                        </tr>
                        <tr>
                            <td style="width: 125px"></td>
                            <td>
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="CONSULTAR AGENDAMIENTO" OnClick="btnFiltrar_Click" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
        <table style="width: 90%" id="TablaInfoTurnos" runat="server" visible="false">
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblNoRegistros" runat="server" CssClass="badge badge-info" Text="No. de registros: " Visible="false"></asp:Label>
                    <asp:Label ID="tbNoRegistros" runat="server" CssClass="badge badge-primary rounded-pill" Visible="false"></asp:Label>
                    <asp:Label ID="lblSinRegistros" runat="server" Text="No se encontraron registros!" CssClass="badge badge-info" Visible="False"></asp:Label>
                </td>
            </tr>
<%--            <tr>
                <td colspan="2" align="center">
                    <h4>
                    <asp:Label ID="Label5" runat="server" Text="ESTADÍSTICAS AGENDAMIENTO"></asp:Label>
                    </h4>
                </td>
            </tr>--%>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvAgendados" runat="server" AutoGenerateColumns="False" CssClass="table border-dark table-hover" OnRowDataBound="gvAgendados_RowDataBound" OnPageIndexChanged="gvAgendados_PageIndexChanged" 
                        OnPageIndexChanging="gvAgendados_PageIndexChanging" OnSorting="gvAgendados_Sorting"
                        AllowPaging="True" PageSize="30" AllowSorting="True" >
                        <Columns>
                            <asp:BoundField DataField="FechaAgendamiento" SortExpression="FechaAgendamiento" HeaderText="Fecha Solicitud" />
                            <asp:BoundField DataField="tipoagendamiento" SortExpression="tipoagendamiento" HeaderText="Tipo" />
                            <asp:BoundField DataField="Estado" SortExpression="Estado" HeaderText="Estado" />
                            <asp:BoundField DataField="Oficina" SortExpression="Oficina" HeaderText="Seccional" />
                            <asp:BoundField DataField="Abreviatura" SortExpression="Abreviatura" HeaderText="Abreviatura" ItemStyle-CssClass="c" Visible="false" />
                            <asp:BoundField DataField="Servicio" SortExpression="Servicio" HeaderText="Servicio" />
                            <asp:BoundField DataField="Fecha" SortExpression="Fecha" HeaderText="Agendamiento" />
                            <asp:BoundField DataField="Hora" SortExpression="Hora" HeaderText="Horario" />
                            <asp:BoundField DataField="cedula" SortExpression="cedula" HeaderText="Usuario" />
                        </Columns>
                        <RowStyle CssClass="border-dark table-active small"/>
                        <HeaderStyle CssClass="border-dark table-info text-center small" />
                        <AlternatingRowStyle CssClass="border-dark small" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
      </div>
      <asp:TextBox ID="txSQL" runat="server" Visible="False"></asp:TextBox>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

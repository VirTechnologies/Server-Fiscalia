<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfEstadisticasTurnos.aspx.cs" Inherits="wfEstadisticasTurnos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Estadísticas números de turnos</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css" />
	<link rel="stylesheet" type="text/css" href="datetimepicker/bootstrap-datepicker.min.css" />
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css" />

    <script type="text/javascript" src="./datetimepicker/jquery.min.js"></script>
    <script type="text/javascript" src="./datetimepicker/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" src="./datetimepicker/bootstrap-datepicker.es.min.js"></script>

    <script src="./js/amcharts4/core.js"></script>
    <script src="./js/amcharts4/charts.js"></script>
    <script src="./js/amcharts4/themes/animated.js"></script>
    <script src="./js/amcharts4/themes/kelly.js"></script>
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

        #chartdiv {
          width: 100%;
          height: 400px;
        }
        #chartdivAgendados {
          width: 100%;
          height: 400px;
        }
        #chartdivLlamados {
          width: 100%;
          height: 400px;
        }
        #chartdivCerrados {
          width: 100%;
          height: 400px;
        }
        #chartdivAbandonados {
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
                        <asp:Label ID="lTitulo" runat="server" Text="Estadísticas numéricas de turnos"></asp:Label>
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
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="CONSULTAR ESTADÍSTICAS" OnClick="btnFiltrar_Click" />
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
                <td colspan="2" align="center">
                    <h4>
                    <asp:Label ID="Label11" runat="server" Text="TURNOS SOLICITADOS"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvTurnosTotales" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Oficina" HeaderText="Seccional" />
                            <asp:BoundField DataField="NoTurnos" HeaderText="No. de turnos solicitados" ItemStyle-CssClass="c" />
                        </Columns>
                        <RowStyle CssClass="border-dark table-active small"/>
                        <HeaderStyle CssClass="border-dark table-info text-center small" />
                        <AlternatingRowStyle CssClass="border-dark small" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                   <div id="chartdiv"></div>    
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr style="border-width: 4px;width:50%" />
                </td>
            </tr>
 <%--          <tr>
                <td colspan="2" align="center">
                    <h4>
                    <asp:Label ID="Label5" runat="server" Text="TURNOS AGENDADOS"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvAgendados" runat="server" AutoGenerateColumns="False" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Oficina" HeaderText="Seccional" />
                            <asp:BoundField DataField="NoTurnos" HeaderText="No. de turnos agendados" ItemStyle-CssClass="c" />
                        </Columns>
                        <RowStyle CssClass="border-dark table-active small"/>
                        <HeaderStyle CssClass="border-dark table-info text-center small" />
                        <AlternatingRowStyle CssClass="border-dark small" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                   <div id="chartdivAgendados"></div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr style="border-width: 4px;width:50%" />
                </td>
            </tr>--%>
            <tr>
                <td colspan="2" align="center">
                    <h4>
                        <asp:Label ID="Label1" runat="server" Text="TURNOS LLAMADOS"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvAtendidos" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Oficina" HeaderText="Seccional" />
                            <asp:BoundField DataField="NoTurnos" HeaderText="No. de turnos llamados"  ItemStyle-CssClass="c"/>
                        </Columns>
                        <RowStyle CssClass="border-dark table-active small"/>
                        <HeaderStyle CssClass="border-dark table-info text-center small" />
                        <AlternatingRowStyle CssClass="border-dark small" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                   <div id="chartdivLlamados"></div>    
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr style="border-width: 4px;width:50%" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <h4>
                        <asp:Label ID="Label3" runat="server" Text="TURNOS CERRADOS"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvCerrados" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Oficina" HeaderText="Seccional" />
                            <asp:BoundField DataField="NoTurnos" HeaderText="No. de turnos cerrados" ItemStyle-CssClass="c" />
                        </Columns>
                        <RowStyle CssClass="border-dark table-active small"/>
                        <HeaderStyle CssClass="border-dark table-info text-center small" />
                        <AlternatingRowStyle CssClass="border-dark small" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                   <div id="chartdivCerrados"></div>    
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr style="border-width: 4px;width:50%" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <h4>
                        <asp:Label ID="Label2" runat="server" Text="TURNOS ABANDONADOS"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvAbandonados" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Oficina" HeaderText="Seccional" />
                            <asp:BoundField DataField="NoTurnos" HeaderText="No. de turnos abandonados"  ItemStyle-CssClass="c"/>
                        </Columns>
                        <RowStyle CssClass="border-dark table-active small"/>
                        <HeaderStyle CssClass="border-dark table-info text-center small" />
                        <AlternatingRowStyle CssClass="border-dark small" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                   <div id="chartdivAbandonados"></div>    
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr style="border-width: 4px;width:50%" />
                </td>
            </tr>
        </table>
        <script>

            var categoryName= "groupname";
            var valueName= "count";

            function countChart(chartData, elName, countAxisName) {
                // Create chart instance
                let chart = am4core.create(elName, am4charts.XYChart);
                chart.scrollbarX = new am4core.Scrollbar();

                // Add data
                chart.data = chartData;

                // Create axes
                let checkoutsAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                checkoutsAxis.dataFields.category = categoryName;
                checkoutsAxis.renderer.grid.template.location = 0;
                checkoutsAxis.renderer.minGridDistance = 30;
                checkoutsAxis.renderer.labels.template.horizontalCenter = "right";
                checkoutsAxis.renderer.labels.template.verticalCenter = "middle";
                checkoutsAxis.renderer.labels.template.rotation = 300;
                checkoutsAxis.tooltip.disabled = true;
                checkoutsAxis.renderer.minHeight = 110;

                let countAxis = chart.yAxes.push(new am4charts.ValueAxis());
                countAxis.title.text = countAxisName;
                countAxis.renderer.minWidth = 50;
                countAxis.min = 0;

                // Create series
                let columnSerie = chart.series.push(new am4charts.ColumnSeries());
                columnSerie.name = countAxisName;
                // columnSerie.sequencedInterpolation = true;
                columnSerie.dataFields.valueY = valueName;
                columnSerie.dataFields.categoryX = categoryName;
                columnSerie.tooltipText = "{categoryX}: \t [#fff font-size: 20px]{valueY} turnos[/]";
                columnSerie.columns.template.strokeWidth = 0;
                // columnSerie.tooltip.pointerOrientation = "vertical";

                columnSerie.columns.template.adapter.add("fill", (fill, target) => {
                    return chart.colors.getIndex(target.dataItem.index);
                })

                // Cursor
                chart.cursor = new am4charts.XYCursor();

                // Add legend
                //chart.legend = new am4charts.Legend();
                //chart.legend.position = "top";

                // MENÚ EXPORTAR GRÁFICA
                chart.exporting.menu = new am4core.ExportMenu();
                chart.exporting.menu.align = "right";
                chart.exporting.menu.verticalAlign = "bottom";      

                return chart;
              }
        </script>
      </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

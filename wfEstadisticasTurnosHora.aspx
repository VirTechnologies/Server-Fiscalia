<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfEstadisticasTurnosHora.aspx.cs" Inherits="wfEstadisticasTurnosHora" %>

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
                <td colspan="2">
                    <h4>
                        <asp:Label ID="lTitulo" runat="server" Text="ESTADÍSTICAS NUMÉRICAS DE TURNOS POR FRANJA HORARIA"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table style="width: 100%" >
                        <tr>
                            <td style="width: 20%" class="fila1" align="right">
                                <asp:Label ID="Label8" runat="server" Text="SECCIONAL: "></asp:Label></td>
                            <td style="width: 40%" class="fila1">
                                <asp:DropDownList ID="ddlOficinaId" CssClass="custom-select" Width="80%" runat="server">
                                </asp:DropDownList>
                                </td>
                            <td class="fila1" style="width: 40%">
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label9" runat="server" Text="FECHA: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbFechaIni" CssClass="form-control" Width="120px" runat="server" style="display:inline;" autocomplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ValidatorFechaInicial" runat="server" CssClass="text-danger" ControlToValidate="tbFechaIni" ErrorMessage="¡Debe suministrar la fecha!"></asp:RequiredFieldValidator>
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
                                <asp:Label ID="Label4" runat="server" Text="HORA INICIAL: "></asp:Label>
                            </td>
                            <td>
                                <%--<asp:TextBox ID="HoraI" runat="server" MaxLength="2"></asp:TextBox>--%>
                                <asp:DropDownList ID="HoraI" CssClass="custom-select" Width="65px"  runat="server">
                                </asp:DropDownList>

                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="HoraI" ErrorMessage="¡Debe suministrar la hora de inicio!">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label10" runat="server" Text="HORA FINAL: "></asp:Label>
                            </td>
                            <td>
<%--                                <asp:TextBox ID="HoraF" runat="server" MaxLength="2"></asp:TextBox>--%>
                                <asp:DropDownList ID="HoraF" CssClass="custom-select" Width="65px" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator2" runat="server" ControlToValidate="HoraF" ErrorMessage="¡Debe suministrar la hora de finalización!"></asp:RequiredFieldValidator>                  
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
                            <td style="width: 125px">
                                </td>
                            <td>
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="CONSULTAR ESTADÍSTICAS" OnClick="btnFiltrar_Click" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    </td>
            </tr>
        </table>
        <table style="width: 99%" id="TablaInfoTurnos" runat="server" visible="false">
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
                            <asp:BoundField DataField="Oficina" HeaderText="SECCIONAL" />
                            <asp:BoundField DataField="hora" HeaderText="HORA DEL DÍA" ItemStyle-CssClass="c" />
                            <asp:BoundField DataField="NoTurnos" HeaderText="NO. DE TURNOS" ItemStyle-CssClass="c" />
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
                            <asp:BoundField DataField="Oficina" HeaderText="SECCIONAL" />
                            <asp:BoundField DataField="hora" HeaderText="HORA DEL DÍA" ItemStyle-CssClass="c" />
                            <asp:BoundField DataField="NoTurnos" HeaderText="NO. DE TURNOS LLAMADOS" ItemStyle-CssClass="c" />
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
                            <asp:BoundField DataField="Oficina" HeaderText="SECCIONAL" />
                            <asp:BoundField DataField="hora" HeaderText="HORA DEL DÍA" ItemStyle-CssClass="c" />
                            <asp:BoundField DataField="NoTurnos" HeaderText="NO. DE TURNOS CERRADOS" ItemStyle-CssClass="c" />
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
                <td colspan="3" align="center">
                    <h4>
                        <asp:Label ID="Label2" runat="server" Text="TURNOS ABANDONADOS"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvAbandonados" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Oficina" HeaderText="SECCIONAL" />
                            <asp:BoundField DataField="hora" HeaderText="HORA DEL DÍA" ItemStyle-CssClass="c" />
                            <asp:BoundField DataField="NoTurnos" HeaderText="NO. DE TURNOS ABANDONADOS" ItemStyle-CssClass="c" />
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

            var categoryHour = "hora";
            var categoryName = "groupname";
            var valueName= "count";

            function countChart(chartData, elName, countAxisName, oficinas) {

                //console.log("oficinas " + oficinas);
                const oficinasArray = oficinas.split(",");
                // Create chart instance
                let chart = am4core.create(elName, am4charts.XYChart);
                chart.scrollbarX = new am4core.Scrollbar();

                // Add data
                chart.data = chartData;

                // Create axes
                let checkoutsAxis = chart.xAxes.push(new am4charts.CategoryAxis());
                checkoutsAxis.dataFields.category = categoryHour;
                checkoutsAxis.renderer.grid.template.location = 0;
                checkoutsAxis.renderer.minGridDistance = 5;
                checkoutsAxis.renderer.labels.template.horizontalCenter = "center";
                checkoutsAxis.renderer.labels.template.verticalCenter = "middle";
                ////checkoutsAxis.renderer.labels.template.rotation = 300;
                checkoutsAxis.tooltip.disabled = true;
                //checkoutsAxis.renderer.minHeight = 110;

                let countAxis = chart.yAxes.push(new am4charts.ValueAxis());
                countAxis.renderer.inside = true;
                countAxis.renderer.labels.template.disabled = true;
                countAxis.min = 0;
                countAxis.title.text = countAxisName;
                //countAxis.renderer.minWidth = 50;
                //countAxis.min = 0;

                // Create series
                function createSeries(field, name) {

                    // Set up series
                    var series = chart.series.push(new am4charts.ColumnSeries());
                    series.name = name;
                    series.dataFields.valueY = field;
                    series.dataFields.categoryX = categoryHour;
                    series.sequencedInterpolation = true;

                    // Make it stacked
                    series.stacked = true;

                    // Configure columns
                    series.columns.template.width = am4core.percent(60);
                     //= "{categoryX}: \t [#fff font-size: 20px]{valueY} turnos[/]"
                    series.columns.template.tooltipText = "[bold]{name}:[/][#fff font-size:18px] {valueY} turnos[/]";

                    // Add label
                    var labelBullet = series.bullets.push(new am4charts.LabelBullet());
                    labelBullet.label.text = "{valueY}";
                    labelBullet.locationY = 0.5;
                    labelBullet.label.hideOversized = true;

                    return series;
                }

                for (var valor of oficinasArray) {
                    //console.log("Valor: " + valor);
                    createSeries(valor, valor);
                }
                //oficinasArray.forEach(
                //    createSeries(valor, valor);
                //    function (valor, indice, oficinasArray)
                //    {
                //        console.log("En el índice " + indice + " está: " + valor);
                //    }
                //);
                

                //columnSerie.columns.template.adapter.add("fill", (fill, target) => {
                //    return chart.colors.getIndex(target.dataItem.index);
                //})

                //// Cursor
                //chart.cursor = new am4charts.XYCursor();

                // Add legend
                chart.legend = new am4charts.Legend();

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

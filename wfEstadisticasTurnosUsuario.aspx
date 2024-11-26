<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfEstadisticasTurnosUsuario.aspx.cs" Inherits="wfEstadisticasTurnosUsuario" %>

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
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 99%">
            <tr>
                <td colspan="3">
                    <h4>
                        <asp:Label ID="lTitulo" runat="server" Text="Estadísticas por cliente" ></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table style="width: 100%" >
                        <tr>
                            <td style="width: 20%" class="fila1" align="right">
                                <asp:Label ID="Label4" runat="server" Text="Identificación cliente: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbIdentificacion" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbIdentificacion" ErrorMessage="Ingrese identificación!"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td align="right">
                                <asp:Label ID="Label9" runat="server" Text="Fecha inicial (dd/mm/yyyy): "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbFechaIni" CssClass="form-control" Width="120px" runat="server" style="display:inline;" autocomplete="off"></asp:TextBox>
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
                        <tr style="display:none;">
                            <td align="right">
                                <asp:Label ID="Label10" runat="server" Text="Fecha final: "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbFechaFin" CssClass="form-control" Width="120px" runat="server" style="display:inline;" autocomplete="off"></asp:TextBox>
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
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 125px">
                                </td>
                            <td>
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="CONSULTAR ESTADÍSTICAS" OnClick="btnFiltrar_Click" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
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
                    </table>
                    </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
            </tr>
        </table>
        <table style="width: 99%" id="TablaInfoTurnos" runat="server" visible="false">
            <tr>
                <td colspan="3" align="center">
                    <h4>
                    <asp:Label ID="Label11" runat="server" Text="TURNOS SOLICITADOS"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gvTurnosTotales" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Identificacion" HeaderText="Identificación cliente" />
                            <asp:BoundField DataField="NoTurnos" HeaderText="No. de turnos" />
                        </Columns>
                        <RowStyle CssClass="border-dark table-active"/>
                        <HeaderStyle CssClass="border-dark table-info text-center" />
                        <AlternatingRowStyle CssClass="border-dark" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 100px">
                    &nbsp;</td>
                <td style="width: 100px">
                    &nbsp;</td>
                <td style="width: 100px">
                    &nbsp;</td>
            </tr>
        </table>
      </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

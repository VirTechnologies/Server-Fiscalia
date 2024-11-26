<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfEstadisticasActividadAsesor.aspx.cs" Inherits="wfEstadisticasActividadAsesor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Estadísticas - Actividad Asesores</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css" />
	<link rel="stylesheet" type="text/css" href="datetimepicker/bootstrap-datepicker.min.css" />
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css" />

    <script type="text/javascript" src="./datetimepicker/jquery.min.js"></script>
    <script type="text/javascript" src="./datetimepicker/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" src="./datetimepicker/bootstrap-datepicker.es.min.js"></script>
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
                        <asp:Label ID="lTitulo" runat="server" Text="ESTADÍSTICAS - ACTIVIDAD ASESORES" ></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="3">
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
                            <td style="width: 20%" align="right">
                                <asp:Label ID="Label4" runat="server" Text="RAZÓN DE SUSPENSIÓN: "></asp:Label></td>
                            <td style="width: 40%" class="fila1">
                                <asp:DropDownList ID="ddlRazonId" CssClass="custom-select" Width="80%" runat="server">
                                </asp:DropDownList>
                                </td>
                            <td class="fila1" style="width: 40%">
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label9" runat="server" Text="FECHA INICIAL: "></asp:Label>
                            </td>
                            <td class="auto-style2">
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
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label10" runat="server" Text="FECHA FINAL: "></asp:Label>
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
                            <td style="width: 125px">
                                </td>
                            <td>
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="CONSULTAR ESTADÍSTICAS" OnClick="btnFiltrar_Click" />&nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                    </td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;
                </td>
            </tr>
        </table>
        <table style="width: 99%" id="TablaInfoTurnos" runat="server" visible="false">
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="30" Width="100%"  AllowSorting="True" OnPageIndexChanged="gvActivity_PageIndexChanged" CssClass="table border-dark table-hover" OnPageIndexChanging="gvActivity_PageIndexChanging" OnRowDataBound="gvActivity_RowDataBound" OnSorting="gvActivity_Sorting">
                        <Columns>
                            <asp:BoundField DataField="Oficina" SortExpression="Oficina" HeaderText="SECCIONAL" />
                            <asp:BoundField DataField="Nombre" SortExpression="Nombre" HeaderText="NOMBRE ASESOR" />
                            <asp:BoundField DataField="TimeActivity" SortExpression="TimeActivity" HeaderText="FECHA ACTIVIDAD" />
                            <asp:BoundField DataField="Estado" SortExpression="Estado" HeaderText="ESTADO" />
                            <asp:BoundField DataField="Razon" SortExpression="Razon" HeaderText="RAZÓN DE SUSPENSIÓN" />
                        </Columns>
                        <RowStyle CssClass="border-dark table-active"/>
                        <HeaderStyle CssClass="border-dark table-info text-center" />
                        <AlternatingRowStyle CssClass="border-dark" />
                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager GridPager-Info" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="trnotitulo" colspan="3" style="height: 16px">
                    <asp:Label ID="lblNoRegistros" runat="server" CssClass="badge badge-info" Text="No. de registros: " Visible="false"></asp:Label>
                    <asp:Label ID="tbNoRegistros" runat="server" CssClass="badge badge-primary rounded-pill" Visible="false"></asp:Label>
                    <asp:Label ID="lblSinRegistros" runat="server" Text="No se encontraron registros!" CssClass="badge badge-info" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:TextBox ID="txSQL" runat="server" Visible="False"></asp:TextBox>
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
      </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

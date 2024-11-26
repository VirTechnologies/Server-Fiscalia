<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfEncuestaRespuestas.aspx.cs" Inherits="wfEncuestaRespuestas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Respuestas a Encuesta</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="datetimepicker/bootstrap-datepicker.min.css" />
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css" />

    <script type="text/javascript" src="./datetimepicker/jquery.min.js"></script>
    <script type="text/javascript" src="./datetimepicker/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" src="./datetimepicker/bootstrap-datepicker.es.min.js"></script>

</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <div>
        <table style="width: 99%">
            <tr>
                <td align="left" style="width: 80%;" colspan="3">
                    <h3><asp:Label ID="Label2" runat="server" Text="Respuestas a Encuesta: " ></asp:Label>
                    <small class="text-muted">
                    <asp:Label ID="lblNombreEncuesta" runat="server" Font-Bold="True"></asp:Label>
                    </small>
                    </h3>
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 25%">
                    <asp:Button ID="btnSalir" runat="server" CssClass="btn btn-outline-info" CausesValidation="False" Text="VOLVER" OnClick="btnSalir_Click" />
                </td>

                <td align="right" colspan="2">
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label9" runat="server" Text="Fecha inicial "></asp:Label>
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
                    <asp:Label ID="Label10" runat="server" Text="Fecha final "></asp:Label>
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
                    <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="CONSULTAR" OnClick="btnFiltrar_Click" />&nbsp;
                    <asp:Button ID="btnExportar" runat="server" CssClass="btn btn-outline-primary" Text="EXPORTAR RESPUESTAS" OnClick="btnExportar_Click" Visible="false" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gvRespuestas" runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="True" OnRowDataBound="gvRespuestas_RowDataBound" OnSorting="gvRespuestas_Sorting" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="oficina" HeaderText="Seccional" SortExpression="Oficina"/>
                            <asp:BoundField DataField="pregunta" HeaderText="Pregunta" SortExpression="pregunta" />
                            <asp:BoundField DataField="opcion" HeaderText="Respuesta" SortExpression="opcion" />
                            <asp:BoundField DataField="total" HeaderText="Ocurrencia" SortExpression="total" />
                        </Columns>
                        <HeaderStyle CssClass="table-info text-center" />
                        <RowStyle CssClass="border-dark table-active"/>
                        <AlternatingRowStyle CssClass="border-dark" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
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
            <tr>
                <td colspan="3">
                    <asp:HiddenField ID="hfConsulta" runat="server" />
                    <asp:HiddenField ID="hfid" runat="server" />
                </td>
            </tr>
        </table>
      </div>
    </form>
</body>
</html>

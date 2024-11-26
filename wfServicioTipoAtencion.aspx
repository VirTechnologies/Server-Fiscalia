<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfServicioTipoAtencion.aspx.cs" Inherits="wfServicioTipoAtencion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edición de servicio tipo de atención</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
</head>
<body runat="server" id="body">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="98%" >
            <tr>
                <td align="left" colspan="2">
                    <h4>
                        <asp:Label ID="Label1" runat="server" Text="CONFIGURACIÓN DE SERVICIO Y TIPO DE ATENCIÓN"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td align="left"  style="width: 20%">
                    <asp:Button ID="btnSalir" runat="server" CssClass="btn btn-outline-info" CausesValidation="False" Text="VOLVER" OnClick="btnSalir_Click" />
                    </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%">
                        <tr>
                            <td align="left" style="width: 20%">
                            </td>
                            <td align="right" style="width: 40%">
                                <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR"
                                    OnClick="btnGrabar_Click" />&nbsp;
                                <asp:Button Visible="false" ID="btnEliminar" runat="server" CssClass="btn btn-danger" Text="ELIMINAR" OnClick="btnEliminar_Click" CausesValidation="False"/>
                            </td>
                            <td align="left" style="width: 60%">
                            </td>
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
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label8" runat="server" Text="SERVICIO*:"></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:DropDownList ID="ddlServicioId" runat="server" Width="80%" CssClass="custom-select" >
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlServicioId" ErrorMessage="Seleccione!"></asp:RequiredFieldValidator>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label9" runat="server" Text="TIPO DE ATENCIÓN*:"></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:DropDownList ID="ddlTipoAtencionId" runat="server" Width="80%" CssClass="custom-select" >
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlTipoAtencionId" ErrorMessage="Seleccione!"></asp:RequiredFieldValidator>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style1">
                                <asp:Label ID="Label14" runat="server" Text="ABREVIATURA*:"></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="tbAbreviatura" style="display:inline;" runat="server" CssClass="form-control" Width="80" MaxLength="8"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Digite la abreviatura!" ControlToValidate="tbAbreviatura"></asp:RequiredFieldValidator>
                                </td>
                            <td align="left">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label11" runat="server" Text="HABILITA FRANJA HORARIA: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:CheckBox ID="cbHabilitaFranjaHoraria" runat="server" />
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style1">
                                <asp:Label ID="Label2" runat="server" Text="HORA INICIO (HH:mm): "></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="tbHoraDesde" runat="server" CssClass="form-control" Width="100" MaxLength="5"></asp:TextBox>
                                </td>
                            <td align="left">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label10" runat="server" Text="HORA FIN (HH:mm): "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbHoraFin" runat="server" CssClass="form-control" Width="100" MaxLength="5"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label12" runat="server" Text="TIEMPO MÁXIMO DE ATENCIÓN "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbTiempoAtencionMinutos" style="display:inline;" runat="server" CssClass="form-control" Width="50" MaxLength="2"/> (Minutos)
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label13" runat="server" Text="TIEMPO DE ESPERA "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbTiempoEsperaMinutos" style="display:inline;" runat="server" CssClass="form-control" Width="50" MaxLength="2" /> (Minutos)
                            </td>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" class="fila1">
                                &nbsp;</td>
                            <td align="left" class="fila1" colspan="2">
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:HiddenField ID="hfConsulta" runat="server" />
                    <asp:HiddenField ID="hfid" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

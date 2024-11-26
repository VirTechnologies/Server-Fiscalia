<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfServicioOficina.aspx.cs" Inherits="wfServicioOficina" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edición de servicios</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
</head>
<body runat="server" id="body">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="98%" >
            <tr>
                <td align="left" colspan="2" style="height: 16px">
                    <h4>
                        <asp:Label ID="Label1" runat="server" Text="Configuración servicio en Seccional"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 66px">
                    <table width="100%">
                        <tr>
                            <td align="left" style="width: 20%">
                                <asp:Button ID="btnSalir" runat="server" CssClass="btn btn-outline-info" CausesValidation="False" Text="VOLVER" OnClick="btnSalir_Click" /></td>
                            <td align="right" style="width: 60%">
                                <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR SERVICIO" OnClick="btnGrabar_Click" /></td>
                            <td align="left" style="width: 40%">
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
                            <td align="left" style="width: 20%">
                            </td>
                            <td align="left" style="width: 60%; height: 30px">
                            </td>
                            <td align="left" style="width: 40%">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label8" runat="server" Text="Seccional"></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbNombreOficina" runat="server" CssClass="form-control" Width="80%" MaxLength="100" ReadOnly="True"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label9" runat="server" Text="Nombre del servicio: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbNombreServicio" runat="server" CssClass="form-control" Width="80%" MaxLength="100" ReadOnly="True"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style1">
                                <asp:Label ID="Label2" runat="server" Text="Hora inicio: "></asp:Label></td>
                            <td align="left">
                                <asp:TextBox ID="tbHoraIni" runat="server" CssClass="form-control" Width="9%" MaxLength="100"></asp:TextBox>
                                </td>
                            <td align="left">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label10" runat="server" Text="Hora fin: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbHoraFin" runat="server" CssClass="form-control" Width="9%" MaxLength="100"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label11" runat="server" Text="Mensaje personalizado: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbMensajeServicio" runat="server" CssClass="form-control" Width="80%" MaxLength="100"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label12" runat="server" Text="Tiempo máximo de atención (Segundos)"></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbTiempoMaximoSegundos" runat="server" CssClass="form-control" Width="9%" MaxLength="100"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label13" runat="server" Text="Número de llamados por turno"></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbNumeroLlamados" runat="server" CssClass="form-control" Width="9%" MaxLength="100"></asp:TextBox>
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
                    <asp:HiddenField ID="hfServicioId" runat="server" />
                    <asp:HiddenField ID="hfOficinaId" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

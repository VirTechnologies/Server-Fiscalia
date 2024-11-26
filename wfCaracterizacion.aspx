<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfCaracterizacion.aspx.cs" Inherits="wfUsuarioCaja" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edición de usuarios asesores</title>
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
                        <asp:Label ID="Label1" runat="server"  Text="ADMINISTRACIÓN DE CARACTERIZACIONES"></asp:Label>
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
                                <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR CARACTERIZACIÓN" OnClick="btnGrabar_Click" />&nbsp;
                                <asp:Button Visible="false" ID="btnEliminar" runat="server" CssClass="btn btn-danger" Text="ELIMINAR USUARIO"/></td>
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
                                <asp:Label ID="LabelNombre" runat="server" Text="NOMBRE CARACTERIZACIÓN*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbNombre" runat="server" CssClass="form-control" Width="80%" MaxLength="100"></asp:TextBox>
                                </td>
                        </tr>
                        
                        
                        
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label7" runat="server" Text="CARACTERIZACION PADRE*: "></asp:Label></td>
                            <td>
                                <asp:DropDownList ID="ddlCaracterizacionPadre" runat="server" CssClass="custom-select" style="width: 30%">
                                    <asp:ListItem Text="No Aplica" Value="0" />
                                </asp:DropDownList>
                 
                        </tr>

                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="labelHabilitado" runat="server" Text="Habilitar: "></asp:Label>
                            </td>
                            <td align="left" style="width: 40%">
                                <asp:CheckBox ID="cbHabilitado" runat="server"  />
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;
                            </td>
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

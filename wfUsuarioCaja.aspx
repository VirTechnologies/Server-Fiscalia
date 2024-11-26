<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfUsuarioCaja.aspx.cs" Inherits="wfUsuarioCaja" %>

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
                        <asp:Label ID="Label1" runat="server"  Text="ADMINISTRACIÓN DE USUARIO"></asp:Label>
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
                                <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR USUARIO" OnClick="btnGrabar_Click" />&nbsp;<asp:Button Visible="false" ID="btnEliminar" runat="server" CssClass="btn btn-danger" Text="ELIMINAR USUARIO" OnClick="btnEliminar_Click" /></td>
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
                                <asp:Label ID="Label8" runat="server" Text="NOMBRE USUARIO*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbNombre" runat="server" CssClass="form-control" Width="80%" MaxLength="100"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbNombre"
                                    ErrorMessage="¡Debe escribir el nombre!"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label2" runat="server" Text="USUARIO O LOGIN*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbUserName" runat="server" CssClass="form-control" Width="80%" MaxLength="100"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbUserName"
                                    ErrorMessage="¡Debe escribir el usuario o login!"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr runat="server" id="CambioClave">
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label5" runat="server" Text="CLAVE*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbPasswordHash" runat="server" CssClass="form-control" Width="80%" MaxLength="100"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbPasswordHash" ErrorMessage="¡Debe digitar la clave, que debe contener al menos una mayúscula, un carácter especial y números!"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label3" runat="server" Text="EMAIL*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbEmail" runat="server" CssClass="form-control" Width="80%" MaxLength="100"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbEmail"
                                    ErrorMessage="¡Debe escribir el correo eletrónico!"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator CssClass="text-danger" ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbEmail" ErrorMessage="No es un correo válido!" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label7" runat="server" Text="PERFIL SECCIONAL*: "></asp:Label></td>
                            <td>
                                <asp:DropDownList ID="ddlPerfilOficina" runat="server" CssClass="custom-select" style="width: 30%" >
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="True">USUARIO</asp:ListItem>
                                <asp:ListItem Value="False">SÚPER USUARIO</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 20%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlPerfilOficina" ErrorMessage="Seleccione el perfil seccional"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label18" runat="server" Text="PROCEDENCIA*: "></asp:Label></td>
                            <td>
                                <asp:DropDownList ID="ddlProcedencia" runat="server" CssClass="custom-select" style="width: 30%" >
                                </asp:DropDownList>
                            </td>
                            <td style="width: 20%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator18" runat="server" ControlToValidate="ddlPerfilOficina" ErrorMessage="Seleccione la procedencia"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label4" runat="server" Text="DOCUMENTO: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbPhoneNumber" runat="server" CssClass="form-control" Width="80%" MaxLength="100"></asp:TextBox>
                                </td>
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

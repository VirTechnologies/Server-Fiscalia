<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="wfCambioClave.aspx.cs" Inherits="wfCambioClave" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Cambio contraseña</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">    </asp:ScriptManager>

    <div>
        <table runat="server" style="width: 100%">
            <tr>
                <td align="left">
                    <table class="Tabla" width="100%">
                        <tr>
                            <td colspan="3">
                                <h4>Cambio contraseña</h4>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label2" runat="server" Text="Contraseña ACTUAL"></asp:Label>
                            </td>
                            <td class="fila1" align="center" style="width: 180px">
                                <asp:TextBox ID="tbClaveAntigua" runat="server" TextMode="Password" CssClass="form-control" Width="175px"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbClaveAntigua"
                                    ErrorMessage="Digite la contraseña"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label1" runat="server" Text="Contraseña NUEVA"></asp:Label>
                            </td>
                            <td class="fila1" align="center" style="width: 180px">
                                <asp:TextBox ID="tbpwdNuevo" runat="server" TextMode="Password" CssClass="form-control" Width="175px"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbpwdNuevo"
                                    ErrorMessage="Digite la contraseña"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label5" runat="server" Text="Repetir Contraseña NUEVA"></asp:Label>
                            </td>
                            <td class="fila1" align="center" style="width: 180px">
                                <asp:TextBox ID="tbPwdOtra" runat="server" TextMode="Password" CssClass="form-control" Width="175px"></asp:TextBox>
                            </td>
                            <td align="left" class="fila1">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbPwdOtra"
                                    ErrorMessage="Vuelva a digitar la contraseña"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left" class="fila1">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:Button ID="btCambia" runat="server" CssClass="btn btn-primary" Text="CAMBIAR" OnClick="btCambia_Click"/>
                                &nbsp;
                                <asp:Button ID="btSalir" runat="server" Text="VOLVER" CausesValidation="false" CssClass="btn btn-outline-info" OnClick="btSalir_Clic"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center" style="width: 60%; height: 30px">&nbsp;
                                <div id="notificacion" style="width: 80%; align-content:center" runat="server" visible="false" class="alert alert-dismissible">
                                    <button type="button" class="close" data-dismiss="alert">&times;</button>
                                    <strong>
                                        <asp:Label ID="lbConfirmacion" runat="server"></asp:Label>
                                    </strong>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="id_usuario" runat="server" />
                    <asp:HiddenField ID="hfOpcional" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

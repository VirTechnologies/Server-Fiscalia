<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfModulo.aspx.cs" Inherits="wfModulo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Administrar módulo</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <h4>
                            <asp:Label ID="Label1" runat="server" Text="ADMINISTRACIÓN DE MÓDULOS"></asp:Label>
                        </h4>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 20%">
                        <asp:Button ID="bCancelar" runat="server" CausesValidation="false" CssClass="btn btn-outline-info" Text="VOLVER" OnClick="bCancelar_Click" />
                    </td>
                    <td style="width: 60%">&nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="width: 20%">
                        <asp:Label ID="Label3" runat="server" Text="Módulo*"></asp:Label>
                    </td>
                    <td class="trnotitulo">
                        <asp:TextBox ID="tbmodulo" runat="server" CssClass="caja"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbModulo"
                            ErrorMessage="Digite el nombre del módulo"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 20%">
                        <asp:Label ID="Label4" runat="server" Text="Descripción*"></asp:Label></td>
                    <td class="trnotitulo">
                        <asp:TextBox ID="tbdescripcion" runat="server" CssClass="caja" Width="54%"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbdescripcion"
                            ErrorMessage="Digite la descripción del módulo"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 20%">
                        <asp:Label ID="Label5" runat="server" Text="Visible"></asp:Label></td>
                    <td class="trnotitulo">
                        &nbsp;<asp:RadioButtonList ID="rbVisible" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Value="1">SI</asp:ListItem>
                            <asp:ListItem Value="0">NO</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td align="right" style="width: 20%">
                        <asp:Label ID="Label2" runat="server" Text="Módulo padre"></asp:Label></td>
                    <td class="trnotitulo">
                        <asp:DropDownList ID="ddlid_modulo_padre" runat="server" CssClass="custom-select" Width="54%" >
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td align="right" style="width: 20%">
                        <asp:Label ID="Label6" runat="server" Text="Página"></asp:Label>
                    </td>
                    <td class="trnotitulo">
                        <asp:TextBox ID="tbpagina" runat="server" CssClass="caja" Width="54%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="width: 20%">
                        <asp:Label ID="Label7" runat="server" Text="Orden*"></asp:Label></td>
                    <td class="trnotitulo">
                        <asp:TextBox ID="tborden" runat="server" CssClass="caja" Width="10%"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator5" runat="server" ControlToValidate="tborden"
                            ErrorMessage="Digite el orden de aparición en el menú"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" CssClass="text-danger" runat="server" ControlToValidate="tborden"
                            ErrorMessage="Debe ser un número entero" MaximumValue="99" MinimumValue="0" Type="Integer"></asp:RangeValidator></td>
                </tr>
                <tr>
                    <td align="center" class="trtitulo" colspan="2">
                        <asp:Button ID="bGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR" OnClick="bGrabar_Click" />
                        <asp:Button ID="bEliminar" runat="server" CssClass="btn btn-danger" Text="ELIMINAR" OnClick="bEliminar_Click" />
                        <%--<asp:Button ID="bCancelar" runat="server" CssClass="btn btn-primary" Text=" Salir " OnClick="bCancelar_Click" /></td>--%>
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
                        </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:HiddenField ID="hfid_modulo" runat="server" />
                        <asp:HiddenField ID="hfConsulta" runat="server" />
                        &nbsp;
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="false"
                            ShowSummary="false" CssClass="text-danger" />
                    </td>
                </tr>
            </table>
        </div>
    
    </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

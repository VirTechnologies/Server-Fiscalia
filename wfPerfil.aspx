<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfPerfil.aspx.cs" Inherits="wfPerfil" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edición de perfiles</title>
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
                        <asp:Label ID="Label1" runat="server"  Text="ADMINISTRACIÓN DE PERFIL"></asp:Label>
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
                            <td style="width: 20%" align="left">
                                <asp:Button ID="btnSalir" runat="server" CssClass="btn btn-outline-info" CausesValidation="False" Text="VOLVER" OnClick="btnSalir_Click" /></td>
                            <td align="right" style="width: 60%">
                                <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR PERFIL" OnClick="btnGrabar_Click" /><asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-danger" Text="ELIMINAR PERFIL" Visible="False" OnClick="btnEliminar_Click" /></td>
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
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label2" runat="server" Text="PERFIL *"></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="perfil" runat="server" CssClass="form-control" Width="96%" MaxLength="50"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="perfil"
                                    ErrorMessage="¡Debe escribir el perfil!"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td align="right" class="fila1">
                                <asp:Label ID="Label3" runat="server" Text="DESCRIPCIÓN"></asp:Label></td>
                            <td align="left" class="fila1">
                                <asp:TextBox ID="descripcion" runat="server" CssClass="form-control" Width="96%" MaxLength="100"></asp:TextBox></td>
                            <td align="left"></td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td align="center" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="fila1">
                            </td>
                            <td align="left" class="fila1" colspan="2">
                                </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="id_perfil" runat="server" Visible="False"></asp:TextBox>
                    <asp:HiddenField ID="hfConsulta" runat="server" /><asp:HiddenField ID="hfElimina" runat="server" />
                </td>
            </tr>
        </table>
    </div>
<%--        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Provider=SQLNCLI11;Data Source=DESKTOP-1K6DDSQ;Password=manager;User ID=sa" ProviderName="System.Data.OleDb"></asp:SqlDataSource>--%>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

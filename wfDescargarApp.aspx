<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfDescargarApp.aspx.cs" Inherits="wfDescargarApp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Administración de usuarios</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
    <style type="text/css">
        .auto-style1 {
            height: 24px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <div>
                <table style="width: 99%" >
                    <tr>
                        <td align="center">
                            <h4>
                                <asp:Label ID="Label2" runat="server" Text="Descarga App Mobile Turnos"></asp:Label>
                            </h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table  style="width: 100%">
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="auto-style1">
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="Label4" runat="server"  Font-Size="Medium">Descargue la aplicación en el siguiente link y siga las instrucciones en su teléfono Android</asp:Label></td>
                                    <td class="auto-style1">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <h3>
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl=".\app\ComfandiAppTurnos.apk">DESCARGAR AQUÍ LA APLICACIÓN</asp:HyperLink>
                                        </h3>
                                        </td>
                                </tr>
                                <tr>
                                    <td class="auto-style1">
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="Label1" runat="server"  Font-Size="Medium">Utilice este código QR en sus oficinas para que los usuarios descarguen la aplicación</asp:Label></td>
                                    <td class="auto-style1">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/app/QRAppComfandi.png" />
                                        </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label3" runat="server"  Font-Bold="True" Text="Códigos QR de confirmación de turnos en las oficinas"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvOficinas" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvOficinas_RowDataBound" CssClass="table border-dark table-hover">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="id" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField HeaderText="Texto QR Confirmación de turnos en oficinas" />
                                    <asp:BoundField HeaderText="QR Code" />
                                </Columns>
                                <RowStyle CssClass="border-dark table-active"/>
                                <HeaderStyle CssClass="border-dark table-info text-center" />
                                <AlternatingRowStyle CssClass="border-dark" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

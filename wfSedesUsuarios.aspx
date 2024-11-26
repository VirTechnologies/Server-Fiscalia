<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfSedesUsuarios.aspx.cs" Inherits="wfSedesUsuarios" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administración de Seccional</title>
    <script language='javascript' src='utiles.js'></script>
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css"/>
</head>
<body runat="server" id="body">
    <form id="form1" runat="server">
        <div>
            <table border="0" cellpadding="0" cellspacing="0" width="98%">
                <tr>
                    <td align="left" colspan="2" style="height: 16px">
                        <h4>
                            <asp:Label ID="Label1" runat="server" Text="ADMINISTRACIÓN DE USUARIOS"></asp:Label>
                        </h4>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 66px">
                        <table width="100%">
                            <tr>
                                <td align="left" style="width: 20%">
                                    <asp:Button ID="btnSalir" runat="server" CssClass="btn btn-outline-info" CausesValidation="False" Text="VOLVER" OnClick="btnSalir_Click" /></td>
                                <td align="right" style="width: 60%"/>
                                    <td align="left" style="width: 40%"></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20%"></td>
                                <td align="right" style="width: 60%">
                                    <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR SECCIONAL" OnClick="btnGrabar_Click" />&nbsp;
                                <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-danger" Text="ELIMINAR SECCIONAL" OnClick="btnEliminar_Click" /></td>
                                <td align="left" style="width: 40%"></td>
                            </tr>
                            <tr>
                                <td align="left"></td>
                                <td align="left" style="width: 60%; height: 30px">
                                    <div id="notificacion" style="width: 80%" runat="server" visible="false" class="alert alert-dismissible">
                                        <button type="button" class="close" data-dismiss="alert">&times;</button>
                                        <strong>
                                            <asp:Label ID="lbConfirmacion" runat="server"></asp:Label>
                                        </strong>
                                    </div>
                                </td>
                                <td align="left" style="width: 40%"></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 20%"></td>
                                <td align="left" style="width: 60%; height: 30px"></td>
                                <td align="left" style="width: 40%"></td>
                            </tr>
                    <asp:GridView ID="gvtematicas" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Descripcion" HeaderText="SEDE" />
                            <asp:BoundField DataField="Nombre" HeaderText="SECCIONAL" />
                        </Columns>
                        <RowStyle CssClass="border-dark table-active"/>
                        <HeaderStyle CssClass="border-dark table-info text-center" />
                        <AlternatingRowStyle CssClass="border-dark" />
                    </asp:GridView>

                            <tr>
                                <td align="left" class="fila1">&nbsp;</td>
                                <td align="left" class="fila1" colspan="2">&nbsp;</td>
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
              <asp:Label ID="lblSinRegistros" runat="server" Text="No se encontraron registros!" CssClass="badge badge-info" Visible="False"></asp:Label>
        </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

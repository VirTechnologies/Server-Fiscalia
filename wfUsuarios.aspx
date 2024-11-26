<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfUsuarios.aspx.cs" Inherits="wfUsuarios" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Administración de usuarios</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <div>
                <table style="width: 99%" >
                    <tr>
                        <td colspan="3">
                            <h4>
                                <asp:Label ID="Label2" runat="server" Text="ADMINISTRACIÓN DE USUARIOS"></asp:Label>
                            </h4>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table  style="width: 100%">
                                <tr>
                                    <td style="width: 20%" align="right">
                                        <asp:Label ID="Label1" runat="server" Text="NOMBRE"></asp:Label></td>
                                    <td style="width: 40%" class="fila1">
                                        <asp:TextBox ID="Nombre" runat="server" CssClass="form-control" MaxLength="50" Width="97%"></asp:TextBox></td>
                                    <td class="fila1" style="width: 40%">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="L_Login" runat="server" Text="LOGIN"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="tblogin" CssClass="form-control" runat="server" MaxLength="20" Width="97%"></asp:TextBox></td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label3" runat="server" Text="PERFIL"></asp:Label></td>
                                    <td class="fila1">
                                        <asp:DropDownList ID="ddl_perfil" runat="server" CssClass="custom-select" Width="97%">
                                        </asp:DropDownList></td>
                                    <td class="fila1">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        </td>
                                    <td align="left">
                                        <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="CONSULTAR USUARIOS" OnClick="btnFiltrar_Click" /></td>
                                    <td>
                                        <asp:Button ID="btnAgregar" runat="server" CssClass="btn btn-primary" Text="AGREGAR USUARIO" OnClick="btnAgregar_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvUsuarios_RowDataBound" CssClass="table border-dark table-hover" OnRowCommand="gvUsuarios_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="id_usuario" HeaderText="ID"/>
                                    <asp:BoundField DataField="nombre" HeaderText="NOMBRE" />
                                    <asp:BoundField DataField="perfil" HeaderText="PERFIL" />
                                    <asp:BoundField DataField="login" HeaderText="LOGIN" />
                                    <asp:BoundField DataField="estado" HeaderText="ESTADO" />
                                    <asp:ButtonField Text="Consultar" HeaderText="CONSULTAR">
                                    </asp:ButtonField>
                                    <asp:ButtonField Text="Editar" HeaderText="EDITAR">
                                    </asp:ButtonField>
                                    <asp:ButtonField Text="Eliminar" HeaderText="ELIMINAR" Visible="false"/>
                                </Columns>
                                <RowStyle CssClass="border-dark table-active"/>
                                <AlternatingRowStyle CssClass="border-dark" />
                                <HeaderStyle CssClass="border-dark table-info text-center" />
                            </asp:GridView>
                            <asp:Label ID="lblNoRegistros" runat="server" CssClass="badge badge-info" Text="No. de registros: " Visible="false"></asp:Label>
                            <asp:Label ID="tbNoRegistros" runat="server" CssClass="badge badge-primary rounded-pill" Visible="false"></asp:Label>
                            <asp:Label ID="lblSinRegistros" runat="server" Text="No se encontraron registros!" CssClass="badge badge-info" Visible="False"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txSQL" runat="server" Visible="False"></asp:TextBox><asp:TextBox
                                ID="id_usuario" runat="server" Visible="False"></asp:TextBox></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

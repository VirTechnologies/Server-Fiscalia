﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfCanales.aspx.cs" Inherits="wfCanales" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administración de canales de atención</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 99%">
            <tr>
                <td colspan="3">
                    <h4>
                       <asp:Label ID="lTitulo" runat="server" Text="ADMINISTRACIÓN DE CANALES DE ATENCIÓN"></asp:Label>
                    </h4>

                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table style="width: 100%" >
                        <tr>
                            <td style="width: 20%" align="right"  class="fila1">
                                <asp:Label ID="Label1" runat="server" Text="CANAL"></asp:Label></td>
                            <td style="width: 40%" class="fila1">
                                <asp:TextBox ID="tbNombre" runat="server" CssClass="form-control" MaxLength="50" Width="97%"></asp:TextBox></td>
                            <td class="fila1" style="width: 40%">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 125px">
                                </td>
                            <td>
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="CONSULTAR CANALES" OnClick="btnFiltrar_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnAgregar" runat="server" CssClass="btn btn-primary" Text="AGREGAR UN CANAL" OnClick="btnAgregar_Click" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gvCanales" runat="server" AutoGenerateColumns="False" Width="100%" OnRowCommand="gvCanales_RowCommand" OnRowDataBound="gvCanales_RowDataBound" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="id" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:ButtonField Text="Consultar" HeaderText="Consultar" >
                            </asp:ButtonField>
                            <asp:ButtonField Text="Editar" HeaderText="Editar" >
                            </asp:ButtonField>
                        </Columns>
                        <RowStyle CssClass="border-dark table-active"/>
                        <HeaderStyle CssClass="border-dark table-info text-center" />
                        <AlternatingRowStyle CssClass="border-dark" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblNoRegistros" runat="server" CssClass="badge badge-info" Text="No. de registros: " Visible="false"></asp:Label>
                    <asp:Label ID="tbNoRegistros" runat="server" CssClass="badge badge-primary rounded-pill" Visible="false"></asp:Label>
                    <asp:Label ID="lblSinRegistros" runat="server" Text="No se encontraron registros!" CssClass="badge badge-info" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:TextBox ID="txSQL" runat="server" Visible="False"></asp:TextBox>
                </td>
                <td style="width: 100px">
                </td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
      </div>
    </form>
</body>
</html>

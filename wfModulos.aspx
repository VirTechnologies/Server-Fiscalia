<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfModulos.aspx.cs" Inherits="wfModulos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Módulos</title>
    <script language='javascript' type="text/javascript" src='utiles.js'></script>
<!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <table width="100%">
                <tr>
                    <td colspan="3">
                        <h4>
                            <asp:Label ID="Label1" runat="server" Text="ADMINISTRACIÓN DE MÓDULOS"></asp:Label>
                        </h4>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%" align="right">
                        <asp:Label ID="Label6" runat="server" Text="NOMBRE DEL MÓDULO"></asp:Label>
                    </td>
                    <td style="width: 40%" class="fila1">
                        <asp:TextBox ID="tbnombre_modulo" runat="server" CssClass="form-control" Width="97%"></asp:TextBox></td>
                    <td class="fila1" style="width: 40%"></td>
                </tr>
                <tr>
                    <td style="width: 125px"></td>
                    <td class="trnotitulo" style="width: 20%">
                        <asp:Button ID="bConsultar" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                            Text="CONSULTAR" OnClick="bConsultar_Click" />
                    </td>
                    <td class="trnotitulo">
                        <asp:Button ID="bAgregar" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                            Text="AGREGAR MÓDULO" OnClick="bAgregar_Click" /></td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:GridView ID="gvModulos" runat="server" AllowPaging="True" AutoGenerateColumns="False" Width="100%" AllowSorting="true"
                            PageSize="25" CssClass="table border-dark table-hover" OnPageIndexChanged="gvModulos_PageIndexChanged" OnPageIndexChanging="gvModulos_PageIndexChanging" OnRowDataBound="gvModulos_RowDataBound" OnRowCommand="gvModulos_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="id_modulo" HeaderText="id_modulo" />
                                <asp:BoundField DataField="modulo_padre" HeaderText="MÓDULO PADRE" />
                                <asp:BoundField DataField="modulo" HeaderText="MÓDULO" />
                                <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN" />
                                <asp:BoundField DataField="pagina" HeaderText="PÁGINA">
                                    <ItemStyle Width="100px" />
                                    <HeaderStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="orden" HeaderText="ORDEN" />
                                <asp:ButtonField Text="Consultar" HeaderText="CONSULTAR" />
                                <asp:ButtonField Text="Administrar" HeaderText="ADMINISTRAR" />
                            </Columns>
                            <RowStyle CssClass="border-dark table-active"/>
                            <HeaderStyle CssClass="border-dark table-info text-center" />
                            <AlternatingRowStyle CssClass="border-dark" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td class="trnotitulo" colspan="2" style="height: 16px">
                    <asp:Label ID="lblNoRegistros" runat="server" CssClass="badge badge-info" Text="No. de registros: " Visible="false"></asp:Label>
                    <asp:Label ID="tbNoRegistros" runat="server" CssClass="badge badge-primary rounded-pill" Visible="false"></asp:Label>
                    <asp:Label ID="lblSinRegistros" runat="server" Text="No se encontraron registros!" CssClass="badge badge-info" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="trnotitulo" colspan="2">
                        <asp:TextBox ID="txSQL" runat="server" Visible="False"></asp:TextBox>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>

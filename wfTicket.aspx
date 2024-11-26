<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfTicket.aspx.cs" Inherits="wfTicket" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edición de Ticket de impresión</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css" />
</head>
<body runat="server" id="body">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="98%" >
            <tr>
                <td align="left" colspan="2" style="height: 16px">
                    <h4><asp:Label ID="Label1" runat="server" Text="CONFIGURACIÓN DE TICKET IMPRESO"></asp:Label></h4>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Button ID="btnSalir" runat="server" CssClass="btn btn-outline-info" Text="VOLVER" OnClick="btnSalir_Click" UseSubmitBehavior="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr id="FilaBotones" runat="server" visible="false">
                            <td align="right">
                                <asp:Button ID="btnAgregarCampo" runat="server" CssClass="btn btn-primary" Text="AGREGAR CAMPO" OnClick="btnAgregar_Click" UseSubmitBehavior="false" />&nbsp;
                                <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR TICKET" OnClick="btnGrabar_Click" UseSubmitBehavior="false" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" style="width: 60%; height: 30px">
                                <div id="notificacion" style="width: 80%" runat="server" visible="false" class="alert alert-dismissible">
                                    <button type="button" class="close" data-dismiss="alert">&times;</button>
                                    <strong>
                                        <asp:Label ID="lbConfirmacion" runat="server"></asp:Label>
                                    </strong>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="gvCampos" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table border-dark table-hover small"
                                    OnRowDataBound="gvCampos_RowDataBound" OnRowCommand="gvCampos_RowCommand" OnRowUpdating="gvCampos_RowUpdating">
                                    <Columns>
                                        <asp:ButtonField Text="SingleClick" CommandName="SingleClick" Visible="False"/>
                                        <asp:BoundField DataField="idCampo" HeaderText="id" />
                                        <asp:BoundField DataField="orden" HeaderText="ORDEN" />
                                        <asp:BoundField DataField="obligatorio" HeaderText="OBLIGATORIO" />
                                        <asp:BoundField DataField="tipo" HeaderText="TIPO" />
                                        <asp:BoundField DataField="nombre" HeaderText="NOMBRE" />
                                        <asp:TemplateField HeaderText="VALOR IMPRESO">
                                            <ItemTemplate>
                                                <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("contenido") %>'></asp:Label>
                                                <asp:TextBox ID="Description" runat="server" Text='<%# Eval("contenido") %>' Width="175px" visible="false" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TAMAÑO">
                                            <ItemTemplate>
                                                <asp:Label ID="tamanoFuenteLabel" runat="server" Text='<%# Eval("tamanoFuente") %>'></asp:Label>
                                                <asp:TextBox ID="tamanoFuente" runat="server" Text='<%# Eval("tamanoFuente") %>' Width="175px" visible="false"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FUENTE">
                                            <ItemTemplate>
                                                <asp:Label ID="FuenteLabel" runat="server" Text='<%# Eval("tipoFuente") %>'></asp:Label>
                                                <asp:TextBox ID="tipoFuente" runat="server" Text='<%# Eval("tipoFuente") %>' Width="175px" visible="false"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ALINEACIÓN">
                                            <ItemTemplate>
                                                <asp:Label ID="LblAlineacion" runat="server" Text='<%# Eval("align") %>'></asp:Label>
                                                <asp:DropDownList ID="align" DataValueField="align" runat="server" Visible="false" AutoPostBack="true" CausesValidation="true">
                                                    <asp:ListItem Value="Izquierda" Text="IZQUIERDA"></asp:ListItem>
                                                    <asp:ListItem Value="Centro" Text="CENTRO"></asp:ListItem>
                                                    <asp:ListItem Value="Derecha" Text="DERECHA"></asp:ListItem>
                                                    <asp:ListItem Value="Justificado" Text="JUSTIFICADO"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
            <%--                            <asp:BoundField DataField="estilo" HeaderText="Estilo" />--%>
                                        <asp:TemplateField HeaderText="ACCIONES" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                        <asp:Button ID="btnUp" CommandName="Up" ToolTip="Subir Campo" Text=" &#916; " class="btn btn-primary btn-sm" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                        <asp:Button ID="btnDown" CommandName="Down" ToolTip="Bajar Campo" Text=" &#8711; " class="btn btn-primary btn-sm" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                        <asp:Button ID="btnBorrar" CommandName="Borrar" ToolTip="Borrar" Text=" X " class="btn btn-primary btn-sm"  runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                        </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <RowStyle CssClass="border-dark table-active"/>
                                    <HeaderStyle CssClass="border-dark table-info text-center" />
                                    <AlternatingRowStyle CssClass="border-dark" />
                                </asp:GridView>
                            </td>
                            <td align="left" style="width: 20%">&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfPrioridades.aspx.cs" Inherits="wfPrioridades" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ADMINISTRACIÓN DE PRIORIDADES</title>
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
                            <asp:Label ID="lTitulo" runat="server" Text="ADMINISTRACIÓN DE PRIORIDADES"></asp:Label>
                        </h4>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 20%" align="right">
                                    <asp:Label ID="Label18" runat="server" Text="TEMÁTICA"></asp:Label></td>
                                <td style="width: 40%" class="fila1">
                                    <asp:DropDownList ID="ddltematica" runat="server" CssClass="custom-select" CausesValidation="false" AutoPostBack="true" OnSelectedIndexChanged="ddltematica_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="fila1" style="width: 40%"></td>
                            </tr>     
                            <tr>
                                <td style="width: 20%" align="right">
                                    <asp:Label ID="Label1" runat="server" Text="SECCIONAL"></asp:Label></td>
                                <td style="width: 40%" class="fila1">
                                    <asp:DropDownList ID="ddlOficinaId" runat="server" CssClass="custom-select" CausesValidation="false" AutoPostBack="true" OnSelectedIndexChanged="ddlOficinaId_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="fila1" style="width: 40%"></td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 20%">
                                    <asp:Label ID="Label15" runat="server" Text="SEDE*: "></asp:Label></td>
                                <td align="left" style="width: 40%">
                                    <asp:DropDownList ID="dllsala" Style="width: 80%" runat="server" CssClass="custom-select">
                                    </asp:DropDownList>
                                </td>
                                <td align="left" style="width: 40%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 125px"></td>
                                <td>
                                    <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="CONSULTAR PRIORIDADES" OnClick="btnFiltrar_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnAgregar" runat="server" CssClass="btn btn-primary" Text="AGREGAR PRIORIDAD" OnClick="btnAgregar_Click" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:GridView ID="gvPrioridades" runat="server" AutoGenerateColumns="False" Width="100%" OnRowCommand="gvPrioridades_RowCommand" OnRowDataBound="gvPrioridades_RowDataBound" CssClass="table border-dark table-hover">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="ID" />
                                <asp:BoundField DataField="tematica" HeaderText="TEMÁTICA" />
                                <asp:BoundField DataField="oficina" HeaderText="SECCIONAL" />
                                <asp:BoundField DataField="Descripcion" HeaderText="SEDE" />
                                <asp:BoundField DataField="perfildeatencion" HeaderText="PERFIL DE ATENCIÓN" />
                                <asp:BoundField DataField="abreviatura" HeaderText="ABREVIATURA" />
                                <asp:BoundField DataField="servicio" HeaderText="SERVICIO" />
                                <asp:BoundField DataField="tipodeatencion" HeaderText="TIPO DE ATENCIÓN" />
                                <asp:BoundField DataField="Ponderacion" HeaderText="PONDERACIÓN" />
                                <asp:BoundField DataField="CantMaxTurnos" HeaderText="CANTIDAD MAXIMA DE TURNOS"/>
                                <asp:BoundField DataField="Habilitado" HeaderText="HABILITADO" />
                                <asp:ButtonField Text="CONSULTAR" HeaderText="CONSULTAR"></asp:ButtonField>
                                <asp:ButtonField Text="EDITAR" HeaderText="EDITAR"></asp:ButtonField>
                            </Columns>
                            <RowStyle CssClass="border-dark table-active" />
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
                    <td style="width: 100px"></td>
                    <td style="width: 100px"></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

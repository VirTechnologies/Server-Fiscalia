<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfConfiguracionAgendamiento.aspx.cs" Inherits="wfPrioridades" %>

<!DOCTYPE html>
<style>
    .separadorBotones{
        width: 400px;
        display: flex;
        justify-content: flex-start;
    }

</style>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Configuración Agendamiento</title>
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
                        <asp:Label ID="lTitulo" runat="server" Text="CONFIGURACIÓN AGENDAMIENTO" ></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 20%" align="right">

                                <asp:Label ID="Label1" runat="server" Text="ESCOGER SECCIONAL "></asp:Label>
                            </td>
                            <td style="width: 40%" class="fila1">
                                <asp:DropDownList ID="ddlSeccionalId" runat="server" CssClass="custom-select" AutoPostBack="True" OnSelectedIndexChanged="ddlSeccionalId_SelectedIndexChanged"></asp:DropDownList>
                                <%--<asp:ListItem Text="Seleccione una opción" Value="" Selected="True" />--%>
                            </td>
                            <td class="fila1" style="width: 40%"></td>
                        </tr>
                        <tr>
                            <td style="width: 20%" align="right">
                                <asp:Label ID="Label2" runat="server" Text="ESCOGER SEDE "></asp:Label>
                            </td>
                            <td style="width: 40%" class="fila1">
                                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="custom-select" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td class="fila1" style="width: 40%"></td>
                        </tr>
                        <tr>
                            
                        </tr>
                    </table>
                    <asp:Label ID="LabelSetGlobal1" runat="server" Text="ESTABLECER TODOS LOS TURNOS DE LA SEDE " Visible="false"></asp:Label>
                    <asp:Label ID="labelSede" runat="server" Text="" Visible="false" Font-Bold="True"></asp:Label>
                    <asp:Label ID="LabelSetGlobal" runat="server" Text=" en cero" Visible="false"></asp:Label>
                    

                    <div class="separadorBotones">
                        <asp:Button ID="btnFiltrarCero" runat="server" CssClass="btn btn-primary" Text="ACEPTAR" OnClick="btnFiltrarCero_Click" Style="display:none;" />
                      
                    </div>
                   
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
            </tr>
            <tr>
               <td colspan="3">
                    <asp:GridView ID="gvPrioridades" runat="server" AutoGenerateColumns="False" Width="100%" OnRowCommand="gvPrioridades_RowCommand" OnRowDataBound="gvPrioridades_RowDataBound" CssClass="table border-dark table-hover">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenID_Agendamiento" runat="server" Value='<%# Eval("ID_Agendamiento") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Nombre_Sede" HeaderText="NOMBRE DE SEDE" />
                        <asp:BoundField DataField="Nombre_Dia" HeaderText="NOMBRE DE DÍA" />
                        <asp:BoundField DataField="TurnosPorFranja" HeaderText="TURNOS POR FRANJA" />
                        <asp:BoundField DataField="HorarioFranja" HeaderText="HORARIO DE FRANJA" />
                        <asp:ButtonField Text="Editar Turnos" HeaderText="EDITAR TURNOS" CommandName="EDITARTURNOS" ButtonType="Link" />
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

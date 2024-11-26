<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfGrupoOficinas.aspx.cs" Inherits="wfGrupoOficinas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ADMINISTRACIÓN DE TEMÁTICAS Y SECCIONALES</title>
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
                        <asp:Label ID="lTitulo" runat="server" Text="ADMINISTRACIÓN DE TEMÁTICAS Y SECCIONALES" ></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table style="width: 100%" >
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label1" runat="server" Text="TEMÁTICA"></asp:Label></td> 
                            <td>
                                <asp:DropDownList ID="ddlGrupoId" AutoPostBack="true" runat="server" CssClass="custom-select" style="width: 60%" OnSelectedIndexChanged="ddlGrupoId_SelectedIndexChanged" >
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlGrupoId" ErrorMessage="Seleccione el grupo"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 20%">&nbsp;</td>
                        </tr>
                        <tr style="display:none;">
                            <td style="width: 20%">&nbsp;</td>
                            <td>
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="CONSULTAR SECCIONAL" OnClick="btnFiltrar_Click" />
                            </td>
                            <td align="left" style="width: 20%">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                    </table>
                    </td>
            </tr>
            <tr>
                <td style="width: 20%">&nbsp;</td>
                <td align="left" >
                    <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Visible="false" Text="ASIGNAR SECCIONAL A LA TEMÁTICA" OnClick="btnGrabar_Click" />
                </td>
                <td style="width: 20%">&nbsp;</td>
            </tr>
            <tr>
                <td align="left">
                </td>
                <td align="left" style="width: 60%; height: 30px">
                    <div id="notificacion" style="width: 60%" runat="server" visible="false" class="alert alert-dismissible">
                        <button type="button" class="close" data-dismiss="alert">&times;</button>
                        <strong>
                            <asp:Label ID="lbConfirmacion" runat="server"></asp:Label>
                        </strong>
                    </div>
                </td>
                <td align="left" style="width: 20%">
                </td>
            </tr>
			<tr>
                <td colspan="3">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gvGrupoOficinas" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvOficinas_RowDataBound" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Id_Grupo" HeaderText="ID" />
                            <asp:BoundField DataField="Id" HeaderText="ID SECCIONAL" />
                            <asp:BoundField DataField="Nombre" HeaderText="NOMBRE" />
                            <asp:BoundField DataField="Ubicacion" HeaderText="UBICACIÓN" />
                            <asp:TemplateField > 
<%--                                <HeaderTemplate>
                                    <asp:CheckBox ID="checkAll" Text="Seleccionar todas " TextAlign="Left" runat="server" onclick="checkAll(this);" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox name="CheckBoxOficina" runat="server" />
                                </ItemTemplate>--%>
                            </asp:TemplateField>
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
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js">
    </script>--%>
    <script type="text/javascript">
        function checkAll(data) {
            $('[id~=checkAll]').click(function() {
                $("[name~='CheckBoxOficina']").attr('checked', true);
            });     
            $('[id$=RejAll]').click(function() {
                $("[id$='CheckReject']").attr('checked', false);
            });
        }
    </script>


</body>
</html>

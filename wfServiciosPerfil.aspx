    <%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfServiciosPerfil.aspx.cs" Inherits="wfServiciosPerfil" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administración de servicios</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
    <style type="text/css">
        .auto-style1 {
            background: #F8F7F3;
            width: 20%;
            height: 18px;
        }
        .auto-style2 {
            background: #F8F7F3;
            width: 40%;
            height: 18px;
        }
        .auto-style3 {
            height: 33px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 99%">
            <tr>
                <td colspan="3">
                    <h4>
                        <asp:Label ID="lTitulo" runat="server" Text="ADMINISTRACIÓN DE SERVICIOS DE PERFILES DE SECCIONAL" ></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table style="width: 100%" >
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label1" runat="server" Text="Seleccione un perfil"></asp:Label></td>
                            <td>
                                <asp:DropDownList ID="ddlIdPerfil" runat="server" CssClass="form-control" Width="60%" MaxLength="100" AutoPostBack="True" OnSelectedIndexChanged="ddlIdPerfil_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            <td>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlIdPerfil" ErrorMessage="Seleccione un perfil"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR SERVICIOS DEL PERFIL SELECCIONADO" OnClick="btnGrabar_Click"/>
                            </td>
                        </tr>
                    </table>
                    </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
            </tr>
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
                <td align="left">
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gvServicios" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvServicios_RowDataBound" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="id" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="serviciopadre" HeaderText="ServicioPadre" />
                            <asp:TemplateField HeaderText="Seleccionar"><ItemTemplate><asp:Checkbox ID="CheckBox_sel" runat="server" /></ItemTemplate></asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="border-dark table-active"/>
                        <HeaderStyle CssClass="border-dark table-info text-center" />
                        <AlternatingRowStyle CssClass="border-dark" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="auto-style3">
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
            <tr>
                <td style="width: 100px">
                    &nbsp;</td>
                <td style="width: 100px">
                    &nbsp;</td>
                <td style="width: 100px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Table ID="tablaServicios" runat="server" Width="100%" CssClass="table table-bordered">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td style="width: 100px">
                    &nbsp;</td>
                <td style="width: 100px">
                    &nbsp;</td>
                <td style="width: 100px">
                    &nbsp;</td>
            </tr>
        </table>
      </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

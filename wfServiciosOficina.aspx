<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfServiciosOficina.aspx.cs" Inherits="wfServiciosOficina" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administración de servicios</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
    <style type="text/css">
        .auto-style1 {
            height: 26px;
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
                <td colspan="2">
                    <h4>
                        <asp:Label ID="lTitulo" runat="server" Text="Administración de servicios por Seccional" ></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table style="width: 100%" >
                        <tr>
                            <td style="width: 20%" align="right">
                                <asp:Label ID="Label1" runat="server" Text="Seleccione un Seccional"></asp:Label></td>
                            <td class="auto-style1">
                                <asp:DropDownList ID="ddlIdOficina" runat="server" CssClass="form-control" Width="80%" MaxLength="100" AutoPostBack="True" OnSelectedIndexChanged="ddlIdOficina_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td class="fila1" style="width: 40%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlIdOficina" ErrorMessage="Seleccione el Seccional"></asp:RequiredFieldValidator>
                            </td>
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
                            <td align="left" style="width: 40%">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <h4>
                                    <asp:Label ID="lTitulo0" runat="server" Text="Servicios en Seccional" ></asp:Label>
                                </h4>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                </td>
                            <td class="auto-style1">
                                <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR SERVICIOS" OnClick="btnGrabar_Click" />
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                    </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvServicios" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvServicios_RowDataBound" OnRowCommand="gvServicios_RowCommand" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="id" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Abreviatura" HeaderText="Abreviatura" />
                            <asp:BoundField DataField="serviciopadre" HeaderText="ServicioPadre" />
                            <asp:ButtonField Text="Editar" HeaderText="Editar" />
                            <asp:TemplateField HeaderText="Seleccionar"><ItemTemplate><asp:Checkbox ID="CheckBox_sel" runat="server" /></ItemTemplate></asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="border-dark table-active"/>
                        <HeaderStyle CssClass="border-dark table-info text-center" />
                        <AlternatingRowStyle CssClass="border-dark" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblNoRegistros" runat="server" CssClass="badge badge-info" Text="No. de registros: " Visible="false"></asp:Label>
                    <asp:Label ID="tbNoRegistros" runat="server" CssClass="badge badge-primary rounded-pill" Visible="false"></asp:Label>
                    <asp:Label ID="lblSinRegistros" runat="server" Text="No se encontraron registros!" CssClass="badge badge-info" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td >
                    <asp:TextBox ID="txSQL" runat="server" Visible="False"></asp:TextBox>
                </td>
                <td >
                </td>
            </tr>
            <tr>
                <td >
                    &nbsp;</td>
                <td >
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Table ID="tablaServicios" runat="server" Width="100%" CssClass="table table-bordered">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td class="auto-style1" >
                    </td>
                <td class="auto-style1" >
                    </td>
            </tr>
            <tr>
                <td class="auto-style1" colspan="2" >
                    <table style="width: 100%" >
                        <tr>
                            <td style="width: 20%" class="fila1">
                                </td>
                            <td>
                                </td>
                            <td>
                                </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <h4>
                                    <asp:Label ID="lTitulo1" runat="server" Text="Perfiles en Seccional" ></asp:Label>
                                </h4>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                            </td>
                            <td align="left" style="width: 40%">
                                <asp:Button ID="btnGrabarPerfiles" runat="server" CssClass="btn btn-primary" Text="GRABAR PERFILES" OnClick="btnGrabarPerfiles_Click"/>
                            </td>
                            <td align="left" style="width: 60%; height: 30px">
                                <div id="notificacionP" style="width: 80%" runat="server" visible="false" class="alert alert-dismissible">
                                    <button type="button" class="close" data-dismiss="alert">&times;</button>
                                    <strong>
                                        <asp:Label ID="lbConfirmacionP" runat="server"></asp:Label>
                                    </strong>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="3">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                    <asp:GridView ID="gvPerfiles" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvPerfiles_RowDataBound" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="id" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:TemplateField HeaderText="Seleccionar"><ItemTemplate><asp:Checkbox ID="CheckBox_selP" runat="server" /></ItemTemplate></asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="border-dark table-active"/>
                        <HeaderStyle CssClass="border-dark table-info text-center" />
                        <AlternatingRowStyle CssClass="border-dark" />
                    </asp:GridView>
                                </td>
                        </tr>
                        <tr>
                            <td class="auto-style3" colspan="3">
                            <asp:Label ID="lblNoRegistrosP" runat="server" CssClass="badge badge-info" Text="No. de registros: " Visible="false"></asp:Label>
                            <asp:Label ID="tbNoRegistrosP" runat="server" CssClass="badge badge-primary rounded-pill" Visible="false"></asp:Label>
                            <asp:Label ID="lblSinRegistrosP" runat="server" Text="No se encontraron registros!" CssClass="badge badge-info" Visible="False"></asp:Label>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                    </td>
            </tr>
            <tr>
                <td >
                    &nbsp;</td>
                <td >
                    &nbsp;</td>
            </tr>
        </table>
      </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

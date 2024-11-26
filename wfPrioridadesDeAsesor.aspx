<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfPrioridadesDeAsesor.aspx.cs" Inherits="wfPrioridadesDeAsesor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administración de prioridades de asesor</title>
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
                        <asp:Label ID="lTitulo" runat="server" Text="ADMINISTRACIÓN DE PRIORIDADES DE USUARIO" ></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table style="width: 100%" >
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label1" runat="server" Text="USUARIO"></asp:Label></td>
                            <td style="width: 50%" class="fila1">
                                <asp:DropDownList ID="ddlUsuarioId" runat="server" CssClass="custom-select" >
                                </asp:DropDownList>
                                
                            </td>
                            <td><asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlUsuarioId" ErrorMessage="Seleccione el asesor"></asp:RequiredFieldValidator>&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label2" runat="server" Text="SECCIONAL"></asp:Label></td>
                            <td style="width: 50%" class="fila1">
                                <asp:DropDownList ID="ddlOficina" runat="server" CssClass="custom-select" CausesValidation="false" AutoPostBack="true" OnSelectedIndexChanged="ddlOficinaId_SelectedIndexChanged">
                                </asp:DropDownList>
                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label3" runat="server" Text="SALA DE ATENCIÓN"></asp:Label></td>
                            <td style="width: 50%" class="fila1">
                                <asp:DropDownList ID="ddlSalas" runat="server" CssClass="custom-select" >
                                </asp:DropDownList>
                                
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 20%">&nbsp;</td>
                            <td>
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="CONSULTAR PRIORIDADES" OnClick="btnFiltrar_Click" />
                            </td>
                            <td align="left">
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
                    <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Visible="false" Text="GRABAR PRIORIDADES USUARIO" OnClick="btnGrabar_Click" />
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
                    <asp:GridView ID="gvPrioridades" runat="server" AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvPrioridades_RowDataBound" CssClass="table border-dark table-hover">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="id" />
                            <asp:BoundField DataField="oficina" HeaderText="SECCIONAL" />
                            <asp:BoundField DataField="Descripcion" HeaderText="SEDE" />
                            <asp:BoundField DataField="perfildeatencion" HeaderText="PERFIL DE ATENCIÓN" />
                            <asp:BoundField DataField="abreviatura" HeaderText="ABREVIATURA" />
                            <asp:BoundField DataField="servicio" HeaderText="SERVICIO" />
                            <asp:BoundField DataField="tipodeatencion" HeaderText="TIPO DE ATENCIÓN" />
                            <asp:BoundField DataField="Ponderacion" HeaderText="PONDERACIÓN" />
                            <asp:BoundField HeaderText="HABILITADO" />
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
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfEncuestaPregunta.aspx.cs" Inherits="wfEncuestaPregunta" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edición de preguntas de encuestas</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">

    <style type="text/css">
        .auto-style1 {
            height: 84px;
        }
    </style>
</head>
<body runat="server" id="body">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="98%" >
            <tr>
                <td align="left" colspan="2" style="height: 16px">
                    <h4>
                    <asp:Label ID="Label1" runat="server" Text="Administración de preguntas de encuestas"></asp:Label>
                    </h4>
                 </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 66px">
                    <table width="100%">
                        <tr>
                            <td align="left" style="width: 20%">
                                <asp:Button ID="btnSalir" runat="server" CssClass="btn btn-outline-info" CausesValidation="False" Text="VOLVER" OnClick="btnSalir_Click" /></td>
                            <td align="right" style="width: 60%">
                                <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR PREGUNTA" OnClick="btnGrabar_Click" /> <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-danger" Text="ELIMINAR PREGUNTA" CausesValidation="False" OnClick="btnEliminar_Click" /></td>
                            <td align="left" style="width: 40%">
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
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label8" runat="server" Text="Texto pregunta*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbdescripcion" runat="server" CssClass="form-control" Width="80%" MaxLength="200"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbdescripcion"
                                    ErrorMessage="¡Debe escribir el texto de la pregunta!"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                        <td align="right" style="width: 20%">
                            <asp:Label ID="Label10" runat="server" Text="Tipo pregunta*: "></asp:Label></td>
                        <td align="left" style="width: 40%">
                            <asp:DropDownList ID="ddltipo_pregunta" CssClass="custom-select" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="0">Con respuesta única</asp:ListItem>
                                <asp:ListItem Value="1">Con múltiples respuestas</asp:ListItem>
                            </asp:DropDownList>
                            </td>
                            <td align="left" style="width: 40%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddltipo_pregunta"
                                    ErrorMessage="¡Seleccione el tipo de pregunta!"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label2" runat="server" Text="Orden*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tborden" runat="server" CssClass="form-control" Width="21%" MaxLength="100"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator1" CssClass="text-danger" runat="server" ControlToValidate="tborden" ErrorMessage="Debe ser numérico" MaximumValue="1000" MinimumValue="1" Type="Integer"></asp:RangeValidator>
                                </td>
                            <td align="left" style="width: 40%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="tborden"
                                    ErrorMessage="¡Debe escribir el orden de la pregunta!"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                &nbsp;</td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table id="tablaOpciones" runat="server" style="width:100%; visibility: visible;">
                                    <tr>
                                        <td colspan="3">
                                            <h5>

                        <asp:Label ID="Label9" runat="server" Text="Opciones de la pregunta"></asp:Label>
                                            </h5>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%"></td>
                                        <td >
                                <asp:Button ID="btnAgregarOpcion" runat="server" CssClass="btn btn-primary" Text="Agregar opción de respuesta" OnClick="btnAgregarOpcion_Click"/></td>
                                        <td style="width: 40%"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView ID="gvPreguntasOpciones" runat="server" AutoGenerateColumns="False" Width="100%" OnRowCommand="gvPreguntasOpciones_RowCommand" OnRowDataBound="gvPreguntasOpciones_RowDataBound" CssClass="table border-dark table-hover">
                                                <Columns>
                                                    <asp:BoundField DataField="Id" HeaderText="id" />
                                                    <asp:BoundField DataField="descripcion_opcion" HeaderText="Opción" />
                                                    <asp:BoundField DataField="orden" HeaderText="Orden" />
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
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="fila1">
                                &nbsp;</td>
                            <td align="left" class="fila1" colspan="2">
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="auto-style1">
                    <asp:HiddenField ID="hfConsulta" runat="server" />
                    <asp:HiddenField ID="hfid" runat="server" />
                    <asp:HiddenField ID="hfEncuestaId" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

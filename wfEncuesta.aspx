<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfEncuesta.aspx.cs" Inherits="wfEncuesta" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edición de encuesta</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css" />
	<link rel="stylesheet" type="text/css" href="datetimepicker/bootstrap-datepicker.min.css" />
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css" />

    <script type="text/javascript" src="./datetimepicker/jquery.min.js"></script>
    <script type="text/javascript" src="./datetimepicker/bootstrap-datepicker.min.js"></script>
    <script type="text/javascript" src="./datetimepicker/bootstrap-datepicker.es.min.js"></script>

    <style type="text/css">
        .auto-style1 {
            height: 26px;
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
                        <asp:Label ID="Label1" runat="server" Text="ADMINISTRACIÓN DE ENCUESTA"></asp:Label>
                    </h4>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2"></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 66px">
                    <table width="100%">
                        <tr>
                            <td align="left" colspan="3">
                                <asp:Button ID="btnSalir" runat="server" CssClass="btn btn-outline-info" CausesValidation="False" Text="VOLVER" OnClick="btnSalir_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label3" runat="server" Text="NOMBRE ENCUESTA*: "></asp:Label>
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:TextBox ID="tbnombre_encuesta" runat="server" CssClass="form-control" Width="80%" MaxLength="100"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbnombre_encuesta"
                                    ErrorMessage="¡Debe escribir el nombre de la encuesta!"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label8" runat="server" Text="OBJETIVO*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbobjetivo" runat="server" CssClass="form-control" Width="80%" MaxLength="300" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td align="left" style="width: 40%">
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbobjetivo"
                                    ErrorMessage="¡Debe escribir el objetivo!"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label2" runat="server" Text="FECHA ENCUESTA*: "></asp:Label>
                            </td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbfecha_encuesta" CssClass="form-control" Width="120px" runat="server" style="display:inline;" autocomplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbfecha_encuesta"
                                    ErrorMessage="¡Debe escribir la fecha de la encuesta!"></asp:RequiredFieldValidator>
                                <div class="container">
                                    <div class="row">
                                        <script type="text/javascript">
                                            $('#tbfecha_encuesta').datepicker({
                                                clearBtn: true,
                                                language: "es",
                                                orientation: "bottom left",
                                                autoclose: true,
                                                todayHighlight: true,
                                                toggleActive: true
                                            });
                                        </script>
                                    </div>
                                </div>
                            </td>

                            <td align="left" style="width: 40%">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label4" runat="server" Text="FECHA REGISTRO: " ></asp:Label>
                            </td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbfecha_registro" runat="server"  CssClass="form-control" Width="120px" ReadOnly="True"></asp:TextBox>
                                </td>
                            <td align="left" style="width: 40%">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="fila1">
                                <asp:Label ID="Label7" runat="server" Text="ESTADO:"></asp:Label>
                            </td>
                            <td align="left" class="fila1">
                                <strong>
                                    <asp:Label ID="lblEstado" runat="server" class="text-muted" Text="INACTIVA"></asp:Label>
                                </strong> 

                            </td>
                            <td align="left" class="fila1">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                            </td>
                            <td align="left" style="width: 60%">
                                <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR ENCUESTA" OnClick="btnGrabar_Click" />
                                &nbsp;
                                <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-danger" Text="ELIMINAR ENCUESTA" OnClick="btnEliminar_Click" CausesValidation="False" /></td>
                            <td align="left" style="width: 20%">
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
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table id="tablaPreguntas" runat="server" style="width:100%; visibility: visible;">
                                    <tr>
                                        <td colspan="3">
                                            <h5>
                                                <asp:Label ID="Label9" runat="server" Text="Preguntas de la encuesta"></asp:Label>
                                            </h5>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%"></td>
                                        <td >
                                <asp:Button ID="btnAgregarPregunta" runat="server" CssClass="btn btn-primary" Text="Agregar pregunta" OnClick="btnAgregarPregunta_Click"/></td>
                                        <td style="width: 40%"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:GridView ID="gvPreguntas" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table border-dark table-hover" OnRowCommand="gvPreguntas_RowCommand" OnRowDataBound="gvPreguntas_RowDataBound"  >
                                                <Columns>
                                                    <asp:BoundField DataField="Id" HeaderText="id" />
                                                    <asp:BoundField DataField="descripcion" HeaderText="Descripción pregunta" />
                                                    <asp:BoundField DataField="tipo_pregunta" HeaderText="Tipo pregunta" />
                                                    <asp:BoundField DataField="orden" HeaderText="Orden" />
                                                    <asp:ButtonField Text="Consultar" HeaderText="Consultar"></asp:ButtonField>
                                                    <asp:ButtonField Text="Editar" HeaderText="Editar"></asp:ButtonField>
                                                </Columns>
                                                <HeaderStyle CssClass="border-dark table-info text-center" />
                                                <RowStyle CssClass="border-dark table-active"/>
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
                <td colspan="2">
                    <asp:HiddenField ID="hfConsulta" runat="server" />
                    <asp:HiddenField ID="hfid" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

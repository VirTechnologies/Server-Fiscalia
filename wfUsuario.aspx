<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfUsuario.aspx.cs" Inherits="wfUsuario" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Administración de usuario</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <table border="0" cellpadding="0" cellspacing="0" width="97%">
                <tr>
                    <td align="left" colspan="2" style="height: 16px">
                        <h4>
                            <asp:Label ID="Label1" runat="server"  Text="DATOS DEL USUARIO"></asp:Label>
                        </h4>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2" >
                        <table width="100%">
                            <tr>
                                <td align="left" style="width: 25%">
                                    <asp:Button ID="btnSalir" runat="server" CssClass="btn btn-outline-info" CausesValidation="False" Text="VOLVER" OnClick="btnSalir_Click" />
                                </td>
                                <td align="right" style="width: 45%">
                                    <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR" OnClick="btnGrabar_Click" />&nbsp;
                                    <asp:Button ID="btElimina" runat="server" CssClass="btn btn-danger" Text="ELIMINAR" Visible="False" OnClick="btElimina_Click" />
                                </td>
                                <td align="right" style="width: 30%">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 20%">
                    </td>
                    <td align="left" style="width: 60%; height: 30px">
                        <div id="notificacion" style="width: 80%" runat="server" visible="false" class="alert alert-dismissible">
                            <button type="button" class="close" data-dismiss="alert">&times;</button>
                            <strong>
                                <asp:Label ID="lbConfirmacion" runat="server"></asp:Label>
                            </strong>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" >
                        <table width="100%" >
                            <tr>
                                <td align="right" class="celdas1" style="width: 25%">
                                </td>
                                <td align="left" class="celdas1" style="width: 45%">
                               </td>
                                <td align="left" class="celdas1" style="width: 30%">
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 25%" class="celdas1">
                                    <asp:Label ID="Label2" runat="server" Text="NOMBRES Y APELLIDOS *: "></asp:Label></td>
                                <td align="left" class="celdas1" style="width: 45%">
                                    <asp:TextBox ID="nombre" runat="server" CssClass="form-control" Width="95%" MaxLength="200"></asp:TextBox>
                                    </td>
                                <td align="left" class="celdas1" style="width: 30%">
                                    <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="nombre"
                                        ErrorMessage="¡Debe escribir el nombre!"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td align="right" class="fila1">
                                    <asp:Label ID="Label13" runat="server" Text="NUMERO IDENTIFICACIÓN :"></asp:Label></td>
                                <td align="left" class="fila1">
                                    <asp:TextBox ID="numero_identificacion" runat="server" CssClass="form-control" Width="95%" MaxLength="30"></asp:TextBox></td>
                                <td align="left" >
                                    <asp:RangeValidator ID="RangeValidator2" runat="server" CssClass="text-danger" ControlToValidate="numero_identificacion" ErrorMessage="¡Debe ser numérico!" MaximumValue="9999999999" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="celdas1">
                                    <asp:Label ID="Label14" runat="server" Text="TELÉFONO :"></asp:Label></td>
                                <td align="left" class="celdas1">
                                    <asp:TextBox ID="tbTelefono" runat="server" CssClass="form-control" Width="95%" MaxLength="15"></asp:TextBox></td>
                                <td align="left" class="celdas1">
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="fila1">
                                    <asp:Label ID="Label18" runat="server" Text="DIRECCIÓN :"></asp:Label></td>
                                <td align="left" class="fila1">
                                    <asp:TextBox ID="tbDireccion" runat="server" CssClass="form-control" Width="95%" MaxLength="80"></asp:TextBox></td>
                                <td align="left" class="fila1">
                                    </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label8" runat="server" Text="EMAIL :"></asp:Label></td>
                                <td align="left">
                                    <asp:TextBox ID="tbemail" runat="server" CssClass="form-control" MaxLength="80" Width="95%"></asp:TextBox></td>
                                <td align="left">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbemail" CssClass="text-danger" ErrorMessage="Inválido" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
                            </tr>
                            <tr>
                                <td align="right" class="fila1">
                                    <asp:Label ID="Label11" runat="server" Text="CARGO :"></asp:Label></td>
                                <td align="left" class="fila1">
                                    <asp:TextBox ID="tbCargo" runat="server" CssClass="form-control" MaxLength="80" Width="95%"></asp:TextBox></td>
                                <td align="left" class="fila1">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="celdas1" style="height: 30px">
                                    </td>
                                <td align="left" class="celdas1">
                                    &nbsp;</td>
                                <td align="left" class="celdas1">
                                    </td>
                            </tr>
                            <tr>
                                <td align="right" class="fila1">
                                    <asp:Label ID="Label3" runat="server" Text="USUARIO O LOGIN *:"></asp:Label></td>
                                <td align="left" class="fila1">
                                    <asp:TextBox ID="tblogin" runat="server" MaxLength="20" CssClass="form-control"></asp:TextBox></td>
                                <td align="left" class="fila1">
                                    <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tblogin"
                                        ErrorMessage="¡Debe escribir el usuario!"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td align="right" class="celdas2">
                                    <asp:Label ID="Label5" runat="server" Text="CONTRASEÑA O PASSWORD *:"></asp:Label></td>
                                <td align="left" class="celdas2">
                                    <asp:TextBox ID="clave" runat="server" CssClass="form-control" TextMode="Password" MaxLength="20"></asp:TextBox>
                                    </td>
                                <td align="left" class="celdas2">
                                    <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator3" runat="server" ControlToValidate="clave"
                                        ErrorMessage="¡Debe escribir la contraseña!"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td align="right" class="fila1">
                                    <asp:Label ID="Label12" runat="server" Text="RECONFIRMAR CONTRASEÑA *: "></asp:Label></td>
                                <td align="left" class="fila1">
                                    <asp:TextBox ID="reconfirmar_clave" runat="server" CssClass="form-control" TextMode="Password" MaxLength="20"></asp:TextBox>
                                    </td>
                                <td align="left" class="fila1">
                                    <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator7" runat="server" ControlToValidate="reconfirmar_clave"
                                        ErrorMessage="¡No ha reconfirmado la contraseña!"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label15" runat="server" Text="DÍAS PARA CAMBIO DE CONTRASEÑA *: "></asp:Label></td>
                                <td align="left">
                                    <asp:TextBox ID="tbDias_Contrasena" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" CssClass="text-danger" ControlToValidate="tbDias_Contrasena" ErrorMessage="Debe ser numérico" MaximumValue="9999999" MinimumValue="0" Type="Integer"></asp:RangeValidator>
                                </td>
                                <td align="left">
                                    <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbDias_Contrasena"
                                        ErrorMessage="Falta!"></asp:RequiredFieldValidator>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="fila1">
                                    <asp:Label ID="Label7" runat="server" Text="ESTADO *:"></asp:Label></td>
                                <td align="left" class="fila1">
                                    <asp:DropDownList ID="estado" runat="server" Width="95%" CssClass="custom-select">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>ACTIVO</asp:ListItem>
                                        <asp:ListItem>INACTIVO</asp:ListItem>
                                    </asp:DropDownList></td>
                                <td align="left" class="fila1">
                                    <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator5" runat="server" ControlToValidate="estado"
                                        ErrorMessage="¡Seleccione un estado!"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td align="right" style="height: 20px">
                                    <asp:Label ID="Label6" runat="server" Text="PERFIL *:"></asp:Label></td>
                                <td align="left" style="height: 20px">
                                    <asp:DropDownList ID="id_perfil" runat="server" Width="95%" CssClass="custom-select">
                                    </asp:DropDownList></td>
                                <td align="left" style="height: 20px">
                                    <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator4" runat="server" ControlToValidate="id_perfil"
                                        ErrorMessage="¡Seleccione un perfil!"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3" id="tdPermisosDocumentos" runat="server">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="height: 30px">
                                </td>
                                <td align="left">
                                </td>
                                <td align="left">
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label9" runat="server" Text="FECHA DE ULTIMA ACTUALIZACIÓN"></asp:Label></td>
                                <td align="left" class="fila1">
                                    <asp:TextBox ID="fecha_creacion" runat="server" CssClass="noescribibles" ReadOnly="True" Width="97%"></asp:TextBox></td>
                                <td align="left" class="fila1">
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label10" runat="server" Text="USUARIO ULTIMA ACTUALIZACIÓN"></asp:Label></td>
                                <td align="left" class="fila1">
                                    <asp:TextBox ID="usuario_creacion" runat="server" CssClass="noescribibles" Width="97%"></asp:TextBox></td>
                                <td align="left" class="fila1">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">&nbsp;</td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="id_usuario" runat="server" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="id_usuario_creacion" runat="server" CssClass="camposConsulta" ReadOnly="True"
                            Visible="False"></asp:TextBox>
                        <asp:HiddenField ID="hfConsulta" runat="server" /><asp:HiddenField ID="hfElimina" runat="server" />
                        <asp:HiddenField ID="hfDias_Clave" runat="server" />
                        <asp:HiddenField ID="hfClave" runat="server" />
                        <asp:HiddenField ID="hfTipoUsuario" runat="server" />
                        <asp:HiddenField ID="hfid_proyecto" runat="server" />
                        </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
</body>
</html>

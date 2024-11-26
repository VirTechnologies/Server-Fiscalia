<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfPrioridad.aspx.cs" Inherits="wfPrioridad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administración de prioridades</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
</head>
<body runat="server" id="body">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="98%" >
            <tr>
                <td align="left" colspan="2" style="height: 16px">
                    <h4>
                        <asp:Label ID="Label1" runat="server"  Text="Administración de prioridad"></asp:Label>
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
                                <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR PRIORIDAD" OnClick="btnGrabar_Click" />&nbsp;<asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-danger" Text="ELIMINAR PRIORIDAD" OnClick="btnEliminar_Click" CausesValidation="False" /></td>
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
                            <td align="left" style="width: 20%">
                            </td>
                            <td align="left" style="width: 60%; height: 30px">
                            </td>
                            <td align="left" style="width: 40%">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label17" runat="server" Text="TEMÁTICA*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:DropDownList ID="ddltematica" style="width: 80%" runat="server" CssClass="custom-select" CausesValidation="false" AutoPostBack="true" OnSelectedIndexChanged="ddltematica_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator15" runat="server" ControlToValidate="ddlOficinaId" ErrorMessage="Seleccione!"></asp:RequiredFieldValidator>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>    
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label9" runat="server" Text="SECCIONAL*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:DropDownList ID="ddlOficinaId" style="width: 80%" runat="server" CssClass="custom-select" CausesValidation="false" AutoPostBack="true" OnSelectedIndexChanged="ddlOficinaId_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlOficinaId" ErrorMessage="Seleccione!"></asp:RequiredFieldValidator>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>                      
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label15" runat="server" Text="SEDE*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:DropDownList ID="dllsala" style="width: 80%" runat="server" CssClass="custom-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator6" runat="server" ControlToValidate="dllsala" ErrorMessage="Seleccione!"></asp:RequiredFieldValidator>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label10" runat="server" Text="PERFIL DE ATENCIÓN*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:DropDownList ID="ddlPerfilDeAtencionId" style="width: 80%" runat="server" CssClass="custom-select" >
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlPerfilDeAtencionId" ErrorMessage="Seleccione!"></asp:RequiredFieldValidator>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label11" runat="server" Text="SERVICIO TIPO ATENCIÓN*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:DropDownList ID="ddlServicioTipoAtencionId" style="width: 20%" runat="server" CssClass="custom-select" >
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlServicioTipoAtencionId" ErrorMessage="Seleccione!"></asp:RequiredFieldValidator>
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label2" runat="server" Text="PONDERACIÓN*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbPonderacion" runat="server" CssClass="form-control" Width="20%" style="display:inline;" MaxLength="3"></asp:TextBox>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" CssClass="text-danger"  ControlToValidate="tbPonderacion" ErrorMessage="Debe ser numérico" MaximumValue="1" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbPonderacion"
                                    ErrorMessage="¡Debe escribir la ponderación!"></asp:RequiredFieldValidator>
                                </td>
                            <td align="left" style="width: 40%">
                                
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label8" runat="server" Text="CANTIDAD MÁXIMA DE TURNOS*: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:TextBox ID="tbCantMaxTurnos" runat="server" CssClass="form-control" Width="20%" MaxLength="4" style="display:inline;"></asp:TextBox>
                                <asp:RangeValidator ID="RangeValidator2" runat="server" CssClass="text-danger"  ControlToValidate="tbCantMaxTurnos" ErrorMessage=" Debe ser numérico" MaximumValue="100" MinimumValue="0" Type="Integer"></asp:RangeValidator>    
                                <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbCantMaxTurnos"
                                    ErrorMessage="¡Debe escribir la cantidad máxima de turnos!"></asp:RequiredFieldValidator>
                            </td>
                            <td align="left" style="width: 40%">
                                
                            </td>
                        </tr>

                        <tr>
                            <td align="right" style="width: 20%">
                                <asp:Label ID="Label3" runat="server" Text="HABILITADO: "></asp:Label></td>
                            <td align="left" style="width: 40%">
                                <asp:CheckBox ID="cbVisible" runat="server"  />
                                </td>
                            <td align="left" style="width: 40%">
                                &nbsp;</td>
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

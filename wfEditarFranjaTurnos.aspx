<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfEditarFranjaTurnos.aspx.cs" Inherits="wfVerAgendamiento" %>

<!DOCTYPE html>

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
                <asp:Button ID="btnSalir" runat="server" CssClass="btn btn-outline-info" CausesValidation="False" Text="VOLVER" OnClick="btnSalir_Click" /></td>
               
                <td colspan="3">
                    <h4>
                        <asp:Label ID="lTitulo" runat="server" Text="Editar de turnos por franja horaria" ></asp:Label>
                    </h4>
                </td>
            </tr>
                <tr>
                    <td colspan="3">

                        <table style="width: 100%">
                            <tr>                
                                <asp:Label ID="Label5" runat="server" Text="Editar turno de la sede "></asp:Label>
                                <asp:Label ID="LabelNombreSede" runat="server" Text="" Font-Bold="True"></asp:Label>
                                  <asp:Label ID="Label6" runat="server" Text=" para el dia "></asp:Label>
                                <asp:Label ID="LabelNombreDia" runat="server" Text="" Font-Bold="True"></asp:Label>
                                  <asp:Label ID="Label7" runat="server" Text=" en la franja horaria "></asp:Label>
                                <asp:Label ID="LabelHorarioFranja" runat="server" Text="" Font-Bold="True"></asp:Label>
                             </tr>
                             </table>
                             <table>
                             <tr>
                          
                                <asp:Label ID="Label1" runat="server" Text="Numero de turnos actual en esta franja : " ></asp:Label>
                                <asp:Label ID="LabelTurnosFranja" runat="server" Text="" Font-Bold="True"></asp:Label>                 
                            </tr>
                        </table>
                        <p>&nbsp;</p>
                         <table style="width: 100%">
                            <tr>
                                <td style="width: 60%" class="fila1" align="left">
                                <asp:Label ID="Label2" runat="server"  Text="Nuevo numero de turnos por esta franja horaria"></asp:Label>
                            

                                    <asp:TextBox ID="tbNombre" runat="server" CssClass="form-control" MaxLength="50" Width="40%"></asp:TextBox>
                       
                                    <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="GUARDAR" OnClick="btnFiltrar_Click" />
                                </td>
                            </tr>
     
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>

            <tr>
                
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

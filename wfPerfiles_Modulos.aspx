<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfPerfiles_Modulos.aspx.cs" Inherits="wfPerfiles_Modulos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>wfPerfiles_Modulos</title>
    <script language='javascript' src='utiles.js'></script>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
    <base target="_self">
<script language="javascript">
   function viewSource()
   {
      d=window.open();
      d.copy();
   }
</script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="99%" runat="server">
            <tr>
                <td align="left" style="width: 80%;" colspan="2">
                    <h4>
                    <asp:Label ID="Label1" runat="server" Text="PERFIL: " ></asp:Label>
                    <asp:Label ID="Label2" runat="server" Font-Bold="True"></asp:Label>
                    </h4>
                </td>
                <td align="right" style="width: 20%;">&nbsp;</td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <h4>
                        <asp:Label ID="Label3" runat="server" Text="PERMISOS EN MÓDULOS" ></asp:Label>
                    </h4>
                </td>
                <td align="right" colspan="1">
                    </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Button ID="btnCerrar" runat="server" CssClass="btn btn-outline-info" Text="VOLVER" OnClick="btnCerrar_Click" /></td>
                <td align="left" colspan="1">
                    <asp:Button ID="btnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR" OnClick="btnGrabar_Click" /></td>
            </tr>
            <tr>
                <td align="left" colspan="2" style="height: 30px">
                </td>
                <td align="right" colspan="1">
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
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
                    <asp:Table ID="Table2" runat="server" Width="100%" CssClass="table table-hover border-dark">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:TextBox ID="id_perfil" runat="server" Visible="False"></asp:TextBox>
                    <asp:HiddenField ID="hfConsulta" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>

</body>
</html>

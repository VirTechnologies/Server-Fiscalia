<%@ Page Language="C#" AutoEventWireup="true" CodeFile="titulo_arriba.aspx.cs" Inherits="titulo_arriba" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
  <title>Inteliturnos Admin</title>
  <script language='javascript' src='utiles.js'></script>
    <link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">

</head>
<body style="margin:0; padding: 0px;" class="bg-secondary float-left">
    <div>
        <iframe src="<%=Page.ResolveClientUrl("~/ControlSesion.aspx")%>" width="0" height="0" style="display:none;"></iframe>
    </div>
    <form id="form1" runat="server" target="contenido">
            <div class="container" style="padding:0">
                <div class="row" style="width:1366px" align="right">
                    <div style="width:1366px" >
                        <span class="badge badge-primary">
                            <asp:Label ID="lblUsuario" runat="server" ForeColor="White"></asp:Label>
                        </span>
                        <span class="badge badge-info">
                            <asp:LinkButton ID="lbCambiarClave" runat="server" Visible="False" ForeColor="White" OnClick="lbCambiarClave_Click">Cambiar clave</asp:LinkButton>
                        </span>
                    </div>
                </div>
                <div class="row float-left">
                    <div class="col-12">
                          <img src="images/titulo.jpg" alt=""/>
                    </div>
                </div>
            </div>


<%--    <table width="100%" class="bg-secondary">
        <tr>
            <td align="left">
              <div id="titulo-principal"> 
                  <img src="images/titulo.jpg" alt=""/>
                  <div id="Div1" style="text-align:right; padding-right:5px"> 
                    <span class="badge badge-primary">
                        <asp:Label ID="lblUsuario" runat="server" ForeColor="White"></asp:Label>
                    </span>
                    <span class="badge badge-info">
                        <asp:LinkButton ID="lbCambiarClave" runat="server" Visible="False" ForeColor="White">Cambiar clave</asp:LinkButton>
                    </span>
                  </div>
               </div>
            </td>
        </tr>
    </table>--%>
    </form>
</body>
</html>

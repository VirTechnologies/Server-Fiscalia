<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfBarra.aspx.cs" Inherits="wfBarra" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 5.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>InteliTurnos Admin</title>
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
    	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

<%--<style>
    .dropdown-submenu {
    position: relative !important;
}
.dropdown-submenu a::after {
    transform: rotate(0deg);
    position: absolute;
    right: 6px;
    top: .8em;
}
.dropdown-submenu>.dropdown-menu {
    top: 0;
    left: -10rem; /* 10rem is the min-width of dropdown-menu */
    position: relative;
}
</style>--%>

</head>
<body style="margin:0; padding: 0px;" class="card text-white bg-primary ">
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg bg-dark navbar-dark" style="height:99%; width:259px">
            <div class="container-fluid">
                <a class="navbar-brand" href="wfBienvenida.aspx" target="contenido">MEN�</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="true" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse in show" id="navbarNavDropdown">
                    <ul class="nav navbar-nav ml-auto" id="botonesLaterales" runat="server">
                        <%-- AC� SE AGREGAN LOS BOTONES DEL MEN� --%>
                    </ul>
                </div>
            </div>
        </nav>
<%--        <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
          <a class="navbar-brand" href="#">Men�</a>
          <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarColor02" aria-controls="navbarColor02" aria-expanded="true" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
          </button>

          <div class="navbar-collapse collapse show" id="menuAplicacion2" style="">
            <ul class="navbar-nav mr-auto" id="botonesLaterales2" runat="server">
              <li class="nav-item active">
                <a class="nav-link" href="#">Home</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="#">Features</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="#">Pricing</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="#">About</a>
              </li>
              <li class="dropdown-submenu"><a class="dropdown-item dropdown-toggle" href="#">Submenu wider text</a>
                <ul class="dropdown-menu">
                  <li><a class="dropdown-item" href="#">Submenu</a></li>
                  <li><a class="dropdown-item" href="#">Submenu0</a></li>
                  <li class="dropdown-submenu"><a class="dropdown-item dropdown-toggle" href="#">Submenu 1</a>
                    <ul class="dropdown-menu">
                      <li><a class="dropdown-item" href="#">Subsubmenu1</a></li>
                      <li><a class="dropdown-item" href="#">Subsubmenu1</a></li>
                    </ul>
                  </li>
                  <li class="dropdown-submenu"><a class="dropdown-item dropdown-toggle" href="#">Submenu 2</a>
                    <ul class="dropdown-menu">
                      <li><a class="dropdown-item" href="#">Subsubmenu2</a></li>
                      <li><a class="dropdown-item" href="#">Subsubmenu2</a></li>
                    </ul>
                  </li>
                </ul>
              </li>

              <li class="nav-item dropdown">
                <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">Dropdown</a>
                <div class="dropdown-menu">
                  <a class="dropdown-item" href="#">Action</a>
                  <a class="dropdown-item" href="#">Another action</a>
                  <a class="dropdown-item" href="#">Something else here</a>
                  <div class="dropdown-divider"></div>
                  <a class="dropdown-item" href="#">Separated link</a>
                </div>
              </li>
            </ul>
          </div>
        </nav>
--%>



<%--
        <table width='100%'>
            <tr>
                <td>
                    <script type='text/javascript'>function Go(){return}</script>
                    <script type='text/javascript' src='VerticalFrames_var.js'></script>
                    <script type='text/javascript' src='menu9_compact.js'></script>
                    <noscript>Your browser does not support script</noscript>
                </td>
            </tr> 
        </table>
        <table width='100%'>
            <tr>
                <td width='100'>
                </td>
        </tr>
      </table>
    --%>

    </form>
    <span id="spanContrasena" runat="server">
        <asp:LinkButton ID="lbCambiarClave" runat="server" CausesValidation="False" Visible="false">Cambiar Contrase�a</asp:LinkButton>
            </span>
    <script src="js/jquery.min.js"></script>
    <%--<script src="js/bootstrap.bundle.min.js"></script>--%>
    <script src="js/bootstrap.min.js"></script>
    <script>
      $('.dropdown-menu a.dropdown-toggle').on('click', function(e) {
        if (!$(this).next().hasClass('show')) {
            $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
        }
        var $subMenu = $(this).next(".dropdown-menu");
        $subMenu.toggleClass('show');
        $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function(e) {
            $('.dropdown-submenu .show').removeClass("show");
        });
        return false;
    });
      </script>


</body>
</html>

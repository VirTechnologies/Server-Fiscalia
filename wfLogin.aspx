<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfLogin.aspx.cs" Inherits="wfLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 5.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>

	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Inteliturnos Central - Iniciar sesión</title>
    <meta name="description" content="Sistema inteligente de turnos">
    <meta name="keywords" content="Inteliturnos server, Inteliturnos, Turnos, atención de turnos">
    <meta name="author" content="Virtual Technologies">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0,user-scalable=0,viewport-fit=cover">

    <meta property="og:image" content="http://212.83.170.202/Inpec/TurnosAdmin/images/icon.png" />
    <meta property="og:description" content="Sistema de turnos Inteligente" />
    <meta property="og:type" content="article" />
    <meta property="og:article:author" content="Virtual Technologies" />
    <meta property="og:url" content="http://www.virtual.com.co" />

	<meta name="viewport" content="width=device-width, initial-scale=1">
<!--===============================================================================================-->	
	<link rel="icon" type="image/png" href="images/icons/inteliturno.png"/>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="fonts/font-awesome-4.7.0/css/font-awesome.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="vendor/animate/animate.css">
<!--===============================================================================================-->	
	<link rel="stylesheet" type="text/css" href="vendor/css-hamburgers/hamburgers.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="vendor/select2/select2.min.css">
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="css/util.css">
	<link rel="stylesheet" type="text/css" href="css/main.css">
<!--===============================================================================================-->
	<style type="text/css">
        .modal-dialog {
            max-width: 680px !important;
        }
	</style>
</head>
<body>
	<form id="form1" runat="server">
		       <asp:literal id="AntiForgeryToken" runat="server" />
	<div class="limiter">
		<div class="container-login100">
			<div class="wrap-login100">
				<div class="login100-pic js-tilt" data-tilt>
					<img src="images/img-01.png" alt="Virtual Inteliturno">
				</div>
					<div class="login100-form validate-form">
						<span class="login100-form-title">
							<strong>Inicio de sesión</strong>
						</span>
						<div class="wrap-input100 validate-input">
							<asp:TextBox ID="usuario" runat="server" CssClass="input100" placeholder="Correo"></asp:TextBox>
							<span class="focus-input100"></span>
							<span class="symbol-input100">
								<i class="fa fa-envelope" aria-hidden="true"></i>
							</span>
						</div>
						<div class="wrap-input100 validate-input">
							<asp:TextBox ID="clave" runat="server" CssClass="input100" TextMode="Password" placeholder="Contraseña"></asp:TextBox>
							<span class="focus-input100"></span>
							<span class="symbol-input100">
								<i class="fa fa-lock" aria-hidden="true"></i>
							</span>
						</div>
						<div class="container-login100-form-btn">
						  <asp:Button ID="btnEntrar" runat="server" CssClass="login100-form-btn" Text="Entrar" OnClick="btnEntrar_Click" />
						</div>
						<asp:HiddenField ID="Id_Retornado" runat="server" />
						<asp:TextBox ID="tbError" runat="server" Height="384px" TextMode="MultiLine" Visible="False" Width="592px"></asp:TextBox>
					</div>
					<div class="wrap-input400 validate-input" align="center">
							<div id="notificacion" align="center" runat="server" visible="false" class="alert alert-dismissible" style="margin: 30px 150px">
								<button type="button" class="close" data-dismiss="alert">&times;</button>
									<strong>
										<asp:Label ID="lbConfirmacion" runat="server" ></asp:Label>
										<br />
									</strong>
									<asp:Label ID="Label3" runat="server" CssClass="labelforms" Text="Para cambiarla haga clic " Visible="False"></asp:Label>
									<asp:LinkButton ID="lbCambiarClave" runat="server" Visible="False" Font-Bold="true" OnClientClick="document.getElementById('IcambioClave').src='wfCambioClave.aspx?opcional=false';return false;">aquí</asp:LinkButton>
							</div>
					</div>
			</div>
		</div>
	</div>
	</form>
<!--===============================================================================================-->	
	<script src="vendor/jquery/jquery-3.7.1.min.js"></script>
<!--===============================================================================================-->
	<script src="vendor/bootstrap/js/popper.js"></script>
	<script src="vendor/bootstrap/js/bootstrap.min.js"></script>
<!--===============================================================================================-->
<%--	<script src="vendor/select2/select2.min.js"></script>--%>
<!--===============================================================================================-->
	<script src="vendor/tilt/tilt.jquery.min.js"></script>
	<script >
		$('.js-tilt').tilt({
			scale: 1.1
		})
	</script>
<!--===============================================================================================-->
	<script src="js/main.js"></script>

    <div id='myModal' class='modal'>
        <div class="modal-dialog">
            <div class="modal-content">
                <div id='myModalContent'>
					<div class="modal-header" visible="true">
						<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
					</div>
					<iframe runat="server" id="IcambioClave" width="680" height="350" class="modal-content"></iframe>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(function () {
			$(".verDetalles").click(function () {
                var options = { "backdrop": "static", keyboard: true  };
                $('#myModal').modal(options);
                $('#myModal').modal('show');
            });
        });

        $(function () {
            $("#closbtn").click(function () {
                $('#myModal').modal('hide');
            });
        });
    </script>
</body>

</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfBienvenida.aspx.cs" Inherits="wfBienvenida" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bienvenida Aplicación Server</title>
<!--===============================================================================================-->
	<link rel="stylesheet" type="text/css" href="css/util.css">
	<link rel="stylesheet" type="text/css" href="css/main.css">
<!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="theme/default/bootstrap.css">

<style type="text/css">
.vertical-center {
  min-height: 100%;  /* Fallback for browsers do NOT support vh unit */
  min-height: 100vh; /* These two lines are counted as one :-)       */
  display: flex;
  align-items: center;
}
</style>
</head>
<body style="overflow:hidden;">
	<div class="limiter">
		<div class="container-login100">
        <form id="form1" runat="server">
            <div class="container vertical-center">
                <div class="row ">
                    <div class="col-lg-12">
                        <div class="jumbotron bg-secondary">
                            <center>
                                <div class="login100-pic">
	                                <img src="images/img-01.png" alt="VIRTUAL Inteliturno" />
                                </div>
                                <h3>
                                  Aplicación Central
                                  <small class="text-muted">Inteliturno</small>
                                </h3>
<%--                            <p class="lead">Sistema de gestión y control de colas.</p>--%>
                            <hr class="my-4" style="width:500px"/>
                            <a class="btn btn-primary btn-lg" href="http://www.virtual.com.co" target="_blank" role="button">Virtual Technologies</a>
                            </center>
                        </div>
                    </div>
                </div>
            </div>
        </form>
		</div>
	</div>
</body>
</html>


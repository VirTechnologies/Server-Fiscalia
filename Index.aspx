<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 5.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<meta http-equiv="Content-Security-Policy" content=" default-src 'self'; style-src 'self'; connect-src 'self'; frame-src 'self'; font-src 'self'; media-src 'self'; manifest-src 'self'; object-src 'none'; script-src 'self'; img-src 'self'; base-uri 'none'; form-action 'none';" />

    <title>Inteliturnos Central - Inicio</title>
    <meta name="description" content="Sistema inteligente de turnos">
    <meta name="keywords" content="Inteliturnos server, Inteliturnos, Turnos, atención de turnos">
    <meta name="author" content="Virtual Technologies">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0,user-scalable=0,viewport-fit=cover">
    <link rel="icon" type="image/png" href="images/icons/inteliturno.png"/>

    <meta property="og:image" content="/images/icon.png" />
    <meta property="og:description" content="Sistema de turnos Inteligente" />
    <meta property="og:type" content="article" />
    <meta property="og:article:author" content="Virtual Technologies" />
    <meta property="og:url" content="index.aspx" />



</head>
<frameset rows="100%" frameborder="no" border="0" framespacing="0">
  <frame src="wfLogin.aspx" name="bottomFrame" scrolling="No" noresize="noresize" id="bottomFrame" title="contenido" />
</frameset>
<body>
    <form id="form1" runat="server">
               <asp:literal id="AntiForgeryToken" runat="server" />
    </form>
</body>
</html>

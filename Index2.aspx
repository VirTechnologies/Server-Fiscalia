<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index2.aspx.cs" Inherits="Index2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 5.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>InteliTurnos Admin</title>
    <meta name="description" content="Sistema inteligente de turnos">
    <meta name="keywords" content="Inteliturnos server, Inteliturnos, Turnos, atención de turnos">
    <meta name="author" content="Virtual Technologies">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0,user-scalable=0,viewport-fit=cover">
    <link rel="icon" type="image/png" href="images/icons/inteliturno.png"/>

    <meta property="og:image" content="/images/icon.png" />
    <meta property="og:description" content="Sistema de turnos Inteligente" />
    <meta property="og:type" content="article" />
    <meta property="og:article:author" content="Virtual Technologies" />
    <style type="text/css">
        .fondoIzquierda
        {
            background-color: #2fa4e7;
        }
        .fondoDerecha
        {
            background-color: white;
        }
        .fondoArriba
        {
            background-color: #e9ecef;
        }
    </style>
</head>

<%--<frameset rows="174,80,*" cols="*" frameborder="NO" border="0" framespacing="0">
    <frame src="titulo_arriba.aspx" name="topFrame" scrolling="NO" noresize >
    <frame src="wfBarra.aspx" name="leftFrame" scrolling="yes" noresize>
    <frame src="wfBienvenida.aspx" name="contenido" style="z-index: -1; position: absolute;">
</frameset>--%>

<frameset rows="174,*" cols="*" frameborder="NO" border="0" framespacing="0">
    <frame src="titulo_arriba.aspx" name="topFrame" scrolling="NO" class="fondoArriba" noresize >
    <frameset cols="260,*" frameborder="YES" border="1" framespacing="0">
    <frame src="wfBarra.aspx" allowtransparency="true" name="leftFrame" class="fondoIzquierda" scrolling="yes" noresize>
    <frame src="wfBienvenida.aspx" class="fondoDerecha" scrolling="yes" allowtransparency="false" style="padding:20px 0px 0px 20px;" name="contenido">
    </frameset>
</frameset>
    
<body style="margin:0; padding: 0px;">
    <form id="form1" runat="server">
    </form>
</body>
</html>

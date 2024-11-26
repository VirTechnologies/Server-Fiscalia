<%@ Page Language="C#" AutoEventWireup="true" CodeFile="wfSesionTimeOut.aspx.cs" Inherits="wfSesionTimeOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CERRAR SESIÓN</title>
<script>
  function Cerrar()
  {
    window.parent.location="Index.aspx";
  }  
</script>
</head>
<body onload="javascript:Cerrar();">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>

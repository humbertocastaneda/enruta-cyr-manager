<%@ Page Language="VB" AutoEventWireup="false" CodeFile="refresh.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ver Ordenes</title>
    <link rel="Stylesheet" href="style.css" type="text/css">
    <link rel="Stylesheet" href="menu.css" type="text/css">
    <link rel="icon" type="image/png" href="botones/favicon.png">
    <!--[if (gte IE 6)&(lte IE 8)]>
      <script type="text/javascript" src="selectivizr.js"></script>
      <noscript><link rel="stylesheet" href="[fallback css]" /></noscript>
    <![endif]-->
    <!--[if lt IE 9]>
    <script src="http://ie7-js.googlecode.com/svn/version/2.1(beta4)/IE9.js"></script>
    <![endif]-->

    <!--meta http-equiv=”refresh” content="10" /-->
</head>
<body onload="javascript:setTimeout(function(){ location.reload(); },300000);">
    <form id="form1" runat="server">
        <div id="pagina">
            <div id="cabecera">
                <div style="width: auto; height: auto; min-height: 50px; float: left">
                   
                        <img alt="Enruta Lectura y Reparto" style="float: left" height="50px" src="botones/enruta.jpg" />
                </div>
                <div id="contenedor">
                </div>
            </div>
            <div id="contenido">
                <div id="blanco" style="background-color: white">
                </div>
                <div id="navegador" style="background-color: transparent">
                </div>
                <br />
            </div>
        </div>
        <div align="center" class="footer" style="position: fixed; height: 5%; width: 100%; bottom: 0; background-color: #004FC4; left: -2px; right: 48; color: #FFFFFF;">

            <br />
            <%--AQUI VA EL MENSAJE DEL FOOTER--%>
            <br />

        </div>
    </form>
</body>
</html>

<%@ Page Language="VB" AutoEventWireup="false" CodeFile="logIn.aspx.vb" Inherits="_default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Enruta Lectura y Reparto</title>
    <link rel="Stylesheet" href="style.css" type="text/css" />
    <link rel="icon" type="image/png" href="botones/favicon.png">
    <!--[if (gte IE 6)&(lte IE 8)]>
      <script type="text/javascript" src="selectivizr.js"></script>
      <noscript><link rel="stylesheet" href="[fallback css]" /></noscript>
    <![endif]-->
    <!--[if lt IE 9]>
    <script src="http://ie7-js.googlecode.com/svn/version/2.1(beta4)/IE9.js"></script>
    <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
    <div id="pagina">
        <div id="blanco" style="background-color: white">
        </div>
        <div id="cabecera">
            <div style="width: auto; height: auto; min-height: 50px;">
                <img alt="Enruta Lectura y Reparto" style="float: left" height="50px" src="botones/enruta.jpg" />
            </div>
        </div>
        <div id="contenido">
            <div>
                <br />
                <asp:Label ID="lbl_msg" runat="server" Text="Bienvenido" Style="visibility: hidden"></asp:Label>
                <asp:Login ID="Login1" Style="margin-left: 35%; margin-right: 35%; margin-top: 50px;"
                    loginUrl="verOrdenes.aspx" runat="server">
                </asp:Login>
            </div>
        </div>
        <div id="pie">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
          
        <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
        <asp:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="lblOculto"
            CancelControlID="btnaceptar" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Style="display: none">
            <div align="center">
                <div align="center" id="mensajes">
                    <asp:Label ID="lbldialogo" runat="server" Font-Names="Segoe UI"  
                        CssClass="label" ></asp:Label>
                </div>
                <div>
                    <asp:Button ID="btnaceptar" runat="server" Text="Aceptar" BackColor="#004FC4" Font-Names="Segoe UI"
                        ForeColor="#EAEAEA"  BorderStyle="None" Font-Size="18px" />
                </div>
            </div>
        </asp:Panel>
       


        </div>
    </div>
    </form>
</body>
</html>

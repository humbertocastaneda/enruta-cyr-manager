<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Ajustes.aspx.vb" Inherits="Ajustes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Enviar archivo de Excel</title>
    <link rel="Stylesheet" href="style.css" type="text/css">
    <link rel="Stylesheet" href="menu.css" type="text/css">
</head>
<body>
    <form id="form1" runat="server">
    <div id="pagina">
        <div id="cabecera">
            <div style="width: auto; height: 50px; float: left">
                <a href="default.aspx">
                    <img alt="Enruta Lectura y Reparto" style="float: left" height="50px" src="botones/enruta.jpg" />
                </a>
            </div>
            <div id="contenedor">
                <ul id="menu">
                    <li><a href="envio.aspx">Importar</a></li>
                    <li><a href="asignacion.aspx">Asignar</a></li>
                    <li><a href="empleadosABC.aspx">Técnicos</a></li>
                    <li><a href="#">Ver Ordenes</a>
                        <ul class="children">
                            <li><a href="verOrdenesPorEmpleado.aspx">Por Técnico</a></li>
                            <li><a href="verOrdenes.aspx">Por Número de Orden/Cte.</a></li>
                        </ul>
                    </li>
                    <li><a href="logIn.aspx/?log=1">Salir</a></li>
                </ul>
            </div>
        </div>
            

        <div id="contenido" align="left" style="font-family: 'Segoe UI'; font-size: medium">
        <div id="navegador" style=" background-color:transparent">
                <div id="buscar">
                    <div style="margin-left: 20%; margin-right: 20%">
                        <div style="width: auto; float: left; margin-left: auto">
                        </div>
                        <div style="width: auto; float: left; margin-left: 20px; height: 100%; margin-top: 10px">
                        </div>
                    </div>
                </div>
                <div id="ordenes">
                </div>
            </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            Cambio de Contraseña
            <div style="margin-top: 20px; border-top: 5px solid #004FC4; width: 69%; height: 228px;
                margin: 1px 135px 123px 148px; border-left-color: #eaeaea; border-left-width: 5px;
                border-right-color: #eaeaea; border-right-width: 5px; border-bottom-color: #eaeaea;
                border-bottom-width: 5px; font-family: 'Segoe UI'; font-size: medium;" align="center">
                <br />
                Contraseña Anterior:
                <br />
                <asp:TextBox ID="txtAnterior" runat="server" TextMode="Password" 
                   ></asp:TextBox>
                <br />
                <!-- SM VALIDADOR DE CONTRASEÑA 25 DE JULIO -->
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Requiere contraseña Actual"
                    ForeColor="red" ControlToValidate="txtAnterior" Display="Dynamic" Font-Names="Segoe UI"></asp:RequiredFieldValidator>
                <br />
                <br />
                Contraseña Nueva:
                <br />
                <asp:TextBox ID="txtNueva" runat="server" TextMode="Password"></asp:TextBox>
                <br />
                <!-- SM VALIDADOR DE CONTRASEÑA 25 DE JULIO -->
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Nueva Contraseña"
                    ForeColor="red" ControlToValidate="txtNueva" Display="Dynamic" Font-Names="Segoe UI"></asp:RequiredFieldValidator>
                <br />
                <br />
                Confirme Contraseña:<br />
                <asp:TextBox ID="txtConfirmar" runat="server" TextMode="Password"></asp:TextBox>
                <br />
                <!-- SM VALIDADOR DE CONTRASEÑA 25 DE JULIO -->
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Confirme Contraseña"
                    ForeColor="red" ControlToValidate="txtConfirmar" Display="Dynamic" Font-Names="Segoe UI"></asp:RequiredFieldValidator>
                <br />
                <br />
                <!-- SM VALIDADOR DE CONTRASEÑA 25 DE JULIO -->
                <asp:CompareValidator ID="valComparar" ControlToValidate="txtConfirmar" runat="server"
                    ControlToCompare="txtNueva" Display="Dynamic" EnableClientScript="true" ErrorMessage="No coniciden las contraseñas"
                    ForeColor="#CC3300" Font-Names="Segoe UI" />
                <br />
                <br />
                <asp:Button Style="float: right" ID="cambiarPass" runat="server" Text="Cambiar" 
                    CssClass="buttongrl" />
            </div>
        </div>
        <!-- SM 25 DE JULIO CUADRO DE DIALOGO  25 DE JULIO 2014-->
        <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
        <asp:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="lblOculto"
            CancelControlID="btnaceptar" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Style="display: none">
            <div align="center">
                <div align="center" id="mensajes">
                    <asp:Label ID="lbldialogo" runat="server" Font-Names="Segoe UI" CssClass="label"></asp:Label>
                </div>
                <div>
                    <asp:Button ID="btnaceptar" runat="server" Text="Aceptar" 
                        Font-Size="18px" CssClass="buttongrl"/>
                </div>
            </div>
        </asp:Panel>
        <!-- SM 25 DE JULIO CUADRO DE DIALOGO -->
    </div>
    <div align="center" class="footer" style="position: fixed; height: 5%; width: 100%;
        bottom: 0; background-color: #004FC4; left: -2px; right: 48; color: #FFFFFF;">
        <br />
        <%--AQUI VA EL MENSAJE DEL FOOTER--%>
        <br />
    </div>
    </form>
</body>
</html>

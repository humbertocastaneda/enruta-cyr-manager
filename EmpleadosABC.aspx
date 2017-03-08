<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EmpleadosABC.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Técnicos</title>
    <link rel="Stylesheet" href="style.css" type="text/css">
    <link rel="Stylesheet" href="menu.css" type="text/css">
    <link rel="icon" type="image/png" href="botones/favicon.png">
    <style type="text/css">
        #lblAlerta
        {
            width: 100%;
            top: 0;
            margin-top: 20px;
            position: relative;
            width: 90%;
            margin-top: auto;
            margin-right: auto;
            margin-bottom: auto;
            margin-left: auto;
        }
    </style>
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
        <div id="cabecera">
            <div style="width: auto; height: auto; min-height: 50px; float: left">
                <a href="default.aspx">
                    <img alt="Enruta Lectura y Reparto" style="float: left" height="50px" src="botones/enruta.jpg" />
                </a>
            </div>
            <div id="contenedor"  runat="server">
                <ul id="menu">
                    <li><a href="envio.aspx">Importar/Exportar</a></li>
                    <!--li><a href="asignacion.aspx">Asignar</a></--li-->
                    <li><a href="verSeguimiento.aspx">Seguimiento</a></li>
                    <li><a href="#">Ver Lecturas</a>
                        <ul class="children">
                            <li><a href="verOrdenesPorEmpleado.aspx">Por Lecturista</a></li>
                            <li><a href="verOrdenesPorEmpleadoFotos.aspx">Con Foto</a></li>
                            <li><a href="verOrdenes.aspx">Por Clave Usuario</a></li>
                             <li><a href="verCambiosDeMedidor.aspx">Cambio de Medidor</a></li>
                    <li><a href="verNoRegistrados.aspx">No Registrados</a></li>
                        </ul>
                    </li>
                    <li><a href="#">Catálogos</a>
                        <ul class="children">
                            <li><a href="empleadosABC.aspx">Lecturistas</a></li>
                        </ul>
                    </li>
                    <li><a href="logIn.aspx/?log=1">Salir</a></li>
                    <li></li>
                </ul>
            </div>
        </div>
        <div id="navegador">
            <div id="buscar">
                <asp:Button ID="ib_nuevo" Style="float: right" runat="server" Text="Nuevo" CssClass="buttongrl" />
                <asp:Button ID="ib_eliminar" Style="float: right" runat="server" Text="Eliminar"
                    Visible="False" CssClass="buttongrl" />
            </div>
            <div id="ordenes">
                <div style="width: 100%; height: auto">
                    <asp:Button ID="ib_cancelar" Style="float: right;" runat="server" Text="Cancelar"
                        CssClass="buttongrl" />
                    <asp:Button ID="ib_guardar" Style="float: right" runat="server" Text="Guardar" CssClass="buttongrl" />
                </div>
            </div>
        </div>
        <div id="contenido">
            <div id="blanco" style="background-color: white">
            </div>
            <div id="emp" runat="server" style="width: 100%; display: inline-block">
                <div style="width: 50%; float: left">
                    <asp:ListBox ID="lbEmpleados" Style="height: 150px !important; margin-top: 35px;
                        margin-bottom: 35px; top: 0px; left: 0px;" AutoPostBack="true" runat="server"
                        HorizontalContentAlignment="Stretch"></asp:ListBox>
                </div>
                <div id="datos" runat="server" style="width: 50%; float: left">
                    <asp:Label ID="lblId" runat="server" Width="123px">Id: <%# idEmpleado%></asp:Label>
                    <br />
                    <asp:Label ID="Label4" runat="server" Text="Estado del Técnico:"></asp:Label>
                    &nbsp;
                    <asp:DropDownList ID="ddl_estadosTecnico" runat="server">
                        <asp:ListItem Selected="True" Value="EG001">Activo</asp:ListItem>
                        <asp:ListItem Value="EG002">Inactivo</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <br />
                    <asp:Label ID="label" runat="server" Height="21px" Text="No. Nómina:" Width="130px"></asp:Label>
                    &nbsp;
                    <asp:TextBox ID="txtNomina" runat="server" Enabled="False" Text="<%# nomina%>"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="CustomValidator1" runat="server" ErrorMessage="El número de nómina no puede estar vacio."
                        ControlToValidate="txtNomina" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="CustomValidator3" runat="server" ControlToValidate="txtNomina"
                        ErrorMessage="El número de nómina no puede contener letras." ForeColor="Red"
                        ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                    <br />
                    <br />
                    <asp:Label ID="label5" runat="server" Text="Alias" Width="130px"></asp:Label>
                    &nbsp;
                    <asp:TextBox ID="txtAlias" runat="server" Width="222px" Text="<%# apodo%>"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="Nombre:" Width="130px" Height="21px"></asp:Label>
                    &nbsp;
                    <asp:TextBox ID="txtNombre" runat="server" Width="222px" Text="<%# nombre%>"></asp:TextBox><br />
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="Apellido Paterno:" Width="130px" Height="21px"></asp:Label>
                    &nbsp;
                    <asp:TextBox ID="txtApPat" runat="server" Width="222px" Text="<%# apPat%>"></asp:TextBox><br />
                    <br />
                    <asp:Label ID="Label3" runat="server" Text="Apellido Materno:" Height="21px" Width="130px"></asp:Label>
                    &nbsp;
                    <asp:TextBox ID="txtApMat" runat="server" Width="222px" Text="<%# apMat%>"></asp:TextBox>
                    <br />
                    <br />
                </div>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
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
                    <asp:Button ID="btnaceptar" runat="server" Text="Aceptar" CssClass="buttongrl" Font-Size="18px" />
                </div>
            </div>
        </asp:Panel>
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

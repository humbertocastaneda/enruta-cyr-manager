<%@ Page Language="VB" AutoEventWireup="false" CodeFile="envio.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Importar/Exportar</title>
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
</head>
<body>
    <form id="form1" runat="server">
        <div id="pagina">
            <div id="cabecera">
                <div style="width: auto; height: auto; float: left">
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
            <div id="contenido" align="center">
                <div id="blanco" style="background-color: white">
                </div>
                <div id="navegador"
                    style="background-color: transparent;">
                    <div id="buscar">
                    </div>
                    <div id="ordenes">
                    </div>
                </div>
                <br />
                <asp:Label ID="lbl_msj" runat="server" Text="Hola" Style="visibility: hidden">
                </asp:Label>
                <br />
                <div style="width: 70%; height: 50%; margin: auto 0px">
                    <div align="left">
                        Seleccione el archivo a cargar<br />
                    </div>

                    <div style="text-align: right; width=50%">
                        <asp:Button ID="b_cargar" runat="server" Text="Cargar" CssClass="buttongrl" />
                        <asp:Button ID="bEnviar" runat="server" Font-Names="Segoe UI" Text="Enviar al Celular"
                             CssClass="buttongrl" />
                        <asp:Button ID="b_decargar0" runat="server" Text="Exportar a Excel" CssClass="buttongrl" />
                    </div>
                    <br />
                    <div align="right" style="float: left">
                        <asp:Label ID="Label1" runat="server" Font-Names="Segoe UI">Fecha programada de la toma de lecturas</asp:Label>
                        &nbsp;<asp:TextBox ID="fechaProgram" runat="server" ToolTip="AñoMesDía (20001231)"></asp:TextBox>
                        <asp:CalendarExtender ID="cexfecha1" runat="server" TargetControlID="fechaProgram"
                            Format="yyyyMMdd" TodaysDateFormat="yyyy/MM/dd">
                        </asp:CalendarExtender>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="fechaProgram"
                            ErrorMessage="* Error (AñoMesDía)" ValidationExpression="^\d{4}((0[1-9])|(1[012]))((0[1-9]|[12]\d)|3[01])$"
                            Display="Dynamic" ForeColor="Red" SetFocusOnError="True" Font-Names="Segoe UI"
                            Font-Size="12px"></asp:RegularExpressionValidator>
                        <br />
                        <br />
                        <div align="left">
                            <asp:FileUpload ID="FileUploadControl" Multiple="Multiple" runat="server" Height="23px"
                                Width="231px" />
                        </div>
                        <br />
                    </div>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div align="left" style="position: static; float: none;">
                        Ultimos archivos cargados<br />
                        <br />
                        <div style="text-align: right">
                            <asp:Button ID="bEliminar" runat="server" Text="Eliminar" CssClass="buttongrl" />
                        </div>
                        <br />
                        <br />
                        <asp:GridView ID="gv_tecnicos" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            ForeColor="#333333" GridLines="Horizontal" OnRowDataBound="gv_tecnicos_RowDataBound"
                            ShowHeader="true" Style="width: 100%">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="nombre" HeaderText="Archivo" ItemStyle-Width="60%" />
                                <asp:BoundField DataField="id" HeaderText="">
                                    <ItemStyle CssClass="hide" />
                                    <HeaderStyle CssClass="hide" />
                                </asp:BoundField>
                                <asp:BoundField DataField="fecha" HeaderText="Fecha y Hora de Carga" ItemStyle-Width="40%">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:CommandField ButtonType="Button" ShowSelectButton="true">
                                    <ItemStyle CssClass="hide" />
                                    <HeaderStyle CssClass="hide" />
                                </asp:CommandField>
                            </Columns>
                            <EditRowStyle BackColor="#2461BF" />
                            <RowStyle BackColor="White" />
                            <SelectedRowStyle BackColor="#004FC4" Font-Bold="True" ForeColor="white" />
                        </asp:GridView>
                    </div>
                </div>
                <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" EnableScriptLocalization="false"
                    runat="server">
                </asp:ScriptManager>
                <!— CUADRO DE DIALOGO -->
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
                            <asp:Button ID="btnaceptar" runat="server" Text="Aceptar" Font-Size="18px" CssClass="buttongrl" />
                        </div>
                    </div>
                </asp:Panel>
                <!—FIN DEL CUADRO DE DIALOGO -->
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

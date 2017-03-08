<%@ Page Language="VB" AutoEventWireup="false" CodeFile="verOrdenesPorEmpleadoFotos.aspx.vb"
    Inherits="verOrdenesPorEmpleado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ordenes por Técnico</title>
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
    <script type="text/javascript" src="ZeroClipboard.js"></script>
    <script type="text/javascript">


        window.onload = MaintainDivScrollPosition;

        function nuevaVentana() {


            var q = document.getElementById("query").value;

            if (q != "") {
                window.open('vistaCaptura.aspx?query=' + q, '_blank');
            }
            else {
                alert("Seleccione 'Obtener Ordenes' para utilizar esta opción.")
            }

        }



        // Functions to save, read and erase cookies
        function createCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";
            document.cookie = name + "=" + value + expires + "; path=/";
        }

        function readCookie(name) {
            var nameEQ = name + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') c = c.substring(1, c.length);
                if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
            }
            return null;
        }

        function eraseCookie(name) {
            createCookie(name, "", -1);
        }

        // Set Div Position using Cookie
        function SetDivPosition() {
            var intY = document.getElementById("bigDiv").scrollTop;
            eraseCookie('WTdivSampleScrollYPorEmp');
            createCookie('WTdivSampleScrollYPorEmp', intY, 1);
        }

        function MaintainDivScrollPosition() {
            var intY = readCookie('WTdivSampleScrollYPorEmp');
            document.getElementById("bigDiv").scrollTop = intY;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <input id="query" type="hidden" runat="server" />
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
                <div>
                    <br />
                    <asp:Label ID="lbInfo" runat="server" Text="Label"></asp:Label>
                    <br />
                    <br />
                    <div id="bigDiv" class="searchStyle" style="height: 130px; width: 99% !important;
                        overflow: scroll; overflow-x: hidden; left: 0px;" 
                        onscroll="SetDivPosition()">
                        <asp:GridView ID="gv_tecnicos" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            ForeColor="#333333" GridLines="Horizontal" OnRowDataBound="gv_tecnicos_RowDataBound"
                            ShowHeader="true" Style="width: 100%" Width="349px">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="nombreComp" ItemStyle-Width="60%" HeaderText="Lecturista" >
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="id" HeaderText="Nombre">
                                    <ItemStyle CssClass="hide" />
                                    <HeaderStyle CssClass="hide" />
                                </asp:BoundField>
                                <asp:BoundField DataField="desconexiones" HeaderText="Lecturas" ItemStyle-Width="40%">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:CommandField ShowSelectButton="true" ButtonType="Button">
                                    <ItemStyle CssClass="hide" />
                                    <HeaderStyle CssClass="hide" />
                                </asp:CommandField>
                            </Columns>
                            <EditRowStyle BackColor="#2461BF" />
                            <RowStyle BackColor="White" />
                            <SelectedRowStyle BackColor="#004FC4" Font-Bold="True" ForeColor="white" />
                        </asp:GridView>
                    </div>
                    <br />
                    <asp:Label ID="lbStatus" runat="server" Text="Label"></asp:Label>
                </div>
            </div>
            <div id="ordenes">
                <div style="width: 100%; margin-bottom: 40px; height: auto">
                    <div style="width: auto; float: left">
                        <asp:RadioButtonList ID="rbTipoDeBusqueda" AutoPostBack="true" runat="server" Visible="False">
                            <asp:ListItem Value="1" Selected="True">Por Fecha</asp:ListItem>
                            <asp:ListItem Value="0">Todas las pendientes</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div style="width: auto; float: left; margin-left: 20px;">
                        <asp:Label ID="Label1" Style="float: left" runat="server" Text="Fecha"></asp:Label>
                        <br />
                        <br />
                        <div style="width: auto; float: right; text-align:right">
                            <asp:Label ID="Label2" Style="float: left" runat="server" Text="Inicio"></asp:Label>
                            <asp:TextBox ID="dia" runat="server" Style="float: right" ToolTip="AñosMesDia (&quot;20001231&quot;)"></asp:TextBox>
                            <br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="dia"
                                ErrorMessage="* Error (AñoMesDía)" ValidationExpression="^\d{4}((0[1-9])|(1[012]))((0[1-9]|[12]\d)|3[01])$"
                                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" Font-Names="Segoe UI"
                                Font-Size="12px"></asp:RegularExpressionValidator>
                            <asp:CalendarExtender ID="dia_CalendarExtender" runat="server" TargetControlID="dia"
                                Format="yyyyMMdd">
                            </asp:CalendarExtender>
                            <br />
                            <asp:Label ID="Label3" Style="float: left" runat="server" Text="Final"></asp:Label>
                            <asp:TextBox ID="dia2" Style="float: right" runat="server"></asp:TextBox><br />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="dia2"
                                ErrorMessage="* Error (AñoMesDía)" ValidationExpression="^\d{4}((0[1-9])|(1[012]))((0[1-9]|[12]\d)|3[01])$"
                                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" Font-Names="Segoe UI"
                                Font-Size="12px"></asp:RegularExpressionValidator>
                            <asp:CalendarExtender ID="dia2_CalendarExtender" runat="server" TargetControlID="dia2"
                                Format="yyyyMMdd">
                            </asp:CalendarExtender>
                        </div>
                        &nbsp;
                    </div>
                </div>
                <div style="width: 100%; float: left; height: auto">
                    <br />
                    <br />
                    <asp:DropDownList ID="ddl_motivos" runat="server" Visible="False">
                        <asp:ListItem Value="0">Asignadas</asp:ListItem>
                        <asp:ListItem Value="1">Pendientes</asp:ListItem>
                        <asp:ListItem Value="2">Con Lectura</asp:ListItem>
                        <asp:ListItem Value="4">Sin Lectura</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBox ID="ck_PorUsuario" Text="Filtrar por Usuario" Checked="true" runat="server" Visible="False" />
                    <asp:CheckBox ID="ck_filtrarRealizadas" Text="Filtrar Leidas" runat="server" Visible="False" Checked="true" />
                    <br />
                    <br />
                    <asp:Button ID="bObtenerOrdenes" runat="server" Style="font-family: 'Segoe UI'" Text="Obtener Lecturas"
                        CssClass="buttongrl" />
                    <asp:Button ID="bAExcel" runat="server" Text="Exportar" CssClass="buttongrl" />
                    <asp:Button ID="b_verMapa" runat="server" Text="Ver Mapa" CssClass="buttongrl" />
                    <asp:Button ID="b_imprimir" runat="server" Text="Imprimir" CssClass="buttongrl" Visible="False" />
                    <asp:Button ID="bBajarFotos" runat="server" Text="Bajar fotografias" CssClass="buttongrl" Visible="False" />
                    <br />
                    <br />
                </div>
            </div>
        </div>
        <div id="contenido">
            <div id="blanco" style="background-color: white">
            </div>
            <asp:DataGrid ID="dgOrdenes" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False"
                Font-Strikeout="False" Font-Underline="False" ForeColor="Black">
                <AlternatingItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                    Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False"
                    ForeColor="Black" />
                <Columns>
                    <asp:BoundColumn DataField="tecnico" HeaderText="Lecturista" />
                    <asp:TemplateColumn HeaderText="Orden">
                        <ItemTemplate>
                            <asp:LinkButton ID="hlNumOrden" runat="server" OnClick="LinkButton_Click" Text='<%# Container.DataItem("numOrden")%>'></asp:LinkButton>
                            &nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="clave_usuario" HeaderText="Clave Usuario" />
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:Image ID="Image1" ImageUrl='<%# Container.DataItem("Foto") %>' style="width:268px; height:auto" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="serieMedidor" HeaderText="Número de Medidor" />
                    <asp:BoundColumn DataField="lectura" HeaderText="Lectura" />
                    <asp:BoundColumn DataField="motivo" HeaderText="Anomalia" />
                    <asp:BoundColumn DataField="fechadeejecucion" HeaderText="Fecha de Ejecucion" />
                    <asp:BoundColumn DataField="comentarios" HeaderText="Comentarios" />

                    
                </Columns>
                <HeaderStyle BackColor="#004FC5" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                <ItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                    Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False"
                    ForeColor="Black" />
                <SelectedItemStyle BackColor="#004FC5" Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI"
                    Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False"
                    ForeColor="White" />
            </asp:DataGrid>
            <br />
            <asp:Label ID="lbAlerta" runat="server" Text="Sin Datos"></asp:Label>
            <br />
            <div id="d_imprimir" style="display: none; width: 100%;">
                <asp:Label ID="lb_tecnico" runat="server" Text="Label"></asp:Label>
                <br />
                <br />
                <asp:DataGrid ID="dgOrdenListado" Style="width: 100%;" runat="server" AllowSorting="true"
                    AutoGenerateColumns="False" Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI"
                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="Black">
                    <AlternatingItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                        Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False"
                        ForeColor="Black" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="Orden">
                            <ItemTemplate>
                                <asp:LinkButton ID="hlNumOrden" runat="server" OnClick="LinkButton_Click" Text='<%# Container.DataItem("numOrden")%>'></asp:LinkButton>
                                &nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="serieMedidor" HeaderText="Medidor" />
                        <asp:BoundColumn DataField="sectorCorto" HeaderText="Clave de Loc." />
                        <asp:BoundColumn DataField="cliente" HeaderText="Cliente" />
                    </Columns>
                    <HeaderStyle BackColor="#004FC5" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                    <ItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                        Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False"
                        ForeColor="Black" />
                    <SelectedItemStyle BackColor="#004FC5" Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI"
                        Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False"
                        ForeColor="White" />
                </asp:DataGrid>
            </div>
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

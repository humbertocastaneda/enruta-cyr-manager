<%@ Page Language="VB" AutoEventWireup="false" CodeFile="verOrdenesPorEmpleadoFotos.aspx.vb"
    Inherits="verOrdenesPorEmpleadoFotos" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ordenes por Técnico</title>
    <link rel="Stylesheet" href="style.css" type="text/css">
    <link rel="Stylesheet" href="menu.css" type="text/css">
    <link rel="icon" type="image/png" href="botones/favicon.png">
    <script src="Scripts/jquery-2.0.3.min.js" type="text/javascript"></script>
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
    <script src="Scripts/jquery-2.0.3.min.js" type="text/javascript"></script>
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

    <script src="Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>
    <script type = "text/javascript">
        $(document).ready(function () {
            $('#<%=dgOrdenes.ClientID%>').Scrollable({
                ScrollHeight: 500
            });
        });
    </script>

    <form id="form1" runat="server">
    <input id="query" type="hidden" runat="server" />
    <div id="pagina">
        <div id="blanco" style="background-color: white">
        </div>
        <div id="cabecera">
            <div style="width: auto; height: auto; min-height: 50px; float: left">
                <a href="default.aspx">
                    <img alt="Enruta Lectura y Reparto" style="float: left" height="50px" src="botones/enruta.jpg" />
                </a>
            </div>
            <div id="contenedor" runat="server">
                                    <ul id="menu1" runat="server" visible="false">
                          <li><a href="envio.aspx">Importar</a></li>
                    <li><a href="verSeguimiento.aspx">Seguimiento</a></li>
                    <li><a href="verInicioFin.aspx">Inicio y Fin de Jornada</a></li>
                    <li>
                        <a href="#">Ver Ordenes</a>
                        <ul class="children">
                             <li><a href="verOrdenesPorEmpleado.aspx">Por Técnico</a></li>
                            <li><a href="verOrdenes.aspx">Por Número de Orden/Cte.</a></li>
                            <li><a href="verResumenEstados.aspx">Por Estados de la Orden</a></li>
                            <li><a href="verOrdenesporCalle.aspx">Por Calle</a></li>
                        </ul>
                    </li>
                        <li>
                            <a href="#">Catálogos</a>
                            <ul class="children">
                                <li><a href="empleadosABC.aspx">Técnicos</a></li>
                            </ul>
                            
                        </li>
  
                    <li><a href="logIn.aspx/?log=1">Salir</a></li>
                        <li></li>
                </ul>

                    

                    <ul id="menu2" runat="server" visible="false">
                   
                    <!--li><a href="asignacion.aspx">Asignar</a></--li-->
                    <li><a href="verSeguimiento.aspx">Seguimiento</a></li>
                    <li><a href="verInicioFin.aspx">Inicio y Fin de Jornada</a></li>
                    <li>
                        <a href="#">Ver Ordenes</a>
                        <ul class="children">
                             <li><a href="verOrdenesPorEmpleado.aspx">Por Técnico</a></li>
                            <li><a href="verOrdenes.aspx">Por Número de Orden/Cte.</a></li>
                            <li><a href="verResumenEstados.aspx">Por Estados de la Orden</a></li>
                            <li><a href="verOrdenesporCalle.aspx">Por Calle</a></li>
                        </ul>
                    </li>
                        <li>
                            <a href="#">Catálogos</a>
                            <ul class="children">
                                <li><a href="empleadosABC.aspx">Técnicos</a></li>
                            </ul>
                            
                        </li>
  
                    <li><a href="logIn.aspx/?log=1">Salir</a></li>
                        <li></li>
                </ul>

                    <ul id="menu3" runat="server" visible="false">
                    <li><a href="envio.aspx">Importar</a></li>
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
                    <div class="searchStyle" style="width: 70% !important;">

                        <!--Centro-->&nbsp;
                    <asp:DropDownList Visible="false" ID="lstcentral" runat="server" Height="25px" Width="144px" AutoPostBack="True">
                        <asp:ListItem Value="CE000">Todos</asp:ListItem>
                        <asp:ListItem Value="CE001">Tampico</asp:ListItem>
                        <asp:ListItem Value="CE002">Madero</asp:ListItem>
            </asp:DropDownList>
                    
                    <br />
                        <br />
                    <div id="bigDiv" style="height: 130px;width:100% ;
                        overflow: scroll; overflow-x: hidden;" onscroll="SetDivPosition()">
                        <asp:GridView ID="gv_tecnicos" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            ForeColor="#333333" GridLines="Horizontal" OnRowDataBound="gv_tecnicos_RowDataBound"
                            ShowHeader="true" Style="width: 100%" Width="349px">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="nombreComp" ItemStyle-Width="60%" HeaderText="Técnico" />
                                <asp:BoundField DataField="id" HeaderText="Nombre">
                                    <ItemStyle CssClass="hide" />
                                    <HeaderStyle CssClass="hide" />
                                </asp:BoundField>
                                <asp:BoundField DataField="desconexiones" HeaderText="Cortes" ItemStyle-Width="40%">
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
                        <asp:Label ID="Label1" Style="float: left" runat="server" Text="Fecha:"></asp:Label>
                        <div style="width: auto; float: right">
                            <asp:TextBox ID="dia" runat="server"></asp:TextBox>
                            <!-- SM- Calendario-29-08-2014-->
                            <asp:CalendarExtender ID="cexfecha2" runat="server" TargetControlID="dia" Format="yyyyMMdd">
                                
 </asp:CalendarExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="dia"
                                ErrorMessage="* Error (AñoMesDía)" ValidationExpression="^\d{4}((0[1-9])|(1[012]))((0[1-9]|[12]\d)|3[01])$"
                                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" Font-Names="Segoe UI"
                                Font-Size="12px"></asp:RegularExpressionValidator>
                            <!-- SM- FIN Calendario-->
                            <br />
                            <asp:TextBox ID="dia2" runat="server"></asp:TextBox>
                            <!-- SM- Calendario-29-08-2014-->
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="dia2"
                                Format="yyyyMMdd">
                                
</asp:CalendarExtender>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="dia2"
                                ErrorMessage="* Error (AñoMesDía)" ValidationExpression="^\d{4}((0[1-9])|(1[012]))((0[1-9]|[12]\d)|3[01])$"
                                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" Font-Names="Segoe UI"
                                Font-Size="12px"></asp:RegularExpressionValidator>
                            <!-- SM- FIN Calendario-->
                        </div>
                        &nbsp;
                    </div>
                    <asp:CheckBox ID="ck_filtrarRealizadas" Text="Filtrar Trabajadas" runat="server" Visible="False" />
                </div>
                <div style="width: 100%; float: left; height: auto">
                    <asp:CheckBox ID="ck_PorUsuario" Text="Filtrar por Usuario" Checked="true" runat="server" Visible="False" />
                    <br />
                    <asp:CheckBox ID="ck_filtrarSinVerificar" runat="server" Text="Sin Verificar" Checked="True" />
                    <asp:CheckBox ID="ck_filtrarCorrectas" runat="server" Checked="True" Text="Correctas" />
                    <asp:CheckBox ID="ck_filtrarIncorrectas" runat="server" Checked="True" Text="Incorrectas" />
                    <br />
                    <asp:DropDownList ID="ddl_estadosOrden" runat="server">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddl_motivos" runat="server">
                        <asp:ListItem Value="0">Asignadas</asp:ListItem>
                         <asp:ListItem Value="2">Ejecutadas</asp:ListItem>
                        <asp:ListItem Value="4">Con Anomalia</asp:ListItem>
                        <asp:ListItem Value="1">Pendientes</asp:ListItem>
                        <asp:ListItem Value="9">Sin Registro</asp:ListItem>
                        <asp:ListItem Value="3">Autoreconectados</asp:ListItem>
                        <asp:ListItem Value="6">No visitadas</asp:ListItem>
                        <asp:ListItem Value="7">Incorrectas</asp:ListItem>
                        
                    </asp:DropDownList>
                     <!--SM 29-08-2014-->
            <!--SM 29-08-2014-->

                    <br />
                    <br />
                    <asp:Button ID="bObtenerOrdenes" runat="server" Style="font-family: 'Segoe UI'" Text="Obtener Ordenes" />
                    <asp:Button ID="Button1" runat="server" OnClientClick="nuevaVentana()" Text="Vista Captura" Visible="False" />
                    <asp:Button ID="bBajarFotos" runat="server" Text="Bajar fotografias" Visible="False" />
                    <asp:Button ID="bAExcel" runat="server" Text="Exportar" />
                    <asp:Button ID="b_verMapa" runat="server" Text="Ver Mapa" />
                    <asp:Button ID="b_imprimir" runat="server" Text="Imprimir" />
                    <asp:Button ID="Button2" runat="server" Text="Ver PDF" Visible="False" />
                </div>
            </div>
        </div>
<div id="contenido">
    <asp:Label ID="lblTecnico" runat="server" Visible="false" Text="Label"></asp:Label>
            <asp:DataGrid ID="dgOrdenes" runat="server" AllowSorting="true" AutoGenerateColumns="False"
                Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False"
                Font-Strikeout="False" Font-Underline="False" ForeColor="Black">
                <AlternatingItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                    Font-Overline="False"  Font-Strikeout="False" Font-Underline="False"
                    ForeColor="Black" />
                <Columns>
                    <asp:BoundColumn DataField="tecnico" HeaderText="Técnico" />
                    <asp:TemplateColumn HeaderText="Orden">
                        <ItemTemplate>
                            <asp:LinkButton ID="hlNumOrden" runat="server" OnClick="LinkButton_Click" Text='<%#Container.DataItem("numOrden")%>'></asp:LinkButton>
                            &nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="poliza" HeaderText="Póliza" />
                    <asp:TemplateColumn>
                        <ItemTemplate >
                            <asp:Repeater ID="RepeaterImages"  runat="server" DataSource= '<%# mostrarImagenes(Container.DataItem("numOrden"), Container.DataItem("poliza"), Container.DataItem("sinRegistro"), Container.DataItem("fechadeejecucion"))%>' >
                                <ItemTemplate>
                                    <asp:Image ID="Image" runat="server"   style="width:268px; height:auto" ImageUrl='<%# Container.DataItem %>' />
                                </ItemTemplate>
                            </asp:Repeater>
                           
                           
                        </ItemTemplate>
                        <ItemStyle Width="268px" />
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="numMedidor" HeaderText="Número de Medidor" />
                    <asp:BoundColumn DataField="accionCorte" HeaderText="Accion de Corte" />
                    <asp:BoundColumn DataField="motivo" HeaderText="Anomalia" />
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# getDireccion(Container.DataItem("calle"), "", Container.DataItem("numInterior"), Container.DataItem("colonia"), Container.DataItem("municipio"))%>'>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                     <asp:BoundColumn DataField="fechadeinicio" HeaderText="Fecha de Inicio" />
                    <asp:BoundColumn DataField="fechadeejecucion" HeaderText="Fecha de Ejecucion" />
                    <asp:BoundColumn DataField="comentarios" HeaderText="Comentarios" />

                    
                </Columns>
                <HeaderStyle BackColor="#004FC5" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                <ItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                    Font-Overline="False"  Font-Strikeout="False" Font-Underline="False"
                    ForeColor="Black" />
                <SelectedItemStyle BackColor="#004FC5" Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI"
                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
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
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                        ForeColor="Black" />
                    <Columns>
                        <asp:TemplateColumn HeaderText="Orden">
                            <ItemTemplate>
                                <asp:LinkButton ID="hlNumOrden" runat="server" OnClick="LinkButton_Click" Text='<%# Container.DataItem("numOrden")%>'></asp:LinkButton>
                                &nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="numMedidor" HeaderText="Medidor" />
                        <asp:BoundColumn DataField="cliente" HeaderText="Cliente" />
                    </Columns>
                    <HeaderStyle BackColor="#004FC5" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                    <ItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                        ForeColor="Black" />
                    <SelectedItemStyle BackColor="#004FC5" Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI"
                        Font-Overline="False"  Font-Strikeout="False" Font-Underline="False"
                        ForeColor="White" />
                </asp:DataGrid>
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
                        <asp:Button ID="btnaceptar" runat="server" Text="Aceptar" BackColor="#004FC4" Font-Names="Segoe UI"
                            ForeColor="#EAEAEA" BorderStyle="None" Font-Size="18px" />
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div id="pie">
        </div>
    </div>
    </form>
</body>
</html>

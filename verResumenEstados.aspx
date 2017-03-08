<%@ Page Language="VB" AutoEventWireup="false" CodeFile="verResumenEstados.aspx.vb"
    Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ordenes por Estados</title>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
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
                    <li><a href="#">Ver Ordenes</a>
                        <ul class="children">
                            <li><a href="verOrdenesPorEmpleado.aspx">Por Técnico</a></li>
                            <li><a href="verOrdenesPorEmpleadoFotos.aspx">Con Foto</a></li>
                            <li><a href="verOrdenes.aspx">Por Número de Orden/Cte.</a></li>
                            <li><a href="verResumenEstados.aspx">Por Estados de la Orden</a></li>
                            <li><a href="verOrdenesporCalle.aspx">Por Calle</a></li>
                             <li><a href="verCambiosDeMedidor.aspx">Cambio de Medidor</a></li>
                    <li><a href="verNoRegistrados.aspx">No Registrados</a></li>
                        </ul>
                    </li>
                    <li><a href="#">Catálogos</a>
                        <ul class="children">
                            <li><a href="empleadosABC.aspx">Técnicos</a></li>
                        </ul>
                    </li>
                    <li><a href="logIn.aspx/?log=1">Salir</a></li>
                    <li></li>
                </ul>
            </div>
        </div>
        <div id="contenido">
            <div id="blanco" style="background-color: white">
            </div>
            <div id="navegador"  style="background-color:transparent">
            </div>
            <br />
            <br />
            &nbsp;&nbsp;&nbsp;Fecha:
            <asp:TextBox ID="fecha1" runat="server" ToolTip="AñoMesDia &quot;20001231&quot;"></asp:TextBox>
            <asp:CalendarExtender ID="cexfecha1" runat="server" TargetControlID="fecha1" Format="yyyyMMdd"
                TodaysDateFormat="yyyy/MM/dd">
            </asp:CalendarExtender>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="fecha1"
                ErrorMessage="* Error (AñoMesDía)" ValidationExpression="^\d{4}((0[1-9])|(1[012]))((0[1-9]|[12]\d)|3[01])$"
                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" Font-Names="Segoe UI"
                Font-Size="12px"></asp:RegularExpressionValidator>
            &nbsp; a &nbsp;
            <asp:TextBox ID="fecha2" runat="server" ToolTip="AñoMesDia &quot;20001231&quot;"></asp:TextBox>&nbsp;&nbsp;&nbsp;
            <asp:CalendarExtender ID="cexfecha2" runat="server" TargetControlID="fecha2" Format="yyyyMMdd"
                TodaysDateFormat="yyyy/MM/dd">
            </asp:CalendarExtender>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="fecha2"
                ErrorMessage="* Error (AñoMesDía)" ValidationExpression="^\d{4}((0[1-9])|(1[012]))((0[1-9]|[12]\d)|3[01])$"
                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" Font-Names="Segoe UI"
                Font-Size="12px"></asp:RegularExpressionValidator>
            <asp:Button ID="bRetrieve" runat="server" Text="Seleccionar" CssClass="buttongrl" />
            <br />
            <br />
            <asp:DataGrid ID="dgOrdenes" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False"
                Font-Strikeout="False" Font-Underline="False" ForeColor="Black" Style="text-align: center;
                top: 0; left: 0px;">
                <AlternatingItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                    Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False"
                    ForeColor="Black" />
                <Columns>
                    <asp:BoundColumn DataField="nombre" HeaderText="Técnico">
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="Transmitidas">
                        <ItemTemplate>
                            <a id="A2" runat="server" href='<%#getLinkVerResumen(Container.DataItem("transmitidas"), Container.DataItem("idEmpleado"), "EO001", fecha1.Text, fecha2.Text)%>'>
                                <%# Container.DataItem("transmitidas")%></a> &nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="No Realizadas">
                        <ItemTemplate>
                            <a runat="server" href='<%#getLinkVerResumen(Container.DataItem("noRealizadas"), Container.DataItem("idEmpleado"), "EO002", fecha1.Text, fecha2.Text)%>'>
                                <%# Container.DataItem("noRealizadas")%></a> &nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Cierres Forzados">
                        <ItemTemplate>
                            <a id="A1" runat="server" href='<%#getLinkVerResumen(Container.DataItem("cierreForzado"), Container.DataItem("idEmpleado"), "EO012", fecha1.Text, fecha2.Text)%>'>
                                <%# Container.DataItem("cierreForzado")%></a> &nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Ejecutadas">
                        <ItemTemplate>
                            <a id="A1" runat="server" href='<%#getLinkVerResumen(Container.DataItem("ejecutadas"), Container.DataItem("idEmpleado"), "EO004", fecha1.Text, fecha2.Text)%>'>
                                <%# Container.DataItem("ejecutadas")%></a> &nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Pagadas">
                        <ItemTemplate>
                            <a id="A1" runat="server" href='<%#getLinkVerResumen(Container.DataItem("pagadas"), Container.DataItem("idEmpleado"), "EO005", fecha1.Text, fecha2.Text)%>'>
                                <%# Container.DataItem("pagadas")%></a> &nbsp;&nbsp;
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn Visible="false" DataField="idEmpleado" HeaderText="id"></asp:BoundColumn>
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

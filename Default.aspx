<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

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
        <div id="contenido">
            <div id="blanco" style="background-color: white">
            </div>
            <div id="navegador" style="background-color:transparent">
            </div>
            <br />
            <asp:DataGrid ID="dgOrdenes" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False"
                Font-Strikeout="False" Font-Underline="False" ForeColor="Black">
                <AlternatingItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI"
                    Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False"
                    ForeColor="Black" />
                <Columns>
                    <asp:BoundColumn DataField="nombre" HeaderText="Técnico" />
                    <asp:BoundColumn DataField="desconexionesAsign" HeaderText="Lecturas Asignados">
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Desconexiones" HeaderText="Lecturas Pendientes">
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundColumn>
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

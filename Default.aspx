<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>
<!DOCTYPE html>



<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Ver Ordenes</title>
    <LINK REL="Stylesheet" HREF="style.css" TYPE="text/css">
    <LINK REL="Stylesheet" HREF="menu.css" TYPE="text/css">
    <link rel="icon" 
      type="image/png" 
      href="botones/favicon.png">

    <script src="Scripts/jquery-2.0.3.min.js" type="text/javascript"></script>
<script src="Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>
<script type = "text/javascript">
    $(document).ready(function () {
        $('#<%=dgOrdenes.ClientID%>').Scrollable({
            ScrollHeight: 500%
        });
    });
</script>
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
            <div id="blanco" style="background-color:white">

            </div>
            
            <div id="cabecera">

                <div style="width:auto;height:auto;min-height:50px; float:left">
                    <a href="default.aspx">
                    <img alt="Enruta Lectura y Reparto"  style="float:left" height="50px" src="botones/enruta.jpg" />
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

                
            </div>

            <div id="contenido">
                
           <br />
               
                    <!--SM 29-08-2014-->
                    &nbsp;<asp:DropDownList ID="lstcentral" runat="server" Height="25px" Width="144px" AutoPostBack="True" Visible="false">
                <asp:ListItem Value="CE000">Todos</asp:ListItem>
                <asp:ListItem Value="CE001">Tampico</asp:ListItem>
                <asp:ListItem Value="CE002">Madero</asp:ListItem>
            </asp:DropDownList>
            <!--SM 29-08-2014-->

                    <br />

                </asp:Label>
                
           
                <br />
                <br />
                
            
            
                <asp:DataGrid ID="dgOrdenes" runat="server" AllowSorting="True" AutoGenerateColumns="False" Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="Black">
                    <AlternatingItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False" ForeColor="Black" />
                    <Columns>
                        
                        <asp:BoundColumn DataField="nombre" HeaderText="Técnico" >
                            <ItemStyle Width="41%"/>
                            <HeaderStyle  Width="41%"/>
                            </asp:BoundColumn>
                        <asp:BoundColumn DataField="desconexionesAsign" HeaderText="Cortes Asignados"> <ItemStyle HorizontalAlign="Center" width="28%" /> <HeaderStyle HorizontalAlign="Center" width="28%" /></asp:BoundColumn>
                        
                        <asp:BoundColumn DataField="Desconexiones" HeaderText="Cortes Pendientes" > <ItemStyle HorizontalAlign="Center" Width="29%" /> <HeaderStyle HorizontalAlign="Center" Width="29%" /> </asp:BoundColumn>
                       
                        
                    </Columns>
                    <HeaderStyle BackColor="#004FC5" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                    <ItemStyle BackColor="#EAEAEA" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False" ForeColor="Black" />
                    <SelectedItemStyle BackColor="#004FC5" Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                </asp:DataGrid>
                
           
            </div>
            <div id="pie">

            </div>

        </div>
       
    </form>
</body>
</html>

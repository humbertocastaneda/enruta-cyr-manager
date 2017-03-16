<%@ Page Language="VB" AutoEventWireup="false" CodeFile="verOrdenes.aspx.vb" Inherits="_Default" EnableEventValidation="false"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ver Ordenes</title>
    <link rel="Stylesheet" href="style.css" type="text/css">
    <link rel="Stylesheet" href="menu.css" type="text/css">
    <link rel="icon" type="image/png" href="botones/favicon.png">
    <script type="text/javascript" src="[JS library]"></script>
    <!--script language="javascript" type="text/javascript">

        window.onload = resize;

        function resize() {

            
            
            var antes = $('#<%= iAntes.ClientID %>');
            var widthTmp = $(window).width() * .5;
            var heightTmp = (antes.clientHeight / antes.clientWidth) * widthTmp;


            antes.clientWidth = widthTmp;
            antes.clientHeight = heightTmp;

            var despues = $('#<%= iDespues.ClientID%>');
            widthTmp = $(window).width() * .5;
            heightTmp = (despues.clientHeight / despues.clientWidth) * widthTmp;

            despues.clientWidth = widthTmp;
            despues.clientHeight = heightTmp;
        }
    </--!script--!>

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
                <div style="margin-left: 20%; margin-right: 20%">
                    <div style="width: auto; float: left; margin-left: auto">
                        <asp:RadioButtonList ID="rbTipoBusqueda" runat="server">
                            <asp:ListItem Value="ord.idOrden">Orden</asp:ListItem>
                            <asp:ListItem Value="cli.poliza" Selected="True">Cliente</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div style="width: auto; float: left; margin-left: 20px; height: 100%; margin-top: 10px">
                        Buscar:&nbsp;<asp:TextBox ID="txtBuscar" runat="server" Width="207px" Style="text-align: left"></asp:TextBox>
                        <asp:Button ID="Button1" runat="server" Text="Seleccionar" />
                    </div>
                </div>
            </div>
            <div id="ordenes">
                <asp:ListBox ID="lbOrdenes" runat="server" Width="350px" AutoPostBack="true"></asp:ListBox>
            </div>
        </div>
        <div id="contenido" style="display: inline-block; padding-left: 5%" runat="server">
            <div id="DatosOrden" style="height: auto; width: 70%; float: left" runat="server">
                <asp:Button ID="bAtras" Style="margin: 10px;" runat="server" Text="&lt;" Width="23px" />
                <asp:Button ID="bIrATarea" Style="margin: 10px;" runat="server" Text="Ir a la Tarea" />
                <asp:Button ID="bIrTabla" Style="margin: 10px;" runat="server" Text="Ir a la Tabla" />
                &nbsp;<asp:Button ID="bSiguiente" Style="margin: 10px;" runat="server" Text="&gt;" />
                <br />
                <asp:Label ID="Label29" runat="server" Text="Datos del Cliente" Style="font-size: large;
                    font-style: italic;"></asp:Label>
                <br />
                <table style="width: 100%; word-break: break-all;">
                    <tr>
                        <td>
                            <asp:Label ID="Label25" runat="server" class="label" Text="Cliente"></asp:Label>
                        <br />
                            <asp:Label ID="Label26" runat="server" class="text" Text="<%# ls_poliza%>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label6" class="label" runat="server" Text="Nombre"></asp:Label>
                        <br />
                            <asp:Label ID="Label8" class="text" runat="server" Text="<%# ls_nombre%>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" colspan="2">
                            <asp:Label ID="Label10" class="label" runat="server" Text="Direccion"></asp:Label>
                        <br />
                            <asp:Label ID="Label21" class="text" runat="server" Text="<%# ls_direccion%>"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <asp:Label ID="Label14" class="label" runat="server" Text="Tipo de Usuario"></asp:Label>
                        <br />
                            <asp:Label ID="Label15" class="text" runat="server" Text="<%# ls_tipoUsuario%>"></asp:Label>
                        </td>

                         <td>
                            <asp:Label ID="Label23" runat="server" Text="Giro" class="label"></asp:Label>
                        <br />
                            <asp:Label ID="Label27" runat="server" Text="<%# ls_giro%>" class="text"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="Label30" runat="server" Text="Datos de la Orden" Style="font-size: large;
                    font-style: italic;"></asp:Label>
                <br />
                <table style="width: 100%; word-break: break-all;">
                    <tr>
                      
                        <td>
                            <asp:Label ID="Label11" runat="server" class="label" Text="Número de Medidor"></asp:Label>
                       <br />
                            <asp:Label ID="Label12" class="text" runat="server" Text="<%# ls_medidor%>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label19" class="label" runat="server" Text="Técnico"></asp:Label>
                        <br />
                            <asp:Label ID="Label20" class="text" runat="server" Text="<%# ls_tecnico%>"></asp:Label>
                        </td>
                        
                    </tr>

                     <tr>
                        <td>
                            <asp:Label ID="Label16" runat="server" Text="Diametro" class="label"></asp:Label>
                        <br />
                            <asp:Label ID="Label22" runat="server" Text="<%# ls_diametro%>" class="text"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Tipo de Orden" class="label"></asp:Label>
                       <br />
                            <asp:Label ID="lblTipoDeOrden" runat="server" Text="<%# ls_tipoDeOrden%>" class="text"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                         <td>
                            <asp:Label ID="Label32" class="label" runat="server" Text="Ultimo Pago"></asp:Label>
                        <br />
                            <asp:Label ID="Label33" class="text" runat="server" Text="<%# ls_ultimoPago%>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label28" class="label" runat="server" Text="Fecha Ultimo Pago"></asp:Label>
                        &nbsp;
                        <br />
                            <asp:Label ID="Label34" runat="server" Text="<%# ls_fechaUltimoPago%>" class="text"></asp:Label>
                        </td>
                        
                       
                    </tr>
                    <tr>
                        <td >
                            <asp:Label ID="Label37" runat="server" Text="Balance" class="label"></asp:Label>
                        <br />
                            <asp:Label ID="Label38" runat="server" Text="<%# ls_balance%>" class="text"></asp:Label>
                        </td>

                       <td>
                            <asp:Label ID="Label36" runat="server" Text="Vencido" class="label"></asp:Label>
                        <br />
                            <asp:Label ID="Label35" runat="server" Text="<%# ls_vencido%>" class="text"></asp:Label>
                        </td>

                    </tr>

                   

                   
                    <tr>
                        <td >
                            <asp:Label ID="Label24" runat="server" class="label" Text="Edo. Rev."></asp:Label>
                        <br />
                            <asp:Label ID="Label40" runat="server" class="text" Text="<%# ls_descEstadoRev%>"></asp:Label>
                        
                        
                        
                            <asp:Button ID="bCorrecta" runat="server" Text="Correcta" />
                            <asp:Button ID="bIncorrecta" runat="server" Text="Incorrecta" />
                        </td>

                         
                    </tr>
                </table>
                <br />
                <asp:Label ID="Label31" runat="server" Text="Datos de la Ejecución" Style="font-size: large;
                    font-style: italic;"></asp:Label>
                <br />
                <table style="width: 100%; word-break: break-all;">
                     <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" class="label" Text="Estado de la Orden"></asp:Label>
                        <br />
                            <asp:Label ID="lblEstadoDeLaOrden" runat="server" class="text" Text="<%# ls_estadoDeLaOrden%>"></asp:Label>
                        </td>
                       <td >
                            <asp:Button ID="bPagada" runat="server" Text="Pagada" /><asp:Button ID="bCancelada"
                                runat="server" Text="Cancelada" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" class="label" Text="Fecha de Inicio"></asp:Label>
                        <br />
                            <asp:Label ID="Label13" class="text" runat="server" Text="<%# ls_fechaDeInicio%>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label7" runat="server" class="label" Text="Fecha de Ejecución"></asp:Label>
                        <br />
                            <asp:Label ID="lblFechaDeEjecucion" class="text" runat="server" Text="<%# ls_fechaDeEjecucion%>"></asp:Label>
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <asp:Label ID="Label9" runat="server" class="label" Text="Acción de Corte"></asp:Label>
                        <br />
                             <asp:Label ID="lbl_accionCorte" class="text" runat="server" Text="<%# ls_accion%>"></asp:Label>
                        </td>

                        <td>
                            <asp:Label ID="Label5" runat="server" class="label" Visible="<%#motivoVisible%>"
                                Text="Anomalia"></asp:Label>
                        <br />
                            <asp:Label ID="lblMotivo" class="text" runat="server" Visible="<%#motivoVisible%>"
                                Text="<%#ls_motivo%>"></asp:Label>
                        </td>
                    </tr>

                    
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label17" class="label" runat="server" Text="Comentarios"></asp:Label>
                        <br />
                            <asp:Label ID="Label18" class="text" runat="server" Text="<%# ls_comentarios%>"></asp:Label>
                        </td>
                    </tr>
                   
                    
                    <tr>
                         <td colspan="2">
                            <asp:Label ID="Label4" class="text" runat="server" Text="<%# ls_habitada%>"></asp:Label>
                        </td>
                    </tr>

                     <tr>
                         <td colspan="2">
                           <asp:Button ID="ver_mapa" runat="server" Text="Ver Mapa" />
                <asp:Button ID="b_imprimir" runat="server" Text="Imprimir" />
                <asp:Button ID="Button2" runat="server" Text="Ver PDF" />
                        </td>
                    </tr>
                </table>
                <br />
                
                <input id="ruta" runat="server" style="visibility: hidden" type="text" /><br />
                <br />
            </div>
            <div runat="server" id="datosCliente" style="width: 30%; float: right; height: auto;">
                <asp:Repeater ID="RepeaterImages" runat="server">
                    <ItemTemplate>
                        <asp:Image ID="Image" runat="server"  Style="width: 100%;" ImageUrl='<%# Container.DataItem %>' />
                    </ItemTemplate>
                </asp:Repeater>

                <div id="dAntes" style="float: left; width: 100%; margin-right: 20px; height: auto">
                    
                    

                    <br />
                    <asp:Image ID="iAntes" Style="width: 100%;" runat="server" Visible="False" />
                    <br />
                    
                </div>
                <div id="dDespues" style="float: right; width: 100%; height: auto">
                    
                    <br />
                    <asp:Image ID="iDespues" Style="width: 100%;" runat="server" Visible="False" />
                    <br />
                    
                </div>
            </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
        

        </div>
        <div id="pie">
        </div>
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
    </form>
</body>
</html>

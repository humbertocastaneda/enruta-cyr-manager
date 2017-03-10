<%@ Page Language="VB" AutoEventWireup="false" CodeFile="verOrdenes.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Ver Lecturas</title>
    <link rel="Stylesheet" href="style.css" type="text/css">
    <link rel="Stylesheet" href="menu.css" type="text/css">
    <link rel="icon" type="image/png" href="botones/favicon.png">
    <script type="text/javascript" src="[JS library]"></script>
    <script type="text/javascript" src="Scripts/jquery-2.0.3.min.js"></script>
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
    <style type="text/css">
        .auto-style8
        {
            height: 32px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
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
                <div style="margin-left: 20%; margin-right: 20%; width:100%">
                    <div style=" float: left; margin-left: 20px; margin-top: 10px; width:100%">
                        <br />
                        Clave Usuario:&nbsp;<asp:TextBox ID="txtBuscar" runat="server" Width="200px" Style="text-align: left"
                            Height="21px"></asp:TextBox>  &nbsp;<asp:Button ID="Button1" runat="server" Text="Seleccionar" CssClass="buttongrl" />
                        
                    </div>
                   
                </div>
            </div>
            <div id="ordenes">
                <div style="margin-top: 10px; margin-left: 43px; margin-bottom: 14px; ">
                     Historico de Lecturas<br />
                <asp:ListBox ID="lbOrdenes" runat="server"  AutoPostBack="true" 
                    Height="87px" width="318px" >
                </asp:ListBox>
                </div>
               
            </div>
        </div>
    <div id="contenido" >
        <div id="blanco" style="background-color: white">
            <br />
            <br />
            <br />
            <br />
        </div>
             
        <div id="DatosOrden" style="height: auto; width: 70%; float: left" runat="server">
            <asp:Button ID="bAtras" Style="margin: 10px;" runat="server" Text="&lt;" Width="23px"
                CssClass="buttongrl" />
            <asp:Button ID="bIrATarea" Style="margin: 10px;" runat="server" Text="Ir a la Tarea"
                CssClass="buttongrl" />
            <asp:Button ID="bIrTabla" Style="margin: 10px;" runat="server" Text="Ir al Resumen"
                CssClass="buttongrl" />
            &nbsp;<asp:Button ID="bSiguiente" Style="margin: 10px;" runat="server" Text="&gt;"
                CssClass="buttongrl" />
            <br />
            <asp:Label ID="Label29" runat="server" Text="Datos de la Poliza" Style="font-size: large;
                font-style: italic;"></asp:Label>
            <br />
            <table style="width: 100%;word-break:break-all;">
                <tr>
                     <td>
                        <asp:Label ID="Label6" class="label" runat="server" Text="Clave Ubicacion:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label8" class="text" runat="server" Text="<%# ls_sectorCorto%>"></asp:Label>
                    </td>

                    <td>
                        <asp:Label ID="Label25" runat="server"  Text="Contrato:" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label26" runat="server"  Text="<%# ls_poliza %>" 
                            CssClass="text"></asp:Label>
                    </td>
                   
                </tr>
                <tr>
                     <td>
                        <asp:Label ID="Label9" class="label" runat="server" Text="Cliente:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label13" class="text" runat="server" Text="<%# ls_nombre%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label10" class="label" runat="server" Text="Direccion:" 
                            CssClass="label"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="Label21" class="text" runat="server" Text="<%# ls_direccion %>" 
                            CssClass="text"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="Label30" runat="server" Text="Datos de la Lectura" Style="font-size: large;
                font-style: italic;" CssClass="label"></asp:Label>
            <br />
            <table style="width: 100%;word-break:break-all;">
                    <tr>
                        <td>
                            <asp:Label ID="Label11" runat="server" class="label" Text="Serie de Medidor:" 
                                CssClass="label"></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:Label ID="Label12" class="text" runat="server" Text="<%# ls_medidor %>" 
                                CssClass="text"></asp:Label>
                        </td>
                    </tr>

                <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" class="label" Text="Tipo de Lectura" 
                                CssClass="label"></asp:Label>
                        </td>
                        <td colspan="4">
                            <asp:Label ID="Label4" class="text" runat="server" Text="<%# ls_estadoDeLaLectura%>" 
                                CssClass="text"></asp:Label>
                        </td>
                    </tr>
                <tr>

                <td >
                        <asp:Label ID="Label1" runat="server" class="label"
                            Text="Lectura:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label2" class="text" runat="server" 
                            Text="<%#ls_lectura%>"></asp:Label>
                    </td>   

                    <td >
                        <asp:Label ID="Label5" runat="server" class="label" 
                            Text="Anomalia:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMotivo" class="text" runat="server"
                            Text="<%#ls_motivo%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server"  Text="Fecha de Ejecución" 
                            CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblFechaDeEjecucion" class="text" runat="server" Text="<%# ls_fechaDeEjecucion%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style8">
                        <asp:Label ID="Label17" class="label" runat="server" Text="Comentarios"></asp:Label>
                    </td>
                    <td colspan="4" class="auto-style8">
                        <asp:Label ID="Label18" class="text" runat="server" Text="<%# ls_comentarios%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                     <td>
                        <asp:Label ID="Label19" class="label" runat="server" Text="Lecturista" 
                            CssClass="label"></asp:Label>
                    </td>
                    <td colspan="4">
                        <asp:Label ID="Label20" class="text"  runat="server" Text="<%# ls_tecnico %>" 
                            CssClass="text"></asp:Label>
                    </td>
                </tr>
                <tr>
                        <td colspan="2">
                        <asp:Label ID="Label39" class="label" runat="server" Text="Lectura"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="tb_lectura" Text="<%# ls_lectura%>" runat="server"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="b_guardaLectura" runat="server" Text="Guardar Lectura" />
                    </td>
                </tr>
                <tr>
                        <td colspan="2">
                        <asp:Label ID="Label15" class="label" runat="server" Text="Anomalia"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="tb_anomalia" Text="<%# ls_anomalia%>" runat="server"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="b_guardaAnomalia" runat="server" Text="Guardar Anomalia" />
                    </td>
                </tr>
                <tr>
                        <td colspan="2">
                        <asp:Label ID="Label14" class="label" runat="server" Text="Comentario"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="tb_comentario" Text="<%# ls_comentarios%>" runat="server"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="b_guardaComentario" runat="server" Text="Guardar Comentario" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="ver_mapa" runat="server" Text="Ver Mapa" CssClass="buttongrl" />
            <asp:Button ID="b_imprimir" runat="server" Text="Imprimir" CssClass="buttongrl" />
            <input id="ruta" runat="server" style="visibility: hidden" type="text" /><br />
            <br />
        </div>
        <div runat="server" id="datosCliente" style="width: 28%; float: right; height: auto;">
            <asp:Repeater ID="RepeaterImages" runat="server">
                <ItemTemplate>
                    <asp:Image ID="Image" runat="server" Style="width: 100%;" ImageUrl='<%# Container.DataItem %>' />
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
        bottom: 0px; background-color: #004FC4; left: -2px; right: 2px; color: #FFFFFF;">
        <br />
        <%--AQUI VA EL MENSAJE DEL FOOTER--%>
        <br />
    </div>
    </form>
</body>
</html>

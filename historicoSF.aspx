<%@ Page Language="VB" AutoEventWireup="false" CodeFile="historicoSF.aspx.vb" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>



<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Exportar</title>
    <LINK REL="Stylesheet" HREF="style.css" TYPE="text/css">
    <LINK REL="Stylesheet" HREF="menu.css" TYPE="text/css">
    <link rel="icon" 
      type="image/png" 
      href="botones/favicon.png">
    <script type="text/javascript">




        function alertaNoTodasAsignadas() {

                return confirm("Esta a punto de borrar las ordenes del dia " + document.getElementById('txtfecha').value + " ¿Desea continuar?");
            
        }




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

               <div style="width:auto;height:auto; float:left">
                    <!--a href="default.aspx">
                    <img alt="Enruta Lectura y Reparto"  style="float:left" height="50px" src="botones/enruta.jpg" />
                        </a-->
                   <a href="default.aspx">
                       <img alt="Enruta Lectura y Reparto" style="float: left" height="50px" src="botones/enruta.jpg" />
                   </a>
                   <div id="contenedor">
                <ul id="menu">
                    <li><a href="logIn.aspx/?log=1">Salir</a></li>
                </ul>
            </div>
                </div>
                
                <!--div id="contenedor">
                    <ul id="menu">
                    <li><a href="envio.aspx">Importar</a></li>
                    <li><a href="asignacion.aspx">Asignar</a></li>
                    <li><a href="verSeguimiento.aspx">Seguimiento</a></li>
                    <li>
                        <a href="#">Ver Ordenes</a>
                        <ul class="children">
                             <li><a href="verOrdenesPorEmpleado.aspx">Por Técnico</a></li>
                            <li><a href="verOrdenes.aspx">Por Número de Orden/Cte.</a></li>
                            <li><a href="verResumenEstados.aspx">Por Estados de la Orden</a></li>
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
                </div-->
                
            </div>
            <div id="navegador">

                <div id="buscar">
                    <div style="margin-left:20%; margin-right:20%">
                        <div style="width:auto; float:left; margin-left:auto">
                    </div>

                    <div style="width:auto; float:left; margin-left:20px; height:100%; margin-top:10px ">
                    </div>
                    </div>
                    
                    
                    

                </div >

                <div id="ordenes">

                </div>
                
            </div>
            <asp:Label ID="lbl_msj" runat="server" Text="Hola"  style="visibility:hidden">
                </asp:Label>
                    
            <div id="contenido" >
                
                     
                    <br />
                    Seleccione la fecha de la cual desea hacer un respaldo o borrar fotos<br />
                
                     
                    <br />
                    <div style="text-align: left">
                        <asp:TextBox ID="txtfecha" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
                        <asp:CalendarExtender ID="cexfecha1" runat="server" Format="yyyyMMdd" TargetControlID="txtfecha" TodaysDateFormat="yyyy/MM/dd">
                        </asp:CalendarExtender>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtfecha" Display="Dynamic" ErrorMessage="*Fecha Invalida" Font-Names="Segoe UI" Font-Size="12px" ForeColor="Red" SetFocusOnError="True" ValidationExpression="^\d{4}((0[1-9])|(1[012]))((0[1-9]|[12]\d)|3[01])$"></asp:RegularExpressionValidator>
                        <asp:Button ID="b_decargar0" runat="server" Text="Seleccionar" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button1" OnClientClick="return alertaNoTodasAsignadas()" runat="server" Text="Eliminar" />
                    </div>
    
                    <br />
    
                    <br />
    
                    <br />
                    
                    <br />
                    
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                   

                         


            </div>
            <div id="pie">

            </div>
<!— CUADRO DE DIALOGO -->
        <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />
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
       
 <!—FIN DEL CUADRO DE DIALOGO -->


        </div>
       
    </form>
</body>
</html>

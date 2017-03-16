<%@ Page Language="VB" AutoEventWireup="false" CodeFile="envio.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>



<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Importar</title>
    <LINK REL="Stylesheet" HREF="style.css" TYPE="text/css">
    <LINK REL="Stylesheet" HREF="menu.css" TYPE="text/css">
    <link rel="icon" 
      type="image/png" 
      href="botones/favicon.png">

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
                <div style="width:50%; height:50%; margin:auto" >
                     
                    Seleccione el archivo a cargar<br />
                    <div style="text-align: right"><asp:Button ID="b_cargar" runat="server" Text="Cargar" />
                
                             <asp:Button ID="bEnviar" runat="server" Font-Names="Segoe UI" Text="Enviar al Celular" OnClientClick="return alertaNoTodasAsignadas()" />
                    
                    </div>
                    
    
                    <br />
                    <br />
                     <asp:Label ID="Label1" runat="server" Font-Names="Segoe UI">Fecha programada de las ordenes</asp:Label>
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
                    <asp:FileUpload ID="FileUploadControl" runat="server" />
                    <br />
                    <br />
                    Ultimos archivos cargados<br />
                    <div style="text-align: right"><asp:Button ID="bEliminar" runat="server" Text="Eliminar"  /></div>
                    
                    <br />
                    <asp:GridView ID="gv_tecnicos" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Horizontal" OnRowDataBound="gv_tecnicos_RowDataBound" ShowHeader="true" style="width:100%" Width="349px">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="nombre" HeaderText="Archivo" ItemStyle-Width="60%" />
                            <asp:BoundField DataField="id" HeaderText="">
                            <ItemStyle CssClass="hide" />
                            <HeaderStyle CssClass="hide" />
                            </asp:BoundField>
                            <asp:BoundField DataField="fecha" HeaderText="Fecha" ItemStyle-Width="40%">
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
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <!— CUADRO DE DIALOGO -->

                <asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" />

                <asp:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="lblOculto"
                    CancelControlID="btnaceptar" BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>

                <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Style="display: none">

                    <div align="center">

                        <div align="center" id="mensajes">

                            <asp:Label ID="lbldialogo" runat="server" Font-Names="Segoe UI"
                                CssClass="label"></asp:Label>

                        </div>

                        <div>

                            <asp:Button ID="btnaceptar" runat="server" Text="Aceptar" BackColor="#004FC4" Font-Names="Segoe UI"
                                ForeColor="#EAEAEA" BorderStyle="None" Font-Size="18px" />

                        </div>

                    </div>

                </asp:Panel>

                <!—FIN DEL CUADRO DE DIALOGO -->

            </div>
            <div id="pie">

            </div>

        </div>
       
    </form>
</body>
</html>

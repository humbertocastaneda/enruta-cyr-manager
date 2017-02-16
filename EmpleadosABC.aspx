<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EmpleadosABC.aspx.vb" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Técnicos</title>
     <LINK REL="Stylesheet" HREF="style.css" TYPE="text/css">
    <LINK REL="Stylesheet" HREF="menu.css" TYPE="text/css">
    <link rel="icon" 
      type="image/png" 
      href="botones/favicon.png">
    <style type="text/css">
        


        #lblAlerta {
            width: 100%;
            top: 0;
            margin-top:20px;
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

                <div id="buscar">
                    
                    <asp:Button ID="ib_nuevo" style="float:right" runat="server" Text="Nuevo" />
                    <asp:Button ID="ib_eliminar" style="float:right" runat="server" Text="Eliminar" Visible="False" />
                    
                </div >

                <div id="ordenes">

                    

                    <div style="width:100%; height:auto">
                            <asp:Button ID="ib_cancelar" style="float:right; " runat="server" Text="Cancelar" />
                            <asp:Button ID="ib_guardar" style="float:right" runat="server" Text="Guardar" />
                    </div>
                    
                    
                    

                </div>
                
            </div>
                <div id="contenido" >
                <div id="emp" runat="server" style="width:100%; display:inline-block">
                     <div style="width:50%; float:left">
                         <asp:Label Visible="false" style="margin-left: 20%;margin-top:35px;" runat="server">Filtrar por Centro</asp:Label> &nbsp;&nbsp;
                         
                         <asp:DropDownList Visible="false" style="margin-top:35px;" ID="ddl_FiltroEmp" runat="server" Height="25px" Width="146px" AutoPostBack="True">
                            <asp:ListItem Value="CE000">Sin Definir</asp:ListItem>
                            <asp:ListItem Value="CE001">Tampico</asp:ListItem>
                            <asp:ListItem Value="CE002">Madero</asp:ListItem>
                        </asp:DropDownList>
                         <br />
                         <br />
                    <asp:ListBox ID="lbEmpleados" style="height:150px !important; margin-bottom:35px; top: 0px; left: 0px;" AutoPostBack="true" runat="server" HorizontalContentAlignment="Stretch"></asp:ListBox>
                    
                    
                   
                    
                    
                </div>

                <div id="datos" runat="server" style="width:50%; float:left">
                    <asp:Label ID="lblId" runat="server"  width="123px" >Id: <%# idEmpleado%></asp:Label>
                    
                    <br />
                    <asp:Label ID="Label4" runat="server" Text="Estado del Técnico:"></asp:Label> &nbsp;
                    <asp:DropDownList ID="ddl_estadosTecnico" runat="server">
                        <asp:ListItem Selected="True" Value="EG001">Activo</asp:ListItem>
                        <asp:ListItem Value="EG002">Inactivo</asp:ListItem>
                    </asp:DropDownList>
                    <br /> <br />
                  <asp:Label Visible="false" ID="Label6" runat="server" Text="Central:" width="130px" height="21px" ></asp:Label> &nbsp;
                      <!--SM 29-08-2014-->
            <asp:DropDownList Visible="false" ID="lstcentral" runat="server" Height="25px" Width="146px">
                <asp:ListItem Value="CE000">Sin Definir</asp:ListItem>
                <asp:ListItem Value="CE001">Tampico</asp:ListItem>
                <asp:ListItem Value="CE002">Madero</asp:ListItem>
            </asp:DropDownList>
            <!--SM 29-08-2014-->
                    <br />
                     <br />
                    <asp:Label ID="label" runat="server" height="21px" Text="PTN:" width="130px"></asp:Label>
                    &nbsp;
                    <asp:TextBox ID="txtNomina" runat="server" Enabled="False"  Text=<%# nomina%>></asp:TextBox>
                    <asp:requiredFieldvalidator ID="CustomValidator1" runat="server" ErrorMessage="El número de PTN no puede estar vacio." ControlToValidate="txtNomina" ForeColor="Red"></asp:requiredFieldvalidator>
                    <asp:RegularExpressionValidator ID="CustomValidator3" runat="server" ControlToValidate="txtNomina" ErrorMessage="El número de PTN no puede contener letras." ForeColor="Red" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                    <br />
                    <br />
                    <asp:Label ID="label5" runat="server" Text="Alias" width="130px"></asp:Label>
                    &nbsp;
                    <asp:TextBox ID="txtAlias" runat="server"  width="222px"  Text=<%# apodo%>></asp:TextBox>
                    <br /><br />

                        <asp:Label ID="Label1" runat="server" Text="Nombre:" width="130px" height="21px" ></asp:Label> &nbsp;
                        <asp:TextBox ID="txtNombre" runat="server" Width="222px" Text=<%# nombre%> ></asp:TextBox><br />

                        <br />
                        <asp:Label ID="Label2" runat="server" Text="Apellido Paterno:" width="130px" height="21px" ></asp:Label> &nbsp;
                        <asp:TextBox ID="txtApPat" runat="server" Width="222px" Text=<%# apPat%> ></asp:TextBox><br />

                        <br />

                        <asp:Label ID="Label3" runat="server" Text="Apellido Materno:" height="21px" width="130px" ></asp:Label> &nbsp;
                        <asp:TextBox ID="txtApMat" runat="server" Width="222px"  Text=<%# apMat%> ></asp:TextBox>
                    <br />
                    <br />

                </div>
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

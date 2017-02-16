<%@ Page Language="VB" AutoEventWireup="false" CodeFile="asignacion.aspx.vb" Inherits="_Default" EnableEventValidation = "false" MaintainScrollPositionOnPostBack= true%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Asignar Ordenes</title>
    <LINK REL="Stylesheet" HREF="style.css" TYPE="text/css">
    <LINK REL="Stylesheet" HREF="menu.css" TYPE="text/css">
    <link rel="icon" 
      type="image/png" 
      href="botones/favicon.png">


    <!--[if (gte IE 6)&(lte IE 8)]>
      <script type="text/javascript" src="dist/selectivizr.js"></script>
      <noscript><link rel="stylesheet" href="[fallback css]" /></noscript>
    <![endif]-->

    <!--[if lt IE 9]>
    <script type="text/javascript" src="http://ie7-js.googlecode.com/svn/version/2.1(beta4)/IE9.js"></script>
    <![endif]-->

    <script type="text/javascript">
        

        /*window.onload = MaintainDivScrollPosition;

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
            eraseCookie('WTdivSampleScrollYAsignacion');
            createCookie('WTdivSampleScrollYAsignacion', intY, 1);
        }

        function MaintainDivScrollPosition() {


                var intY = readCookie('WTdivSampleScrollYAsignacion');
                document.getElementById("bigDiv").scrollTop = intY;
            
        }*/

        /*function setScroll(val) {
            document.getElementById("hidscrollPos").value = val.scrollTop;
        }

        function scrollTo(what) {
            document.getElementById(what).scrollTop =
        document.getElementById("hidscrollPos").value;
        }*/

        function alertaNoTodasAsignadas(){

            if (document.getElementById('inputRestantes').value > 0) {
                return confirm("Aún hay " + document.getElementById('inputRestantes').value + " ordene(s) por ser asignadas, ¿Desea continuar?");
            }
            else {
                return true;
            }
        }

        

        
    </script>



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

        #gv_tecnicos .Normal, .Alternate
        {
            cursor:default;
        }
        #gv_tecnicos .Normal:Hover, .Alternate:Hover
        {
            cursor:pointer;
        }

        </style>


        
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
                
                <div id="contenedor">
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
                </div>
                </div>
            <div id="navegador">


                <div id="buscar">
                    <div class="centrado">
                        <asp:DropDownList class="filtros" ID="ddl_tecnicos" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList class="filtros" ID="ddl_tipoOrden" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList class="filtros" ID="ddl_porcion" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList class="filtros" ID="ddl_zona" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList class="filtros" ID="ddl_unidad" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList class="filtros" ID="ddl_municipio" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList class="filtros" ID="ddl_colonia" runat="server">
                        </asp:DropDownList>
                    </div>
                    
                    
                </div >

                <div id="ordenes">

                    &nbsp;<div style="width:100%; margin-bottom:20px; height:auto">
                        <div style="width:auto; float:left; height: 27px;">
                            <asp:Label ID="mensajeSeleccionarTecnico" runat="server" Text="Para habilitar el botón guardar seleccione un técnico"></asp:Label>
                            <br />
                            <asp:Button ID="bFiltrar" style="margin-right:10px" runat="server" Text="Filtrar" />
                            <asp:Button ID="bSeleccionar" runat="server" Text="Seleccionar" />
                            <asp:Button ID="bDeseleccionar" runat="server" Text="Deseleccionar" />
                            <asp:Button ID="bAceptar" runat="server"  Font-Names="Segoe UI" Text="Guardar" />
                
                
                             <asp:Button ID="bEnviar" runat="server" Font-Names="Segoe UI" Text="Enviar al Celular" OnClientClick="return alertaNoTodasAsignadas()" />
                    <asp:Button ID="bObtenerOrdenes" runat="server" Text="Obtener Ordenes" Visible="false" />
                    
                
                           
                    </div>
                
                
                <input id="inputRestantes" type="text" style="visibility:hidden" runat="server" /></div>
                    
                    <div style="width:100%; float:left;  height:auto">
                        <asp:CheckBox ID="cbModoSeleccion" runat="server" Checked="true" Font-Names="Segoe UI" text="Modo Selección" />
                    </div>
                        
                    
                    

                
                

                </div>
                
            </div>

            <div id="contenido">
                
                <br />
                
                <asp:Label ID="ls_tecnico" Visible="false" runat="server" Text="Label"></asp:Label>
            
                <br />
                <asp:Label ID="lb_Info" runat="server" Text="" Visible="false"></asp:Label>
            
                <br />
            
                <br />
            
                <asp:DataGrid ID="dgOrdenes" runat="server" AllowSorting="true" AutoGenerateColumns="False" Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="Black">
                    <AlternatingItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False" ForeColor="Black" />
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbAsignadas" runat="server" Checked='<%# Container.DataItem("asignada")%>' AutoPostBack="false" />
                                <asp:Label ID="lbIdOrden" runat="server" text='<%# Container.DataItem("idOrden")%>' visible="false" />
                                &nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Número de Orden">
                            <ItemStyle HorizontalAlign="Center"  />
                            <ItemTemplate>
                                <asp:LinkButton ID="hlNumOrden" runat="server" OnClick="orden_Click" Text='<%# Container.DataItem("numOrden")%>'></asp:LinkButton>
                                &nbsp;&nbsp;
                            </ItemTemplate>
                            </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderText="Tipo de Orden">
                            <ItemTemplate>
                                <asp:LinkButton ID="hltipoDeOrden" runat="server" OnClick="LinkButton_Click" Text='<%# Container.DataItem("tipoDeOrden")%>'></asp:LinkButton>
                                &nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Porcion">
                            <ItemStyle HorizontalAlign="Center"  />
                            <ItemTemplate>
                                <asp:LinkButton ID="hlPorcion" runat="server" OnClick="LinkButton_Click" Text='<%# Container.DataItem("porcion")%>'></asp:LinkButton>
                                &nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Unidad">
                            <ItemStyle HorizontalAlign="Center"  />
                            <ItemTemplate>
                                <asp:LinkButton ID="hlUnidad" runat="server" OnClick="LinkButton_Click" Text='<%# Container.DataItem("unidad")%>'></asp:LinkButton>
                                &nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Municipio">
                            <ItemTemplate>
                                <asp:LinkButton ID="hlMunicipio" runat="server" OnClick="LinkButton_Click" Text='<%# Container.DataItem("municipio")%>'></asp:LinkButton>
                                &nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        <asp:TemplateColumn HeaderText="Colonia" SortExpression="colonia desc">
                            <ItemTemplate>
                                <asp:LinkButton ID="hlColonia" runat="server" OnClick="LinkButton_Click" Text='<%# Container.DataItem("colonia")%>'></asp:LinkButton>
                                &nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Calle">
                            <ItemTemplate>
                                <asp:LinkButton ID="hlCalle" runat="server" OnClick="LinkButton_Click" Text='<%# Container.DataItem("calle")%>'></asp:LinkButton>
                                &nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateColumn>


                        <asp:BoundColumn DataField="numExterior" HeaderText="NumExt" />
                        <asp:BoundColumn DataField="numInterior" HeaderText="NumInt" />

                        <asp:BoundColumn DataField="entreCalles" HeaderText="Manzana/Lote" />
                    </Columns>
                    <HeaderStyle BackColor="#004FC5" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                    <ItemStyle BackColor="White" Font-Bold="False" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False" ForeColor="Black" />
                    <SelectedItemStyle BackColor="#004FC5" Font-Bold="True" Font-Italic="False" Font-Names="Segoe UI" Font-Overline="False" Font-Size="Small" Font-Strikeout="False" Font-Underline="False" ForeColor="White" />
                </asp:DataGrid>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
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
       


            
            </div>
            <div id="pie">
            </div>

        </div>
       
    </form>
</body>
</html>

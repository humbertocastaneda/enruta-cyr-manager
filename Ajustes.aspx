<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Ajustes.aspx.vb" Inherits="Ajustes" %>

<!DOCTYPE html>



<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Enviar archivo de Excel</title>
    <LINK REL="Stylesheet" HREF="style.css" TYPE="text/css">
    <LINK REL="Stylesheet" HREF="menu.css" TYPE="text/css">

</head>
    

<body>
    <form id="form1" runat="server">
       
        <div id="pagina">
            <div id="blanco" style="background-color:white">

            </div>
            
            <div id="cabecera">

                <div style="width:auto; height:50px; float:left">
                    <a href="default.aspx">
                    <img alt="Enruta Lectura y Reparto"  style="float:left" height="50px" src="botones/enruta.jpg" />
                        </a>
                </div>
                
                <div id="contenedor">
                    <ul id="menu">
                    <li><a href="envio.aspx">Importar</a></li>
                    <li><a href="asignacion.aspx">Asignar</a></li>
                    <li><a href="empleadosABC.aspx">Técnicos</a></li>
                    <li>
                        <a href="#">Ver Ordenes</a>
                        <ul class="children">
                            <li><a href="verOrdenesPorEmpleado.aspx">Por Técnico</a></li>
                            <li><a href="verOrdenes.aspx">Por Número de Orden/Cte.</a></li>
                        </ul>
                    </li>
  
                    <li><a href="logIn.aspx/?log=1">Salir</a></li>
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

            <div id="contenido" >
                Cambio de Contraseña
                <div style="margin-top: 20px; width: 50%; height: auto; margin: auto; border-top-style: solid; border-color: #eaeaea; border-width:5px" >
                    Contraseña Anterior:
                    <br />
                    <asp:TextBox ID="txtAnterior" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    Contraseña Nueva:
                    <br />
                    <asp:TextBox ID="txtNueva" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    Confirme Contraseña:<br />
                    <asp:TextBox ID="txtConfirmar" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button style="float:right" ID="cambiarPass" runat="server" Text="Cambiar" />
                  </div>            


            </div>
            <div id="pie">

            </div>

        </div>
       
    </form>
</body>
</html>

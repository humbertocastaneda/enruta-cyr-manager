<%@ Page Language="VB" AutoEventWireup="false" CodeFile="erroresComunes.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>
<!DOCTYPE html>



<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Errores Comunes</title>
    <link rel="Stylesheet" href="style.css" type="text/css">
    <link rel="Stylesheet" href="menu.css" type="text/css">
    <link rel="icon"
        type="image/png"
        href="botones/favicon.png">

    <script src="Scripts/jquery-2.0.3.min.js" type="text/javascript"></script>
    <script src="Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>

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
            </div>

            <div id="contenido">
                <p>Esta pagina tiene como objetivo presentar los errores que se suceden dirariamente ya sea en la transmisión, carga y ejecucion de las ordenes con la solución del mismo
                    que puedan ser resueltos de manera facil por personal administrativo.
                     <a name="inicio"></a>
                     </p>
                
                <ul runat="server" visible="true">
                        <li>Problemas con la carga de las ordenes a la pagina web
                            <ul class="children">
                                <li><a href="#error1">Ocurrio un error: Unknown column  'x' in 'where clause' Verifique linea x del archivo</a></li>
                                <li><a href="#error2">The table doesn't have the espected format</a></li>
                                
                            </ul>
                        </li>

                    <li>Problemas con el envio o recepcion del ordenes al celular
                            <ul class="children">
                                <li><a href="#error3">No se pueden <i>recibir</i> ordenes del celular o no puedo realizar un cierre forzado</a></li>
                                <li><a href="#error4">No se pueden <i>enviar</i> ordenes al celular</a></li>
                                 <li><a href="#error5">¿Como se a que celular realizarle un cierre forzado?</a></li>
                            </ul>
                        </li>

                    <li>Problemas relacionados con el sistema operativo
                            <ul class="children">
                                <li><a href="#error6">El celular no cuenta con la aplicacion y en la parte inferior izquierda dice Modo Seguro</a></li>
                                
                            </ul>
                        </li>
                        
                    </ul>

                <h2><A name="titulo1">Problemas con la carga de las ordenes a la pagina web </A></h2>

                <h3 style="margin-left: 40px"><A name="error1"> Ocurrio un error: Unknown column  'x' in 'where clause' Verifique linea x del archivo </A></h3>
                <p style="margin-left: 40px">El archivo de ordenes a cargar contiene un error en la linea que se indica. Verifique dicha linea, modifquela y vuelva a cargar el archivo</p>

                 <h3 style="margin-left: 40px"><A name="error2">The table doesn't have the espected format</A></h3>
                <p style="margin-left: 40px">Este problema se debe puede ocacionarse por varias situaciones: 
                    <asp:BulletedList ID="BulletedList1" runat="server" style="margin-left: 80px">
                        <asp:ListItem>Problemas con la conexión a internet</asp:ListItem>
                        <asp:ListItem>El archivo cuenta con filtros o le faltan encabezados</asp:ListItem>
                        <asp:ListItem>El archivo se encuentra abierto.</asp:ListItem>
                        <asp:ListItem>El archivo se encuentra corrupo o dañado</asp:ListItem>
                    </asp:BulletedList>
                    <p style="margin-left: 40px">
                    Si despues de muchos intentos aun no puede cargar el archivo, se recomienda copiar el contenido y pegarlo en un nuevo documento de excel.</p>
                </p>


                <h2><A name="titulo1">Problemas con el envio o recepcion del ordenes al celular</A></h2>
                    <h3 style="margin-left: 40px"> <A name="error3">No se pueden <i>recibir</i> ordenes del celular o no puedo realizar un cierre forzado</A></h3>

                    <p style="margin-left: 40px">Asegurese que el técnico haya realizado las ordenes en el celular <br />
                        Verifique que el celular tenga los <i>datos</i> encendidos.<br />
                        Verifique que el celular tenga internet. Regularmente se muestra un <i>3G</i> o una <i>E</i> a un lado del indicador del señal del celular.<br />
                       Abra una pagina de Google Chrome en el celular y verifque que tenga acceso a cualquier pagina.<br />
                    </p>

                    <h3 style="margin-left: 40px"> <A name="error4">No se pueden <i>enviar</i> ordenes al celular</A></h3>
                    <p style="margin-left: 40px">Asegurese que se han cargado y enviado ordenes a dicho celular. <br />
                        Verifique que el celular tenga los <i>datos</i> encendidos.<br />
                        Verifique que el celular tenga internet. Regularmente se muestra un <i>3G</i> o una <i>E</i> a un lado del indicador del señal del celular.<br />
                       Abra una pagina de Google Chrome en el celular y verifque que tenga acceso a cualquier pagina.<br />
                    </p>

                    <h3 style="margin-left: 40px"> <A name="error5">¿Como se a que celular realizarle un cierre forzado?</A></h3>
                    <p style="margin-left: 40px">Para saber a que celulares realizar el cierre forzado, realice lo siguiente: <br />
                        Dirijase a <i>Seguimiento</i><br />
                       Seleccione el rango de fechas de las ordenes a las cual hara un cierre forzado<br />
                        Palomee el cuadro que dice "Solo con pendientes"<br />
                        Presione "Seleccionar"<br />
                        Enseguida se mostrará un listado con los técnicos que requieren el cierre forzado<br />
                    </p>

                <h2>Problemas relacionados con el sistema operativo</h2>
                    <h3 style="margin-left: 40px"><A name="error6"> El celular no cuenta con la aplicacion y en la parte inferior izquierda dice <i>Modo Seguro</i></A></h3>
                    <p style="margin-left: 40px">Reinicie el celular.<br />
                       Procure no presionar algun botón durante el inicio del celular.
                    </p>


            </div>
            <div id="pie">
            </div>

        </div>

    </form>
</body>
</html>

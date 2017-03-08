


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
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
            
            
            <div id="cabecera">

                <div style="width:auto; height:auto; float">
                    <img alt="Enruta Lectura y Reparto"  style="float:left" height="50px" src="botones/enruta.jpg" />
                </div>
                
                <div id="contenedor">
                    <ul id="menu">
                    <li><a href="#">Importar</a></li>
                    <li><a href="#">Asignar</a></li>
                    <li><a href="#">Técnicos</a></li>
                    <li>
                        <a href="#">Ver Ordenes</a>
                        <ul class="children">
                            <li><a href="#">Por Empleado</a></li>
                            <li><a href="#">Por Numero de Orden</a></li>
                            <li><a href="verResumenEstados.aspx">Por Estados de la Orden</a></li>
                              <li><a href="verCambiosDeMedidor.aspx">Cambio de Medidor</a></li>
                               <li><a href="verNoRegistrados.aspx">No Registrados</a></li>
                        </ul>
                    </li>
  
                    <li><a href="#">Salir</a></li>
                </ul>
                </div>
                
            </div>
            <div id="blanco" style="background-color:white">

            </div>

       </div>
    </form>
</body>
</html>


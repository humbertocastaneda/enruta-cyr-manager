<?php
$con = new mysqli("10.61.37.44", $_POST["usuario"], $_POST["password"], $_POST["db"]);

if (mysqli_connect_errno()) {
    printf("holaFallo la conexión: %s\n", mysqli_connect_error());
    exit();
}
	
	$con->autocommit(TRUE);
	
mysqli_query($con,$_POST["cadena"]); //Aqui le voy a mandar la instruccion por medio del celular

mysqli_close($con);
	
?>
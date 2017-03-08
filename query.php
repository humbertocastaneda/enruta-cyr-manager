<?php
//Conexion con sql
 $mysqli = new mysqli( "10.61.37.44","u1003479_hcastan", "pracinformatica" , "db1003479_Mexicon");
if ($mysqli -> connect_errno)
	{
		echo 'Ocurrio un error al momento de conectarse a la base de datos: ' . $mysqli -> connect_errno;
		exit();
	}
	
	$query ="select FolioPalabra,Palabra from Lexicon where NumLetras=2";
	
if ($result = $mysqli->query($query)) {
	for ($i=0; $i<=100; $i++){
		$row = $result->fetch_row();
    	//echo $i."-".$row[1];
		printf("%s - %s ", $i, $row[1]);
		echo '<BR>';
	}
    $result->close();
}
	
?>
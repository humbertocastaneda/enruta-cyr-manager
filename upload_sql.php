<?php 
$error = $_FILES['upload_field']['error'];
 $medidor=$_POST["medidor"];
 //Declaramos las posiciones, despues nos preocupamos con los demas paises...
 $POS_DATOS_POLIZA 			= 335;
 $POS_DATOS_LECTURA 		= 361;
 $POS_DATOS_ANOMALIA_MEDIDOR = 383;
 $POS_DATOS_SERIE_MEDIDOR 	= 226;
 
 //declaramos las longitudes
 $LONG_CAMPO_POLIZA 			= 7;
 $LONG_CAMPO_LECTURA 		= 8;
 $LONG_CAMPO_ANOMALIA_MEDIDOR 		= 3;
 $LONG_CAMPO_SERIE_MEDIDOR 		= 10;
 
 
 //detalles de la conexion
 $basededatos="db1003479_reportes";
 $servidor= "10.61.37.44";
 $pass="pracinformatica";
 $usu="u1003479_hcasta";
 
 //Conexion con sql
 $mysqli = new mysqli( $servidor, $usu, $pass , $basededatos);
if ($mysqli -> connect_errno)
	{
		echo 'Ocurrio un error al momento de conectarse a la base de datos: ' . $mysqli -> connect_errno;
		exit();
	}

if($error==UPLOAD_ERR_OK) {
	//Empezamos a obtener las variables
	$poliza= substr($medidor,$POS_DATOS_POLIZA, $LONG_CAMPO_POLIZA  );
	$lectura= substr($medidor,$POS_DATOS_LECTURA, $LONG_CAMPO_LECTURA  );
	$anomalia= substr($medidor,$POS_DATOS_ANOMALIA_MEDIDOR, $LONG_CAMPO_ANOMALIA_MEDIDOR  );
	$medidor= substr($medidor,$POS_DATOS_SERIE_MEDIDOR, $LONG_CAMPO_SERIE_MEDIDOR  );
	
	if (!$mysqli -> query("insert into Lecturas(poliza, lectura, anomalia, medidor) values ('$poliza', '$lectura', '$anomalia', '$medidor') ")){
		echo "Ocurrio un error al momento de actualizar la base de datos: " . $mysqli->sqlstate;
	} 
}


 ?>

<?php
include 'SQLHelper.php';
$destFolder = dirname ( __FILE__ );
$uploadfile = $destFolder . $_POST ["ruta"];

$subido = false;
$grabarencadena = false;
$cadena = $_POST ["cadena"];
$resultado = 0;
$errorcod = 0;

$carpeta = $_POST ["carpeta"];

$uploadfile = str_replace ( " ", "_", $uploadfile );

if (! file_exists ( $destFolder . $carpeta )) {
	mkdir ( $destFolder . $carpeta, 0777, true );
}

if (file_exists ( $uploadfile )) {
	if (! unlink ( $uploadfile )) {
		$resultado = - 1;
	}
}

$cadena = base64_decode ( $cadena );

if ($resultado == 0) {
	
	if ($error == UPLOAD_ERR_OK) {
		$fh = fopen ( $uploadfile, 'a+' );
		fwrite ( $fh, $cadena );
		$subido = true;
		fclose ( $fh );
	}
	
	if ($fh) {
		$helper = new SQLHelper();
		$helper->saveImage(substr($_POST ["ruta"], 2,14), substr($_POST ["ruta"], -39));
		echo "0";
	} else if ($subido) {
		echo "No se pudo subir el archivo " . $error;
	} else {
		echo "Se ha producido un error: " . $error;
	}
} else {
	echo "La imagen ya existe y no puede ser eliminada";
}

?>

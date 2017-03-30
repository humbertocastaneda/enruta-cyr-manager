<?php 
$uploaddir = dirname(__FILE__)."\uploads\\";
 $uploadfilename = strtolower(str_replace(" ", "_",basename($_FILES['upload_field']['name'])));
 $uploadfile = $uploaddir.$uploadfilename; $error = $_FILES['upload_field']['error'];
 $subido = false; 
 if($error==UPLOAD_ERR_OK) {
	if($_FILES['upload_field']['type']!="image/jpg" ) {
		$error = "Comprueba que el archivo sea una imagen en formato jpg ";
	} 
	elseif(preg_match("/[^0-9a-zA-Z_.-]/",$uploadfilename)) {
		$error = "El nombre del archivo contiene caracteres no válidos."; 
	} else { $subido = copy($_FILES['upload_field']['tmp_name'], $uploadfile); 
}
 } 
 if($subido) { 
 echo "El archivo subio con exito";
 } 
 else 
 { 
 echo "Se ha producido un error: ".$error ; 
 } 
 ?>

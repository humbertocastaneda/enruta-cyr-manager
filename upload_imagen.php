<?php 
$uploaddir = dirname(__FILE__)."\\";

if (strlen ($_POST["carpeta"])>0){
$uploaddir = $uploaddir . $_POST["carpeta"] ."\\";
}


if (!is_dir ($uploaddir)){
	if (!mkdir($uploaddir, 0777, true)){
		echo "Error al crear directorio";
	}
}

 $uploadfilename = str_replace(" ", "_",basename($_FILES['upload_field']['name']));
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
 echo "0";
 } 
 else 
 { 
 echo "Se ha producido un error: ".$error ; 
 } 
 ?>

<?php 
//$uploaddir = dirname(__FILE__).$_POST["cadena"];
$uploadfile= dirname(__FILE__)."\\".$_POST["ruta"];
 //$uploadfilename = strtolower(str_replace(" ", "_",basename($_FILES['upload_field']['name'])));
 //$uploadfile = $uploaddir.$uploadfilename;
// $error = $_FILES['upload_field']['error'];
 $subido = false; 
 $grabarencadena=false;
 $cadena=$_POST["cadena"];
 
if (!file_exists  (dirname($uploadfile))){
	if (!mkdir(dirname($uploadfile), 0777, true)){
		echo "Error al crear directorio";
	}
}

 
 if($error==UPLOAD_ERR_OK) {
		$fh = fopen($uploadfile, 'a+') ;
		fwrite($fh, utf8_decode ($cadena));
		$subido=true;
		fclose($fh);
 } 
 
 if($fh) { 
 echo "0";
 } 
 else 
 { 
 echo "Se ha producido un error: ".$error ; 
 } 
 ?>

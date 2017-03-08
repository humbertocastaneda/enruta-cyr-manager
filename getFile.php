<?php 
//$uploaddir = dirname(__FILE__)."\\";

/*if (strlen ($_POST["carpeta"])>0){
$uploaddir = $uploaddir . $_POST["carpeta"] ."\\";
}*/

 //$uploadfilename = strtolower(str_replace(" ", "_",basename($_FILES['upload_field']['name'])));
 //$uploadfile = $uploaddir.$uploadfilename; 
 $uploadfile=dirname(__FILE__)."\\".$_POST["ruta"];
 $error = $_FILES['upload_field']['error'];
 //echo  $uploadfile;
 
// $uploadfile=dirname(__FILE__)."\\uploads\\holamundo.txt";
 
 if (file_exists($uploadfile)){
 //Si el archivo existe, lo procesamos, de otra manera no hacemos nada
	$fh = fopen($uploadfile, 'rb');
	$contenido .= fread($fh, filesize($uploadfile ));
	fclose($fh);
 
	/*$contenido=str_replace(" ", "&nbsp;",  $contenido);
	$contenido=str_replace("\n", "<br />",  $contenido);
	$contenido=str_replace("\r", "",  $contenido);*/

 
	echo $contenido;
 }
 else
 {
	header('X-PHP-Response-Code: 404', true, 404);
 }
 
 
 
 ?>
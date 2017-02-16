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
 
 if (file_exists($uploadfile) && !file_exists(substr($uploadfile, 0, -4).$_POST["extension"]) ){
 //Si el archivo existe, lo procesamos, de otra manera no hacemos nada
	 if (copy ( $uploadfile , substr($uploadfile, 0, -4).$_POST["extension"]  )){
		echo "verdadero";
		
	 }
	 else{
	 echo "falso";
	 }
 }
 else
 {
	echo "falso" ;
 }
 
 
 
 ?>
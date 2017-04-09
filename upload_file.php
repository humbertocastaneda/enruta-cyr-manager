<?php 
$uploaddir = dirname(__FILE__)."\uploads\\";
 $uploadfilename = strtolower(str_replace(" ", "_",basename($_FILES['file']['name'])));
 $uploadfile = $uploaddir.$uploadfilename; $error = $_FILES['file']['error'];
 $subido = false; 
 
 echo $_FILES['file']['tmp_name']. ($_FILES["file"]["size"] / 1024) . " Kb<br />";

 $subido = copy($_FILES['file']['tmp_name'], $uploadfile); 

 if($subido) { 
 echo "El archivo subio con exito";
 } 
 else 
 { 
 echo "Se ha producido un error: ".$error ; 
 } 
 ?>

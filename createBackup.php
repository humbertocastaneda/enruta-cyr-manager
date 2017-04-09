<?php
//Es una prueba ya que no tengo permisos...
$destFolder=dirname(__FILE__)."\\"."uploads"."\\"."backup"."\\".$_POST["ruta"]."\\";
$sourcefile= dirname(__FILE__)."\\".$_POST["ruta"]."\\".$_POST["archivo"];
$destfile=$destFolder.$_POST["backup"];



if (!file_exists($destFolder)){
	mkdir($destFolder, 0777, true);
}
$result = rename ( $sourcefile, $destfile );
if (!$result) echo "-1";
else echo "0";
?>
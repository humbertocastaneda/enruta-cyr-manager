<?php
//Es una prueba ya que no tengo permisos...
$destFolder=dirname(__FILE__)."\\".$_POST["ruta"]."\\".$_POST["archivo"];
$sourcefile= dirname(__FILE__)."\\".$_POST["ruta"]."\\prueba\\".$_POST["archivo"];

$result = rename ( $sourcefile, $destFolder );
if (!$result) echo "-1";
else echo "0";
?>
<?php
$sourcefile= dirname(__FILE__)."\\"."uploads"."\\"."in"."\\"."ordenes"."\\"."9776.txt";
$destfile=dirname(__FILE__)."\\"."uploads"."\\"."backup"."\\"."in"."\\"."ordenes"."\\"."9776.txt";


if (!file_exists((dirname(__FILE__)."\\"."uploads"."\\"."backup"."\\"."in"."\\"."ordenes"."\\"))){
echo "no existe";
	mkdir(dirname(__FILE__)."\\"."uploads"."\\"."backup"."\\"."in"."\\"."ordenes", 0777, true);
}


$result = rename ( $sourcefile, $destfile );
if (!$result) echo "ERROR renaming $sourcefile -> $destfile";
?>
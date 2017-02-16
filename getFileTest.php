<?php
 
 $uploadfile=dirname(__FILE__)."\\11604480.03C";

  if (file_exists($uploadfile)){
 //Si el archivo existe, lo procesamos, de otra manera no hacemos nada
	$fh = fopen($uploadfile, 'rb');
	$contenido .= fread($fh, filesize($uploadfile ));
	fclose($fh);
	//echo $contenido;
	//echo" ----- ";
	echo pack('C*',desencriptar($contenido ,0));
	
	
	
 }
 else
 {
	header('X-PHP-Response-Code: 404', true, 404);
 }
 
 
    function desencriptar($medidor, $nContBytesClaveEncriptado)
{
echo "Entre";
	$strClaveEncriptadoElectricaribe="AJ1LXMNP2MDDGX8Y4NLQ5XAAC6WQJ7ZAY8NQ0Z";

	$medidor=unpack('C*',$medidor);
	for ($i=0; $i<count($medidor); $i++)
	{
	//echo "For " . pack('C*',$medidor[i]);
	//CAR == medidor(), i - 1, 1, "ISO-8859-1";
	 $nContBytesClaveEncriptado ++;
	  $codigo=(substr($strClaveEncriptadoElectricaribe,($nContBytesClaveEncriptado-1),1));
	 $codigo=unpack('C*',$codigo);
	 
		echo $codigo[0]^ $medidor[i];
		}
		 return $medidor;
  }

 


 ?>
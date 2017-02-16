<?php 

 $uploadfile=dirname(__FILE__)."\\".$_POST["upload_field"];
 //$error = $_FILES['upload_field']['error'];
  echo "archivo a procesar " . $uploadfile;
 
 $LONG_DATOS_MEDIDOR=357;
 
 
function EncriptarDesencriptarElectricaribe($medidor, $nContBytesClaveEncriptado){
		//$Codigo;
		//$CAR;	
		$i = 0;	
		//char codigo[],car[];		
		$strClaveEncriptadoElectricaribe = "AJ1LXMNP2MDDGX8Y4NLQ5XAAC6WQJ7ZAY8NQ0Z";
		for($i = 1; i <= strlen($medidor); $i++){
			$CAR = substr($medidor, $i, 1);
			$nContBytesClaveEncriptado++;
			$car = str_split($CAR);			
			echo $car . " ".$strClaveEncriptadoElectricaribe;
			$Codigo = substr($strClaveEncriptadoElectricaribe, (($nContBytesClaveEncriptado - 1) % strlen($strClaveEncriptadoElectricaribe)) + 1, 1);
			$codigo= str_split($Codigo);			
			substr_replace ( $vObservaciones[i] ,$codigo[0] ^ $car[0], $i-1 , 1 );
		}        
		return $nContBytesClaveEncriptado;
	}
	
function EncriptarDesencriptarPanamaConParametros($medidor, $inicio, $longitud){
		$comodin0 = 2;
  		$comodin1 = 3;
	  	$comodin2 = 4;

  		$X = 0;
  		$rep = 0;
		$byteLetra;
		for ($i=0; $i<$longitud; $i++){
    			$byteLetra = $medidor[i+inicio];
    			if      ($byteLetra == char(10)) 	$medidor[$i+$inicio] = char(10);
    			else if ($byteLetra == char(9))  	$medidor[$i+$inicio] = char(9);
    			else if (byteLetra == char(94)) 	$medidor[$i+$inicio] = char(94);
    			else if ($X == 0) 		$medidor[$i+$inicio] = $byteLetra ^ $comodin0;
    			else if ($X == 1) 		$medidor[$i+$inicio] = $byteLetra ^ $comodin1;
    			else if ($X == 2) 		$medidor[$i+vinicio] = $byteLetra ^ $comodin2;
    			
    			$X = $X +  1;
    			if (($X == 2) && ($rep == 0)) {
      				$X = 1;
      				$rep = 1;
    			}else{ 
				if (($X == 2) && ($rep > 0)) $rep = 0;
    			}
    			if ($X == 3) $X = 0;
  		}
	}
 
 

 
 if (file_exists($uploadfile)){
 //Si el archivo existe, lo procesamos, de otra manera no hacemos nada
	$fh = fopen($uploadfile, 'rb');
	
	$quedaMas=true;
	$errorEnLectura=false;
	$leidos=0;
	$totalLeidos=0;
	$tamanoArchivo=filesize($uploadfile);
	
	$encabezado=true;
	
	$nContBytesClaveEncriptadoUsuarios=0;
	

	while($quedaMas || !$errorEnLectura) {
		$contenido = fread($fh, $LONG_DATOS_MEDIDOR);
		echo "Primer ciclo";
		$quedaMas=false;
	$errorEnLectura=true;
		
		if (!$contenido){
			$errorEnLectura=true;
		}else{
			$leidos = strlen($contenido);
			$totalLeidos +=$leidos;
			
			if ($totalLeidos== $tamanoArchivo){
				$quedaMas=false;
			}
			
			
			if ($leidos>0){
			
				if ($encabezado){
					$encabezado=false;
					$nLongCopiarEnEncabezado = 26;
					
					for($i=0; $i<$LONG_DATOS_MEDIDOR; $i++) $medidorSalida[$i] = ' ';
					$medidorSalida[2] = 'L';
					
					$medidorSalida[$LONG_DATOS_MEDIDOR-1] = 10;
					$medidorSalida[$LONG_DATOS_MEDIDOR-2] = 13;
					
					$nContBytesClaveEncriptadoEntrada = EncriptarDesencriptarElectricaribe($contenido,$nContBytesClaveEncriptadoEntrada);
					for($i=0; $i<$nLongCopiarEnEncabezado; $i++) $medidorSalida[$i+200] = $medidor[$i+4];
					$vMedidores[]=$medidorSalida;
				}
			
			
			}else{
				$nContBytesClaveEncriptadoEntrada = EncriptarDesencriptarElectricaribe($contenido,$nContBytesClaveEncriptadoEntrada);
				$vMedidores[]=$contenido;
			}
			
		}
		
		
	}
	
	
	
	
	
 
	for($i=0;$i<count($vMedidores); $i++){
		echo $vMedidores[$i];
	}
 }
 else
 {
	//header('X-PHP-Response-Code: 404', true, 404);
 }
 
 
 
 ?>
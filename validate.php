<?php
date_default_timezone_set("America/Mexico_City");
$fechamin= date('YmdHis', strtotime(date("Y/m/d H:i:s" ).' -15 minute')) ;
$version=7.5;

//$fechamin=strtotime('15 minutes', $fechamin);
//$fechamin = date("YmdHis", $fechamin ) ;
$fechamax=date('YmdHis', strtotime(date("Y/m/d H:i:s" ).' +15 minute')) ;
//$fechamax += (15*60);
//$fechamax=date("YmdHis" , $fechamax);

//$fechaComparar= DateTime::createFromFormat('YmdHis', $_POST["fecha"]);
//$fechaComparar= DateTime::createFromFormat('YmdHis', '20040504121446');

$fechaComparar = strtotime($_POST["fecha"]);

$newformat = date('YmdHis',$fechaComparar);
/*echo $fechamax.'<br>';
echo $fechamin.'<br>';
echo $newformat.'<br>';*/
if ($newformat>= $fechamin && $newformat<= $fechamax ){
	if ($_POST["version"]==$version){
		echo 0; //todo bien
	}
	else
	{
		echo 2; //Version mala
	}
	
	}
	else
	{
		if ($_POST["version"]==$version){
			echo 1; //solo fecha mala
		}
		else
		{
			echo 3; //todo mal
		}
	}
	


?>
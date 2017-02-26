<?php
include 'SQLHelper.php';
$mainFolder = 'fotos';
/*$folders = array_diff(scandir("./$mainFolder"), 
		array('..', '.', '20170120', '20170121', '20170122', '20170123', 
				'20170124', '20170125', '20170126', '20170226'));
foreach ($folders as $folder){*/
	$folder = $_GET['folder'];
	$path = "$mainFolder/$folder";
	
	$picturesFolder = array_diff(scandir("./$path"), array('..', '.'));
	foreach ($picturesFolder as $picture){
		$helper = new SQLHelper();
		$helper->saveImage($path, $picture);
	}
	
	
/*}*/
echo "Done!";
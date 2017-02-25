<?php
include 'SQLHelper.php';
$mainFolder = 'fotos';
$folders = array_diff(scandir("./$mainFolder"), array('..', '.'));
foreach ($folders as $folder){
	$path = "$mainFolder/$folder";
	
	$picturesFolder = array_diff(scandir("./$path"), array('..', '.'));
	foreach ($picturesFolder as $picture){
		$helper = new SQLHelper();
		$helper->saveImage($path, $picture);
	}
	
	
}
echo "Done!";
<?php


class SQLHelper{
	
	private function connect(){
		$basededatos="db1007397_cortrexGNF";
		$servidor= "10.61.37.44";
		$pass="Sotixe_69";
		$usu="u1007397_cgnf";
		
		$mysqli = new mysqli( $servidor, $usu, $pass , $basededatos);
		if ($mysqli -> connect_errno)
		{
			var_dump("Error opening DB {$mysqli -> connect_errno}");
			return;
		}
	
		return $mysqli;
	}
	
	private function execQuery($query){
		$mysqli = $this->connect();
		
		
		$mysqli->query($query);
		
		$mysqli->close();
	}
	
	private function insertRaw($table, $object, $where = null){
		$mysqli = $this->connect();
		
		if ($mysqli == null){
			
			return;
		}
	
		$fields = '';
		$values = '';
		
		foreach(get_object_vars($object) as $field=>$value){
			if ($fields != ''){
				$fields .= ',';
			}
			$fields .= $field;
			
			if ($values != ''){
				$values .= ',';
			}
			
			if (is_string($value)){
				$values .= "'$value'";
			}
			else{
				$values .= $value;
			}
			
		}
		
		$insert = "Insert into $table($fields) values ($values)";
		
		$mysqli->query($insert);
		
		$mysqli->commit();
		
		$mysqli->close();
	}
	
	
	function saveImage($path, $imageName){
		$insert = new stdClass();
		$insert->idorden = substr($imageName, 0, 10);
		$insert->path = $path;
		$insert->name = $imageName;
		$insert->fecha = substr($path, -8);
		
		$this->insertRaw("fotos", $insert);
		
	}
}



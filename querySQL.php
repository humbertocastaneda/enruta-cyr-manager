<?php
$con = new mysqli("mysql.site.infoquest.com", $_POST["usuario"], $_POST["password"], $_POST["db"]);

if (mysqli_connect_errno()) {
    printf("Fallo la conexión: %s\n", mysqli_connect_error());
    exit();
}
	
//mysqli_query($con,$_POST["cadena"]); //Aqui le voy a mandar la instruccion por medio del celular
$result= mysqli_query($con,$_POST["cadena"]);
//echo mysqli_num_rows($result);
if (mysqli_num_rows($result) > 0) {
    // output data of each row
	 
    while($row = mysqli_fetch_assoc($result)) {
	$i=0;
		foreach ($row as &$val)
		{
		$i++;
			echo $val;
			echo "<br>";
			
		}
        //echo $row['texto'];
		//echo"2";
    }
} else {
    echo "0 results";
}


mysqli_close($con);
	
?>
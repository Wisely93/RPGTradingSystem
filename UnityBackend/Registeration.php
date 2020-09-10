<?php

require 'ConnectionSettings.php';

$loginUser = $_POST["loginUser"];
$loginPassword = $_POST["loginPass"];

//$today = date("F j, Y, g:i a");
//echo "Date : " . $today."<br><br>";


// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);        //die means return
}
echo "Connected successfully.<br><br>";

$sql = "SELECT username FROM user WHERE username = '" . $loginUser . "'";        //select title from table of database

$result = $conn->query($sql);

if ($result->num_rows > 0) {		//if result number is more than 0 means the username is exist
 
  echo "This username had been taken, please register another username";
}else {
	//Register user into the database
	$Registersql = "INSERT INTO user (username, password, level, coins) VALUES ('" . $loginUser . "', '" . $loginPassword . "', 1, 200)";

	if ($conn->query($Registersql) === TRUE) {
	 echo "New record created successfully";
	} else {
	echo "Error: " . $Registersql . "<br>" . $conn->error;
	}
}


$conn -> close();
?>
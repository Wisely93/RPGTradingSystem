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
$sql = "SELECT password, id FROM user WHERE username = '" . $loginUser . "'";        //select title from table of database

$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  while($row = $result->fetch_assoc()) {            //fetch dictionary
   if($row["password"] == $loginPassword){

		//Get User's data
		//echo "id: " . $row["id"]. $row["level"]. "". $row["coins"] ."'";		//echo $row["level"];
		echo $row["id"];

		//Get player info

		//Get Inventory

	    //Modify Player's data

	    //Update Inventory
		}
 else {
  echo "Wrong username or password. Please try again.";}
}
}else {
	echo "User does not exist";
}
$conn -> close();
?>
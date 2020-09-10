<?php

require 'ConnectionSettings.php';


// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);        //die means return
}

//User submit this variable
$itemID = $_POST["ItemID"];

$ImagePath = "http://localhost/UnityBackend/IconImages/" . $itemID . ".png";

//Get Image and convert into string
$image = file_get_contents($ImagePath);

echo $image;

$conn->close();
?>
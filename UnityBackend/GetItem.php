<?php

require 'ConnectionSettings.php';

$itemID = $_POST["ItemID"];

//$today = date("F j, Y, g:i a");
//echo "Date : " . $today."<br><br>";


// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);        //die means return
}
echo "Connected successfully.<br><br>";

$sql = "SELECT name, description , price FROM items WHERE ID = '" . $itemID . "'";        //select title from table of database

$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  $rows = array();
  while($row = $result->fetch_assoc()) {            //fetch dictionary
        $rows[] = $row;
  }
        //after whole array is created
        echo json_encode($rows);
  } else {
  echo "0 results";
}


$conn -> close();
?>
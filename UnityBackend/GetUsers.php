<?php

require 'ConnectionSettings.php';

$userID = $_POST['userID'];

echo $userID;
// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);        //die means return
}

$sql = "SELECT * FROM user WHERE id = '" .$userID. "'";        //select title from table of database

$result = $conn->query($sql);

if ($result->num_rows > 0) {
    
  $rows = array();
  // output data of each row
  while($row = $result->fetch_assoc()) {            //fetch dictionary

    $rows[] = $row;
    echo json_encode($rows);
  }
} else {
  echo "0 results";
}
?>
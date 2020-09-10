<?php

require 'ConnectionSettings.php';

//variable submit by user
$ID = $_POST["ID"];
$itemID = $_POST["ItemID"];
$userID = $_POST["userID"];

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);        //die means return
}

//Get price item from database
$sql = "SELECT price FROM items WHERE ID = '" . $itemID . "'";        //select title from table of database

$result = $conn->query($sql);

if ($result->num_rows > 0) {
 
 //store item price to itemPrice variable
 $itemPrice = $result->fetch_assoc()["price"];

 //Delete past transaction history after sold
 $sql2 = "DELETE FROM usersitems WHERE ID = '" . $ID . "'";

 $result2 = $conn->query($sql2);

 if ($result2) {

	echo $itemPrice;
	//If delete successfully
	$sql3 = "UPDATE `user` SET `coins`= coins + '". $itemPrice. "' WHERE `id` = '" . $userID . "'";
	$result3 = $conn->query($sql3);
	if($result3)
	{
		echo "Item sold";
	}else
	{
		echo "Failed";
	}
	} else 
	{
	echo "No item purchased";
	}
	}
$conn -> close();
?>
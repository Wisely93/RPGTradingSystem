<?php

require 'ConnectionSettings.php';

//variable submit by user
$itemID = $_POST["ItemID"];
$userID = $_POST["userID"];

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);        //die means return
}

//Get price item from database
$sql = "SELECT price FROM items WHERE ID = '" . $itemID . "'";        //select title from table of database

$sql2 = "SELECT coins FROM user WHERE id = '" . $userID . "'";   

$result = $conn->query($sql);
$result2 = $conn->query($sql2);

if ($result->num_rows > 0 && $result2 ->num_rows > 0) {
 
 //store item price to itemPrice variable
 $itemPrice = $result->fetch_assoc()["price"];
 $userCoins =  $result2->fetch_assoc()["coins"];

 if($userCoins > $itemPrice)
 {
	 //Added transaction history after purchased
	$sql3 = "INSERT INTO usersitems(userID, ItemID) VALUES ('" . $userID . "', '" . $itemID . "')";

	$result3 = $conn->query($sql3);

	if ($result3) {

		//If purchased successfully
		$sql4 = "UPDATE `user` SET `coins`= coins - '". $itemPrice. "' WHERE `id` = '" . $userID . "'";

		$result4 = $conn->query($sql4);

	if($result4)
	{
		echo "Item purchased";
	}else
	{
		echo "Error, please contact customer service for future assistance";
	}
	}
	}else echo "Not enough coins";
	}
$conn -> close();
?>
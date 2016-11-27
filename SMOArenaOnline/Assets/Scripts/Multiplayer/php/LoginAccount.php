<?php

$Hostname = "localhost";
$DBName = "accounts";
$User = "root";
$PasswordP	= "";

mysql_connect($Hostname, $User, $PasswordP) or die ("Can't connect to database");
mysql_select_db($DBName) or die ("Can't connect to database");

$Email = $_REQUEST["Email"];
$Password = $_REQUEST["Password"];

if(!$Email || !$Password){
	echo"Must fill in all information";
}
else{
	$SQL = "Select * FROM accounts WHERE Email = '". $Email ."'";
	$result_id = @mysql_query($SQL) or die ("Error in Database");
	$total = mysql_num_rows($result_id);
	if($total){
		$datas = @mysql_fetch_array($result_id);
		if(strcmp($Password, $datas["Password"])){
			$sql2 = "Select * FROM accounts WHERE Email = '". $Email ."'";
          		$result_id2 = @mysql_query($sql2) or die ("database error");
			while($row = mysql_fetch_array($result_id2)){
				echo"Success";
				echo":";
				echo $row['Characters'];
			}
       			
		}else{
			echo"Wrong Password";
		}
	}
	else{
		echo"Email does not exist";
	}
    }
    mysql_close();
?>
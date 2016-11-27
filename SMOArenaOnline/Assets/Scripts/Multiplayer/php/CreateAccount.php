<?Php

$Email = $_REQUEST["Email"];
$Password = $_REQUEST["Password"];
$Alias = $_REQUEST["Alias"];

//PHP only
$Hostname = "localhost";
$DBName = "accounts";
$User = "root";
$PasswordP = "";

mysql_connect($Hostname, $User, $PasswordP) or die ("Can't connect to database");
mysql_select_db($DBName) or die ("Can't connect to database");

if(!$Email || !$Password || !$Alias){
	echo"Empty";
}
else{
	$SQL = "Select * FROM accounts WHERE Email = '". $Email ."'";
	$Result = @mysql_query($SQL) or die ("Error in Database");
	$Total = mysql_num_rows($Result);
	if($Total == 0){
		$insert = "INSERT INTO `accounts` (`Email`,`Password`, `Characters`) VALUES ('" . $Email . "', MD5('" . $Password . "'),'" . $Alias . "')";
		$SQL1 = mysql_query($insert);
		echo "Success";   
		
	}else{
		echo"AlreadyUsed";
	}
}

mysql_close();
?>
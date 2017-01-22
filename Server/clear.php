<?php
/*
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);
*/

// veritabani baglantisi kur
$servername = "localhost";
$username = "kinix_ggj";
$password = "pass";
$dbname = "kinix_ggj";

$conn = new mysqli($servername, $username, $password, $dbname);

// clientten gelen veriyi al
$room = $_GET["room"];

// guncel verileri veritabanina yaz
$query = "UPDATE game SET x=0, y=0, vx=0, vy=0, meteroids='[]', meteroidspeed='[]', meteroidCount=0, jumpn=0, jumpe=0, jumps=0, jumpw=0, lastUpdate=0, health=10, createTime=". round(microtime(true) * 1000) ." WHERE room=". $room;
$conn->query($query);

// veritabani baglantisini kapat
$conn->close();

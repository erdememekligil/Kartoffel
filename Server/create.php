<?php
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);

// veritabani baglantisi kur
$servername = "localhost";
$username = "kinix_ggj";
$password = "pass";
$dbname = "kinix_ggj";

$conn = new mysqli($servername, $username, $password, $dbname);

// yeni oyun kur
$query = "INSERT INTO game (vx, vy, x, y, meteroids, meteroidspeed, health, createTime) VALUES (0, 0, 0, 0, \"[]\", \"[]\", 10, ". round(microtime(true) * 1000) .")";
$conn->query($query);

// oyunun idsini yaz
echo $conn->insert_id;

// veritabani baglantisini kapat
$conn->close();

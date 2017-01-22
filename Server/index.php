<?php
/*
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);
*/
const MAX_X = 17;
const MAX_Y = 17;
const MIN_X = -17;
const MIN_Y = -17;

const CRASH_DISTANCE = 2.4;

const FRICTION = 0.05;
const SPEED_MULT = 0.8;
const STOP_SPEED = 0.1;
const MAX_SPEED = 3;
const METEROID_CREATE_TIME = 1500;
const METEROID_SPEED_MAX_LIMIT = 50;
const METEROID_SPEED_MIN_LIMIT = 25;
const REFRESH_PERIOD = 500;
const METEROID_LAPSE = 250;
const DAMAGE = 500;
const HEALTH_GAIN = 20;

$now = round(microtime(true) * 1000); // milisaniye

// veritabani baglantisi kur
$servername = "localhost";
$username = "kinix_ggj";
$password = "pass";
$dbname = "kinix_ggj";

$conn = new mysqli($servername, $username, $password, $dbname);

// clientten gelen veriyi al
$room = $_GET["room"];
$region = $_GET["region"];
$force = $_GET["force"] * SPEED_MULT;

// veritabanindan oyun verisini cek
$query = "SELECT * FROM game WHERE room=". $room;
$result = $conn->query($query);
$data = $result->fetch_assoc();

if($now - $data["lastUpdate"] > 45000) {
	$data = [
		'x' => 0,
		'y' => 0,
		'vx' => 0,
		'vy' => 0,
		'meteroids' => '[]',
		'meteroidspeed' => '[]',
		'meteroidCount' => 0,
		'jumpn' => 0,
		'jumpe' => 0,
		'jumps' => 0,
		'jumpw' => 0,
		'lastUpdate' => 0,
		'createTime' => $now,
		'health' => 10
	];
}

// gezegenin hizini guncelle
switch ($region) {
	case 'n': $data["vy"] -= $force; break;
	case 'e': $data["vx"] += $force; break;
	case 's': $data["vy"] += $force; break;
	case 'w': $data["vx"] -= $force; break;
}

// gezegenin konumunu guncelle
if($now - $data["lastUpdate"] > REFRESH_PERIOD) {
	$data["x"] += $data["vx"];
	$data["y"] += $data["vy"];

	$data["health"]+=HEALTH_GAIN;
}

// gezegenin hizini asalt (surtunme -- artik nereye surtunuyorsa bombos uzayda?)
$data["vx"] -= $data["vx"]*FRICTION;
$data["vy"] -= $data["vy"]*FRICTION;

if($data["vx"] > MAX_SPEED) $data["vx"] = MAX_SPEED;
if($data["vx"] < -MAX_SPEED) $data["vx"] = -MAX_SPEED;
if($data["vy"] > MAX_SPEED) $data["vy"] = MAX_SPEED;
if($data["vy"] < -MAX_SPEED) $data["vy"] = -MAX_SPEED;


// gezegen cok yavaslarsa durdur (bu gerekli olmayabilir de)
if($data["vx"] < STOP_SPEED && $data["vx"] > -STOP_SPEED) $data["vx"] = 0;
if($data["vy"] < STOP_SPEED && $data["vy"] > -STOP_SPEED) $data["vy"] = 0;

// ziplayan yonu kaydet (diger kisilere haber vermek icin gerekli)
if($force > 0) {
	$data["jump". $region] = $now+1;
}

// ziplayan varsa cevap verisine bu bilgiyi yaz
$jumpers = ["n" => 0, "e" => 0, "s" => 0, "w" => 0];
if($now - $data["jumpn"] < REFRESH_PERIOD) $jumpers["n"] = 1;
if($now - $data["jumpe"] < REFRESH_PERIOD) $jumpers["e"] = 1;
if($now - $data["jumps"] < REFRESH_PERIOD) $jumpers["s"] = 1;
if($now - $data["jumpw"] < REFRESH_PERIOD) $jumpers["w"] = 1;

// meteroid jsonunu decode et
$meteroids = json_decode($data["meteroids"], true);
$meteroidSpeed = json_decode($data["meteroidspeed"], true);

// meteroid konumlarini guncelle
if($now - $data["lastUpdate"] > REFRESH_PERIOD) {
	foreach ($meteroids as $key => $meteroid) {
		$distx = $meteroids[$key]["x"] - $data["x"];
		$disty = $meteroids[$key]["y"] - $data["y"];
		if (($distx*$distx + $disty*$disty) < CRASH_DISTANCE*CRASH_DISTANCE) {
			unset($meteroids[$key]);
			unset($meteroidSpeed[$key]);
			$data["health"]-= DAMAGE;
			if($data["health"] < 0) $data["health"] = 0;
		} else {
			$meteroids[$key]["x"] = (float)number_format($meteroids[$key]["x"] + $meteroidSpeed[$key]["x"], 3); 
			$meteroids[$key]["y"] = (float)number_format($meteroids[$key]["y"] + $meteroidSpeed[$key]["y"], 3); 
		}
	}
}

// meteroid yarat
if($now - $data["lastUpdate"] > REFRESH_PERIOD && $data["lastUpdate"] % METEROID_CREATE_TIME < REFRESH_PERIOD) {
	$createDirection = rand(0,3);
	switch ($createDirection) {
		case 0: $newMeteroid = ["x" => rand(MIN_X + $data["x"], MAX_X + $data["x"]), "y" => MIN_Y + $data["y"]]; break;
		case 1: $newMeteroid = ["x" => MAX_X + $data["x"], "y" => rand(MIN_Y + $data["y"], MAX_Y + $data["y"])]; break;
		case 2:	$newMeteroid = ["x" => rand(MIN_X + $data["x"], MAX_X + $data["x"]), "y" => MAX_Y + $data["y"]]; break;
		case 3:	$newMeteroid = ["x" => MIN_X + $data["x"], "y" => rand(MIN_Y + $data["y"], MAX_Y + $data["y"])]; break;
	}

	$newMeteroidSpeed = [];
	$angle = atan2(($data["x"] - $newMeteroid["x"]), ($data["y"] - $newMeteroid["y"]));
	$newMeteroidSpeed["x"] = sin($angle) * rand(METEROID_SPEED_MIN_LIMIT, METEROID_SPEED_MAX_LIMIT)/10 + $data["vx"] * rand(0, METEROID_LAPSE)/100;
	$newMeteroidSpeed["y"] = cos($angle) * rand(METEROID_SPEED_MIN_LIMIT, METEROID_SPEED_MAX_LIMIT)/10 + $data["vy"] * rand(0, METEROID_LAPSE)/100;

	$newMeteroid["angle"] = (float)number_format(atan2($newMeteroidSpeed["x"], $newMeteroidSpeed["y"]), 3);
	$newMeteroid["id"] = $data["meteroidCount"]++;
	$meteroids[] = $newMeteroid;
	$meteroidSpeed[] = $newMeteroidSpeed;
}

// ekrandan cikan meteroidleri yok et
foreach ($meteroids as $key => $meteroid) {
	if ($meteroids[$key]["x"] < $data["x"] + MIN_X-10 || $meteroids[$key]["x"] >  $data["x"] + MAX_X+10 || $meteroids[$key]["y"] <  $data["y"] + MIN_Y-10 || $meteroids[$key]["y"] >  $data["y"] + MAX_Y+10) {
		unset($meteroids[$key]);
		unset($meteroidSpeed[$key]);
	}
}

// fix meteroid format
$meteroids = array_values($meteroids);	
$meteroidSpeed = array_values($meteroidSpeed);

// guncel verileri veritabanina yaz
if ($now - $data["lastUpdate"] > REFRESH_PERIOD) {
	$data["lastUpdate"] = $now;
}

$query = "UPDATE game SET x=". $data["x"] .", y=". $data["y"] .", vx=". $data["vx"] .", vy=". $data["vy"] .", meteroids='". json_encode($meteroids) ."', meteroidspeed='". json_encode($meteroidSpeed) ."', meteroidCount=". $data["meteroidCount"] .", jumpn='". $data["jumpn"] ."', jumpe='". $data["jumpe"] ."', jumps='". $data["jumps"] ."', jumpw='". $data["jumpw"] ."', lastUpdate=". $data["lastUpdate"] .", health=". $data["health"] ." WHERE room=". $room;
$conn->query($query);

// veritabani baglantisini kapat
$conn->close();

// cevap icin json olustur
$response = [
	"x" => (float)number_format($data["x"], 3),
	"y" => (float)number_format($data["y"], 3),
	"meteroids" => $meteroids,
	"jumpers" => $jumpers,
	"time" => $now - $data["createTime"],
	"health" => $data["health"]
];

echo json_encode($response);

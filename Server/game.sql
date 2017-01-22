-- phpMyAdmin SQL Dump
-- version 4.0.10deb1
-- http://www.phpmyadmin.net
--
-- Anamakine: localhost
-- Üretim Zamanı: 22 Oca 2017, 07:07:14
-- Sunucu sürümü: 5.5.53-0ubuntu0.14.04.1
-- PHP Sürümü: 5.5.9-1ubuntu4.20

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Veritabanı: `kinix_ggj`
--

-- --------------------------------------------------------

--
-- Tablo için tablo yapısı `game`
--

CREATE TABLE IF NOT EXISTS `game` (
  `room` int(11) NOT NULL AUTO_INCREMENT,
  `vx` float NOT NULL,
  `vy` float NOT NULL,
  `x` float NOT NULL,
  `y` float NOT NULL,
  `meteroids` text NOT NULL,
  `meteroidspeed` text NOT NULL,
  `meteroidCount` int(11) DEFAULT '0',
  `jumpn` bigint(11) DEFAULT '0',
  `jumpe` bigint(11) DEFAULT '0',
  `jumps` bigint(11) DEFAULT '0',
  `jumpw` bigint(11) DEFAULT '0',
  `lastUpdate` double DEFAULT '0',
  `health` int(10) NOT NULL,
  `createTime` double NOT NULL,
  PRIMARY KEY (`room`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4 ;

--
-- Tablo döküm verisi `game`
--

INSERT INTO `game` (`room`, `vx`, `vy`, `x`, `y`, `meteroids`, `meteroidspeed`, `meteroidCount`, `jumpn`, `jumpe`, `jumps`, `jumpw`, `lastUpdate`, `health`, `createTime`) VALUES
(1, -2.36598, 0.588074, 184.395, -915.819, '[{"x":197.608,"y":-918.797,"angle":2.344,"id":"393"},{"x":178.795,"y":-904.408,"angle":-1.043,"id":"394"},{"x":168.964,"y":-908.644,"angle":-1.094,"id":"395"}]', '[{"x":1.7680692274361,"y":-1.7241547615622},{"x":-7.9729868142304,"y":4.6477682689965},{"x":-0.68857766856387,"y":0.35565485627772}]', 396, 1485086833590, 1485086833808, 1485086833597, 1485086834004, 1485086833589, 1630, 0),
(3, 0, 1.59872, 0, 322.005, '[]', '[]', 4, 0, 0, 1485067575573, 0, 1485071187968, 38, 1485032652800);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

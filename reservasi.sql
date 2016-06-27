/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : reservasi

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2016-06-27 18:55:08
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for extra_bed
-- ----------------------------
DROP TABLE IF EXISTS `extra_bed`;
CREATE TABLE `extra_bed` (
  `id_reservasi` int(10) unsigned DEFAULT NULL,
  `tgl_sewa` date DEFAULT NULL,
  `tgl_berhenti` date DEFAULT NULL,
  KEY `id_reservasi` (`id_reservasi`),
  CONSTRAINT `extra_bed_ibfk_1` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for reservasi
-- ----------------------------
DROP TABLE IF EXISTS `reservasi`;
CREATE TABLE `reservasi` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `id_kamar` int(10) unsigned DEFAULT NULL,
  `tgl_check_in` date DEFAULT NULL,
  `jam_check_in` time DEFAULT NULL,
  `temp_bayar` int(11) DEFAULT '0',
  `status_out` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for tamu
-- ----------------------------
DROP TABLE IF EXISTS `tamu`;
CREATE TABLE `tamu` (
  `id` varchar(50) NOT NULL,
  `nama` varchar(50) DEFAULT NULL,
  `alamat` varchar(255) DEFAULT NULL,
  `telepon` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for transaksi
-- ----------------------------
DROP TABLE IF EXISTS `transaksi`;
CREATE TABLE `transaksi` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `id_reservasi` int(10) unsigned DEFAULT NULL,
  `id_tamu` varchar(50) DEFAULT NULL,
  `tgl_check_in` date DEFAULT NULL,
  `jam_check_in` time DEFAULT NULL,
  `tgl_check_out` date DEFAULT NULL,
  `jam_check_out` time DEFAULT NULL,
  `jumlah_bayar` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id_reservasi` (`id_reservasi`),
  KEY `id_tamu` (`id_tamu`),
  CONSTRAINT `transaksi_ibfk_2` FOREIGN KEY (`id_tamu`) REFERENCES `tamu` (`id`),
  CONSTRAINT `transaksi_ibfk_1` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

-- ----------------------------
-- View structure for transaksi_tamu
-- ----------------------------
DROP VIEW IF EXISTS `transaksi_tamu`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER  VIEW `transaksi_tamu` AS SELECT
transaksi.id,
transaksi.id_reservasi,
transaksi.id_tamu,
tamu.nama,
tamu.alamat,
tamu.telepon,
reservasi.id_kamar,
transaksi.tgl_check_out,
transaksi.jam_check_out,
transaksi.jumlah_bayar
FROM
tamu
INNER JOIN transaksi ON transaksi.id_tamu = tamu.id
INNER JOIN reservasi ON transaksi.id_reservasi = reservasi.id ;

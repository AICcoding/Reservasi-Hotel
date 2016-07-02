/*
Navicat MySQL Data Transfer

Source Server         : MySQL
Source Server Version : 50617
Source Host           : localhost:3306
Source Database       : reservasi

Target Server Type    : MYSQL
Target Server Version : 50617
File Encoding         : 65001

Date: 2016-07-02 20:03:10
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for extra_bed
-- ----------------------------
DROP TABLE IF EXISTS `extra_bed`;
CREATE TABLE `extra_bed` (
  `id_reservasi` int(10) unsigned NOT NULL,
  `id_tarif` int(10) unsigned DEFAULT NULL,
  `tgl_sewa` date DEFAULT NULL,
  `tgl_berhenti` date DEFAULT NULL,
  KEY `fk_id_reservasi2` (`id_reservasi`),
  KEY `fk_id_tarif2` (`id_tarif`),
  CONSTRAINT `fk_id_tarif2` FOREIGN KEY (`id_tarif`) REFERENCES `tarif` (`id_tarif`),
  CONSTRAINT `fk_id_reservasi2` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for kamar
-- ----------------------------
DROP TABLE IF EXISTS `kamar`;
CREATE TABLE `kamar` (
  `id_kamar` int(255) unsigned NOT NULL AUTO_INCREMENT,
  `id_tarif` int(10) unsigned DEFAULT NULL,
  `fasilitas` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id_kamar`),
  KEY `fk_id_tarif` (`id_tarif`),
  CONSTRAINT `fk_id_tarif` FOREIGN KEY (`id_tarif`) REFERENCES `tarif` (`id_tarif`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for konsumsi
-- ----------------------------
DROP TABLE IF EXISTS `konsumsi`;
CREATE TABLE `konsumsi` (
  `id_konsumsi` int(255) unsigned NOT NULL AUTO_INCREMENT,
  `nama` varchar(255) DEFAULT NULL,
  `harga` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id_konsumsi`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for log_reservasi
-- ----------------------------
DROP TABLE IF EXISTS `log_reservasi`;
CREATE TABLE `log_reservasi` (
  `id_reservasi` int(10) unsigned NOT NULL,
  `id_konsumsi` int(10) unsigned DEFAULT NULL,
  `qty` int(11) DEFAULT NULL,
  `custom` varchar(255) DEFAULT NULL,
  `tarif` int(10) unsigned DEFAULT NULL,
  KEY `fk_id_reservasi3` (`id_reservasi`),
  KEY `fk_id_konsumsi` (`id_konsumsi`),
  CONSTRAINT `fk_id_konsumsi` FOREIGN KEY (`id_konsumsi`) REFERENCES `konsumsi` (`id_konsumsi`),
  CONSTRAINT `fk_id_reservasi3` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for reservasi
-- ----------------------------
DROP TABLE IF EXISTS `reservasi`;
CREATE TABLE `reservasi` (
  `id_reservasi` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `id_kamar` int(10) unsigned DEFAULT NULL,
  `tgl_check_in` date DEFAULT NULL,
  `jam_check_in` time DEFAULT NULL,
  `tgl_check_out` date DEFAULT NULL,
  `jam_check_out` time DEFAULT NULL,
  `temp_bayar` int(11) DEFAULT '0',
  `status_out` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`id_reservasi`),
  KEY `fk_id_kamar` (`id_kamar`),
  CONSTRAINT `fk_id_kamar` FOREIGN KEY (`id_kamar`) REFERENCES `kamar` (`id_kamar`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for tamu
-- ----------------------------
DROP TABLE IF EXISTS `tamu`;
CREATE TABLE `tamu` (
  `id_tamu` varchar(50) NOT NULL,
  `nama` varchar(50) DEFAULT NULL,
  `alamat` varchar(255) DEFAULT NULL,
  `telepon` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id_tamu`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for tarif
-- ----------------------------
DROP TABLE IF EXISTS `tarif`;
CREATE TABLE `tarif` (
  `id_tarif` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `nama_tarif` varchar(255) DEFAULT NULL,
  `nominal` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id_tarif`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for transaksi
-- ----------------------------
DROP TABLE IF EXISTS `transaksi`;
CREATE TABLE `transaksi` (
  `id_transaksi` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `id_reservasi` int(10) unsigned DEFAULT NULL,
  `id_tamu` varchar(50) DEFAULT NULL,
  `tgl_masuk` date DEFAULT NULL,
  `jam_masuk` time DEFAULT NULL,
  `tgl_keluar` date DEFAULT NULL,
  `jam_keluar` time DEFAULT NULL,
  `jumlah_bayar` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_transaksi`),
  KEY `fk_id_reservasi` (`id_reservasi`),
  KEY `fk_id_tamu` (`id_tamu`),
  CONSTRAINT `fk_id_tamu` FOREIGN KEY (`id_tamu`) REFERENCES `tamu` (`id_tamu`),
  CONSTRAINT `fk_id_reservasi` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- View structure for transaksi_tamu
-- ----------------------------
DROP VIEW IF EXISTS `transaksi_tamu`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER  VIEW `transaksi_tamu` AS SELECT
transaksi.id_transaksi,
transaksi.id_reservasi,
transaksi.id_tamu,
tamu.nama,
tamu.alamat,
tamu.telepon,
reservasi.id_kamar,
transaksi.tgl_masuk,
transaksi.jam_masuk,
transaksi.tgl_keluar,
transaksi.jam_keluar,
transaksi.jumlah_bayar
FROM
tamu
INNER JOIN transaksi ON transaksi.id_tamu = tamu.id_tamu
INNER JOIN reservasi ON transaksi.id_reservasi = reservasi.id_reservasi ;

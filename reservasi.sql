/*
Navicat MySQL Data Transfer

Source Server         : mysql
Source Server Version : 100109
Source Host           : localhost:3306
Source Database       : reservasi

Target Server Type    : MYSQL
Target Server Version : 100109
File Encoding         : 65001

Date: 2016-07-13 22:55:18
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for extra_bed
-- ----------------------------
DROP TABLE IF EXISTS `extra_bed`;
CREATE TABLE `extra_bed` (
  `id_extra_bed` int(255) NOT NULL AUTO_INCREMENT,
  `id_reservasi` int(10) unsigned NOT NULL,
  `id_tarif` int(10) unsigned DEFAULT NULL,
  `tgl_sewa` date DEFAULT NULL,
  `tgl_berhenti` date DEFAULT NULL,
  `status_selesai` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`id_extra_bed`),
  KEY `fk_id_reservasi2` (`id_reservasi`) USING BTREE,
  KEY `fk_id_tarif2` (`id_tarif`) USING BTREE,
  CONSTRAINT `extra_bed_ibfk_1` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`),
  CONSTRAINT `extra_bed_ibfk_2` FOREIGN KEY (`id_tarif`) REFERENCES `tarif` (`id_tarif`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- ----------------------------
-- Table structure for kamar
-- ----------------------------
DROP TABLE IF EXISTS `kamar`;
CREATE TABLE `kamar` (
  `id_kamar` int(255) unsigned NOT NULL AUTO_INCREMENT,
  `id_tarif` int(10) unsigned DEFAULT NULL,
  `fasilitas` varchar(255) DEFAULT NULL,
  `lantai` int(255) DEFAULT NULL,
  `type` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_kamar`),
  KEY `fk_id_tarif` (`id_tarif`) USING BTREE,
  CONSTRAINT `kamar_ibfk_1` FOREIGN KEY (`id_tarif`) REFERENCES `tarif` (`id_tarif`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- ----------------------------
-- Table structure for konsumsi
-- ----------------------------
DROP TABLE IF EXISTS `konsumsi`;
CREATE TABLE `konsumsi` (
  `id_konsumsi` int(255) unsigned NOT NULL AUTO_INCREMENT,
  `nama` varchar(255) DEFAULT NULL,
  `harga` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id_konsumsi`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

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
  KEY `fk_id_reservasi3` (`id_reservasi`) USING BTREE,
  KEY `fk_id_konsumsi` (`id_konsumsi`) USING BTREE,
  CONSTRAINT `log_reservasi_ibfk_1` FOREIGN KEY (`id_konsumsi`) REFERENCES `konsumsi` (`id_konsumsi`),
  CONSTRAINT `log_reservasi_ibfk_2` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

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
  KEY `fk_id_kamar` (`id_kamar`) USING BTREE,
  CONSTRAINT `reservasi_ibfk_1` FOREIGN KEY (`id_kamar`) REFERENCES `kamar` (`id_kamar`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

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
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- ----------------------------
-- Table structure for tarif
-- ----------------------------
DROP TABLE IF EXISTS `tarif`;
CREATE TABLE `tarif` (
  `id_tarif` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `nama_tarif` varchar(255) DEFAULT NULL,
  `nominal` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id_tarif`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

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
  KEY `fk_id_reservasi` (`id_reservasi`) USING BTREE,
  KEY `fk_id_tamu` (`id_tamu`) USING BTREE,
  CONSTRAINT `transaksi_ibfk_1` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`),
  CONSTRAINT `transaksi_ibfk_2` FOREIGN KEY (`id_tamu`) REFERENCES `tamu` (`id_tamu`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- ----------------------------
-- View structure for cek_tarif_kamar
-- ----------------------------
DROP VIEW IF EXISTS `cek_tarif_kamar`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost`  VIEW `cek_tarif_kamar` AS SELECT
reservasi.id_reservasi,
tarif.nominal
FROM
reservasi
INNER JOIN kamar ON reservasi.id_kamar = kamar.id_kamar
INNER JOIN tarif ON kamar.id_tarif = tarif.id_tarif ;

-- ----------------------------
-- View structure for rekomendasi
-- ----------------------------
DROP VIEW IF EXISTS `rekomendasi`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost`  VIEW `rekomendasi` AS SELECT
transaksi.id_reservasi,
tamu.nama,
transaksi.tgl_masuk,
transaksi.tgl_keluar
FROM
transaksi
INNER JOIN tamu ON transaksi.id_tamu = tamu.id_tamu ;

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

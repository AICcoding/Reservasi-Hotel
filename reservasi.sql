/*
Navicat MySQL Data Transfer

Source Server         : mysql
Source Server Version : 50621
Source Host           : localhost:3306
Source Database       : reservasi

Target Server Type    : MYSQL
Target Server Version : 50621
File Encoding         : 65001

Date: 2016-07-04 15:26:00
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for extra_bed
-- ----------------------------
DROP TABLE IF EXISTS `extra_bed`;
CREATE TABLE `extra_bed` (
`id_extra_bed`  int(255) NOT NULL AUTO_INCREMENT ,
`id_reservasi`  int(10) UNSIGNED NOT NULL ,
`id_tarif`  int(10) UNSIGNED NULL DEFAULT NULL ,
`tgl_sewa`  date NULL DEFAULT NULL ,
`tgl_berhenti`  date NULL DEFAULT NULL ,
`status_selesai`  tinyint(4) NULL DEFAULT 0 ,
PRIMARY KEY (`id_extra_bed`),
FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`) ON DELETE RESTRICT ON UPDATE RESTRICT,
FOREIGN KEY (`id_tarif`) REFERENCES `tarif` (`id_tarif`) ON DELETE RESTRICT ON UPDATE RESTRICT,
INDEX `fk_id_reservasi2` (`id_reservasi`) USING BTREE ,
INDEX `fk_id_tarif2` (`id_tarif`) USING BTREE 
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=latin1 COLLATE=latin1_swedish_ci
AUTO_INCREMENT=7

;

-- ----------------------------
-- Records of extra_bed
-- ----------------------------
BEGIN;
INSERT INTO `extra_bed` VALUES ('2', '1', '13', '2016-07-03', '2016-07-04', '0'), ('4', '1', '12', '2016-07-03', '2016-07-04', '0'), ('6', '1', '12', '2016-07-03', '2016-07-04', '0');
COMMIT;

-- ----------------------------
-- Table structure for kamar
-- ----------------------------
DROP TABLE IF EXISTS `kamar`;
CREATE TABLE `kamar` (
`id_kamar`  int(255) UNSIGNED NOT NULL AUTO_INCREMENT ,
`id_tarif`  int(10) UNSIGNED NULL DEFAULT NULL ,
`fasilitas`  varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL ,
PRIMARY KEY (`id_kamar`),
FOREIGN KEY (`id_tarif`) REFERENCES `tarif` (`id_tarif`) ON DELETE RESTRICT ON UPDATE RESTRICT,
INDEX `fk_id_tarif` (`id_tarif`) USING BTREE 
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=latin1 COLLATE=latin1_swedish_ci
AUTO_INCREMENT=5

;

-- ----------------------------
-- Records of kamar
-- ----------------------------
BEGIN;
INSERT INTO `kamar` VALUES ('1', '13', 'TV LED, Wifi, AC'), ('2', '14', 'TV LED, Wifi, AC, Air hangat, Kulkas'), ('4', '14', 'TV LED, Wifi, AC, Air hangat, Kulkas');
COMMIT;

-- ----------------------------
-- Table structure for konsumsi
-- ----------------------------
DROP TABLE IF EXISTS `konsumsi`;
CREATE TABLE `konsumsi` (
`id_konsumsi`  int(255) UNSIGNED NOT NULL AUTO_INCREMENT ,
`nama`  varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL ,
`harga`  int(10) UNSIGNED NULL DEFAULT NULL ,
PRIMARY KEY (`id_konsumsi`)
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=latin1 COLLATE=latin1_swedish_ci
AUTO_INCREMENT=3

;

-- ----------------------------
-- Records of konsumsi
-- ----------------------------
BEGIN;
INSERT INTO `konsumsi` VALUES ('1', 'Aqua gelas', '1000'), ('2', 'Aqua tanggung', '4000');
COMMIT;

-- ----------------------------
-- Table structure for log_reservasi
-- ----------------------------
DROP TABLE IF EXISTS `log_reservasi`;
CREATE TABLE `log_reservasi` (
`id_reservasi`  int(10) UNSIGNED NOT NULL ,
`id_konsumsi`  int(10) UNSIGNED NULL DEFAULT NULL ,
`qty`  int(11) NULL DEFAULT NULL ,
`custom`  varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL ,
`tarif`  int(10) UNSIGNED NULL DEFAULT NULL ,
FOREIGN KEY (`id_konsumsi`) REFERENCES `konsumsi` (`id_konsumsi`) ON DELETE RESTRICT ON UPDATE RESTRICT,
FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`) ON DELETE RESTRICT ON UPDATE RESTRICT,
INDEX `fk_id_reservasi3` (`id_reservasi`) USING BTREE ,
INDEX `fk_id_konsumsi` (`id_konsumsi`) USING BTREE 
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=latin1 COLLATE=latin1_swedish_ci

;

-- ----------------------------
-- Records of log_reservasi
-- ----------------------------
BEGIN;
COMMIT;

-- ----------------------------
-- Table structure for reservasi
-- ----------------------------
DROP TABLE IF EXISTS `reservasi`;
CREATE TABLE `reservasi` (
`id_reservasi`  int(10) UNSIGNED NOT NULL AUTO_INCREMENT ,
`id_kamar`  int(10) UNSIGNED NULL DEFAULT NULL ,
`tgl_check_in`  date NULL DEFAULT NULL ,
`jam_check_in`  time NULL DEFAULT NULL ,
`tgl_check_out`  date NULL DEFAULT NULL ,
`jam_check_out`  time NULL DEFAULT NULL ,
`temp_bayar`  int(11) NULL DEFAULT 0 ,
`status_out`  tinyint(4) NULL DEFAULT 0 ,
PRIMARY KEY (`id_reservasi`),
FOREIGN KEY (`id_kamar`) REFERENCES `kamar` (`id_kamar`) ON DELETE RESTRICT ON UPDATE RESTRICT,
INDEX `fk_id_kamar` (`id_kamar`) USING BTREE 
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=latin1 COLLATE=latin1_swedish_ci
AUTO_INCREMENT=3

;

-- ----------------------------
-- Records of reservasi
-- ----------------------------
BEGIN;
INSERT INTO `reservasi` VALUES ('1', '2', '2016-07-03', '16:55:38', '0000-00-00', '16:55:45', '0', '0'), ('2', '1', '2016-07-04', '14:24:34', null, null, '0', '0');
COMMIT;

-- ----------------------------
-- Table structure for tamu
-- ----------------------------
DROP TABLE IF EXISTS `tamu`;
CREATE TABLE `tamu` (
`id_tamu`  varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL ,
`nama`  varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL ,
`alamat`  varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL ,
`telepon`  varchar(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL ,
PRIMARY KEY (`id_tamu`)
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=latin1 COLLATE=latin1_swedish_ci

;

-- ----------------------------
-- Records of tamu
-- ----------------------------
BEGIN;
INSERT INTO `tamu` VALUES ('12', 'agus', 'klungkung city', '081'), ('1313', 'Agus suarya', 'Klungkung', '085'), ('1334', 'Putu', 'Bali', '087');
COMMIT;

-- ----------------------------
-- Table structure for tarif
-- ----------------------------
DROP TABLE IF EXISTS `tarif`;
CREATE TABLE `tarif` (
`id_tarif`  int(10) UNSIGNED NOT NULL AUTO_INCREMENT ,
`nama_tarif`  varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL ,
`nominal`  int(10) UNSIGNED NULL DEFAULT NULL ,
PRIMARY KEY (`id_tarif`)
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=latin1 COLLATE=latin1_swedish_ci
AUTO_INCREMENT=15

;

-- ----------------------------
-- Records of tarif
-- ----------------------------
BEGIN;
INSERT INTO `tarif` VALUES ('12', 'Extra bed', '50000'), ('13', 'Kamar biasa', '100000'), ('14', 'Kamar VIP', '150000');
COMMIT;

-- ----------------------------
-- Table structure for transaksi
-- ----------------------------
DROP TABLE IF EXISTS `transaksi`;
CREATE TABLE `transaksi` (
`id_transaksi`  int(10) UNSIGNED NOT NULL AUTO_INCREMENT ,
`id_reservasi`  int(10) UNSIGNED NULL DEFAULT NULL ,
`id_tamu`  varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL ,
`tgl_masuk`  date NULL DEFAULT NULL ,
`jam_masuk`  time NULL DEFAULT NULL ,
`tgl_keluar`  date NULL DEFAULT NULL ,
`jam_keluar`  time NULL DEFAULT NULL ,
`jumlah_bayar`  int(11) NULL DEFAULT NULL ,
PRIMARY KEY (`id_transaksi`),
FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`) ON DELETE RESTRICT ON UPDATE RESTRICT,
FOREIGN KEY (`id_tamu`) REFERENCES `tamu` (`id_tamu`) ON DELETE RESTRICT ON UPDATE RESTRICT,
INDEX `fk_id_reservasi` (`id_reservasi`) USING BTREE ,
INDEX `fk_id_tamu` (`id_tamu`) USING BTREE 
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=latin1 COLLATE=latin1_swedish_ci
AUTO_INCREMENT=5

;

-- ----------------------------
-- Records of transaksi
-- ----------------------------
BEGIN;
INSERT INTO `transaksi` VALUES ('1', '1', '1313', '2016-07-04', '14:21:03', null, null, '0'), ('2', '1', '1313', '2016-07-04', '14:21:30', null, null, '0'), ('3', '2', '1334', '2016-07-04', '14:26:04', null, null, '0'), ('4', '1', '12', '2016-07-04', '15:16:49', null, null, '0');
COMMIT;

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

-- ----------------------------
-- Auto increment value for extra_bed
-- ----------------------------
ALTER TABLE `extra_bed` AUTO_INCREMENT=7;

-- ----------------------------
-- Auto increment value for kamar
-- ----------------------------
ALTER TABLE `kamar` AUTO_INCREMENT=5;

-- ----------------------------
-- Auto increment value for konsumsi
-- ----------------------------
ALTER TABLE `konsumsi` AUTO_INCREMENT=3;

-- ----------------------------
-- Auto increment value for reservasi
-- ----------------------------
ALTER TABLE `reservasi` AUTO_INCREMENT=3;

-- ----------------------------
-- Auto increment value for tarif
-- ----------------------------
ALTER TABLE `tarif` AUTO_INCREMENT=15;

-- ----------------------------
-- Auto increment value for transaksi
-- ----------------------------
ALTER TABLE `transaksi` AUTO_INCREMENT=5;

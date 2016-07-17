/*
Navicat MySQL Data Transfer

Source Server         : mysql
Source Server Version : 50621
Source Host           : localhost:3306
Source Database       : reservasi

Target Server Type    : MYSQL
Target Server Version : 50621
File Encoding         : 65001

Date: 2016-07-17 23:01:53
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
AUTO_INCREMENT=5

;

-- ----------------------------
-- Records of extra_bed
-- ----------------------------
BEGIN;
INSERT INTO `extra_bed` VALUES ('1', '1', '27', '2016-07-12', '2016-07-15', '1'), ('2', '9', '28', '2016-07-13', '2016-07-16', '0'), ('3', '8', '28', '2016-07-18', '2016-07-14', '1'), ('4', '9', '28', '2016-07-15', '2016-07-16', '0');
COMMIT;

-- ----------------------------
-- Table structure for kamar
-- ----------------------------
DROP TABLE IF EXISTS `kamar`;
CREATE TABLE `kamar` (
`id_kamar`  int(255) UNSIGNED NOT NULL AUTO_INCREMENT ,
`id_tarif`  int(10) UNSIGNED NULL DEFAULT NULL ,
`fasilitas`  varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL ,
`lantai`  int(255) NULL DEFAULT NULL ,
`type`  varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL ,
PRIMARY KEY (`id_kamar`),
FOREIGN KEY (`id_tarif`) REFERENCES `tarif` (`id_tarif`) ON DELETE RESTRICT ON UPDATE RESTRICT,
INDEX `fk_id_tarif` (`id_tarif`) USING BTREE 
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=latin1 COLLATE=latin1_swedish_ci
AUTO_INCREMENT=502

;

-- ----------------------------
-- Records of kamar
-- ----------------------------
BEGIN;
INSERT INTO `kamar` VALUES ('101', '16', 'Tv, kamar mandi dalam', '1', 'Standard room'), ('102', '16', 'Tv, kamar mandi dalam', '1', 'Standard room'), ('201', '17', 'Tv, kulkas, kamar mandi dalam', '2', 'Superior/Premium room'), ('301', '18', 'tv, kulkas, ac', '3', 'Deluxe room'), ('401', '19', 'tv, kulkas, ac, air hangat', '4', 'Junior Suite/Studio room'), ('501', '20', 'Tv, ac, kulkas, sound system', '5', 'Suite room');
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
AUTO_INCREMENT=7

;

-- ----------------------------
-- Records of konsumsi
-- ----------------------------
BEGIN;
INSERT INTO `konsumsi` VALUES ('3', 'Aqua gelas', '1000'), ('4', 'Aqua tanggung', '3000'), ('5', 'Bir Bintang Pilsener Besar 620ml', '28000'), ('6', 'Green Sands Original 250ml', '9000');
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
AUTO_INCREMENT=10

;

-- ----------------------------
-- Records of reservasi
-- ----------------------------
BEGIN;
INSERT INTO `reservasi` VALUES ('1', '101', '2016-07-12', '22:27:09', '2016-07-14', '21:34:06', '0', '1'), ('3', '102', '2016-07-14', '17:32:01', '2016-07-18', '21:34:09', '0', '0'), ('6', '201', '2016-07-14', '18:20:23', '2016-07-14', '18:20:24', '0', '1'), ('7', '201', '2016-07-14', '18:24:18', '2016-07-17', '18:24:18', '0', '1'), ('8', '201', '2016-07-14', '18:37:09', '2016-07-30', '06:00:00', '0', '0'), ('9', '101', '2016-07-16', '21:35:43', '2016-07-23', '06:00:00', '0', '0');
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
INSERT INTO `tamu` VALUES ('1', '1', '1', '1'), ('13', 'Agus', 'bali', '085'), ('1313', 'I Putu Agus Suarya Wibawa', 'Klungkung city', '085737013771'), ('134', 'Agus', 'bali', '086'), ('55', 'Suarya', 'Bali timur', '0888');
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
AUTO_INCREMENT=29

;

-- ----------------------------
-- Records of tarif
-- ----------------------------
BEGIN;
INSERT INTO `tarif` VALUES ('16', 'Standard room', '100000'), ('17', 'Superior/Premium room', '150000'), ('18', 'Deluxe room', '200000'), ('19', 'Junior Suite/Studio room', '240000'), ('20', 'Suite room', '280000'), ('21', 'Presidential/penthouse room', '300000'), ('27', 'Single bed 1 lapis', '50000'), ('28', 'Single bed 2 lapis', '60000');
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
`status_check_out`  tinyint(4) NULL DEFAULT 0 ,
PRIMARY KEY (`id_transaksi`),
FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`) ON DELETE RESTRICT ON UPDATE RESTRICT,
FOREIGN KEY (`id_tamu`) REFERENCES `tamu` (`id_tamu`) ON DELETE RESTRICT ON UPDATE RESTRICT,
INDEX `fk_id_reservasi` (`id_reservasi`) USING BTREE ,
INDEX `fk_id_tamu` (`id_tamu`) USING BTREE 
)
ENGINE=InnoDB
DEFAULT CHARACTER SET=latin1 COLLATE=latin1_swedish_ci
AUTO_INCREMENT=27

;

-- ----------------------------
-- Records of transaksi
-- ----------------------------
BEGIN;
INSERT INTO `transaksi` VALUES ('23', '9', '1313', '2016-07-14', '21:35:43', '2016-07-16', '21:35:43', '0', '0'), ('24', '8', '13', '2016-07-14', '21:35:54', '2016-07-23', '21:35:54', '0', '0'), ('25', '8', '134', '2016-07-17', '11:25:30', '2016-07-18', '11:25:30', '0', '0'), ('26', '3', '55', '2016-07-17', '11:25:48', '2016-07-18', '11:25:48', '0', '0');
COMMIT;

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
transaksi.jumlah_bayar,
transaksi.status_check_out
FROM
tamu
INNER JOIN transaksi ON transaksi.id_tamu = tamu.id_tamu
INNER JOIN reservasi ON transaksi.id_reservasi = reservasi.id_reservasi ;

-- ----------------------------
-- Auto increment value for extra_bed
-- ----------------------------
ALTER TABLE `extra_bed` AUTO_INCREMENT=5;

-- ----------------------------
-- Auto increment value for kamar
-- ----------------------------
ALTER TABLE `kamar` AUTO_INCREMENT=502;

-- ----------------------------
-- Auto increment value for konsumsi
-- ----------------------------
ALTER TABLE `konsumsi` AUTO_INCREMENT=7;

-- ----------------------------
-- Auto increment value for reservasi
-- ----------------------------
ALTER TABLE `reservasi` AUTO_INCREMENT=10;

-- ----------------------------
-- Auto increment value for tarif
-- ----------------------------
ALTER TABLE `tarif` AUTO_INCREMENT=29;

-- ----------------------------
-- Auto increment value for transaksi
-- ----------------------------
ALTER TABLE `transaksi` AUTO_INCREMENT=27;

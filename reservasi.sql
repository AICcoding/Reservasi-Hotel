CREATE TABLE `extra_bed` (
`id_extra_bed` int(255) NOT NULL AUTO_INCREMENT,
`id_reservasi` int(10) UNSIGNED NOT NULL,
`id_tarif` int(10) UNSIGNED NULL DEFAULT NULL,
`tgl_sewa` date NULL DEFAULT NULL,
`tgl_berhenti` date NULL DEFAULT NULL,
`status_selesai` tinyint(4) NULL DEFAULT 0,
PRIMARY KEY (`id_extra_bed`) ,
INDEX `fk_id_reservasi2` (`id_reservasi` ASC) USING BTREE,
INDEX `fk_id_tarif2` (`id_tarif` ASC) USING BTREE
)
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci
ROW_FORMAT = Compact;

CREATE TABLE `kamar` (
`id_kamar` int(255) UNSIGNED NOT NULL AUTO_INCREMENT,
`id_tarif` int(10) UNSIGNED NULL DEFAULT NULL,
`fasilitas` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
`lantai` int(255) NULL,
`type` varchar(50) NULL,
PRIMARY KEY (`id_kamar`) ,
INDEX `fk_id_tarif` (`id_tarif` ASC) USING BTREE
)
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci
ROW_FORMAT = Compact;

CREATE TABLE `konsumsi` (
`id_konsumsi` int(255) UNSIGNED NOT NULL AUTO_INCREMENT,
`nama` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
`harga` int(10) UNSIGNED NULL DEFAULT NULL,
PRIMARY KEY (`id_konsumsi`) 
)
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci
ROW_FORMAT = Compact;

CREATE TABLE `log_reservasi` (
`id_reservasi` int(10) UNSIGNED NOT NULL,
`id_konsumsi` int(10) UNSIGNED NULL DEFAULT NULL,
`qty` int(11) NULL DEFAULT NULL,
`custom` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
`tarif` int(10) UNSIGNED NULL DEFAULT NULL,
INDEX `fk_id_reservasi3` (`id_reservasi` ASC) USING BTREE,
INDEX `fk_id_konsumsi` (`id_konsumsi` ASC) USING BTREE
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci
ROW_FORMAT = Compact;

CREATE TABLE `reservasi` (
`id_reservasi` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
`id_kamar` int(10) UNSIGNED NULL DEFAULT NULL,
`tgl_check_in` date NULL DEFAULT NULL,
`jam_check_in` time NULL DEFAULT NULL,
`tgl_check_out` date NULL DEFAULT NULL,
`jam_check_out` time NULL DEFAULT NULL,
`temp_bayar` int(11) NULL DEFAULT 0,
`status_out` tinyint(4) NULL DEFAULT 0,
PRIMARY KEY (`id_reservasi`) ,
INDEX `fk_id_kamar` (`id_kamar` ASC) USING BTREE
)
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci
ROW_FORMAT = Compact;

CREATE TABLE `tamu` (
`id_tamu` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
`nama` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
`alamat` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
`telepon` varchar(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
PRIMARY KEY (`id_tamu`) 
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci
ROW_FORMAT = Compact;

CREATE TABLE `tarif` (
`id_tarif` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
`nama_tarif` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
`nominal` int(10) UNSIGNED NULL DEFAULT NULL,
PRIMARY KEY (`id_tarif`) 
)
ENGINE = InnoDB
AUTO_INCREMENT = 15
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci
ROW_FORMAT = Compact;

CREATE TABLE `transaksi` (
`id_transaksi` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
`id_reservasi` int(10) UNSIGNED NULL DEFAULT NULL,
`id_tamu` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
`tgl_masuk` date NULL DEFAULT NULL,
`jam_masuk` time NULL DEFAULT NULL,
`tgl_keluar` date NULL DEFAULT NULL,
`jam_keluar` time NULL DEFAULT NULL,
`jumlah_bayar` int(11) NULL DEFAULT NULL,
PRIMARY KEY (`id_transaksi`) ,
INDEX `fk_id_reservasi` (`id_reservasi` ASC) USING BTREE,
INDEX `fk_id_tamu` (`id_tamu` ASC) USING BTREE
)
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = latin1
COLLATE = latin1_swedish_ci
ROW_FORMAT = Compact;


ALTER TABLE `extra_bed` ADD CONSTRAINT `extra_bed_ibfk_1` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`) ON DELETE RESTRICT ON UPDATE RESTRICT;
ALTER TABLE `extra_bed` ADD CONSTRAINT `extra_bed_ibfk_2` FOREIGN KEY (`id_tarif`) REFERENCES `tarif` (`id_tarif`) ON DELETE RESTRICT ON UPDATE RESTRICT;
ALTER TABLE `kamar` ADD CONSTRAINT `kamar_ibfk_1` FOREIGN KEY (`id_tarif`) REFERENCES `tarif` (`id_tarif`) ON DELETE RESTRICT ON UPDATE RESTRICT;
ALTER TABLE `log_reservasi` ADD CONSTRAINT `log_reservasi_ibfk_1` FOREIGN KEY (`id_konsumsi`) REFERENCES `konsumsi` (`id_konsumsi`) ON DELETE RESTRICT ON UPDATE RESTRICT;
ALTER TABLE `log_reservasi` ADD CONSTRAINT `log_reservasi_ibfk_2` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`) ON DELETE RESTRICT ON UPDATE RESTRICT;
ALTER TABLE `reservasi` ADD CONSTRAINT `reservasi_ibfk_1` FOREIGN KEY (`id_kamar`) REFERENCES `kamar` (`id_kamar`) ON DELETE RESTRICT ON UPDATE RESTRICT;
ALTER TABLE `transaksi` ADD CONSTRAINT `transaksi_ibfk_1` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`) ON DELETE RESTRICT ON UPDATE RESTRICT;
ALTER TABLE `transaksi` ADD CONSTRAINT `transaksi_ibfk_2` FOREIGN KEY (`id_tamu`) REFERENCES `tamu` (`id_tamu`) ON DELETE RESTRICT ON UPDATE RESTRICT;


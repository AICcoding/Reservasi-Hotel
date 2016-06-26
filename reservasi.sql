CREATE TABLE `reservasi` (
`id` int UNSIGNED NOT NULL AUTO_INCREMENT,
`id_kamar` int UNSIGNED NULL,
`tgl_check_in` date NULL,
`jam_check_in` time NULL,
`temp_bayar` int NULL DEFAULT 0,
`status_out` tinyint NULL DEFAULT 0,
PRIMARY KEY (`id`) 
);

CREATE TABLE `transaksi` (
`id` int UNSIGNED NOT NULL AUTO_INCREMENT,
`id_reservasi` int UNSIGNED NULL,
`id_tamu` varchar(50) NULL,
`tgl_check_in` date NULL,
`jam_check_in` time NULL,
`tgl_check_out` date NULL,
`jam_check_out` time NULL,
`jumlah_bayar` int NULL,
PRIMARY KEY (`id`) 
);

CREATE TABLE `tamu` (
`id` varchar(50) NOT NULL,
`nama` varchar(50) NULL,
`alamat` varchar(255) NULL,
`telepon` varchar(20) NULL,
PRIMARY KEY (`id`) 
);

CREATE TABLE `extra_bed` (
`id_reservasi` int UNSIGNED NULL,
`tgl_sewa` date NULL,
`tgl_berhenti` date NULL
);


ALTER TABLE `transaksi` ADD FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id`);
ALTER TABLE `transaksi` ADD FOREIGN KEY (`id_tamu`) REFERENCES `tamu` (`id`);
ALTER TABLE `extra_bed` ADD FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id`);


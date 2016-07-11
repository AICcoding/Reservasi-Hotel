-- phpMyAdmin SQL Dump
-- version 4.1.14
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Jul 11, 2016 at 07:26 AM
-- Server version: 5.5.22
-- PHP Version: 5.5.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `reservasi`
--

-- --------------------------------------------------------

--
-- Table structure for table `extra_bed`
--

CREATE TABLE IF NOT EXISTS `extra_bed` (
  `id_extra_bed` int(255) NOT NULL AUTO_INCREMENT,
  `id_reservasi` int(10) unsigned NOT NULL,
  `id_tarif` int(10) unsigned DEFAULT NULL,
  `tgl_sewa` date DEFAULT NULL,
  `tgl_berhenti` date DEFAULT NULL,
  `status_selesai` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`id_extra_bed`),
  KEY `fk_id_reservasi2` (`id_reservasi`) USING BTREE,
  KEY `fk_id_tarif2` (`id_tarif`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=7 ;

--
-- Dumping data for table `extra_bed`
--

INSERT INTO `extra_bed` (`id_extra_bed`, `id_reservasi`, `id_tarif`, `tgl_sewa`, `tgl_berhenti`, `status_selesai`) VALUES
(2, 1, 13, '2016-07-03', '2016-07-04', 0),
(4, 1, 12, '2016-07-03', '2016-07-04', 0),
(6, 1, 12, '2016-07-03', '2016-07-04', 0);

-- --------------------------------------------------------

--
-- Table structure for table `kamar`
--

CREATE TABLE IF NOT EXISTS `kamar` (
  `id_kamar` int(255) unsigned NOT NULL AUTO_INCREMENT,
  `lantai` tinyint(4) NOT NULL,
  `type` varchar(50) NOT NULL,
  `id_tarif` int(10) unsigned DEFAULT NULL,
  `fasilitas` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id_kamar`),
  KEY `fk_id_tarif` (`id_tarif`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=204 ;

--
-- Dumping data for table `kamar`
--

INSERT INTO `kamar` (`id_kamar`, `lantai`, `type`, `id_tarif`, `fasilitas`) VALUES
(1, 0, '', 13, 'TV LED, Wifi, AC'),
(2, 0, '', 14, 'TV LED, Wifi, AC, Air hangat, Kulkas'),
(4, 0, '', 14, 'TV LED, Wifi, AC, Air hangat, Kulkas'),
(101, 1, 'Single', NULL, NULL),
(102, 1, 'Double', NULL, NULL),
(103, 1, 'Single', NULL, NULL),
(201, 2, 'Balinese Single', NULL, NULL),
(202, 2, 'Triple', NULL, NULL),
(203, 2, 'Single', NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `konsumsi`
--

CREATE TABLE IF NOT EXISTS `konsumsi` (
  `id_konsumsi` int(255) unsigned NOT NULL AUTO_INCREMENT,
  `nama` varchar(255) DEFAULT NULL,
  `harga` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id_konsumsi`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Dumping data for table `konsumsi`
--

INSERT INTO `konsumsi` (`id_konsumsi`, `nama`, `harga`) VALUES
(1, 'Aqua gelas', 1000),
(2, 'Aqua tanggung', 4000);

-- --------------------------------------------------------

--
-- Table structure for table `log_reservasi`
--

CREATE TABLE IF NOT EXISTS `log_reservasi` (
  `id_reservasi` int(10) unsigned NOT NULL,
  `id_konsumsi` int(10) unsigned DEFAULT NULL,
  `qty` int(11) DEFAULT NULL,
  `custom` varchar(255) DEFAULT NULL,
  `tarif` int(10) unsigned DEFAULT NULL,
  KEY `fk_id_reservasi3` (`id_reservasi`) USING BTREE,
  KEY `fk_id_konsumsi` (`id_konsumsi`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `reservasi`
--

CREATE TABLE IF NOT EXISTS `reservasi` (
  `id_reservasi` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `id_kamar` int(10) unsigned DEFAULT NULL,
  `tgl_check_in` date DEFAULT NULL,
  `jam_check_in` time DEFAULT NULL,
  `tgl_check_out` date DEFAULT NULL,
  `jam_check_out` time DEFAULT NULL,
  `temp_bayar` int(11) DEFAULT '0',
  `status_out` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`id_reservasi`),
  KEY `fk_id_kamar` (`id_kamar`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Dumping data for table `reservasi`
--

INSERT INTO `reservasi` (`id_reservasi`, `id_kamar`, `tgl_check_in`, `jam_check_in`, `tgl_check_out`, `jam_check_out`, `temp_bayar`, `status_out`) VALUES
(1, 2, '2016-07-03', '16:55:38', '0000-00-00', '16:55:45', 0, 0),
(2, 1, '2016-07-04', '14:24:34', NULL, NULL, 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `tamu`
--

CREATE TABLE IF NOT EXISTS `tamu` (
  `id_tamu` varchar(50) NOT NULL,
  `nama` varchar(50) DEFAULT NULL,
  `alamat` varchar(255) DEFAULT NULL,
  `telepon` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id_tamu`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tamu`
--

INSERT INTO `tamu` (`id_tamu`, `nama`, `alamat`, `telepon`) VALUES
('12', 'agus', 'klungkung city', '081'),
('1313', 'Agus suarya', 'Klungkung', '085'),
('1334', 'Putu', 'Bali', '087');

-- --------------------------------------------------------

--
-- Table structure for table `tarif`
--

CREATE TABLE IF NOT EXISTS `tarif` (
  `id_tarif` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `nama_tarif` varchar(255) DEFAULT NULL,
  `nominal` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id_tarif`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=15 ;

--
-- Dumping data for table `tarif`
--

INSERT INTO `tarif` (`id_tarif`, `nama_tarif`, `nominal`) VALUES
(12, 'Extra bed', 50000),
(13, 'Kamar biasa', 100000),
(14, 'Kamar VIP', 150000);

-- --------------------------------------------------------

--
-- Table structure for table `transaksi`
--

CREATE TABLE IF NOT EXISTS `transaksi` (
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
  KEY `fk_id_tamu` (`id_tamu`) USING BTREE
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=5 ;

--
-- Dumping data for table `transaksi`
--

INSERT INTO `transaksi` (`id_transaksi`, `id_reservasi`, `id_tamu`, `tgl_masuk`, `jam_masuk`, `tgl_keluar`, `jam_keluar`, `jumlah_bayar`) VALUES
(1, 1, '1313', '2016-07-04', '14:21:03', NULL, NULL, 0),
(2, 1, '1313', '2016-07-04', '14:21:30', NULL, NULL, 0),
(3, 2, '1334', '2016-07-04', '14:26:04', NULL, NULL, 0),
(4, 1, '12', '2016-07-04', '15:16:49', NULL, NULL, 0);

-- --------------------------------------------------------

--
-- Stand-in structure for view `transaksi_tamu`
--
CREATE TABLE IF NOT EXISTS `transaksi_tamu` (
`id_transaksi` int(10) unsigned
,`id_reservasi` int(10) unsigned
,`id_tamu` varchar(50)
,`nama` varchar(50)
,`alamat` varchar(255)
,`telepon` varchar(20)
,`id_kamar` int(10) unsigned
,`tgl_masuk` date
,`jam_masuk` time
,`tgl_keluar` date
,`jam_keluar` time
,`jumlah_bayar` int(11)
);
-- --------------------------------------------------------

--
-- Structure for view `transaksi_tamu`
--
DROP TABLE IF EXISTS `transaksi_tamu`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `transaksi_tamu` AS select `transaksi`.`id_transaksi` AS `id_transaksi`,`transaksi`.`id_reservasi` AS `id_reservasi`,`transaksi`.`id_tamu` AS `id_tamu`,`tamu`.`nama` AS `nama`,`tamu`.`alamat` AS `alamat`,`tamu`.`telepon` AS `telepon`,`id_kamar` AS `id_kamar`,`transaksi`.`tgl_masuk` AS `tgl_masuk`,`transaksi`.`jam_masuk` AS `jam_masuk`,`transaksi`.`tgl_keluar` AS `tgl_keluar`,`transaksi`.`jam_keluar` AS `jam_keluar`,`transaksi`.`jumlah_bayar` AS `jumlah_bayar` from ((`tamu` join `transaksi` on((`transaksi`.`id_tamu` = `tamu`.`id_tamu`))) join `reservasi` on((`transaksi`.`id_reservasi` = `id_reservasi`)));

--
-- Constraints for dumped tables
--

--
-- Constraints for table `extra_bed`
--
ALTER TABLE `extra_bed`
  ADD CONSTRAINT `extra_bed_ibfk_1` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`),
  ADD CONSTRAINT `extra_bed_ibfk_2` FOREIGN KEY (`id_tarif`) REFERENCES `tarif` (`id_tarif`);

--
-- Constraints for table `kamar`
--
ALTER TABLE `kamar`
  ADD CONSTRAINT `kamar_ibfk_1` FOREIGN KEY (`id_tarif`) REFERENCES `tarif` (`id_tarif`);

--
-- Constraints for table `log_reservasi`
--
ALTER TABLE `log_reservasi`
  ADD CONSTRAINT `log_reservasi_ibfk_1` FOREIGN KEY (`id_konsumsi`) REFERENCES `konsumsi` (`id_konsumsi`),
  ADD CONSTRAINT `log_reservasi_ibfk_2` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`);

--
-- Constraints for table `reservasi`
--
ALTER TABLE `reservasi`
  ADD CONSTRAINT `reservasi_ibfk_1` FOREIGN KEY (`id_kamar`) REFERENCES `kamar` (`id_kamar`);

--
-- Constraints for table `transaksi`
--
ALTER TABLE `transaksi`
  ADD CONSTRAINT `transaksi_ibfk_1` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id_reservasi`),
  ADD CONSTRAINT `transaksi_ibfk_2` FOREIGN KEY (`id_tamu`) REFERENCES `tamu` (`id_tamu`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

-- phpMyAdmin SQL Dump
-- version 4.2.11
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Jun 27, 2016 at 05:56 PM
-- Server version: 5.6.21
-- PHP Version: 5.6.3

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
`id` int(11) NOT NULL,
  `id_reservasi` int(10) unsigned DEFAULT NULL,
  `tgl_sewa` date DEFAULT NULL,
  `tgl_berhenti` date DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `extra_bed`
--

INSERT INTO `extra_bed` (`id`, `id_reservasi`, `tgl_sewa`, `tgl_berhenti`) VALUES
(3, 3, '2016-06-01', '2016-06-11');

-- --------------------------------------------------------

--
-- Table structure for table `reservasi`
--

CREATE TABLE IF NOT EXISTS `reservasi` (
`id` int(10) unsigned NOT NULL,
  `id_kamar` int(10) unsigned DEFAULT NULL,
  `tgl_check_in` date DEFAULT NULL,
  `jam_check_in` time DEFAULT NULL,
  `temp_bayar` int(11) DEFAULT '0',
  `status_out` tinyint(4) DEFAULT '0'
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `reservasi`
--

INSERT INTO `reservasi` (`id`, `id_kamar`, `tgl_check_in`, `jam_check_in`, `temp_bayar`, `status_out`) VALUES
(3, 1, '2016-06-27', '21:22:03', 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `tamu`
--

CREATE TABLE IF NOT EXISTS `tamu` (
  `id` varchar(50) NOT NULL,
  `nama` varchar(50) DEFAULT NULL,
  `alamat` varchar(255) DEFAULT NULL,
  `telepon` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tamu`
--

INSERT INTO `tamu` (`id`, `nama`, `alamat`, `telepon`) VALUES
('13', 'Agus Suarya', 'Klungkung city', '085737013771'),
('134', 'A', 'a', '11');

-- --------------------------------------------------------

--
-- Table structure for table `transaksi`
--

CREATE TABLE IF NOT EXISTS `transaksi` (
`id` int(10) unsigned NOT NULL,
  `id_reservasi` int(10) unsigned DEFAULT NULL,
  `id_tamu` varchar(50) DEFAULT NULL,
  `tgl_check_in` date DEFAULT NULL,
  `jam_check_in` time DEFAULT NULL,
  `tgl_check_out` date DEFAULT NULL,
  `jam_check_out` time DEFAULT NULL,
  `jumlah_bayar` int(11) DEFAULT NULL
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `transaksi`
--

INSERT INTO `transaksi` (`id`, `id_reservasi`, `id_tamu`, `tgl_check_in`, `jam_check_in`, `tgl_check_out`, `jam_check_out`, `jumlah_bayar`) VALUES
(7, 3, '13', '2016-06-27', '21:22:03', NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Stand-in structure for view `transaksi_tamu`
--
CREATE TABLE IF NOT EXISTS `transaksi_tamu` (
`id` int(10) unsigned
,`id_reservasi` int(10) unsigned
,`id_tamu` varchar(50)
,`nama` varchar(50)
,`alamat` varchar(255)
,`telepon` varchar(20)
,`id_kamar` int(10) unsigned
,`tgl_check_out` date
,`jam_check_out` time
,`jumlah_bayar` int(11)
,`tgl_check_in` date
,`jam_check_in` time
);
-- --------------------------------------------------------

--
-- Structure for view `transaksi_tamu`
--
DROP TABLE IF EXISTS `transaksi_tamu`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `transaksi_tamu` AS select `transaksi`.`id` AS `id`,`transaksi`.`id_reservasi` AS `id_reservasi`,`transaksi`.`id_tamu` AS `id_tamu`,`tamu`.`nama` AS `nama`,`tamu`.`alamat` AS `alamat`,`tamu`.`telepon` AS `telepon`,`id_kamar` AS `id_kamar`,`transaksi`.`tgl_check_out` AS `tgl_check_out`,`transaksi`.`jam_check_out` AS `jam_check_out`,`transaksi`.`jumlah_bayar` AS `jumlah_bayar`,`tgl_check_in` AS `tgl_check_in`,`jam_check_in` AS `jam_check_in` from ((`tamu` join `transaksi` on((`transaksi`.`id_tamu` = `tamu`.`id`))) join `reservasi` on((`transaksi`.`id_reservasi` = `id`)));

--
-- Indexes for dumped tables
--

--
-- Indexes for table `extra_bed`
--
ALTER TABLE `extra_bed`
 ADD PRIMARY KEY (`id`), ADD KEY `id_reservasi` (`id_reservasi`);

--
-- Indexes for table `reservasi`
--
ALTER TABLE `reservasi`
 ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tamu`
--
ALTER TABLE `tamu`
 ADD PRIMARY KEY (`id`);

--
-- Indexes for table `transaksi`
--
ALTER TABLE `transaksi`
 ADD PRIMARY KEY (`id`), ADD KEY `id_reservasi` (`id_reservasi`), ADD KEY `id_tamu` (`id_tamu`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `extra_bed`
--
ALTER TABLE `extra_bed`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT for table `reservasi`
--
ALTER TABLE `reservasi`
MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT for table `transaksi`
--
ALTER TABLE `transaksi`
MODIFY `id` int(10) unsigned NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=9;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `extra_bed`
--
ALTER TABLE `extra_bed`
ADD CONSTRAINT `extra_bed_ibfk_1` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id`);

--
-- Constraints for table `transaksi`
--
ALTER TABLE `transaksi`
ADD CONSTRAINT `transaksi_ibfk_1` FOREIGN KEY (`id_reservasi`) REFERENCES `reservasi` (`id`),
ADD CONSTRAINT `transaksi_ibfk_2` FOREIGN KEY (`id_tamu`) REFERENCES `tamu` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

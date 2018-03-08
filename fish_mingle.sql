-- phpMyAdmin SQL Dump
-- version 4.6.5.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:8889
-- Generation Time: Mar 08, 2018 at 11:56 PM
-- Server version: 5.6.35
-- PHP Version: 7.0.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `fish_mingle`
--
CREATE DATABASE IF NOT EXISTS `fish_mingle` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `fish_mingle`;

-- --------------------------------------------------------

--
-- Table structure for table `sessions`
--

DROP TABLE IF EXISTS `sessions`;
CREATE TABLE `sessions` (
  `id` bigint(20) NOT NULL,
  `user_id` int(255) NOT NULL,
  `session_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `sessions`
--

INSERT INTO `sessions` (`id`, `user_id`, `session_id`) VALUES
(4, 4, 47446859),
(5, 1, 76371539),
(7, 1, 43025188),
(8, 6, 34378631),
(9, 7, 88717452),
(10, 8, 35069825);

-- --------------------------------------------------------

--
-- Table structure for table `species`
--

DROP TABLE IF EXISTS `species`;
CREATE TABLE `species` (
  `species_id` int(255) NOT NULL,
  `species_name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `species`
--

INSERT INTO `species` (`species_id`, `species_name`) VALUES
(1, 'Flounder'),
(2, 'Grouper'),
(3, 'Clownfish'),
(4, 'Anglerfish'),
(5, 'Goldfish'),
(6, 'Guppy'),
(7, 'Carp'),
(8, 'Swordfish'),
(9, 'Catfish'),
(10, 'Salmon'),
(11, 'Mackerel'),
(12, 'Barracuda'),
(13, 'Piranha'),
(14, 'Pike'),
(15, 'Bass'),
(16, 'Tuna'),
(17, 'Rockfish'),
(18, 'Pufferfish'),
(19, 'Cuttlefish'),
(20, 'Stingray'),
(21, 'Jellyfish'),
(22, 'Electric eel'),
(23, 'Hammerhead shark'),
(24, 'Sea turtles'),
(25, 'Octopus');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `id` bigint(20) NOT NULL,
  `first_name` varchar(255) NOT NULL,
  `last_name` varchar(255) NOT NULL,
  `species_id` int(11) NOT NULL,
  `user_name` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `bio` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `first_name`, `last_name`, `species_id`, `user_name`, `password`, `bio`) VALUES
(1, 'Fish1', '', 1, 'Fish1', 'pwpwpwpw', 'Hi'),
(2, 'Fish2', '', 2, 'Fish2', 'pwpwpwpw', 'Hi HI'),
(3, 'Fish3', '', 23, 'Fish3', 'owpwpwpwpwpw', 'Hi there'),
(4, 'Fish4', '', 19, 'Fish4', 'asdasdasdasd', 'Hiii'),
(5, 'Larry', 'Fishman', 23, 'GuppyLove', 'password', 'Guppy lover 4 life'),
(6, 'guppy', 'guppy', 6, 'guppyyyyy', 'guppyyyyy', 'guyiesss'),
(8, 'sdfsdfsdf', 'sdfsdf', 7, 'dfsdfsdfsdfsdf', 'sdfsdfsdf', 'sdfsdfs');

-- --------------------------------------------------------

--
-- Table structure for table `users_species`
--

DROP TABLE IF EXISTS `users_species`;
CREATE TABLE `users_species` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `species_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `users_species`
--

INSERT INTO `users_species` (`id`, `user_id`, `species_id`) VALUES
(1, 1, 1),
(2, 1, 2),
(3, 1, 3),
(4, 1, 4),
(5, 1, 5),
(6, 1, 6),
(7, 1, 7),
(8, 1, 8),
(9, 1, 9),
(10, 1, 10),
(11, 2, 21),
(12, 2, 22),
(13, 2, 23),
(14, 2, 24),
(15, 2, 25),
(16, 3, 11),
(17, 3, 12),
(18, 3, 13),
(19, 3, 14),
(20, 3, 15),
(21, 3, 16),
(22, 3, 17),
(23, 3, 18),
(24, 3, 19),
(25, 3, 20),
(26, 4, 1),
(27, 4, 2),
(28, 4, 3),
(29, 4, 4),
(30, 4, 5),
(31, 4, 6),
(32, 4, 7),
(33, 4, 8),
(34, 4, 11),
(35, 4, 17),
(36, 4, 21),
(37, 4, 24),
(38, 4, 25),
(39, 5, 6),
(40, 6, 15),
(41, 7, 12),
(42, 8, 16);

-- --------------------------------------------------------

--
-- Table structure for table `users_users`
--

DROP TABLE IF EXISTS `users_users`;
CREATE TABLE `users_users` (
  `id` int(11) NOT NULL,
  `fish1_id` int(11) NOT NULL,
  `fish2_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `users_users`
--

INSERT INTO `users_users` (`id`, `fish1_id`, `fish2_id`) VALUES
(1, 1, 2),
(2, 2, 1),
(3, 3, 4),
(4, 2, 4),
(5, 3, 4),
(6, 4, 3);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `sessions`
--
ALTER TABLE `sessions`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `species`
--
ALTER TABLE `species`
  ADD PRIMARY KEY (`species_id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users_species`
--
ALTER TABLE `users_species`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users_users`
--
ALTER TABLE `users_users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `sessions`
--
ALTER TABLE `sessions`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;
--
-- AUTO_INCREMENT for table `species`
--
ALTER TABLE `species`
  MODIFY `species_id` int(255) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;
--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
--
-- AUTO_INCREMENT for table `users_species`
--
ALTER TABLE `users_species`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=43;
--
-- AUTO_INCREMENT for table `users_users`
--
ALTER TABLE `users_users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

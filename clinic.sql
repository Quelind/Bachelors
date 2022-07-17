-- phpMyAdmin SQL Dump
-- version 5.1.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 09, 2022 at 09:51 AM
-- Server version: 10.4.18-MariaDB
-- PHP Version: 8.0.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `clinic`
--

--
-- Table structure for table `employees`
--

CREATE TABLE `employees` (
  `id` int(11) NOT NULL,
  `specialization` varchar(255) NOT NULL,
  `fk_user` int(11) NOT NULL UNIQUE,
  `fk_room` varchar(255) NOT NULL,
  `image` varchar(255) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `employees`
--

INSERT INTO `employees` (`id`, `specialization`, `fk_user`, `fk_room`, `image`) VALUES
(1, 'Odontologist', 6, '311A', 'https://i.imgur.com/V2sIMU3.png'),
(2, 'Orthodontist', 7, '312A', 'https://i.imgur.com/03HAtfl.png'),
(3, 'Maxillofacial surgeon', 8, '313A', 'https://i.imgur.com/Q31JABC.png'),
(4, 'Endodontist', 9, '313B', 'https://i.imgur.com/PdToOJG.png');

-- --------------------------------------------------------

--
-- Table structure for table `patients`
--

CREATE TABLE `patients` (
  `id` int(11) NOT NULL,
  `information` varchar(255) NOT NULL,
  `fk_user` int(11) NOT NULL UNIQUE,
  `debt` double NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `patients`
--

INSERT INTO `patients` (`id`, `information`, `fk_user`, `debt`) VALUES
(1, '46 tooth cavity', 2, 50),
(2, '23 needs whitening', 3, 50),
(3, '11 falling out, 24 needs a filling', 4, 50),
(4, 'braces tightening on 11/24', 5, 50);

-- --------------------------------------------------------

--
-- Table structure for table `procedures`
--

CREATE TABLE `procedures` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `requirement` varchar(255) NOT NULL,
  `room_type` varchar(255) NOT NULL,
  `information` varchar(255) NOT NULL,
  `duration` int(11) NOT NULL,
  `personnel_count` int(11) NULL,
  `image` varchar(255) NOT NULL,
  `price` DOUBLE NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `procedures`
--

INSERT INTO `procedures` (`id`, `name`,`requirement`, `room_type`, `information`, `duration`, `personnel_count`, `image`, `price`) VALUES
(0, 'Initial visit', 'Odontologist', 'Procedural', 'Take your ID with you.', '60', '1', 'https://i.imgur.com/TeizLNc.png', 50),
(1, 'Teeth Straightening', 'Orthodontist', 'Procedural', 'Register for tightening in 3 weeks.', '60', '2', 'https://i.imgur.com/sJqUleY.png', 2000),
(2, 'Tooth Filling', 'Odontologist', 'Procedural', 'No eating for 30 minutes.', '60', '1', 'https://i.imgur.com/4Y2Cybb.png', 100),
(3, 'Teeth Whitening', 'Odontologist', 'Procedural', 'Take your ID with you.', '60', '1', 'https://i.imgur.com/zXCxdI5.png', 50),
(4, 'Oral Surgery', 'Maxillofacial surgeon', 'Procedural', 'Register for tightening in 3 weeks.', '60', '2', 'https://i.imgur.com/SiME0rQ.png', 50),
(5, 'Root Canal Treatment', 'Endodontist', 'Procedural', 'No eating for 30 minutes.', '60', '1', 'https://i.imgur.com/zy9VejZ.png', 50),
(6, 'Dental Prosthetics', 'Maxillofacial surgeon', 'Procedural', 'Pain may persist for 2 days.', '60', '1', 'https://i.imgur.com/pihUQLA.png', 5000),
(7, 'Oral Hygiene', 'Odontologist', 'Procedural', 'No food for 1 hour.', '60', '1', 'https://i.imgur.com/12iqXqz.png', 100),
(8, 'Treatment of Tooth Decay', 'Odontologist', 'Procedural', 'No food for 1 hour.', '60', '1', 'https://i.imgur.com/ggfOHxr.png', 100),
(9, 'Dentral Treatment of Children', 'Odontologist', 'Procedural', 'Guardian must be present.', '60', '1', 'https://i.imgur.com/WgqFJ5i.png', 100),
(10, 'Treatment of Worn Teeth', 'Odontologist', 'Procedural', 'Come for a checkup in 1 week.', '60', '1', 'https://i.imgur.com/nxmR3l1.png', 100),
(11, 'Dental surgery', 'Odontologist', 'Procedural', 'No eating before.', '60', '1', 'https://i.imgur.com/pwR9DGt.png', 500);

-- --------------------------------------------------------

--
-- Table structure for table `rooms`
--

CREATE TABLE `rooms` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL UNIQUE,
  `type` varchar(255) NOT NULL,
  `capacity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `rooms`
--

INSERT INTO `rooms` (`id`, `name`, `type`, `capacity`) VALUES
('1', '311A', 'Procedural', 2),
('2', '312A', 'Procedural', 1),
('3', '313A', 'Procedural', 2),
('4', '313B', 'Radiology', 1);

-- --------------------------------------------------------

--
-- Table structure for table `visits`
--

CREATE TABLE `visits` (
  `id` int(11) NOT NULL,
  `patient_comment` varchar(255) NULL,
  `fk_patient` int(11) NOT NULL,
  `fk_doctor` int(11) NOT NULL,
  `fk_room` varchar(255) NOT NULL,
  `fk_timetable` int(11) NOT NULL,
  `fk_procedure` int(11) NULL,
  `confirmed` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `visits`
--

INSERT INTO `visits` (`id`, `patient_comment`, `fk_patient`, `fk_doctor`, `fk_room`, `fk_timetable`, `fk_procedure`, `confirmed`) VALUES
(1, 'I have a tooth cavity and it is painful', '1', '1', '311A', '1', '0', 'no'),
(2, 'I feel a severe pain in my root canal', '2', '4', '313B', '19', '0', 'no'),
(3, 'First meeting to discuss getting me braces', '3', '2', '312A', '29', '0', 'no'),
(4, 'Severe pain in upper teeth', '4', '1', '311A', '39', '0', 'no'),
(5, 'Pain left side', '4', '1', '311A', '42', '0', 'no');

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `username` varchar(255) NOT NULL UNIQUE,
  `name` varchar(255) NOT NULL,
  `surname` varchar(255) NULL,
  `password` varchar(255) NOT NULL,
  `birthdate` varchar(255) NOT NULL,
  `phone` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `role` varchar(255) NOT NULL,
  `verified` varchar(255) NULL,
  `token` varchar(255) NULL,
  `reset_token` varchar(255) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `username`, `name`, `surname`, `password`, `birthdate`, `phone`, `email`, `role`, `token`, `verified`, `reset_token`) VALUES
(1, 'admin', 'admin', 'admin', '$2a$11$sZCMRAK0E.iaPJsoCt5SfeofXFBKlivAZ68uQ.P44GOTIiaHTpNAm', '1956-11-22', '867289614', 'admin@gmail.com', 'Admin', NULL, 'true', NULL),
(2, 'user', 'Marius', 'Kazlauskas', '$2a$11$KOyuz4JDXIG.M1MnbWcwyOIZRHMg.O/RjIfCL3N0XpI3o/OKKSJeW', '1997-10-21', '867289619', 'user@gmail.com', 'User', NULL, 'true', NULL),
(3, 'user1', 'Zenius', 'Jonaitis', '$2a$11$KOyuz4JDXIG.M1MnbWcwyOIZRHMg.O/RjIfCL3N0XpI3o/OKKSJeW', '1985-12-23', '867289629', 'user1@gmail.com', 'User', NULL, 'true', NULL),
(4, 'user2', 'Mantas', 'Petraitis', '$2a$11$KOyuz4JDXIG.M1MnbWcwyOIZRHMg.O/RjIfCL3N0XpI3o/OKKSJeW', '1999-07-13', '867285449', 'user2@gmail.com', 'User', NULL, 'true', NULL),
(5, 'user3', 'Vilija', 'Smetona', '$2a$11$KOyuz4JDXIG.M1MnbWcwyOIZRHMg.O/RjIfCL3N0XpI3o/OKKSJeW', '2007-12-31', '867285459', 'user3@gmail.com', 'User', NULL, 'true', NULL),
(6, 'employee', 'Jonas', 'Kazlauskas', '$2a$11$sRNZ.yzN5i30wH3FOfBevOSvjKU0F0uOFjYJsv65.nAXzmgcWlNMy', '1997-10-21', '867289614', 'employee@gmail.com', 'Employee', NULL, 'true', NULL),
(7, 'employee1', 'Alvydas', 'Petraitis', '$2a$11$sRNZ.yzN5i30wH3FOfBevOSvjKU0F0uOFjYJsv65.nAXzmgcWlNMy', '1985-12-23', '867289616', 'employee1@gmail.com', 'Employee', NULL, 'true', NULL),
(8, 'employee2', 'Simas', 'Vilkas', '$2a$11$sRNZ.yzN5i30wH3FOfBevOSvjKU0F0uOFjYJsv65.nAXzmgcWlNMy', '1999-07-13', '867285444', 'employee2@gmail.com', 'Employee', NULL, 'true', NULL),
(9, 'employee3', 'Matas', 'Didysis', '$2a$11$sRNZ.yzN5i30wH3FOfBevOSvjKU0F0uOFjYJsv65.nAXzmgcWlNMy', '1967-12-31', '867285445', 'employee3@gmail.com', 'Employee', NULL, 'true', NULL);


CREATE TABLE `timetables` (
  `id` int(11) NOT NULL,
  `date` DATE NOT NULL,
  `time` varchar(255) NOT NULL,
  `isLocked` varchar(255) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `timetables`
--

INSERT INTO `timetables` (`id`, `date`, `time`, `isLocked`) VALUES

(1, '2022-06-09', '09:00', 'fk1doc'),
(2, '2022-06-09', '10:00', ''),
(3, '2022-06-09', '11:00', ''),
(4, '2022-06-09', '13:00', ''),
(5, '2022-06-09', '14:00', ''),
(6, '2022-06-09', '15:00', ''),
(7, '2022-06-09', '16:00', ''),
(8, '2022-06-10', '09:00', ''),
(9, '2022-06-10', '10:00', ''),
(10, '2022-06-10', '11:00', ''),
(11, '2022-06-10', '13:00', ''),
(12, '2022-06-10', '14:00', ''),
(13, '2022-06-10', '15:00', ''),
(14, '2022-06-10', '16:00', ''),
(15, '2022-06-10', '09:00', ''),
(16, '2022-06-10', '10:00', ''),
(17, '2022-06-10', '11:00', ''),
(18, '2022-06-10', '13:00', ''),
(19, '2022-06-10', '14:00', 'fk4doc'),
(20, '2022-06-10', '15:00', ''),
(21, '2022-06-10', '16:00', ''),
(22, '2022-06-13', '09:00', ''),
(23, '2022-06-13', '10:00', ''),
(24, '2022-06-13', '11:00', ''),
(25, '2022-06-13', '13:00', ''),
(26, '2022-06-13', '14:00', ''),
(27, '2022-06-13', '15:00', ''),
(28, '2022-06-13', '16:00', ''),
(29, '2022-06-14', '09:00', 'fk2doc'),
(30, '2022-06-14', '10:00', ''),
(31, '2022-06-14', '11:00', ''),
(32, '2022-06-14', '13:00', ''),
(33, '2022-06-14', '14:00', ''),
(34, '2022-06-14', '15:00', ''),
(35, '2022-06-14', '16:00', ''),
(36, '2022-06-15', '09:00', ''),
(37, '2022-06-15', '10:00', ''),
(38, '2022-06-15', '11:00', ''),
(39, '2022-06-15', '13:00', 'fk1doc'),
(40, '2022-06-15', '14:00', ''),
(41, '2022-06-15', '15:00', ''),
(42, '2022-06-21', '16:00', 'fk1doc');

CREATE TABLE `histories` (
  `id` int(11) NOT NULL,
  `date` DATE NOT NULL,
  `name` varchar(255) NOT NULL,
  `description` varchar(255) NULL,
  `fk_patient` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `histories`
--

INSERT INTO `histories` (`id`, `date`, `name`, `description`, `fk_patient`) VALUES
(1, '2022-05-23', 'Cavity 46', 'Cured', '1'),
(2, '2022-05-23', 'Periodontitis', 'Cured', '1'),
(3, '2022-05-23', 'Sensitive Teeth', 'Cured', '1'),
(4, '2022-05-23', 'Oral cancer', 'Cured', '3'),
(5, '2022-05-23', 'Gingivitis', 'Cured', '1'),
(6, '2022-05-23', 'Cavity 44', 'Cured', '1'),
(7, '2022-05-23', 'Cavity 13', 'Cured', '1'),
(8, '2022-05-24', 'Cavity 46', 'Cured', '3'),
(9, '2022-05-24', 'Cavity 46', 'Cured', '2');



--
-- Indexes for table `employees`
--
ALTER TABLE `employees`
  ADD PRIMARY KEY (`id`);
  
--
-- Indexes for table `employees`
--
ALTER TABLE `timetables`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `patients`
--
ALTER TABLE `patients`
  ADD PRIMARY KEY (`id`);
  
  --
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);
  
--
-- Indexes for table `procedures`
--
ALTER TABLE `procedures`
  ADD PRIMARY KEY (`id`);
  
--
-- Indexes for table `rooms`
--
ALTER TABLE `rooms`
  ADD PRIMARY KEY (`id`);
  
--
-- Indexes for table `visits`
--
ALTER TABLE `visits`
  ADD PRIMARY KEY (`id`);
  
--
-- Indexes for table `histories`
--
ALTER TABLE `histories`
  ADD PRIMARY KEY (`id`);
  
--
-- AUTO_INCREMENT for table `employees`
--
ALTER TABLE `employees`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
  
--
-- AUTO_INCREMENT for table `histories`
--
ALTER TABLE `histories`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `patients`
--
ALTER TABLE `patients`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
  
--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
  
  
--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `timetables`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=50;
  
--
-- AUTO_INCREMENT for table `procedures`
--
ALTER TABLE `procedures`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `visits`
--
ALTER TABLE `visits`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=0;
  
--
-- AUTO_INCREMENT for table `rooms`
--
ALTER TABLE `rooms`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
  
--
-- Constraints for table `visits`
--
ALTER TABLE `visits`
  ADD CONSTRAINT `visits_ibfk_1` FOREIGN KEY (`fk_doctor`) REFERENCES `employees` (`id`);
  
ALTER TABLE `visits`
  ADD CONSTRAINT `visits_ibfk_2` FOREIGN KEY (`fk_patient`) REFERENCES `patients` (`id`);
  
ALTER TABLE `visits`
  ADD CONSTRAINT `visits_ibfk_3` FOREIGN KEY (`fk_room`) REFERENCES `rooms` (`name`);
  
ALTER TABLE `visits`
  ADD CONSTRAINT `visits_ibfk_4` FOREIGN KEY (`fk_timetable`) REFERENCES `timetables` (`id`);
  
ALTER TABLE `visits`
  ADD CONSTRAINT `visits_ibfk_5` FOREIGN KEY (`fk_procedure`) REFERENCES `procedures` (`id`);
  
--
-- Constraints for table `employees`
--

ALTER TABLE `employees`
  ADD CONSTRAINT `employees_ibfk_1` FOREIGN KEY (`fk_user`) REFERENCES `users` (`id`);
  
ALTER TABLE `employees`
  ADD CONSTRAINT `employees_ibfk_2` FOREIGN KEY (`fk_room`) REFERENCES `rooms` (`name`);
  
--
-- Constraints for table `patients`
--

ALTER TABLE `patients`
  ADD CONSTRAINT `patients_ibfk_1` FOREIGN KEY (`fk_user`) REFERENCES `users` (`id`);
  
--
-- Constraints for table `histories`
--
ALTER TABLE `histories`
  ADD CONSTRAINT `histories_ibfk_1` FOREIGN KEY (`fk_patient`) REFERENCES `patients` (`id`);

COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
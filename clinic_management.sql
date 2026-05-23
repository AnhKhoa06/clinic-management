CREATE DATABASE  IF NOT EXISTS `clinic_management` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `clinic_management`;
-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: localhost    Database: clinic_management
-- ------------------------------------------------------
-- Server version	8.0.42

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20260507110254_InitialCreate','9.0.0'),('20260507120230_AddRefreshToken','9.0.0'),('20260507141443_AddCreatedAtToSpecialty','9.0.0'),('20260511033141_FixSlotAppointmentRelation','9.0.0'),('20260511155936_AutoCalculateMaxSlots','9.0.0'),('20260512040848_AddPriceAndFeeBreakdown','9.0.0'),('20260512050533_RestructurePrescriptionDosage','9.0.0'),('20260516072431_AllowNullWorkingScheduleId','9.0.0');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `appointments`
--

DROP TABLE IF EXISTS `appointments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appointments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `PatientId` int NOT NULL,
  `DoctorId` int NOT NULL,
  `SlotId` int NOT NULL,
  `Reason` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CancelReason` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Notes` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Appointments_DoctorId` (`DoctorId`),
  KEY `IX_Appointments_PatientId` (`PatientId`),
  KEY `IX_Appointments_SlotId` (`SlotId`),
  CONSTRAINT `FK_Appointments_AppointmentSlots_SlotId` FOREIGN KEY (`SlotId`) REFERENCES `appointmentslots` (`Id`),
  CONSTRAINT `FK_Appointments_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `doctors` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Appointments_Patients_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `patients` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=73 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appointments`
--

LOCK TABLES `appointments` WRITE;
/*!40000 ALTER TABLE `appointments` DISABLE KEYS */;
INSERT INTO `appointments` VALUES (10,2,2,145,'mm','Completed',NULL,NULL,'2026-05-11 03:12:50.967573'),(13,2,2,144,'dd','Completed',NULL,NULL,'2026-05-11 03:20:45.253935'),(15,2,2,58,'dd','Completed',NULL,NULL,'2026-05-11 03:27:42.151988'),(16,4,2,171,'mệt mỏi','Completed',NULL,NULL,'2026-05-11 10:36:21.732550'),(51,5,3,596,'ok','Completed',NULL,NULL,'2026-05-16 06:54:31.578280'),(52,5,3,604,'ok','Completed',NULL,NULL,'2026-05-16 06:54:43.599987'),(53,7,4,670,'ok','Completed',NULL,NULL,'2026-05-16 06:55:06.103660'),(54,7,4,671,'ok','Completed',NULL,NULL,'2026-05-16 06:55:15.719332'),(55,6,4,664,'ok','Completed',NULL,NULL,'2026-05-16 06:55:37.104803'),(56,6,3,603,'ok','Completed',NULL,NULL,'2026-05-16 06:55:43.497668'),(57,8,2,815,'ok','Completed',NULL,NULL,'2026-05-16 07:09:39.862262'),(58,5,2,882,'ok','Completed',NULL,NULL,'2026-05-16 07:39:51.633721'),(59,5,2,883,'ok','Completed',NULL,NULL,'2026-05-16 07:41:02.759572'),(60,5,3,595,'ok','Completed',NULL,NULL,'2026-05-16 07:42:23.359621'),(61,5,4,692,'ok','Completed',NULL,NULL,'2026-05-17 12:19:49.853302'),(63,2,2,172,'ok','Completed',NULL,NULL,'2026-05-18 15:24:21.247555'),(64,5,2,170,'ok','Completed',NULL,NULL,'2026-05-18 15:25:07.384625'),(65,6,4,776,'Mệt mỏi trong người','Completed',NULL,NULL,'2026-05-18 15:34:35.697736'),(66,5,2,511,'ok','Completed',NULL,NULL,'2026-05-19 03:52:41.697086'),(67,10,2,505,'Mệt mỏi','Completed',NULL,NULL,'2026-05-19 06:14:58.273027'),(68,5,2,984,'ok','Completed',NULL,NULL,'2026-05-22 16:08:23.884706'),(69,5,2,983,'ok','Completed',NULL,NULL,'2026-05-22 16:25:37.468172'),(70,2,2,982,'z','Completed',NULL,NULL,'2026-05-22 16:31:00.967809'),(71,5,4,1056,'ok','Completed',NULL,NULL,'2026-05-23 03:12:39.580563'),(72,5,4,1044,'ok','Completed',NULL,NULL,'2026-05-23 03:57:11.411165');
/*!40000 ALTER TABLE `appointments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `appointmentslots`
--

DROP TABLE IF EXISTS `appointmentslots`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appointmentslots` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `WorkingScheduleId` int DEFAULT NULL,
  `DoctorId` int NOT NULL,
  `SlotDate` date NOT NULL,
  `SlotTime` time(6) NOT NULL,
  `Status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_AppointmentSlots_DoctorId_SlotDate_SlotTime` (`DoctorId`,`SlotDate`,`SlotTime`),
  KEY `IX_AppointmentSlots_WorkingScheduleId` (`WorkingScheduleId`),
  CONSTRAINT `FK_AppointmentSlots_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `doctors` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AppointmentSlots_WorkingSchedules_WorkingScheduleId` FOREIGN KEY (`WorkingScheduleId`) REFERENCES `workingschedules` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=1139 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appointmentslots`
--

LOCK TABLES `appointmentslots` WRITE;
/*!40000 ALTER TABLE `appointmentslots` DISABLE KEYS */;
INSERT INTO `appointmentslots` VALUES (57,2,2,'2026-05-11','16:47:00.000000','Booked'),(58,2,2,'2026-05-11','17:17:00.000000','Booked'),(143,2,2,'2026-05-11','15:47:00.000000','Booked'),(144,2,2,'2026-05-11','16:17:00.000000','Booked'),(145,2,2,'2026-05-11','17:47:00.000000','Booked'),(170,2,2,'2026-05-18','15:47:00.000000','Booked'),(171,2,2,'2026-05-18','16:17:00.000000','Booked'),(172,2,2,'2026-05-18','16:47:00.000000','Booked'),(173,2,2,'2026-05-18','17:17:00.000000','Available'),(174,2,2,'2026-05-18','17:47:00.000000','Available'),(243,8,3,'2026-05-18','07:00:00.000000','Available'),(244,8,3,'2026-05-18','07:20:00.000000','Available'),(245,8,3,'2026-05-18','07:40:00.000000','Available'),(246,8,3,'2026-05-18','08:00:00.000000','Available'),(247,8,3,'2026-05-18','08:20:00.000000','Available'),(248,8,3,'2026-05-18','08:40:00.000000','Available'),(249,8,3,'2026-05-18','09:00:00.000000','Available'),(250,8,3,'2026-05-18','09:20:00.000000','Available'),(251,8,3,'2026-05-18','09:40:00.000000','Available'),(252,8,3,'2026-05-18','10:00:00.000000','Available'),(253,8,3,'2026-05-18','10:20:00.000000','Available'),(254,8,3,'2026-05-18','10:40:00.000000','Available'),(255,8,3,'2026-05-18','11:00:00.000000','Available'),(256,8,3,'2026-05-18','11:20:00.000000','Available'),(257,8,3,'2026-05-18','11:40:00.000000','Available'),(258,8,3,'2026-05-18','12:00:00.000000','Available'),(259,8,3,'2026-05-18','12:20:00.000000','Available'),(260,8,3,'2026-05-18','12:40:00.000000','Available'),(261,8,3,'2026-05-18','13:00:00.000000','Available'),(262,8,3,'2026-05-18','13:20:00.000000','Available'),(263,8,3,'2026-05-18','13:40:00.000000','Available'),(264,8,3,'2026-05-18','14:00:00.000000','Available'),(265,8,3,'2026-05-18','14:20:00.000000','Available'),(266,8,3,'2026-05-18','14:40:00.000000','Available'),(267,8,3,'2026-05-18','15:00:00.000000','Available'),(268,8,3,'2026-05-18','15:20:00.000000','Available'),(269,8,3,'2026-05-18','15:40:00.000000','Available'),(270,8,3,'2026-05-18','16:00:00.000000','Available'),(271,8,3,'2026-05-18','16:20:00.000000','Available'),(272,8,3,'2026-05-18','16:40:00.000000','Available'),(273,8,3,'2026-05-25','07:00:00.000000','Available'),(274,8,3,'2026-05-25','07:20:00.000000','Available'),(275,8,3,'2026-05-25','07:40:00.000000','Available'),(276,8,3,'2026-05-25','08:00:00.000000','Available'),(277,8,3,'2026-05-25','08:20:00.000000','Available'),(278,8,3,'2026-05-25','08:40:00.000000','Available'),(279,8,3,'2026-05-25','09:00:00.000000','Available'),(280,8,3,'2026-05-25','09:20:00.000000','Available'),(281,8,3,'2026-05-25','09:40:00.000000','Available'),(282,8,3,'2026-05-25','10:00:00.000000','Available'),(283,8,3,'2026-05-25','10:20:00.000000','Available'),(284,8,3,'2026-05-25','10:40:00.000000','Available'),(285,8,3,'2026-05-25','11:00:00.000000','Available'),(286,8,3,'2026-05-25','11:20:00.000000','Available'),(287,8,3,'2026-05-25','11:40:00.000000','Available'),(288,8,3,'2026-05-25','12:00:00.000000','Available'),(289,8,3,'2026-05-25','12:20:00.000000','Available'),(290,8,3,'2026-05-25','12:40:00.000000','Available'),(291,8,3,'2026-05-25','13:00:00.000000','Available'),(292,8,3,'2026-05-25','13:20:00.000000','Available'),(293,8,3,'2026-05-25','13:40:00.000000','Available'),(294,8,3,'2026-05-25','14:00:00.000000','Available'),(295,8,3,'2026-05-25','14:20:00.000000','Available'),(296,8,3,'2026-05-25','14:40:00.000000','Available'),(297,8,3,'2026-05-25','15:00:00.000000','Available'),(298,8,3,'2026-05-25','15:20:00.000000','Available'),(299,8,3,'2026-05-25','15:40:00.000000','Available'),(300,8,3,'2026-05-25','16:00:00.000000','Available'),(301,8,3,'2026-05-25','16:20:00.000000','Available'),(302,8,3,'2026-05-25','16:40:00.000000','Available'),(343,2,2,'2026-05-25','15:47:00.000000','Available'),(344,2,2,'2026-05-25','16:17:00.000000','Available'),(345,2,2,'2026-05-25','16:47:00.000000','Available'),(346,2,2,'2026-05-25','17:17:00.000000','Available'),(347,2,2,'2026-05-25','17:47:00.000000','Available'),(492,13,2,'2026-05-19','08:00:00.000000','Available'),(493,13,2,'2026-05-19','08:30:00.000000','Available'),(494,13,2,'2026-05-19','09:00:00.000000','Available'),(495,13,2,'2026-05-19','09:30:00.000000','Available'),(496,13,2,'2026-05-19','10:00:00.000000','Available'),(497,13,2,'2026-05-19','10:30:00.000000','Available'),(498,13,2,'2026-05-19','11:00:00.000000','Available'),(499,13,2,'2026-05-19','11:30:00.000000','Available'),(500,13,2,'2026-05-19','12:00:00.000000','Available'),(501,13,2,'2026-05-19','12:30:00.000000','Available'),(502,13,2,'2026-05-19','13:00:00.000000','Available'),(503,13,2,'2026-05-19','13:30:00.000000','Available'),(504,13,2,'2026-05-19','14:00:00.000000','Available'),(505,13,2,'2026-05-19','14:30:00.000000','Booked'),(506,13,2,'2026-05-19','15:00:00.000000','Available'),(507,13,2,'2026-05-19','15:30:00.000000','Available'),(508,13,2,'2026-05-19','16:00:00.000000','Available'),(509,13,2,'2026-05-19','16:30:00.000000','Available'),(510,13,2,'2026-05-19','17:00:00.000000','Available'),(511,13,2,'2026-05-19','17:30:00.000000','Booked'),(512,13,2,'2026-05-26','08:00:00.000000','Available'),(513,13,2,'2026-05-26','08:30:00.000000','Available'),(514,13,2,'2026-05-26','09:00:00.000000','Available'),(515,13,2,'2026-05-26','09:30:00.000000','Available'),(516,13,2,'2026-05-26','10:00:00.000000','Available'),(517,13,2,'2026-05-26','10:30:00.000000','Available'),(518,13,2,'2026-05-26','11:00:00.000000','Available'),(519,13,2,'2026-05-26','11:30:00.000000','Available'),(520,13,2,'2026-05-26','12:00:00.000000','Available'),(521,13,2,'2026-05-26','12:30:00.000000','Available'),(522,13,2,'2026-05-26','13:00:00.000000','Available'),(523,13,2,'2026-05-26','13:30:00.000000','Available'),(524,13,2,'2026-05-26','14:00:00.000000','Available'),(525,13,2,'2026-05-26','14:30:00.000000','Available'),(526,13,2,'2026-05-26','15:00:00.000000','Available'),(527,13,2,'2026-05-26','15:30:00.000000','Available'),(528,13,2,'2026-05-26','16:00:00.000000','Available'),(529,13,2,'2026-05-26','16:30:00.000000','Available'),(530,13,2,'2026-05-26','17:00:00.000000','Available'),(531,13,2,'2026-05-26','17:30:00.000000','Available'),(595,NULL,3,'2026-05-16','16:30:00.000000','Booked'),(596,NULL,3,'2026-05-16','17:00:00.000000','Booked'),(603,NULL,3,'2026-05-16','20:30:00.000000','Booked'),(604,NULL,3,'2026-05-16','21:00:00.000000','Booked'),(664,NULL,4,'2026-05-16','12:30:00.000000','Booked'),(670,NULL,4,'2026-05-16','15:30:00.000000','Booked'),(671,NULL,4,'2026-05-16','16:00:00.000000','Booked'),(672,19,4,'2026-05-17','10:30:00.000000','Available'),(673,19,4,'2026-05-17','10:50:00.000000','Available'),(674,19,4,'2026-05-17','11:10:00.000000','Available'),(675,19,4,'2026-05-17','11:30:00.000000','Available'),(676,19,4,'2026-05-17','11:50:00.000000','Available'),(677,19,4,'2026-05-17','12:10:00.000000','Available'),(678,19,4,'2026-05-17','12:30:00.000000','Available'),(679,19,4,'2026-05-17','12:50:00.000000','Available'),(680,19,4,'2026-05-17','13:10:00.000000','Available'),(681,19,4,'2026-05-17','13:30:00.000000','Available'),(682,19,4,'2026-05-17','13:50:00.000000','Available'),(683,19,4,'2026-05-17','14:10:00.000000','Available'),(684,19,4,'2026-05-17','14:30:00.000000','Available'),(685,19,4,'2026-05-17','14:50:00.000000','Available'),(686,19,4,'2026-05-17','15:10:00.000000','Available'),(687,19,4,'2026-05-17','15:30:00.000000','Available'),(688,19,4,'2026-05-17','15:50:00.000000','Available'),(689,19,4,'2026-05-17','16:10:00.000000','Available'),(690,19,4,'2026-05-17','16:30:00.000000','Available'),(691,19,4,'2026-05-17','16:50:00.000000','Available'),(692,19,4,'2026-05-17','17:10:00.000000','Booked'),(693,19,4,'2026-05-17','17:30:00.000000','Available'),(694,19,4,'2026-05-17','17:50:00.000000','Available'),(695,19,4,'2026-05-17','18:10:00.000000','Available'),(696,19,4,'2026-05-17','18:30:00.000000','Available'),(697,19,4,'2026-05-17','18:50:00.000000','Available'),(719,19,4,'2026-05-24','10:30:00.000000','Available'),(720,19,4,'2026-05-24','10:50:00.000000','Available'),(721,19,4,'2026-05-24','11:10:00.000000','Available'),(722,19,4,'2026-05-24','11:30:00.000000','Available'),(723,19,4,'2026-05-24','11:50:00.000000','Available'),(724,19,4,'2026-05-24','12:10:00.000000','Available'),(725,19,4,'2026-05-24','12:30:00.000000','Available'),(726,19,4,'2026-05-24','12:50:00.000000','Available'),(727,19,4,'2026-05-24','13:10:00.000000','Available'),(728,19,4,'2026-05-24','13:30:00.000000','Available'),(729,19,4,'2026-05-24','13:50:00.000000','Available'),(730,19,4,'2026-05-24','14:10:00.000000','Available'),(731,19,4,'2026-05-24','14:30:00.000000','Available'),(732,19,4,'2026-05-24','14:50:00.000000','Available'),(733,19,4,'2026-05-24','15:10:00.000000','Available'),(734,19,4,'2026-05-24','15:30:00.000000','Available'),(735,19,4,'2026-05-24','15:50:00.000000','Available'),(736,19,4,'2026-05-24','16:10:00.000000','Available'),(737,19,4,'2026-05-24','16:30:00.000000','Available'),(738,19,4,'2026-05-24','16:50:00.000000','Available'),(739,19,4,'2026-05-24','17:10:00.000000','Available'),(740,19,4,'2026-05-24','17:30:00.000000','Available'),(741,19,4,'2026-05-24','17:50:00.000000','Available'),(742,19,4,'2026-05-24','18:10:00.000000','Available'),(743,19,4,'2026-05-24','18:30:00.000000','Available'),(744,19,4,'2026-05-24','18:50:00.000000','Available'),(766,20,4,'2026-05-18','09:30:00.000000','Available'),(767,20,4,'2026-05-18','10:00:00.000000','Available'),(768,20,4,'2026-05-18','10:30:00.000000','Available'),(769,20,4,'2026-05-18','11:00:00.000000','Available'),(770,20,4,'2026-05-18','11:30:00.000000','Available'),(771,20,4,'2026-05-18','12:00:00.000000','Available'),(772,20,4,'2026-05-18','12:30:00.000000','Available'),(773,20,4,'2026-05-18','13:00:00.000000','Available'),(774,20,4,'2026-05-18','13:30:00.000000','Available'),(775,20,4,'2026-05-18','14:00:00.000000','Available'),(776,20,4,'2026-05-18','14:30:00.000000','Booked'),(777,20,4,'2026-05-25','09:30:00.000000','Available'),(778,20,4,'2026-05-25','10:00:00.000000','Available'),(779,20,4,'2026-05-25','10:30:00.000000','Available'),(780,20,4,'2026-05-25','11:00:00.000000','Available'),(781,20,4,'2026-05-25','11:30:00.000000','Available'),(782,20,4,'2026-05-25','12:00:00.000000','Available'),(783,20,4,'2026-05-25','12:30:00.000000','Available'),(784,20,4,'2026-05-25','13:00:00.000000','Available'),(785,20,4,'2026-05-25','13:30:00.000000','Available'),(786,20,4,'2026-05-25','14:00:00.000000','Available'),(787,20,4,'2026-05-25','14:30:00.000000','Available'),(815,NULL,2,'2026-05-16','21:30:00.000000','Booked'),(882,NULL,2,'2026-05-16','18:15:00.000000','Booked'),(883,NULL,2,'2026-05-16','19:00:00.000000','Booked'),(917,23,3,'2026-05-16','09:00:00.000000','Available'),(918,23,3,'2026-05-16','09:30:00.000000','Available'),(919,23,3,'2026-05-16','10:00:00.000000','Available'),(920,23,3,'2026-05-16','10:30:00.000000','Available'),(921,23,3,'2026-05-16','11:00:00.000000','Available'),(922,23,3,'2026-05-16','11:30:00.000000','Available'),(923,23,3,'2026-05-16','12:00:00.000000','Available'),(924,23,3,'2026-05-16','12:30:00.000000','Available'),(925,23,3,'2026-05-16','13:00:00.000000','Available'),(926,23,3,'2026-05-16','13:30:00.000000','Available'),(927,23,3,'2026-05-16','14:00:00.000000','Available'),(928,23,3,'2026-05-16','14:30:00.000000','Available'),(929,23,3,'2026-05-16','15:00:00.000000','Available'),(930,23,3,'2026-05-16','15:30:00.000000','Available'),(931,23,3,'2026-05-16','16:00:00.000000','Available'),(932,23,3,'2026-05-23','09:00:00.000000','Available'),(933,23,3,'2026-05-23','09:30:00.000000','Available'),(934,23,3,'2026-05-23','10:00:00.000000','Available'),(935,23,3,'2026-05-23','10:30:00.000000','Available'),(936,23,3,'2026-05-23','11:00:00.000000','Available'),(937,23,3,'2026-05-23','11:30:00.000000','Available'),(938,23,3,'2026-05-23','12:00:00.000000','Available'),(939,23,3,'2026-05-23','12:30:00.000000','Available'),(940,23,3,'2026-05-23','13:00:00.000000','Available'),(941,23,3,'2026-05-23','13:30:00.000000','Available'),(942,23,3,'2026-05-23','14:00:00.000000','Available'),(943,23,3,'2026-05-23','14:30:00.000000','Available'),(944,23,3,'2026-05-23','15:00:00.000000','Available'),(945,23,3,'2026-05-23','15:30:00.000000','Available'),(946,23,3,'2026-05-23','16:00:00.000000','Available'),(947,23,3,'2026-05-23','16:30:00.000000','Available'),(948,23,3,'2026-05-30','09:00:00.000000','Available'),(949,23,3,'2026-05-30','09:30:00.000000','Available'),(950,23,3,'2026-05-30','10:00:00.000000','Available'),(951,23,3,'2026-05-30','10:30:00.000000','Available'),(952,23,3,'2026-05-30','11:00:00.000000','Available'),(953,23,3,'2026-05-30','11:30:00.000000','Available'),(954,23,3,'2026-05-30','12:00:00.000000','Available'),(955,23,3,'2026-05-30','12:30:00.000000','Available'),(956,23,3,'2026-05-30','13:00:00.000000','Available'),(957,23,3,'2026-05-30','13:30:00.000000','Available'),(958,23,3,'2026-05-30','14:00:00.000000','Available'),(959,23,3,'2026-05-30','14:30:00.000000','Available'),(960,23,3,'2026-05-30','15:00:00.000000','Available'),(961,23,3,'2026-05-30','15:30:00.000000','Available'),(962,23,3,'2026-05-30','16:00:00.000000','Available'),(963,23,3,'2026-05-30','16:30:00.000000','Available'),(964,2,2,'0001-01-01','15:47:00.000000','Available'),(965,2,2,'0001-01-01','16:17:00.000000','Available'),(966,2,2,'0001-01-01','16:47:00.000000','Available'),(967,2,2,'0001-01-01','17:17:00.000000','Available'),(968,2,2,'0001-01-01','17:47:00.000000','Available'),(969,24,2,'2026-05-22','09:00:00.000000','Available'),(970,24,2,'2026-05-22','09:30:00.000000','Available'),(971,24,2,'2026-05-22','10:00:00.000000','Available'),(972,24,2,'2026-05-22','10:30:00.000000','Available'),(973,24,2,'2026-05-22','11:00:00.000000','Available'),(974,24,2,'2026-05-22','11:30:00.000000','Available'),(975,24,2,'2026-05-22','12:00:00.000000','Available'),(976,24,2,'2026-05-22','12:30:00.000000','Available'),(977,24,2,'2026-05-22','13:00:00.000000','Available'),(978,24,2,'2026-05-22','13:30:00.000000','Available'),(979,24,2,'2026-05-22','14:00:00.000000','Available'),(980,24,2,'2026-05-22','14:30:00.000000','Available'),(981,24,2,'2026-05-22','15:00:00.000000','Available'),(982,24,2,'2026-05-22','15:30:00.000000','Booked'),(983,24,2,'2026-05-22','16:00:00.000000','Booked'),(984,24,2,'2026-05-22','16:30:00.000000','Booked'),(985,24,2,'2026-05-29','09:00:00.000000','Available'),(986,24,2,'2026-05-29','09:30:00.000000','Available'),(987,24,2,'2026-05-29','10:00:00.000000','Available'),(988,24,2,'2026-05-29','10:30:00.000000','Available'),(989,24,2,'2026-05-29','11:00:00.000000','Available'),(990,24,2,'2026-05-29','11:30:00.000000','Available'),(991,24,2,'2026-05-29','12:00:00.000000','Available'),(992,24,2,'2026-05-29','12:30:00.000000','Available'),(993,24,2,'2026-05-29','13:00:00.000000','Available'),(994,24,2,'2026-05-29','13:30:00.000000','Available'),(995,24,2,'2026-05-29','14:00:00.000000','Available'),(996,24,2,'2026-05-29','14:30:00.000000','Available'),(997,24,2,'2026-05-29','15:00:00.000000','Available'),(998,24,2,'2026-05-29','15:30:00.000000','Available'),(999,24,2,'2026-05-29','16:00:00.000000','Available'),(1000,24,2,'2026-05-29','16:30:00.000000','Available'),(1001,2,2,'2026-06-01','15:47:00.000000','Available'),(1002,2,2,'2026-06-01','16:17:00.000000','Available'),(1003,2,2,'2026-06-01','16:47:00.000000','Available'),(1004,2,2,'2026-06-01','17:17:00.000000','Available'),(1005,2,2,'2026-06-01','17:47:00.000000','Available'),(1006,13,2,'2026-06-02','08:00:00.000000','Available'),(1007,13,2,'2026-06-02','08:30:00.000000','Available'),(1008,13,2,'2026-06-02','09:00:00.000000','Available'),(1009,13,2,'2026-06-02','09:30:00.000000','Available'),(1010,13,2,'2026-06-02','10:00:00.000000','Available'),(1011,13,2,'2026-06-02','10:30:00.000000','Available'),(1012,13,2,'2026-06-02','11:00:00.000000','Available'),(1013,13,2,'2026-06-02','11:30:00.000000','Available'),(1014,13,2,'2026-06-02','12:00:00.000000','Available'),(1015,13,2,'2026-06-02','12:30:00.000000','Available'),(1016,13,2,'2026-06-02','13:00:00.000000','Available'),(1017,13,2,'2026-06-02','13:30:00.000000','Available'),(1018,13,2,'2026-06-02','14:00:00.000000','Available'),(1019,13,2,'2026-06-02','14:30:00.000000','Available'),(1020,13,2,'2026-06-02','15:00:00.000000','Available'),(1021,13,2,'2026-06-02','15:30:00.000000','Available'),(1022,13,2,'2026-06-02','16:00:00.000000','Available'),(1023,13,2,'2026-06-02','16:30:00.000000','Available'),(1024,13,2,'2026-06-02','17:00:00.000000','Available'),(1025,13,2,'2026-06-02','17:30:00.000000','Available'),(1026,24,2,'2026-06-05','09:00:00.000000','Available'),(1027,24,2,'2026-06-05','09:30:00.000000','Available'),(1028,24,2,'2026-06-05','10:00:00.000000','Available'),(1029,24,2,'2026-06-05','10:30:00.000000','Available'),(1030,24,2,'2026-06-05','11:00:00.000000','Available'),(1031,24,2,'2026-06-05','11:30:00.000000','Available'),(1032,24,2,'2026-06-05','12:00:00.000000','Available'),(1033,24,2,'2026-06-05','12:30:00.000000','Available'),(1034,24,2,'2026-06-05','13:00:00.000000','Available'),(1035,24,2,'2026-06-05','13:30:00.000000','Available'),(1036,24,2,'2026-06-05','14:00:00.000000','Available'),(1037,24,2,'2026-06-05','14:30:00.000000','Available'),(1038,24,2,'2026-06-05','15:00:00.000000','Available'),(1039,24,2,'2026-06-05','15:30:00.000000','Available'),(1040,24,2,'2026-06-05','16:00:00.000000','Available'),(1041,24,2,'2026-06-05','16:30:00.000000','Available'),(1042,25,4,'2026-05-23','10:00:00.000000','Available'),(1043,25,4,'2026-05-23','10:30:00.000000','Available'),(1044,25,4,'2026-05-23','11:00:00.000000','Booked'),(1045,25,4,'2026-05-23','11:30:00.000000','Available'),(1046,25,4,'2026-05-23','12:00:00.000000','Available'),(1047,25,4,'2026-05-23','12:30:00.000000','Available'),(1048,25,4,'2026-05-23','13:00:00.000000','Available'),(1049,25,4,'2026-05-23','13:30:00.000000','Available'),(1050,25,4,'2026-05-23','14:00:00.000000','Available'),(1051,25,4,'2026-05-23','14:30:00.000000','Available'),(1052,25,4,'2026-05-23','15:00:00.000000','Available'),(1053,25,4,'2026-05-23','15:30:00.000000','Available'),(1054,25,4,'2026-05-23','16:00:00.000000','Available'),(1055,25,4,'2026-05-23','16:30:00.000000','Available'),(1056,25,4,'2026-05-23','17:00:00.000000','Booked'),(1057,25,4,'2026-05-23','17:30:00.000000','Available'),(1058,25,4,'2026-05-23','18:00:00.000000','Available'),(1059,25,4,'2026-05-23','18:30:00.000000','Available'),(1060,25,4,'2026-05-23','19:00:00.000000','Available'),(1061,25,4,'2026-05-23','19:30:00.000000','Available'),(1062,25,4,'2026-05-30','10:00:00.000000','Available'),(1063,25,4,'2026-05-30','10:30:00.000000','Available'),(1064,25,4,'2026-05-30','11:00:00.000000','Available'),(1065,25,4,'2026-05-30','11:30:00.000000','Available'),(1066,25,4,'2026-05-30','12:00:00.000000','Available'),(1067,25,4,'2026-05-30','12:30:00.000000','Available'),(1068,25,4,'2026-05-30','13:00:00.000000','Available'),(1069,25,4,'2026-05-30','13:30:00.000000','Available'),(1070,25,4,'2026-05-30','14:00:00.000000','Available'),(1071,25,4,'2026-05-30','14:30:00.000000','Available'),(1072,25,4,'2026-05-30','15:00:00.000000','Available'),(1073,25,4,'2026-05-30','15:30:00.000000','Available'),(1074,25,4,'2026-05-30','16:00:00.000000','Available'),(1075,25,4,'2026-05-30','16:30:00.000000','Available'),(1076,25,4,'2026-05-30','17:00:00.000000','Available'),(1077,25,4,'2026-05-30','17:30:00.000000','Available'),(1078,25,4,'2026-05-30','18:00:00.000000','Available'),(1079,25,4,'2026-05-30','18:30:00.000000','Available'),(1080,25,4,'2026-05-30','19:00:00.000000','Available'),(1081,25,4,'2026-05-30','19:30:00.000000','Available'),(1082,19,4,'2026-05-31','10:30:00.000000','Available'),(1083,19,4,'2026-05-31','10:50:00.000000','Available'),(1084,19,4,'2026-05-31','11:10:00.000000','Available'),(1085,19,4,'2026-05-31','11:30:00.000000','Available'),(1086,19,4,'2026-05-31','11:50:00.000000','Available'),(1087,19,4,'2026-05-31','12:10:00.000000','Available'),(1088,19,4,'2026-05-31','12:30:00.000000','Available'),(1089,19,4,'2026-05-31','12:50:00.000000','Available'),(1090,19,4,'2026-05-31','13:10:00.000000','Available'),(1091,19,4,'2026-05-31','13:30:00.000000','Available'),(1092,19,4,'2026-05-31','13:50:00.000000','Available'),(1093,19,4,'2026-05-31','14:10:00.000000','Available'),(1094,19,4,'2026-05-31','14:30:00.000000','Available'),(1095,19,4,'2026-05-31','14:50:00.000000','Available'),(1096,19,4,'2026-05-31','15:10:00.000000','Available'),(1097,19,4,'2026-05-31','15:30:00.000000','Available'),(1098,19,4,'2026-05-31','15:50:00.000000','Available'),(1099,19,4,'2026-05-31','16:10:00.000000','Available'),(1100,19,4,'2026-05-31','16:30:00.000000','Available'),(1101,19,4,'2026-05-31','16:50:00.000000','Available'),(1102,19,4,'2026-05-31','17:10:00.000000','Available'),(1103,19,4,'2026-05-31','17:30:00.000000','Available'),(1104,19,4,'2026-05-31','17:50:00.000000','Available'),(1105,19,4,'2026-05-31','18:10:00.000000','Available'),(1106,19,4,'2026-05-31','18:30:00.000000','Available'),(1107,19,4,'2026-05-31','18:50:00.000000','Available'),(1108,20,4,'2026-06-01','09:30:00.000000','Available'),(1109,20,4,'2026-06-01','10:00:00.000000','Available'),(1110,20,4,'2026-06-01','10:30:00.000000','Available'),(1111,20,4,'2026-06-01','11:00:00.000000','Available'),(1112,20,4,'2026-06-01','11:30:00.000000','Available'),(1113,20,4,'2026-06-01','12:00:00.000000','Available'),(1114,20,4,'2026-06-01','12:30:00.000000','Available'),(1115,20,4,'2026-06-01','13:00:00.000000','Available'),(1116,20,4,'2026-06-01','13:30:00.000000','Available'),(1117,20,4,'2026-06-01','14:00:00.000000','Available'),(1118,20,4,'2026-06-01','14:30:00.000000','Available'),(1119,25,4,'2026-06-06','10:00:00.000000','Available'),(1120,25,4,'2026-06-06','10:30:00.000000','Available'),(1121,25,4,'2026-06-06','11:00:00.000000','Available'),(1122,25,4,'2026-06-06','11:30:00.000000','Available'),(1123,25,4,'2026-06-06','12:00:00.000000','Available'),(1124,25,4,'2026-06-06','12:30:00.000000','Available'),(1125,25,4,'2026-06-06','13:00:00.000000','Available'),(1126,25,4,'2026-06-06','13:30:00.000000','Available'),(1127,25,4,'2026-06-06','14:00:00.000000','Available'),(1128,25,4,'2026-06-06','14:30:00.000000','Available'),(1129,25,4,'2026-06-06','15:00:00.000000','Available'),(1130,25,4,'2026-06-06','15:30:00.000000','Available'),(1131,25,4,'2026-06-06','16:00:00.000000','Available'),(1132,25,4,'2026-06-06','16:30:00.000000','Available'),(1133,25,4,'2026-06-06','17:00:00.000000','Available'),(1134,25,4,'2026-06-06','17:30:00.000000','Available'),(1135,25,4,'2026-06-06','18:00:00.000000','Available'),(1136,25,4,'2026-06-06','18:30:00.000000','Available'),(1137,25,4,'2026-06-06','19:00:00.000000','Available'),(1138,25,4,'2026-06-06','19:30:00.000000','Available');
/*!40000 ALTER TABLE `appointmentslots` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `doctors`
--

DROP TABLE IF EXISTS `doctors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `doctors` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `SpecialtyId` int NOT NULL,
  `LicenseNumber` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Bio` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AvatarUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsActive` tinyint(1) NOT NULL,
  `ExaminationFee` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Doctors_LicenseNumber` (`LicenseNumber`),
  UNIQUE KEY `IX_Doctors_UserId` (`UserId`),
  KEY `IX_Doctors_SpecialtyId` (`SpecialtyId`),
  CONSTRAINT `FK_Doctors_Specialties_SpecialtyId` FOREIGN KEY (`SpecialtyId`) REFERENCES `specialties` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Doctors_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `doctors`
--

LOCK TABLES `doctors` WRITE;
/*!40000 ALTER TABLE `doctors` DISABLE KEYS */;
INSERT INTO `doctors` VALUES (2,10,5,'GP-12345','Khám tai mũi họng','/uploads/avatars/doctor-2-20260515175050.jpg',1,0.000000000000000000000000000000),(3,15,1,'GP-54321','Chuyên về các bệnh nội khoa','/uploads/avatars/doctor-3-20260511110618.jpg',1,0.000000000000000000000000000000),(4,17,6,'GP-12346','Uy tín tạo nên thương hiệu','/uploads/avatars/doctor-4-20260515175433.png',1,0.000000000000000000000000000000);
/*!40000 ALTER TABLE `doctors` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `medicalrecords`
--

DROP TABLE IF EXISTS `medicalrecords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `medicalrecords` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AppointmentId` int NOT NULL,
  `PatientId` int NOT NULL,
  `DoctorId` int NOT NULL,
  `Symptoms` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Diagnosis` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TreatmentNotes` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `FollowUpDate` date DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_MedicalRecords_AppointmentId` (`AppointmentId`),
  KEY `IX_MedicalRecords_DoctorId` (`DoctorId`),
  KEY `IX_MedicalRecords_PatientId` (`PatientId`),
  CONSTRAINT `FK_MedicalRecords_Appointments_AppointmentId` FOREIGN KEY (`AppointmentId`) REFERENCES `appointments` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_MedicalRecords_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `doctors` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_MedicalRecords_Patients_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `patients` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `medicalrecords`
--

LOCK TABLES `medicalrecords` WRITE;
/*!40000 ALTER TABLE `medicalrecords` DISABLE KEYS */;
INSERT INTO `medicalrecords` VALUES (4,10,2,2,'Sốt','Nghiện game','Cai nghiện gấp','2026-05-16','2026-05-11 10:11:54.475599'),(30,56,6,3,'v','v','v','2026-05-19','2026-05-16 06:58:14.140754'),(31,52,5,3,'s','s','s','2026-05-18','2026-05-16 06:58:40.419810'),(33,57,8,2,'z','z','z','2026-05-18','2026-05-16 07:10:18.847397'),(34,61,5,4,'a','a','a','2026-05-19','2026-05-17 12:20:41.583094'),(37,65,6,4,'b','b','b','2026-05-20','2026-05-18 15:35:28.356108'),(38,66,5,2,'s','s','s','2026-05-21','2026-05-19 03:53:16.292762'),(39,67,10,2,'Mệt mỏi','Nghiện game','Nghỉ ngơi','2026-05-21','2026-05-19 06:15:57.836798'),(40,68,5,2,'s','s','s','2026-05-24','2026-05-22 16:08:53.633324'),(41,69,5,2,'a','a','a','2026-05-23','2026-05-22 16:26:04.046564'),(42,70,2,2,'s','s','s',NULL,'2026-05-22 16:31:22.979568'),(43,72,5,4,'z','z','z',NULL,'2026-05-23 03:57:35.370002');
/*!40000 ALTER TABLE `medicalrecords` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `medications`
--

DROP TABLE IF EXISTS `medications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `medications` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Unit` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsActive` tinyint(1) NOT NULL,
  `Price` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `medications`
--

LOCK TABLES `medications` WRITE;
/*!40000 ALTER TABLE `medications` DISABLE KEYS */;
INSERT INTO `medications` VALUES (1,'Panadol','haha','Thuốc hạ sốt',1,0.000000000000000000000000000000),(2,'Paracetamol 500mg','Viên','Thuốc hạ sốt, giảm đau thông thường',1,0.000000000000000000000000000000),(3,'Ibuprofen 400mg','Viên','Thuốc kháng viêm, giảm đau, hạ sốt',1,0.000000000000000000000000000000),(4,'Amoxicillin 500mg','Viên','Kháng sinh điều trị nhiễm khuẩn',1,15000.000000000000000000000000000000),(5,'Azithromycin 250mg','Viên','Kháng sinh phổ rộng',1,25000.000000000000000000000000000000),(6,'Omeprazole 20mg','Viên','Thuốc điều trị viêm loét dạ dày',1,0.000000000000000000000000000000),(7,'Metformin 500mg','Viên','Thuốc điều trị tiểu đường type 2',1,0.000000000000000000000000000000),(8,'Amlodipine 5mg','Viên','Thuốc hạ huyết áp',1,10000.000000000000000000000000000000),(9,'Atorvastatin 20mg','Viên','Thuốc hạ mỡ máu',1,50000.000000000000000000000000000000),(10,'Cetirizine 10mg','Viên','Thuốc kháng histamine, chống dị ứng',1,0.000000000000000000000000000000),(11,'Prednisolone 5mg','Viên','Thuốc kháng viêm corticosteroid',1,0.000000000000000000000000000000),(12,'Vitamin C 1000mg','Viên','Bổ sung vitamin C tăng đề kháng',1,0.000000000000000000000000000000),(13,'Aspirin 81mg','Viên','Thuốc chống kết tập tiểu cầu',1,6000.000000000000000000000000000000),(14,'Colchicine 0.6mg','Viên','Thuốc điều trị gout cấp',1,0.000000000000000000000000000000),(15,'Allopurinol 300mg','Viên','Thuốc điều trị gout mãn',1,20000.000000000000000000000000000000),(16,'Warfarin 5mg','Viên','Thuốc chống đông máu',1,0.000000000000000000000000000000),(17,'Oresol','Gói','Bù điện giải khi tiêu chảy, mất nước',1,0.000000000000000000000000000000),(18,'Berberin','Gói','Điều trị tiêu chảy, rối loạn tiêu hóa',1,25000.000000000000000000000000000000),(19,'Bột nghệ mật ong','Gói','Hỗ trợ điều trị viêm loét dạ dày',1,30000.000000000000000000000000000000),(20,'Smecta','Gói','Thuốc điều trị tiêu chảy cấp',1,0.000000000000000000000000000000),(21,'Vitamin tổng hợp','Gói','Bổ sung vitamin và khoáng chất',1,0.000000000000000000000000000000),(22,'Siro ho Prospan','Chai','Thuốc ho thảo dược cho trẻ em',1,0.000000000000000000000000000000),(23,'Nước muối sinh lý 0.9%','Chai','Rửa mũi, vệ sinh mắt',1,0.000000000000000000000000000000),(24,'Dung dịch Povidone Iodine','Chai','Sát khuẩn vết thương',1,0.000000000000000000000000000000),(25,'Vitamin D3 drops','Chai','Bổ sung vitamin D3 dạng nhỏ giọt',1,0.000000000000000000000000000000),(26,'Thuốc nhỏ mắt Tobramycin','Chai','Kháng sinh nhỏ mắt',1,0.000000000000000000000000000000),(27,'Salbutamol','Ống hít','Thuốc giãn phế quản điều trị hen suyễn',1,0.000000000000000000000000000000),(28,'Budesonide',NULL,'Thuốc corticoid dạng hít trị hen',1,9500.000000000000000000000000000000),(29,'Insulin Lantus','Ống','Insulin tác dụng dài điều trị tiểu đường',1,0.000000000000000000000000000000),(30,'Vitamin B12','Ống','Bổ sung vitamin B12 dạng tiêm',1,0.000000000000000000000000000000),(31,'Furosemide tiêm','Ống','Thuốc lợi tiểu dạng tiêm',1,0.000000000000000000000000000000),(32,'Multivitamin','Lọ','Bổ sung đa vitamin dạng nước',1,0.000000000000000000000000000000),(33,'Sắt hữu cơ Ferrovit','Lọ','Bổ sung sắt điều trị thiếu máu',1,0.000000000000000000000000000000),(34,'Canxi D3','Lọ','Bổ sung canxi và vitamin D3',1,45000.000000000000000000000000000000),(35,'Probiotic Lacteol','Lọ','Bổ sung lợi khuẩn đường ruột',1,0.000000000000000000000000000000),(36,'Omega 3','Lọ','Bổ sung dầu cá omega 3',1,0.000000000000000000000000000000),(37,'Kem bôi Betamethasone','Tuýp','Kem kháng viêm điều trị viêm da',1,0.000000000000000000000000000000),(38,'Kem bôi Clotrimazole','Tuýp','Kem kháng nấm da',1,0.000000000000000000000000000000),(39,'Gel bôi Diclofenac','Tuýp','Gel kháng viêm giảm đau tại chỗ',1,0.000000000000000000000000000000),(40,'Kem dưỡng ẩm Eucerin','Tuýp','Dưỡng ẩm da khô, viêm da cơ địa',1,0.000000000000000000000000000000),(41,'Kem bôi Hydrocortisone','Tuýp','Kem kháng viêm nhẹ điều trị dị ứng da',1,0.000000000000000000000000000000);
/*!40000 ALTER TABLE `medications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `patients`
--

DROP TABLE IF EXISTS `patients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `patients` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `DateOfBirth` date DEFAULT NULL,
  `Gender` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Address` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `BloodType` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmergencyContact` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Patients_UserId` (`UserId`),
  CONSTRAINT `FK_Patients_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `patients`
--

LOCK TABLES `patients` WRITE;
/*!40000 ALTER TABLE `patients` DISABLE KEYS */;
INSERT INTO `patients` VALUES (1,7,'2007-06-17','Nữ','Sài Gòn',NULL,'Anh 5 - 0987654321'),(2,8,'2003-06-16','Nam','Gia Lai',NULL,'Anh 2 - 0987654321'),(3,9,'2003-05-22','Nam','Bình Định',NULL,'Anh 2 - 0984357454'),(4,11,'2005-06-24','Nam','Quy Nhơn',NULL,'Anh 3 - 09876555443'),(5,12,'2004-03-04','Nữ','Bình Định',NULL,'Chị 2 - 0987867454'),(6,13,'2003-05-17','Nam','Quận 1',NULL,'Ba - 0988677544'),(7,14,'2004-08-12','Nam','TP. Hồ Chí Minh',NULL,'Chị 3 - 0123445556'),(8,16,'2005-06-05','Nam','Gia Lai',NULL,'Anh 2 - 0987654321'),(9,18,'2004-05-17','Nam','TP. Hồ Chí Minh',NULL,'Anh 5 - 0987654321'),(10,19,NULL,NULL,NULL,NULL,NULL),(11,20,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `patients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payments`
--

DROP TABLE IF EXISTS `payments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `payments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AppointmentId` int NOT NULL,
  `InvoiceCode` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Amount` decimal(65,30) NOT NULL,
  `Status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Method` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Notes` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PaidAt` datetime(6) DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `ExaminationFee` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `MedicationFee` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Payments_AppointmentId` (`AppointmentId`),
  UNIQUE KEY `IX_Payments_InvoiceCode` (`InvoiceCode`),
  CONSTRAINT `FK_Payments_Appointments_AppointmentId` FOREIGN KEY (`AppointmentId`) REFERENCES `appointments` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payments`
--

LOCK TABLES `payments` WRITE;
/*!40000 ALTER TABLE `payments` DISABLE KEYS */;
INSERT INTO `payments` VALUES (4,10,'INV-20260511-0010',525000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-11 10:20:53.742748','0001-01-01 00:00:00.000000',0.000000000000000000000000000000,0.000000000000000000000000000000),(5,15,'INV-20260511-0015',99000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-11 10:23:52.414926','0001-01-01 00:00:00.000000',0.000000000000000000000000000000,0.000000000000000000000000000000),(6,13,'INV-20260511-0013',250000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-11 10:37:50.274270','0001-01-01 00:00:00.000000',0.000000000000000000000000000000,0.000000000000000000000000000000),(40,51,'INV-20260516-0051',800000.000000000000000000000000000000,'Paid','Cash','ok','2026-05-16 06:59:58.689425','2026-05-16 06:59:05.010233',0.000000000000000000000000000000,800000.000000000000000000000000000000),(41,52,'INV-20260516-0052',684000.000000000000000000000000000000,'Paid','BankTransfer',NULL,'2026-05-16 07:01:24.477809','2026-05-16 06:59:08.944893',0.000000000000000000000000000000,684000.000000000000000000000000000000),(42,56,'INV-20260516-0056',420000.000000000000000000000000000000,'Paid','BankTransfer',NULL,'2026-05-16 07:03:29.318534','2026-05-16 06:59:13.332038',0.000000000000000000000000000000,420000.000000000000000000000000000000),(43,57,'INV-20260516-0057',1050000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-16 07:11:01.260597','2026-05-16 07:10:25.740078',0.000000000000000000000000000000,1050000.000000000000000000000000000000),(44,61,'INV-20260517-0061',1080000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-17 12:21:28.096163','2026-05-17 12:21:00.607229',0.000000000000000000000000000000,1080000.000000000000000000000000000000),(45,64,'INV-20260518-0064',1200000.000000000000000000000000000000,'Paid','Cash',NULL,'2026-05-18 15:28:42.203983','2026-05-18 15:25:51.616434',0.000000000000000000000000000000,1200000.000000000000000000000000000000),(46,63,'INV-20260518-0063',300000.000000000000000000000000000000,'Paid','Cash',NULL,'2026-05-18 15:28:29.621730','2026-05-18 15:26:30.433381',0.000000000000000000000000000000,300000.000000000000000000000000000000),(47,65,'INV-20260518-0065',750000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-22 16:06:23.396630','2026-05-18 15:35:38.422150',0.000000000000000000000000000000,750000.000000000000000000000000000000),(48,66,'INV-20260519-0066',700000.000000000000000000000000000000,'Paid','BankTransfer',NULL,'2026-05-22 11:20:41.292832','2026-05-19 04:00:33.985368',0.000000000000000000000000000000,700000.000000000000000000000000000000),(49,67,'INV-20260519-0067',1050000.000000000000000000000000000000,'Paid','BankTransfer','Thanh toán tiền thuốc','2026-05-19 06:16:59.114825','2026-05-19 06:16:23.910785',0.000000000000000000000000000000,1050000.000000000000000000000000000000),(50,68,'INV-20260522-0068',1050000.000000000000000000000000000000,'Paid','BankTransfer',NULL,'2026-05-22 16:12:57.569973','2026-05-22 16:08:53.741134',0.000000000000000000000000000000,1050000.000000000000000000000000000000),(51,69,'INV-20260522-0069',720000.000000000000000000000000000000,'Paid','Cash',NULL,'2026-05-22 16:29:07.324064','2026-05-22 16:26:04.155766',0.000000000000000000000000000000,720000.000000000000000000000000000000),(52,70,'INV-20260522-0070',360000.000000000000000000000000000000,'Unpaid','BankTransfer',NULL,NULL,'2026-05-22 16:31:23.069865',0.000000000000000000000000000000,360000.000000000000000000000000000000),(53,72,'INV-20260523-0072',750000.000000000000000000000000000000,'Unpaid','Cash',NULL,NULL,'2026-05-23 03:57:35.455393',0.000000000000000000000000000000,750000.000000000000000000000000000000);
/*!40000 ALTER TABLE `payments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `prescriptions`
--

DROP TABLE IF EXISTS `prescriptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `prescriptions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `MedicalRecordId` int NOT NULL,
  `MedicationId` int NOT NULL,
  `DurationDays` int NOT NULL,
  `Notes` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Quantity` int NOT NULL DEFAULT '0',
  `UnitPrice` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `DosagePerTime` decimal(65,30) NOT NULL DEFAULT '0.000000000000000000000000000000',
  `TimesPerDay` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_Prescriptions_MedicalRecordId` (`MedicalRecordId`),
  KEY `IX_Prescriptions_MedicationId` (`MedicationId`),
  CONSTRAINT `FK_Prescriptions_MedicalRecords_MedicalRecordId` FOREIGN KEY (`MedicalRecordId`) REFERENCES `medicalrecords` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Prescriptions_Medications_MedicationId` FOREIGN KEY (`MedicationId`) REFERENCES `medications` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prescriptions`
--

LOCK TABLES `prescriptions` WRITE;
/*!40000 ALTER TABLE `prescriptions` DISABLE KEYS */;
INSERT INTO `prescriptions` VALUES (5,4,15,7,'Uống sau bữa ăn',0,0.000000000000000000000000000000,0.000000000000000000000000000000,0),(32,30,4,7,'Uống sau bữa ăn',28,15000.000000000000000000000000000000,2.000000000000000000000000000000,2),(33,31,28,12,'Uống sau bữa ăn',72,9500.000000000000000000000000000000,2.000000000000000000000000000000,3),(35,33,18,7,'Uống sau bữa ăn',42,25000.000000000000000000000000000000,2.000000000000000000000000000000,3),(36,34,4,7,'Uống sau bữa ăn',42,15000.000000000000000000000000000000,2.000000000000000000000000000000,3),(37,34,9,1,'Uống sau bữa ăn',9,50000.000000000000000000000000000000,3.000000000000000000000000000000,3),(40,37,5,5,'Uống sau bữa ăn',30,25000.000000000000000000000000000000,2.000000000000000000000000000000,3),(41,38,18,7,'Uống sau bữa ăn',28,25000.000000000000000000000000000000,2.000000000000000000000000000000,2),(42,39,5,7,'Uống sau bữa ăn',42,25000.000000000000000000000000000000,2.000000000000000000000000000000,3),(43,40,5,7,'Uống sau bữa ăn',42,25000.000000000000000000000000000000,2.000000000000000000000000000000,3),(44,41,19,4,'Uống sau bữa ăn',24,30000.000000000000000000000000000000,2.000000000000000000000000000000,3),(45,42,19,2,'Uống sau bữa ăn',12,30000.000000000000000000000000000000,2.000000000000000000000000000000,3),(46,43,5,5,'Uống sau bữa ăn',30,25000.000000000000000000000000000000,2.000000000000000000000000000000,3);
/*!40000 ALTER TABLE `prescriptions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `refreshtokens`
--

DROP TABLE IF EXISTS `refreshtokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `refreshtokens` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `Token` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ExpiresAt` datetime(6) NOT NULL,
  `IsRevoked` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_RefreshTokens_UserId` (`UserId`),
  CONSTRAINT `FK_RefreshTokens_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `refreshtokens`
--

LOCK TABLES `refreshtokens` WRITE;
/*!40000 ALTER TABLE `refreshtokens` DISABLE KEYS */;
INSERT INTO `refreshtokens` VALUES (1,1,'DKukdp2RqHzajH4MveQ+263ovivmZzYdtJMuZqPnlhDsVJmJ2mEDKiPKbRRqQHwtUuJTnQbOC7Rl29HQS92U7w==','2026-05-14 12:19:30.184060',0,'2026-05-07 12:19:30.183915'),(2,1,'djcqmSgjXJQ/CR4JPZCgy8WF1p99pynImICDyrPVVOOyMc1/AAgpUCnHFLZER+X2YgzhXEJeLz/6HEI7/xVf4w==','2026-05-14 12:20:46.344890',0,'2026-05-07 12:20:46.344890'),(3,1,'aJ6yChmeEs1GdOVroLw2k2j/DgY2Mcuao2d4RXm0meeXFlALT1fT9HERS3qsml4sAltsm03QAmxgkIKKL14uzw==','2026-05-14 14:18:24.938809',0,'2026-05-07 14:18:24.938758'),(5,1,'80XKrK3ddvFm2MyIsjQDNhi4nrL58AkC4QqQpz1W8BHvnAl6xQ1QzzX+PKXf7PaTLjHsYThkdZNg4WXjOlkaKg==','2026-05-14 14:49:21.754127',0,'2026-05-07 14:49:21.754076'),(6,5,'XENXWP0+dtjjMgYW7vuMYVan7PCoQ8+MEBtmI67qonius1N9xo2xZgQRCvozvwTIHdFLPm1iFffcqIxAnWQVuQ==','2026-05-14 14:52:53.843760',0,'2026-05-07 14:52:53.843652'),(7,5,'M+9Cdr6L1+3099yy51iIfhKWOSTb+kKEQLHbrSKo4UUD6iFfVTQ56YoxJ9Jwyvzgt9gD2VXGJX+Pvx/iq5zU9w==','2026-05-14 15:00:52.603136',0,'2026-05-07 15:00:52.603135'),(8,1,'fku99v5qQiQoO5SMtH71mDUpih6hd6L5F1ns1t72L3vvJf5c/xD6ShhdvLaBFyjdXsveuNHCc708DbD5dHD4Xw==','2026-05-14 15:03:11.762229',0,'2026-05-07 15:03:11.762229'),(9,1,'L/2R0ZoZAMAHM73snLQfTvDFjvRlJwHKktQuk1eD8pyhOguXDeX+zYwa7uajkb1gWwgbxu01T2qkSwLXCqGl/g==','2026-05-14 15:04:25.781077',0,'2026-05-07 15:04:25.781076'),(10,5,'u9aMtAnKk9sMog7Gal6xESQT/GGjIKdWMpUj89hSgrXAQaXuBLLbTEJYrQs8r+78Rw/fQRA2uAcMqkbeLHcTCg==','2026-05-14 15:04:39.469131',0,'2026-05-07 15:04:39.469130'),(11,5,'3kOjJNTj5C6VmIlxJOPADekuEg0PR3sQLCGxmIKErsvCaaw0Js0ww6+ipt2CgYtYiqlAg3dsrzfiWOjJcdgUxg==','2026-05-14 15:04:42.012936',0,'2026-05-07 15:04:42.012936'),(12,5,'Cas8fekrvIIMZ9FDgoY/u5cFmi16hyMQHRD4YcBhOTdqiFcCnuD3FDynoM8nbzvGmrA5op7OHzig69QBEBoofA==','2026-05-14 15:04:45.883471',0,'2026-05-07 15:04:45.883470'),(13,5,'yNZxWDXSQxl5/enLLrBuqg9h64/hkI1Bjm0Q2sc4Lly4IKsf/mWrw4gejl6HoeuO7qSXH8mxx5x38ktMf+NyoA==','2026-05-14 15:04:48.166674',0,'2026-05-07 15:04:48.166674'),(14,5,'hcutu2TbSHSQzW6ln6xiBfnA5ft69mvplcDdOR+OSYNj+P43OiW5QORm7me/lXDJNHBy24SxNmHOSohy4DR+0Q==','2026-05-14 15:07:08.732252',0,'2026-05-07 15:07:08.732252'),(15,6,'gu4I2YtFl6Oa+4LMPms7RWQYJDldaqzR6zT36eUAxiaO3ylhtDB9IcAcmRDIk/XwKlTlnbbSxF0onGcbbes7Kw==','2026-05-14 15:28:53.336124',0,'2026-05-07 15:28:53.336072'),(16,5,'nVi7BnZpL8tyeqxp0udQA/FzuUE0wethZ1xctPmp3ZRl1jINK/skQqKLaV2bDHS7hKmNHyDAwtVLDlK6gwcqaw==','2026-05-14 15:33:36.291450',0,'2026-05-07 15:33:36.291450'),(17,5,'qRwRm2GGaqDcUsTAItEB9nzJZCff4/3YDKog45rO16NXzV8VuheA1tpdy6WTxVJONHgBgeKgDHqG+bAJEpzNTw==','2026-05-15 01:11:06.382159',0,'2026-05-08 01:11:06.382106'),(18,7,'C5yX9ipI2lVkzt5lUxg6xK02gKsFOlUamAr5OGsK85sBMqUMlciWO5Yth/cEsVgtkuzYMkBEgxq3bOMEOVWTzg==','2026-05-15 01:12:56.309905',0,'2026-05-08 01:12:56.309905'),(19,5,'fOJAY3JH3CGLxOLXoahLcXUX4ZQQTbfc4qTss4GJ4E9E3zar9VQ2ttEVYmnrDCEw9z9PxgWyMg69aMadUvnMSg==','2026-05-15 01:23:21.070706',0,'2026-05-08 01:23:21.070608'),(20,5,'ZvxKWUs4/LNSPVzS90uHbYPYyfednImsulHbKCCRa3ZLDtNgJk+qZUiRNinT/lbYJbpIA2DC3WeOEnoRAfI4zA==','2026-05-15 01:32:52.020118',0,'2026-05-08 01:32:52.020063'),(21,5,'J5YOr5ZneAGJJSpEigQ4Y7HkTXYd3BD4v8gT9pt1E9kYO6t8EutvAtFL59ORJY8TYQfmn4FvhSzIvlY3c0U+rg==','2026-05-15 01:40:41.817777',0,'2026-05-08 01:40:41.817711'),(22,5,'3LL7tLF6YI5MpRErag3Gch8gBohfQZXtujDV+8X8jhB35xpIIIw/c0gxq1HST85YzKXRuBaYRoWJdMBBKsA0hw==','2026-05-15 02:57:08.930940',0,'2026-05-08 02:57:08.930887'),(23,5,'2ERkb+FxMVAkM14tHSfgomDW3e313Kp2TdnuWsUJMwJMtbrFv2Bo3TCYi9057V/TB03TW4UCUSTDOANsCyKmyw==','2026-05-15 02:59:17.602373',0,'2026-05-08 02:59:17.602373'),(24,5,'UQ/SuNcXprGunGPgSQ+0NsxVcx9wLJPI0vMEZaj0w57OotBHfVW2DLFl26dWd+mhfKmXPKzkPP52o2mVug6gaw==','2026-05-15 03:02:57.863848',0,'2026-05-08 03:02:57.863792'),(25,5,'JYvN0k9mcABADpw0N5BEM554ectZJncSv9Etu9qOy6EKeH2Tk+1n9g90VCsr9oVXErh3akJRj3J63GU5XHVt2Q==','2026-05-15 03:05:48.564131',0,'2026-05-08 03:05:48.564067'),(26,5,'mFG/QNR/2l/1tRHXvuaZqegfuFm8ld/cR1wt3k8xspTFxa/bCWNTcy5n+phq+PNEVCzhWjZkEijIikNwzoGN3g==','2026-05-15 03:16:15.482664',0,'2026-05-08 03:16:15.482612'),(27,5,'MAVFCU/Q9ZhW6kPqLuLFL+m4OdRCtfKOL2mtgWvKTmWH2Q1LLzm54AGS8OhwNXtxJP0QvapLfqWpLhFIkIKjDA==','2026-05-15 03:24:28.698318',0,'2026-05-08 03:24:28.698267'),(28,5,'POAaut+x+IgkL1/SGFrG3TxTNRGOBpfvtn6y/HM5ShVccVT1wlfbjjcoUXgoPoulJxh8RkAxVZQGwXaEZdz/nA==','2026-05-15 03:31:00.397747',0,'2026-05-08 03:31:00.397694'),(29,5,'0TCvYt8oEjB5xv6amDy/HjIfDmlLULLqg/lRJzOHBJcIy2RLVadVAcNBIL4YRKbBFAx/rg22apHjLGnTMeKJUQ==','2026-05-15 03:41:32.081833',0,'2026-05-08 03:41:32.081780'),(30,5,'IAPxosQ3l1Kvyd7jcM60zV0plNGNyjg8MNSj8QT822H5HkCUkb2WZT42c70/jJeQ+oj4ioyIAEIFnpsDFuUnKQ==','2026-05-15 03:46:22.211708',0,'2026-05-08 03:46:22.211708'),(31,5,'KtMdW9TZQ4/yFG2uHvHqy5GfKbt2HZ1EFv+V5lZU3qofk3YT0QXOkEDM4yumHwxYYqkpQReEKxTMPRlCRXlqYw==','2026-05-15 03:54:54.688585',0,'2026-05-08 03:54:54.688585'),(32,5,'XjZk5fKjqvUyFRv0zup9flXUbJdt6kM0Vp9DLaYz9Hv1qs1pkGJwzmxvAd4LQBzaK47uu4PuH4ECxHE15NfuNQ==','2026-05-15 04:10:00.422235',0,'2026-05-08 04:10:00.422180'),(33,8,'MQe8o0lGeXYzJgGKqSf4zGrCwOu9gTsNDnKcpmWshfr2RWEETvqi6rj0RyU0LHCtLvYBnhp45ei7REoXzXbP1Q==','2026-05-15 04:14:25.356745',0,'2026-05-08 04:14:25.356745'),(34,8,'I4RRqlNwDrtKLEd/r/MpcLJXn2G5y/jgRWT/T2AocA9fsEVs0f1kFGUj+VwPvnmG0QATp7hOf0Q+A8myOV82SQ==','2026-05-15 04:16:57.393556',0,'2026-05-08 04:16:57.393555'),(35,8,'wQ3UZQxEmNOzk88Nm6TRVs5gUtiE8de+mvuy6jWygxfv/GwKDUSb8TJFLn4y2ZwyPB8wEQBijin14YtkKim21Q==','2026-05-15 04:19:46.279477',0,'2026-05-08 04:19:46.279476'),(36,5,'p9sYsQKsYBQf6S+391Zr8xpmlBnrG1f/c3iVL2HHoBZ+ndyiUICiX7GO4a/tnetG4zP+zjQA21HKH5HGqDPpkg==','2026-05-15 04:22:01.994164',0,'2026-05-08 04:22:01.994163'),(37,8,'HQAcS+RPIRI152bm/zqWC4FU+bfrocNqevxbNw1iF4H1bPx36nVezBBivDSnB565duW9hiGc/oACy4QAObDNTA==','2026-05-15 04:24:47.773846',0,'2026-05-08 04:24:47.773845'),(38,5,'v7rzjbULX7Gyfn7hjLjdLhr143rKQC8w/37+7t5glAabBru87hKXBqAOp1xtywT77OH3akbcE7nia0YNna9z7w==','2026-05-15 04:26:35.887348',0,'2026-05-08 04:26:35.887348'),(39,5,'IRkVlkueZxBQKJtKu/p9Dmyav+gICLWqdidnXjxdaaisyqFAowZJT8Ujb5ywB7Po12IkMxXvZyk3ITmjOxKpQg==','2026-05-15 04:35:39.716031',0,'2026-05-08 04:35:39.715979');
/*!40000 ALTER TABLE `refreshtokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reviews`
--

DROP TABLE IF EXISTS `reviews`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reviews` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AppointmentId` int NOT NULL,
  `PatientId` int NOT NULL,
  `DoctorId` int NOT NULL,
  `Rating` int NOT NULL,
  `Comment` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Reviews_AppointmentId` (`AppointmentId`),
  KEY `IX_Reviews_DoctorId` (`DoctorId`),
  KEY `IX_Reviews_PatientId` (`PatientId`),
  CONSTRAINT `FK_Reviews_Appointments_AppointmentId` FOREIGN KEY (`AppointmentId`) REFERENCES `appointments` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Reviews_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `doctors` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Reviews_Patients_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `patients` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reviews`
--

LOCK TABLES `reviews` WRITE;
/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
INSERT INTO `reviews` VALUES (5,56,6,3,5,NULL,'2026-05-16 07:03:51.266798'),(6,55,6,4,4,NULL,'2026-05-16 07:03:55.637329'),(7,54,7,4,5,NULL,'2026-05-16 07:06:12.228379'),(8,52,5,3,5,NULL,'2026-05-16 07:06:50.894683'),(9,57,8,2,5,NULL,'2026-05-16 07:14:03.464648'),(10,53,7,4,5,NULL,'2026-05-16 07:54:48.517959'),(11,61,5,4,4,NULL,'2026-05-17 13:43:11.267640'),(12,70,2,2,5,NULL,'2026-05-23 02:39:08.894479');
/*!40000 ALTER TABLE `reviews` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `specialties`
--

DROP TABLE IF EXISTS `specialties`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `specialties` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsActive` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `specialties`
--

LOCK TABLES `specialties` WRITE;
/*!40000 ALTER TABLE `specialties` DISABLE KEYS */;
INSERT INTO `specialties` VALUES (1,'Nội khoa','Khám và điều trị các bệnh nội khoa',1,'2026-05-07 15:08:46.658791'),(3,'Ngoại khoa','Phẫu thuật và điều trị ngoại khoa',1,'2026-05-07 15:11:11.768660'),(4,'Da liễu','Khám và điều trị các bệnh về da',1,'2026-05-07 15:11:26.150838'),(5,'Tai mũi họng','Khám tai mũi họng',1,'2026-05-07 15:11:35.354790'),(6,'Tim mạch','Khám và điều trị bệnh tim mạch',1,'2026-05-07 15:11:44.734741');
/*!40000 ALTER TABLE `specialties` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FullName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Phone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Role` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT (_utf8mb4'Patient'),
  `IsActive` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_Users_Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'string','user@example.com','$2a$12$gy/Cv8WoLAy5Op3F28Q9GOdOp2jQ0WS6wvwQwusEDg6eQehQOsB0S','string','Patient',1,'2026-05-07 12:19:29.041070'),(5,'Admin','admin@gmail.com','$2a$12$NuNJS8kKosGcJJbhCViIFeRkp8wBYvhJIQMvFbGD4ryvBaTR1U.X6','0987654321','Admin',1,'2026-05-07 14:51:39.594877'),(6,'khoaanh','anhkhoa@gmail.com','$2a$12$Cz/tgZw4IL6XU/yjVzZoK.QJ24SxdPGyU2cyrJZ1c14/v4bYpCw1K','0123456789','Patient',1,'2026-05-07 15:28:52.489903'),(7,'taotenkhoa','benhnhan@gmail.com','$2a$12$bs1R7UYuUk5VycIM4fn4OeIE3TpGFPruJ5lxXVkz.h.kr/sVvP4xe','0123456789','Patient',1,'2026-05-08 01:12:55.719525'),(8,'HoangLich','hoanglich@gmail.com','$2a$12$/cu01CcRQyzYHebphzHoKuPGQdBUWF18h5.DqRV.8YNbPfJYt1SZe','099887766','Patient',1,'2026-05-08 04:14:24.727859'),(9,'Quy Nhơn','quynhon@gmail.com','$2a$12$xowgms.cOdKePJ69Mc0av.HtUERHFSPM8rE9dtwzwA1xSCj3pt24C','+84357937045','Patient',1,'2026-05-09 14:23:44.274488'),(10,'Vũ Sơn Lâm','doctor@gmail.com','$2a$12$DeguiOfARv6tvqieTC/k/.1lAvVZ/H3muZu53M3djDyZcJ0wrNjV6','0123456789','Doctor',1,'2026-05-09 15:03:57.382210'),(11,'105-119 Khoa Lê Anh','anhkhoale2406@gmail.com','$2a$12$LdVXd.AdvN/GL3zHI9GI3ueo559PM0k4I3m7G1hilwl5SVArCwYWS','06655443322','Patient',1,'2026-05-09 15:53:08.030326'),(12,'Anh Khoa','levanthanhcathiep@gmail.com','$2a$12$LgY.TEGECiQWagcv63PYfOZvRVjAUXjMDiIhe8E363i8lLz99mpxe','03579753135','Patient',1,'2026-05-10 03:18:19.443508'),(13,'Lịch Bùi Hoàng','lichhoang505@gmail.com','$2a$12$InLybiZEYheQDcpmX1XmTOT5x.CTTlWygeaaEd7N5xSrcsLDAQJOK','','Patient',1,'2026-05-10 03:36:03.475271'),(14,'Anh Khoa Lê','anhkhoa24605@gmail.com','$2a$12$DH7vyectlUYXo8zVe4z17.2dzmLGfw4Ma8f7kV0uMGyvzONDJKFhO','06543678923','Patient',1,'2026-05-10 11:41:53.042724'),(15,'Nguyễn Hữu Tài','doctor1@gmail.com','$2a$12$jxnIoN2W22rmbMDtq3hJBuJVz1SmRX6eB9Wq7PMVmKpJKJTAV9sDC','0987654321','Doctor',1,'2026-05-11 03:51:54.973111'),(16,'phuc van','vphuc11052005@gmail.com','$2a$12$9qHZ/uD8Ak8rfUS7dnI6UeAH0hZipnrFR/Rxwn6Jq5IHScrJFhTg.','','Patient',1,'2026-05-13 06:47:35.196352'),(17,'Nguyễn Thị Loan','doctor4@gmail.com','$2a$12$vHUCKgCmf9.zHPrxZauiu.bkVncLjnYz/egIlGqR2wcvJq7oeQ3iS','03579753135','Doctor',1,'2026-05-15 10:54:33.377886'),(18,'SoundCloud Music','vo2449199@gmail.com','$2a$12$k0N72k7genpr1TNjzEsP.OUiQVwOddzPPlSwgOxa9tEWT1vwoIDB6','','Patient',1,'2026-05-15 14:32:44.381863'),(19,'Mai Thanh Tu','tu4651170077@st.qnu.edu.vn','$2a$12$sZrxr6DRDeF2hcBDksU9OODgKNacOisbBRO0YW4aW6CESe2NF09gG','','Patient',1,'2026-05-19 06:14:22.233981'),(20,'Kieuluy88 Ds','dslekieu88@gmail.com','$2a$12$IiCSoUtIK7W41mwbpL0WXuSWvsrulYjE0eax5YufEuJrIunPZemIi','','Patient',1,'2026-05-22 11:23:34.481526');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `workingschedules`
--

DROP TABLE IF EXISTS `workingschedules`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `workingschedules` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DoctorId` int NOT NULL,
  `DayOfWeek` int NOT NULL,
  `StartTime` time(6) NOT NULL,
  `EndTime` time(6) NOT NULL,
  `SlotDurationMinutes` int NOT NULL,
  `MaxSlots` int NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_WorkingSchedules_DoctorId` (`DoctorId`),
  CONSTRAINT `FK_WorkingSchedules_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `doctors` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workingschedules`
--

LOCK TABLES `workingschedules` WRITE;
/*!40000 ALTER TABLE `workingschedules` DISABLE KEYS */;
INSERT INTO `workingschedules` VALUES (2,2,1,'15:47:00.000000','17:48:00.000000',30,10,1),(8,3,1,'07:00:00.000000','17:00:00.000000',20,30,1),(13,2,2,'08:00:00.000000','18:00:00.000000',30,20,1),(19,4,0,'10:30:00.000000','19:00:00.000000',20,25,1),(20,4,1,'09:30:00.000000','15:00:00.000000',30,11,1),(23,3,6,'09:00:00.000000','17:00:00.000000',30,16,1),(24,2,5,'09:00:00.000000','17:00:00.000000',30,16,1),(25,4,6,'10:00:00.000000','20:00:00.000000',30,20,1);
/*!40000 ALTER TABLE `workingschedules` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'clinic_management'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-05-23 11:01:24

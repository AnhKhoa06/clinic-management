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
INSERT INTO `__efmigrationshistory` VALUES ('20260507110254_InitialCreate','9.0.0'),('20260507120230_AddRefreshToken','9.0.0'),('20260507141443_AddCreatedAtToSpecialty','9.0.0'),('20260511033141_FixSlotAppointmentRelation','9.0.0'),('20260511155936_AutoCalculateMaxSlots','9.0.0'),('20260512040848_AddPriceAndFeeBreakdown','9.0.0'),('20260512050533_RestructurePrescriptionDosage','9.0.0');
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
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appointments`
--

LOCK TABLES `appointments` WRITE;
/*!40000 ALTER TABLE `appointments` DISABLE KEYS */;
INSERT INTO `appointments` VALUES (4,7,2,150,'mệt mỏi','Completed',NULL,NULL,'2026-05-10 11:42:08.778688'),(5,2,2,157,'Thèm ăn','Completed',NULL,NULL,'2026-05-10 15:23:38.518910'),(6,2,2,156,'Chán ăn','Completed',NULL,NULL,'2026-05-10 16:13:25.384810'),(10,2,2,145,'mm','Completed',NULL,NULL,'2026-05-11 03:12:50.967573'),(13,2,2,144,'dd','Completed',NULL,NULL,'2026-05-11 03:20:45.253935'),(15,2,2,58,'dd','Completed',NULL,NULL,'2026-05-11 03:27:42.151988'),(16,4,2,171,'mệt mỏi','Confirmed',NULL,NULL,'2026-05-11 10:36:21.732550'),(24,5,3,194,'ok','Completed',NULL,NULL,'2026-05-12 02:45:00.892262'),(25,5,3,195,'ok','Completed',NULL,NULL,'2026-05-12 03:07:16.910725'),(26,5,3,197,'ok','Completed',NULL,NULL,'2026-05-12 04:10:11.181209'),(27,5,3,199,'ok','Completed',NULL,NULL,'2026-05-12 06:16:59.316919'),(28,2,3,193,'ok','Completed',NULL,NULL,'2026-05-12 06:43:28.200457'),(29,2,3,192,'ok','Completed',NULL,NULL,'2026-05-12 06:57:51.045166'),(30,6,3,202,'ok','Completed',NULL,NULL,'2026-05-12 07:06:35.296016'),(31,2,3,196,'ok','Completed',NULL,NULL,'2026-05-12 07:09:59.715930'),(32,4,3,198,'ok','Completed',NULL,NULL,'2026-05-12 08:09:10.660908'),(33,4,3,200,'ok','Completed',NULL,NULL,'2026-05-12 16:15:47.613347'),(34,8,2,305,'ok','Completed',NULL,NULL,'2026-05-13 06:50:21.785708'),(35,4,2,371,'ok','Completed',NULL,NULL,'2026-05-15 00:41:03.675839'),(36,4,2,365,'ok','Completed',NULL,NULL,'2026-05-15 00:43:19.666427'),(37,4,2,372,'ok','Completed',NULL,NULL,'2026-05-15 01:20:09.824884'),(38,4,2,373,'ok','Completed',NULL,NULL,'2026-05-15 04:17:01.803118'),(39,4,2,370,'ok','Completed',NULL,NULL,'2026-05-15 04:18:31.814062'),(40,2,2,364,'ok','Completed',NULL,NULL,'2026-05-15 04:23:34.450370'),(41,6,2,368,'Mệt mỏi','Completed',NULL,NULL,'2026-05-15 04:31:34.027327'),(42,4,2,369,'ok','Completed',NULL,NULL,'2026-05-15 04:35:38.377129'),(43,4,2,377,'f','Completed',NULL,NULL,'2026-05-15 04:42:12.852673'),(44,5,4,463,'ok','Completed',NULL,NULL,'2026-05-15 11:58:55.273024'),(45,4,4,462,'ok','Completed',NULL,NULL,'2026-05-15 13:07:18.318663'),(46,4,2,379,'ok','Completed',NULL,NULL,'2026-05-15 13:11:16.907963'),(47,5,4,461,'ok','Completed',NULL,NULL,'2026-05-15 14:20:26.545625'),(48,5,4,458,'ok','Completed',NULL,NULL,'2026-05-15 14:24:09.100817'),(49,9,2,381,'ok','Pending',NULL,NULL,'2026-05-15 14:32:59.311773'),(50,9,2,380,'Mệt mỏi','Completed',NULL,NULL,'2026-05-15 14:42:57.824120');
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
  `WorkingScheduleId` int NOT NULL,
  `DoctorId` int NOT NULL,
  `SlotDate` date NOT NULL,
  `SlotTime` time(6) NOT NULL,
  `Status` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_AppointmentSlots_DoctorId_SlotDate_SlotTime` (`DoctorId`,`SlotDate`,`SlotTime`),
  KEY `IX_AppointmentSlots_WorkingScheduleId` (`WorkingScheduleId`),
  CONSTRAINT `FK_AppointmentSlots_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `doctors` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AppointmentSlots_WorkingSchedules_WorkingScheduleId` FOREIGN KEY (`WorkingScheduleId`) REFERENCES `workingschedules` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=492 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appointmentslots`
--

LOCK TABLES `appointmentslots` WRITE;
/*!40000 ALTER TABLE `appointmentslots` DISABLE KEYS */;
INSERT INTO `appointmentslots` VALUES (57,2,2,'2026-05-11','16:47:00.000000','Booked'),(58,2,2,'2026-05-11','17:17:00.000000','Booked'),(143,2,2,'2026-05-11','15:47:00.000000','Booked'),(144,2,2,'2026-05-11','16:17:00.000000','Booked'),(145,2,2,'2026-05-11','17:47:00.000000','Booked'),(146,5,2,'2026-05-10','18:45:00.000000','Available'),(147,5,2,'2026-05-10','19:05:00.000000','Available'),(148,5,2,'2026-05-10','19:25:00.000000','Available'),(149,5,2,'2026-05-10','19:45:00.000000','Available'),(150,5,2,'2026-05-10','20:05:00.000000','Booked'),(151,5,2,'2026-05-10','20:25:00.000000','Available'),(152,5,2,'2026-05-10','20:45:00.000000','Available'),(153,5,2,'2026-05-10','21:05:00.000000','Available'),(154,5,2,'2026-05-10','21:25:00.000000','Available'),(155,5,2,'2026-05-10','21:45:00.000000','Available'),(156,5,2,'2026-05-10','22:05:00.000000','Booked'),(157,5,2,'2026-05-10','22:25:00.000000','Booked'),(158,5,2,'2026-05-17','18:45:00.000000','Available'),(159,5,2,'2026-05-17','19:05:00.000000','Available'),(160,5,2,'2026-05-17','19:25:00.000000','Available'),(161,5,2,'2026-05-17','19:45:00.000000','Available'),(162,5,2,'2026-05-17','20:05:00.000000','Available'),(163,5,2,'2026-05-17','20:25:00.000000','Available'),(164,5,2,'2026-05-17','20:45:00.000000','Available'),(165,5,2,'2026-05-17','21:05:00.000000','Available'),(166,5,2,'2026-05-17','21:25:00.000000','Available'),(167,5,2,'2026-05-17','21:45:00.000000','Available'),(168,5,2,'2026-05-17','22:05:00.000000','Available'),(169,5,2,'2026-05-17','22:25:00.000000','Available'),(170,2,2,'2026-05-18','15:47:00.000000','Available'),(171,2,2,'2026-05-18','16:17:00.000000','Booked'),(172,2,2,'2026-05-18','16:47:00.000000','Available'),(173,2,2,'2026-05-18','17:17:00.000000','Available'),(174,2,2,'2026-05-18','17:47:00.000000','Available'),(191,7,3,'2026-05-12','08:00:00.000000','Available'),(192,7,3,'2026-05-12','08:45:00.000000','Booked'),(193,7,3,'2026-05-12','09:30:00.000000','Booked'),(194,7,3,'2026-05-12','10:15:00.000000','Booked'),(195,7,3,'2026-05-12','11:00:00.000000','Booked'),(196,7,3,'2026-05-12','11:45:00.000000','Booked'),(197,7,3,'2026-05-12','12:30:00.000000','Booked'),(198,7,3,'2026-05-12','13:15:00.000000','Booked'),(199,7,3,'2026-05-12','14:00:00.000000','Booked'),(200,7,3,'2026-05-12','14:45:00.000000','Booked'),(201,7,3,'2026-05-12','15:30:00.000000','Available'),(202,7,3,'2026-05-12','16:15:00.000000','Booked'),(203,7,3,'2026-05-19','08:00:00.000000','Available'),(204,7,3,'2026-05-19','08:45:00.000000','Available'),(205,7,3,'2026-05-19','09:30:00.000000','Available'),(206,7,3,'2026-05-19','10:15:00.000000','Available'),(207,7,3,'2026-05-19','11:00:00.000000','Available'),(208,7,3,'2026-05-19','11:45:00.000000','Available'),(209,7,3,'2026-05-19','12:30:00.000000','Available'),(210,7,3,'2026-05-19','13:15:00.000000','Available'),(211,7,3,'2026-05-19','14:00:00.000000','Available'),(212,7,3,'2026-05-19','14:45:00.000000','Available'),(213,7,3,'2026-05-19','15:30:00.000000','Available'),(214,7,3,'2026-05-19','16:15:00.000000','Available'),(223,7,3,'2026-05-26','08:00:00.000000','Available'),(224,7,3,'2026-05-26','08:45:00.000000','Available'),(225,7,3,'2026-05-26','09:30:00.000000','Available'),(226,7,3,'2026-05-26','10:15:00.000000','Available'),(227,7,3,'2026-05-26','11:00:00.000000','Available'),(228,7,3,'2026-05-26','11:45:00.000000','Available'),(229,7,3,'2026-05-26','12:30:00.000000','Available'),(230,7,3,'2026-05-26','13:15:00.000000','Available'),(231,7,3,'2026-05-26','14:00:00.000000','Available'),(232,7,3,'2026-05-26','14:45:00.000000','Available'),(233,7,3,'2026-05-26','15:30:00.000000','Available'),(234,7,3,'2026-05-26','16:15:00.000000','Available'),(243,8,3,'2026-05-18','07:00:00.000000','Available'),(244,8,3,'2026-05-18','07:20:00.000000','Available'),(245,8,3,'2026-05-18','07:40:00.000000','Available'),(246,8,3,'2026-05-18','08:00:00.000000','Available'),(247,8,3,'2026-05-18','08:20:00.000000','Available'),(248,8,3,'2026-05-18','08:40:00.000000','Available'),(249,8,3,'2026-05-18','09:00:00.000000','Available'),(250,8,3,'2026-05-18','09:20:00.000000','Available'),(251,8,3,'2026-05-18','09:40:00.000000','Available'),(252,8,3,'2026-05-18','10:00:00.000000','Available'),(253,8,3,'2026-05-18','10:20:00.000000','Available'),(254,8,3,'2026-05-18','10:40:00.000000','Available'),(255,8,3,'2026-05-18','11:00:00.000000','Available'),(256,8,3,'2026-05-18','11:20:00.000000','Available'),(257,8,3,'2026-05-18','11:40:00.000000','Available'),(258,8,3,'2026-05-18','12:00:00.000000','Available'),(259,8,3,'2026-05-18','12:20:00.000000','Available'),(260,8,3,'2026-05-18','12:40:00.000000','Available'),(261,8,3,'2026-05-18','13:00:00.000000','Available'),(262,8,3,'2026-05-18','13:20:00.000000','Available'),(263,8,3,'2026-05-18','13:40:00.000000','Available'),(264,8,3,'2026-05-18','14:00:00.000000','Available'),(265,8,3,'2026-05-18','14:20:00.000000','Available'),(266,8,3,'2026-05-18','14:40:00.000000','Available'),(267,8,3,'2026-05-18','15:00:00.000000','Available'),(268,8,3,'2026-05-18','15:20:00.000000','Available'),(269,8,3,'2026-05-18','15:40:00.000000','Available'),(270,8,3,'2026-05-18','16:00:00.000000','Available'),(271,8,3,'2026-05-18','16:20:00.000000','Available'),(272,8,3,'2026-05-18','16:40:00.000000','Available'),(273,8,3,'2026-05-25','07:00:00.000000','Available'),(274,8,3,'2026-05-25','07:20:00.000000','Available'),(275,8,3,'2026-05-25','07:40:00.000000','Available'),(276,8,3,'2026-05-25','08:00:00.000000','Available'),(277,8,3,'2026-05-25','08:20:00.000000','Available'),(278,8,3,'2026-05-25','08:40:00.000000','Available'),(279,8,3,'2026-05-25','09:00:00.000000','Available'),(280,8,3,'2026-05-25','09:20:00.000000','Available'),(281,8,3,'2026-05-25','09:40:00.000000','Available'),(282,8,3,'2026-05-25','10:00:00.000000','Available'),(283,8,3,'2026-05-25','10:20:00.000000','Available'),(284,8,3,'2026-05-25','10:40:00.000000','Available'),(285,8,3,'2026-05-25','11:00:00.000000','Available'),(286,8,3,'2026-05-25','11:20:00.000000','Available'),(287,8,3,'2026-05-25','11:40:00.000000','Available'),(288,8,3,'2026-05-25','12:00:00.000000','Available'),(289,8,3,'2026-05-25','12:20:00.000000','Available'),(290,8,3,'2026-05-25','12:40:00.000000','Available'),(291,8,3,'2026-05-25','13:00:00.000000','Available'),(292,8,3,'2026-05-25','13:20:00.000000','Available'),(293,8,3,'2026-05-25','13:40:00.000000','Available'),(294,8,3,'2026-05-25','14:00:00.000000','Available'),(295,8,3,'2026-05-25','14:20:00.000000','Available'),(296,8,3,'2026-05-25','14:40:00.000000','Available'),(297,8,3,'2026-05-25','15:00:00.000000','Available'),(298,8,3,'2026-05-25','15:20:00.000000','Available'),(299,8,3,'2026-05-25','15:40:00.000000','Available'),(300,8,3,'2026-05-25','16:00:00.000000','Available'),(301,8,3,'2026-05-25','16:20:00.000000','Available'),(302,8,3,'2026-05-25','16:40:00.000000','Available'),(303,9,2,'2026-05-13','13:00:00.000000','Available'),(304,9,2,'2026-05-13','13:30:00.000000','Available'),(305,9,2,'2026-05-13','14:00:00.000000','Booked'),(306,9,2,'2026-05-13','14:30:00.000000','Available'),(307,9,2,'2026-05-13','15:00:00.000000','Available'),(308,9,2,'2026-05-13','15:30:00.000000','Available'),(309,9,2,'2026-05-13','16:00:00.000000','Available'),(310,9,2,'2026-05-13','16:30:00.000000','Available'),(311,9,2,'2026-05-13','17:00:00.000000','Available'),(312,9,2,'2026-05-13','17:30:00.000000','Available'),(313,9,2,'2026-05-13','18:00:00.000000','Available'),(314,9,2,'2026-05-13','18:30:00.000000','Available'),(315,9,2,'2026-05-13','19:00:00.000000','Available'),(316,9,2,'2026-05-13','19:30:00.000000','Available'),(317,9,2,'2026-05-20','13:00:00.000000','Available'),(318,9,2,'2026-05-20','13:30:00.000000','Available'),(319,9,2,'2026-05-20','14:00:00.000000','Available'),(320,9,2,'2026-05-20','14:30:00.000000','Available'),(321,9,2,'2026-05-20','15:00:00.000000','Available'),(322,9,2,'2026-05-20','15:30:00.000000','Available'),(323,9,2,'2026-05-20','16:00:00.000000','Available'),(324,9,2,'2026-05-20','16:30:00.000000','Available'),(325,9,2,'2026-05-20','17:00:00.000000','Available'),(326,9,2,'2026-05-20','17:30:00.000000','Available'),(327,9,2,'2026-05-20','18:00:00.000000','Available'),(328,9,2,'2026-05-20','18:30:00.000000','Available'),(329,9,2,'2026-05-20','19:00:00.000000','Available'),(330,9,2,'2026-05-20','19:30:00.000000','Available'),(331,5,2,'2026-05-24','18:45:00.000000','Available'),(332,5,2,'2026-05-24','19:05:00.000000','Available'),(333,5,2,'2026-05-24','19:25:00.000000','Available'),(334,5,2,'2026-05-24','19:45:00.000000','Available'),(335,5,2,'2026-05-24','20:05:00.000000','Available'),(336,5,2,'2026-05-24','20:25:00.000000','Available'),(337,5,2,'2026-05-24','20:45:00.000000','Available'),(338,5,2,'2026-05-24','21:05:00.000000','Available'),(339,5,2,'2026-05-24','21:25:00.000000','Available'),(340,5,2,'2026-05-24','21:45:00.000000','Available'),(341,5,2,'2026-05-24','22:05:00.000000','Available'),(342,5,2,'2026-05-24','22:25:00.000000','Available'),(343,2,2,'2026-05-25','15:47:00.000000','Available'),(344,2,2,'2026-05-25','16:17:00.000000','Available'),(345,2,2,'2026-05-25','16:47:00.000000','Available'),(346,2,2,'2026-05-25','17:17:00.000000','Available'),(347,2,2,'2026-05-25','17:47:00.000000','Available'),(348,9,2,'2026-05-27','13:00:00.000000','Available'),(349,9,2,'2026-05-27','13:30:00.000000','Available'),(350,9,2,'2026-05-27','14:00:00.000000','Available'),(351,9,2,'2026-05-27','14:30:00.000000','Available'),(352,9,2,'2026-05-27','15:00:00.000000','Available'),(353,9,2,'2026-05-27','15:30:00.000000','Available'),(354,9,2,'2026-05-27','16:00:00.000000','Available'),(355,9,2,'2026-05-27','16:30:00.000000','Available'),(356,9,2,'2026-05-27','17:00:00.000000','Available'),(357,9,2,'2026-05-27','17:30:00.000000','Available'),(358,9,2,'2026-05-27','18:00:00.000000','Available'),(359,9,2,'2026-05-27','18:30:00.000000','Available'),(360,9,2,'2026-05-27','19:00:00.000000','Available'),(361,9,2,'2026-05-27','19:30:00.000000','Available'),(362,10,2,'2026-05-15','08:00:00.000000','Available'),(363,10,2,'2026-05-15','08:30:00.000000','Available'),(364,10,2,'2026-05-15','09:00:00.000000','Booked'),(365,10,2,'2026-05-15','09:30:00.000000','Booked'),(366,10,2,'2026-05-15','10:00:00.000000','Available'),(367,10,2,'2026-05-15','10:30:00.000000','Available'),(368,10,2,'2026-05-15','11:00:00.000000','Booked'),(369,10,2,'2026-05-15','11:30:00.000000','Booked'),(370,10,2,'2026-05-15','12:00:00.000000','Booked'),(371,10,2,'2026-05-15','12:30:00.000000','Booked'),(372,10,2,'2026-05-15','13:00:00.000000','Booked'),(373,10,2,'2026-05-15','13:30:00.000000','Booked'),(374,10,2,'2026-05-15','14:00:00.000000','Available'),(375,10,2,'2026-05-15','14:30:00.000000','Available'),(376,10,2,'2026-05-15','15:00:00.000000','Available'),(377,10,2,'2026-05-15','15:30:00.000000','Booked'),(378,10,2,'2026-05-15','16:00:00.000000','Available'),(379,10,2,'2026-05-15','16:30:00.000000','Booked'),(380,10,2,'2026-05-15','17:00:00.000000','Booked'),(381,10,2,'2026-05-15','17:30:00.000000','Booked'),(382,10,2,'2026-05-22','08:00:00.000000','Available'),(383,10,2,'2026-05-22','08:30:00.000000','Available'),(384,10,2,'2026-05-22','09:00:00.000000','Available'),(385,10,2,'2026-05-22','09:30:00.000000','Available'),(386,10,2,'2026-05-22','10:00:00.000000','Available'),(387,10,2,'2026-05-22','10:30:00.000000','Available'),(388,10,2,'2026-05-22','11:00:00.000000','Available'),(389,10,2,'2026-05-22','11:30:00.000000','Available'),(390,10,2,'2026-05-22','12:00:00.000000','Available'),(391,10,2,'2026-05-22','12:30:00.000000','Available'),(392,10,2,'2026-05-22','13:00:00.000000','Available'),(393,10,2,'2026-05-22','13:30:00.000000','Available'),(394,10,2,'2026-05-22','14:00:00.000000','Available'),(395,10,2,'2026-05-22','14:30:00.000000','Available'),(396,10,2,'2026-05-22','15:00:00.000000','Available'),(397,10,2,'2026-05-22','15:30:00.000000','Available'),(398,10,2,'2026-05-22','16:00:00.000000','Available'),(399,10,2,'2026-05-22','16:30:00.000000','Available'),(400,10,2,'2026-05-22','17:00:00.000000','Available'),(401,10,2,'2026-05-22','17:30:00.000000','Available'),(402,10,2,'2026-05-29','08:00:00.000000','Available'),(403,10,2,'2026-05-29','08:30:00.000000','Available'),(404,10,2,'2026-05-29','09:00:00.000000','Available'),(405,10,2,'2026-05-29','09:30:00.000000','Available'),(406,10,2,'2026-05-29','10:00:00.000000','Available'),(407,10,2,'2026-05-29','10:30:00.000000','Available'),(408,10,2,'2026-05-29','11:00:00.000000','Available'),(409,10,2,'2026-05-29','11:30:00.000000','Available'),(410,10,2,'2026-05-29','12:00:00.000000','Available'),(411,10,2,'2026-05-29','12:30:00.000000','Available'),(412,10,2,'2026-05-29','13:00:00.000000','Available'),(413,10,2,'2026-05-29','13:30:00.000000','Available'),(414,10,2,'2026-05-29','14:00:00.000000','Available'),(415,10,2,'2026-05-29','14:30:00.000000','Available'),(416,10,2,'2026-05-29','15:00:00.000000','Available'),(417,10,2,'2026-05-29','15:30:00.000000','Available'),(418,10,2,'2026-05-29','16:00:00.000000','Available'),(419,10,2,'2026-05-29','16:30:00.000000','Available'),(420,10,2,'2026-05-29','17:00:00.000000','Available'),(421,10,2,'2026-05-29','17:30:00.000000','Available'),(450,12,4,'2026-05-15','08:00:00.000000','Available'),(451,12,4,'2026-05-15','08:45:00.000000','Available'),(452,12,4,'2026-05-15','09:30:00.000000','Available'),(453,12,4,'2026-05-15','10:15:00.000000','Available'),(454,12,4,'2026-05-15','11:00:00.000000','Available'),(455,12,4,'2026-05-15','11:45:00.000000','Available'),(456,12,4,'2026-05-15','12:30:00.000000','Available'),(457,12,4,'2026-05-15','13:15:00.000000','Available'),(458,12,4,'2026-05-15','14:00:00.000000','Booked'),(459,12,4,'2026-05-15','14:45:00.000000','Available'),(460,12,4,'2026-05-15','15:30:00.000000','Available'),(461,12,4,'2026-05-15','16:15:00.000000','Booked'),(462,12,4,'2026-05-15','17:00:00.000000','Booked'),(463,12,4,'2026-05-15','17:45:00.000000','Booked'),(464,12,4,'2026-05-22','08:00:00.000000','Available'),(465,12,4,'2026-05-22','08:45:00.000000','Available'),(466,12,4,'2026-05-22','09:30:00.000000','Available'),(467,12,4,'2026-05-22','10:15:00.000000','Available'),(468,12,4,'2026-05-22','11:00:00.000000','Available'),(469,12,4,'2026-05-22','11:45:00.000000','Available'),(470,12,4,'2026-05-22','12:30:00.000000','Available'),(471,12,4,'2026-05-22','13:15:00.000000','Available'),(472,12,4,'2026-05-22','14:00:00.000000','Available'),(473,12,4,'2026-05-22','14:45:00.000000','Available'),(474,12,4,'2026-05-22','15:30:00.000000','Available'),(475,12,4,'2026-05-22','16:15:00.000000','Available'),(476,12,4,'2026-05-22','17:00:00.000000','Available'),(477,12,4,'2026-05-22','17:45:00.000000','Available'),(478,12,4,'2026-05-29','08:00:00.000000','Available'),(479,12,4,'2026-05-29','08:45:00.000000','Available'),(480,12,4,'2026-05-29','09:30:00.000000','Available'),(481,12,4,'2026-05-29','10:15:00.000000','Available'),(482,12,4,'2026-05-29','11:00:00.000000','Available'),(483,12,4,'2026-05-29','11:45:00.000000','Available'),(484,12,4,'2026-05-29','12:30:00.000000','Available'),(485,12,4,'2026-05-29','13:15:00.000000','Available'),(486,12,4,'2026-05-29','14:00:00.000000','Available'),(487,12,4,'2026-05-29','14:45:00.000000','Available'),(488,12,4,'2026-05-29','15:30:00.000000','Available'),(489,12,4,'2026-05-29','16:15:00.000000','Available'),(490,12,4,'2026-05-29','17:00:00.000000','Available'),(491,12,4,'2026-05-29','17:45:00.000000','Available');
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
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `medicalrecords`
--

LOCK TABLES `medicalrecords` WRITE;
/*!40000 ALTER TABLE `medicalrecords` DISABLE KEYS */;
INSERT INTO `medicalrecords` VALUES (4,10,2,2,'Sốt','Nghiện game','Cai nghiện gấp','2026-05-16','2026-05-11 10:11:54.475599'),(5,24,5,3,'Mệt mỏi nhức đầu','Over Thingking','Tinh tâm lại là hết','2026-05-16','2026-05-12 02:48:29.331127'),(6,26,5,3,'Mệt mỏi','Overthingking','Nghỉ ngơi','2026-05-22','2026-05-12 05:06:59.890132'),(7,27,5,3,'mệt','Cảm sốt','nghỉ ngơi','2026-05-15','2026-05-12 06:17:53.589373'),(15,35,4,2,'Mệt mỏi','Sốt','Nghỉ ngơi','2026-05-20','2026-05-15 00:42:22.412760'),(24,44,5,4,'Mệt mỏi','Rối loạn thần kinh','Nghỉ ngơi','2026-05-18','2026-05-15 12:01:06.970832'),(25,45,4,4,'v','v','v','2026-05-16','2026-05-15 13:07:46.437489'),(26,46,4,2,'d','d','d',NULL,'2026-05-15 13:11:36.059790'),(27,47,5,4,'Mệt','Viêm họng','nghỉ ngơi','2026-05-17','2026-05-15 14:21:20.270921'),(28,48,5,4,'c','c','c',NULL,'2026-05-15 14:25:00.628419');
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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `patients`
--

LOCK TABLES `patients` WRITE;
/*!40000 ALTER TABLE `patients` DISABLE KEYS */;
INSERT INTO `patients` VALUES (1,7,'2007-06-17','Nữ','Sài Gòn',NULL,'Anh 5 - 0987654321'),(2,8,'2003-06-16','Nam','Gia Lai',NULL,'Anh 2 - 0987654321'),(3,9,'2003-05-22','Nam','Bình Định',NULL,'Anh 2 - 0984357454'),(4,11,'2005-06-24','Nam','Quy Nhơn',NULL,'Anh 3 - 09876555443'),(5,12,'2004-03-04','Nữ','Bình Định',NULL,'Chị 2 - 0987867454'),(6,13,'2003-05-17','Nam','Quận 1',NULL,'Ba - 0988677544'),(7,14,'2004-08-12','Nam','TP. Hồ Chí Minh',NULL,'Chị 3 - 0123445556'),(8,16,'2005-06-05','Nam','Gia Lai',NULL,'Anh 2 - 0987654321'),(9,18,NULL,NULL,NULL,NULL,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payments`
--

LOCK TABLES `payments` WRITE;
/*!40000 ALTER TABLE `payments` DISABLE KEYS */;
INSERT INTO `payments` VALUES (1,6,'INV-20260510-0006',150000.000000000000000000000000000000,'Paid','Cash','ok','2026-05-10 16:31:01.329494','0001-01-01 00:00:00.000000',0.000000000000000000000000000000,0.000000000000000000000000000000),(2,5,'INV-20260511-0005',150000.000000000000000000000000000000,'Paid','BankTransfer','OK','2026-05-11 09:34:39.849692','0001-01-01 00:00:00.000000',0.000000000000000000000000000000,0.000000000000000000000000000000),(3,4,'INV-20260511-0004',65000.000000000000000000000000000000,'Paid','Cash','ok','2026-05-11 11:34:27.364952','0001-01-01 00:00:00.000000',0.000000000000000000000000000000,0.000000000000000000000000000000),(4,10,'INV-20260511-0010',525000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-11 10:20:53.742748','0001-01-01 00:00:00.000000',0.000000000000000000000000000000,0.000000000000000000000000000000),(5,15,'INV-20260511-0015',99000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-11 10:23:52.414926','0001-01-01 00:00:00.000000',0.000000000000000000000000000000,0.000000000000000000000000000000),(6,13,'INV-20260511-0013',250000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-11 10:37:50.274270','0001-01-01 00:00:00.000000',0.000000000000000000000000000000,0.000000000000000000000000000000),(14,24,'INV-20260512-0024',123456.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-12 03:05:19.743393','0001-01-01 00:00:00.000000',0.000000000000000000000000000000,0.000000000000000000000000000000),(15,25,'INV-20260512-0025',987654.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-12 03:17:12.408343','0001-01-01 00:00:00.000000',0.000000000000000000000000000000,0.000000000000000000000000000000),(16,26,'INV-20260512-0026',1050000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-12 05:12:43.079720','2026-05-12 05:11:07.292581',0.000000000000000000000000000000,1050000.000000000000000000000000000000),(17,27,'INV-20260512-0027',252000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-12 07:23:27.025011','2026-05-12 06:18:01.213657',0.000000000000000000000000000000,252000.000000000000000000000000000000),(18,28,'INV-20260512-0028',840000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-12 06:45:11.355944','2026-05-12 06:43:56.155704',0.000000000000000000000000000000,840000.000000000000000000000000000000),(19,29,'INV-20260512-0029',840000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-12 07:00:51.388906','2026-05-12 06:58:24.978819',0.000000000000000000000000000000,840000.000000000000000000000000000000),(20,30,'INV-20260512-0030',252000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-12 07:27:58.231878','2026-05-12 07:07:50.469348',0.000000000000000000000000000000,252000.000000000000000000000000000000),(21,31,'INV-20260512-0031',1050000.000000000000000000000000000000,'Unpaid','BankTransfer','ok',NULL,'2026-05-12 07:10:30.672834',0.000000000000000000000000000000,1050000.000000000000000000000000000000),(22,32,'INV-20260512-0032',420000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-12 08:11:14.909848','2026-05-12 08:10:22.612571',0.000000000000000000000000000000,420000.000000000000000000000000000000),(23,33,'INV-20260512-0033',200000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-12 16:19:03.169291','2026-05-12 16:17:15.905881',0.000000000000000000000000000000,200000.000000000000000000000000000000),(24,34,'INV-20260513-0034',300000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-13 06:51:29.925934','2026-05-13 06:50:57.311390',0.000000000000000000000000000000,300000.000000000000000000000000000000),(25,35,'INV-20260515-0035',2100000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-15 01:00:49.472120','2026-05-15 00:42:29.574299',0.000000000000000000000000000000,2100000.000000000000000000000000000000),(26,36,'INV-20260515-0036',1050000.000000000000000000000000000000,'Paid','Cash','ok','2026-05-15 00:59:19.375647','2026-05-15 00:43:50.557772',0.000000000000000000000000000000,1050000.000000000000000000000000000000),(27,37,'INV-20260515-0037',600000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-15 01:24:07.420912','2026-05-15 01:22:28.507141',0.000000000000000000000000000000,600000.000000000000000000000000000000),(28,38,'INV-20260515-0038',120000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-15 04:20:36.485226','2026-05-15 04:18:07.479709',0.000000000000000000000000000000,120000.000000000000000000000000000000),(29,39,'INV-20260515-0039',1440000.000000000000000000000000000000,'Paid','Cash','ok','2026-05-15 04:21:47.690818','2026-05-15 04:19:02.857737',0.000000000000000000000000000000,1440000.000000000000000000000000000000),(30,40,'INV-20260515-0040',216000.000000000000000000000000000000,'Paid','Cash','ok','2026-05-15 04:24:51.302745','2026-05-15 04:24:14.486028',0.000000000000000000000000000000,216000.000000000000000000000000000000),(31,41,'INV-20260515-0041',2835000.000000000000000000000000000000,'Paid','BankTransfer','No','2026-05-15 04:33:07.912739','2026-05-15 04:32:24.761715',0.000000000000000000000000000000,2835000.000000000000000000000000000000),(32,42,'INV-20260515-0042',600000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-15 04:36:33.578443','2026-05-15 04:36:05.917531',0.000000000000000000000000000000,600000.000000000000000000000000000000),(33,43,'INV-20260515-0043',114000.000000000000000000000000000000,'Paid','Cash','r','2026-05-15 04:43:09.421608','2026-05-15 04:42:46.520620',0.000000000000000000000000000000,114000.000000000000000000000000000000),(34,44,'INV-20260515-0044',360000.000000000000000000000000000000,'Paid','Cash',NULL,'2026-05-15 12:42:16.126422','2026-05-15 12:13:35.504815',0.000000000000000000000000000000,360000.000000000000000000000000000000),(35,45,'INV-20260515-0045',630000.000000000000000000000000000000,'Paid','Cash','ok','2026-05-15 13:08:50.820236','2026-05-15 13:08:09.911343',0.000000000000000000000000000000,630000.000000000000000000000000000000),(36,46,'INV-20260515-0046',120000.000000000000000000000000000000,'Paid','BankTransfer',NULL,'2026-05-15 14:41:23.738702','2026-05-15 13:11:49.393681',0.000000000000000000000000000000,120000.000000000000000000000000000000),(37,47,'INV-20260515-0047',150000.000000000000000000000000000000,'Paid','BankTransfer','ok','2026-05-15 14:29:34.727695','2026-05-15 14:23:46.047297',0.000000000000000000000000000000,150000.000000000000000000000000000000),(38,48,'INV-20260515-0048',540000.000000000000000000000000000000,'Paid','Cash','ok','2026-05-15 14:30:26.327154','2026-05-15 14:27:50.917538',0.000000000000000000000000000000,540000.000000000000000000000000000000),(39,50,'INV-20260515-0050',1396500.000000000000000000000000000000,'Paid','BankTransfer','Bệnh nhân thanh toán','2026-05-15 14:45:23.543408','2026-05-15 14:44:55.745503',0.000000000000000000000000000000,1396500.000000000000000000000000000000);
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
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prescriptions`
--

LOCK TABLES `prescriptions` WRITE;
/*!40000 ALTER TABLE `prescriptions` DISABLE KEYS */;
INSERT INTO `prescriptions` VALUES (5,4,15,7,'Uống sau bữa ăn',0,0.000000000000000000000000000000,0.000000000000000000000000000000,0),(6,5,13,7,'Uống sau bữa ăn',0,0.000000000000000000000000000000,0.000000000000000000000000000000,0),(7,6,15,7,'Uống sau bữa ăn',21,20000.000000000000000000000000000000,1.000000000000000000000000000000,3),(8,6,4,7,NULL,42,15000.000000000000000000000000000000,2.000000000000000000000000000000,3),(9,7,13,7,'Uống sau bữa ăn',42,6000.000000000000000000000000000000,2.000000000000000000000000000000,3),(17,15,9,7,'Uống sau bữa ăn',42,50000.000000000000000000000000000000,2.000000000000000000000000000000,3),(26,24,8,6,'Uống sau bữa ăn',36,10000.000000000000000000000000000000,2.000000000000000000000000000000,3),(27,25,4,7,'Uống sau bữa ăn',42,15000.000000000000000000000000000000,2.000000000000000000000000000000,3),(28,26,4,2,'Uống sau bữa ăn',8,15000.000000000000000000000000000000,2.000000000000000000000000000000,2),(29,27,8,5,'Uống sau bữa ăn',15,10000.000000000000000000000000000000,1.000000000000000000000000000000,3),(30,28,34,3,'Uống sau bữa ăn',12,45000.000000000000000000000000000000,2.000000000000000000000000000000,2);
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reviews`
--

LOCK TABLES `reviews` WRITE;
/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
INSERT INTO `reviews` VALUES (1,5,2,2,4,'ok','2026-05-10 15:55:36.349476'),(2,33,4,3,5,'ok','2026-05-15 01:01:49.278102'),(3,42,4,2,5,NULL,'2026-05-15 10:32:09.126089'),(4,48,5,4,5,'Tận tình','2026-05-15 15:23:55.070786');
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
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'string','user@example.com','$2a$12$gy/Cv8WoLAy5Op3F28Q9GOdOp2jQ0WS6wvwQwusEDg6eQehQOsB0S','string','Patient',1,'2026-05-07 12:19:29.041070'),(5,'Admin','admin@gmail.com','$2a$11$7WufvF/xyYRB2PAE9HOGLedB1IIVHBy4ZxxtDvjYO/vDH1sgvKI4e','0987654321','Admin',1,'2026-05-07 14:51:39.594877'),(6,'khoaanh','anhkhoa@gmail.com','$2a$12$Cz/tgZw4IL6XU/yjVzZoK.QJ24SxdPGyU2cyrJZ1c14/v4bYpCw1K','0123456789','Patient',1,'2026-05-07 15:28:52.489903'),(7,'taotenkhoa','benhnhan@gmail.com','$2a$12$bs1R7UYuUk5VycIM4fn4OeIE3TpGFPruJ5lxXVkz.h.kr/sVvP4xe','0123456789','Patient',1,'2026-05-08 01:12:55.719525'),(8,'HoangLich','hoanglich@gmail.com','$2a$12$/cu01CcRQyzYHebphzHoKuPGQdBUWF18h5.DqRV.8YNbPfJYt1SZe','099887766','Patient',1,'2026-05-08 04:14:24.727859'),(9,'Quy Nhơn','quynhon@gmail.com','$2a$12$xowgms.cOdKePJ69Mc0av.HtUERHFSPM8rE9dtwzwA1xSCj3pt24C','+84357937045','Patient',1,'2026-05-09 14:23:44.274488'),(10,'Vũ Sơn Lâm','doctor@gmail.com','$2a$12$DeguiOfARv6tvqieTC/k/.1lAvVZ/H3muZu53M3djDyZcJ0wrNjV6','0123456789','Doctor',1,'2026-05-09 15:03:57.382210'),(11,'105-119 Khoa Lê Anh','anhkhoale2406@gmail.com','$2a$12$LdVXd.AdvN/GL3zHI9GI3ueo559PM0k4I3m7G1hilwl5SVArCwYWS','06655443322','Patient',1,'2026-05-09 15:53:08.030326'),(12,'Anh Khoa','levanthanhcathiep@gmail.com','$2a$12$LgY.TEGECiQWagcv63PYfOZvRVjAUXjMDiIhe8E363i8lLz99mpxe','03579753135','Patient',1,'2026-05-10 03:18:19.443508'),(13,'Lịch Bùi Hoàng','lichhoang505@gmail.com','$2a$12$InLybiZEYheQDcpmX1XmTOT5x.CTTlWygeaaEd7N5xSrcsLDAQJOK','','Patient',1,'2026-05-10 03:36:03.475271'),(14,'Anh Khoa Lê','anhkhoa24605@gmail.com','$2a$12$DH7vyectlUYXo8zVe4z17.2dzmLGfw4Ma8f7kV0uMGyvzONDJKFhO','06543678923','Patient',1,'2026-05-10 11:41:53.042724'),(15,'Nguyễn Hữu Tài','doctor1@gmail.com','$2a$12$jxnIoN2W22rmbMDtq3hJBuJVz1SmRX6eB9Wq7PMVmKpJKJTAV9sDC','0987654321','Doctor',1,'2026-05-11 03:51:54.973111'),(16,'phuc van','vphuc11052005@gmail.com','$2a$12$9qHZ/uD8Ak8rfUS7dnI6UeAH0hZipnrFR/Rxwn6Jq5IHScrJFhTg.','','Patient',1,'2026-05-13 06:47:35.196352'),(17,'Nguyễn Thị Loan','doctor4@gmail.com','$2a$12$vHUCKgCmf9.zHPrxZauiu.bkVncLjnYz/egIlGqR2wcvJq7oeQ3iS','03579753135','Doctor',1,'2026-05-15 10:54:33.377886'),(18,'SoundCloud Music','vo2449199@gmail.com','$2a$12$k0N72k7genpr1TNjzEsP.OUiQVwOddzPPlSwgOxa9tEWT1vwoIDB6','','Patient',1,'2026-05-15 14:32:44.381863');
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
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workingschedules`
--

LOCK TABLES `workingschedules` WRITE;
/*!40000 ALTER TABLE `workingschedules` DISABLE KEYS */;
INSERT INTO `workingschedules` VALUES (2,2,1,'15:47:00.000000','17:48:00.000000',30,10,1),(5,2,0,'18:45:00.000000','22:45:00.000000',20,5,1),(7,3,2,'08:00:00.000000','17:00:00.000000',45,12,1),(8,3,1,'07:00:00.000000','17:00:00.000000',20,30,1),(9,2,3,'13:00:00.000000','20:00:00.000000',30,14,1),(10,2,5,'08:00:00.000000','18:00:00.000000',30,20,1),(12,4,5,'08:00:00.000000','18:00:00.000000',45,13,1);
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

-- Dump completed on 2026-05-15 22:34:32

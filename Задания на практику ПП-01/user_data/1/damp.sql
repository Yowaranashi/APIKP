-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: localhost    Database: db_bsn_user1
-- ------------------------------------------------------
-- Server version	8.0.29

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `competition`
--

DROP TABLE IF EXISTS `competition`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `competition` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(1000) NOT NULL,
  `date_start` date NOT NULL,
  `date_end` date NOT NULL,
  `description` varchar(10000) NOT NULL,
  `city` varchar(50) DEFAULT NULL,
  `skill_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `skill_fk` (`skill_id`),
  CONSTRAINT `skill_fk` FOREIGN KEY (`skill_id`) REFERENCES `skill` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `competition`
--

LOCK TABLES `competition` WRITE;
/*!40000 ALTER TABLE `competition` DISABLE KEYS */;
INSERT INTO `competition` VALUES (1,'Мировой чемпионат по проф мастерству WorldSkills Abu Dhabi 2017','2023-05-22','2023-05-26','типо описание','Абу-Даби',1),(2,'Национального чемпионата «Молодые профессионалы» (WorldSkills Russia) 2017 в Краснодаре','2023-05-22','2023-05-26','типо описание2','Краснодар',23);
/*!40000 ALTER TABLE `competition` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `region`
--

DROP TABLE IF EXISTS `region`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `region` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `region`
--

LOCK TABLES `region` WRITE;
/*!40000 ALTER TABLE `region` DISABLE KEYS */;
/*!40000 ALTER TABLE `region` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `regions`
--

DROP TABLE IF EXISTS `regions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `regions` (
  `код` int NOT NULL AUTO_INCREMENT,
  `столица` varchar(20) NOT NULL,
  `округ` varchar(30) NOT NULL,
  `название_региона` varchar(30) NOT NULL,
  PRIMARY KEY (`код`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `regions`
--

LOCK TABLES `regions` WRITE;
/*!40000 ALTER TABLE `regions` DISABLE KEYS */;
INSERT INTO `regions` VALUES (1,'Майкоп','Южный','Республика Адыгея'),(2,'Уфа','Приволжский','Республика Башкортостан'),(3,'Улан-Удэ','Сибирский','Республика Бурятия'),(4,'Горно-Алтайск','Сибирский','Республика Алтай'),(5,'Махачкала','Северо-Кавказский','Республика Дагестан');
/*!40000 ALTER TABLE `regions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `id` int NOT NULL,
  `role` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (1,'Участник'),(2,'Эксперт'),(3,'Главный эксперт'),(4,'Заместитель главного эксперта'),(5,'Технический эксперт'),(6,'Организатор');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `skill`
--

DROP TABLE IF EXISTS `skill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `skill` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(30) NOT NULL,
  `skill_block_id` int NOT NULL,
  `description` varchar(10000) NOT NULL,
  `try_a_skill_square` int NOT NULL,
  `work_square` int NOT NULL,
  `expert_square` int NOT NULL,
  `briffing_square` int NOT NULL,
  `square_sclade` int NOT NULL,
  `count_of_players` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `skill_block_fk` (`skill_block_id`),
  CONSTRAINT `skill_block_fk` FOREIGN KEY (`skill_block_id`) REFERENCES `skill_block` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `skill`
--

LOCK TABLES `skill` WRITE;
/*!40000 ALTER TABLE `skill` DISABLE KEYS */;
INSERT INTO `skill` VALUES (1,'Изготовление прототипов',3,'<p>Прототипирование &ndash; это создание опытных образцов или работающей модели с целью проверки возможности реализации проекта для демонстрации модели заказчику. Прототип &ndash; модель изделия создается в соответствии с проектными критериями и характеристиками изделия перед этапом его запуска в массовое производство. Окончательный проект определяется тестированием и изучением функциональности и модификаций изготовленной модели &ndash; прототипа. Основной сферой применения технологии прототипирования является промышленность.</p>\n<p>Специалист по прототипированию занимается разработкой, созданием, испытанием и модификацией прототипов. Он должен обладать умением твердотельного моделирования, CAD моделирования, иметь понимание технических деталей, уметь читать чертежи, обладать знанием программы для создания трехмерных моделей, создания трехмерного проекта модели, уметь работать с 3D-принтером, станками с ПУ, 3D-сканером, ручным электроинструментом. Специалист должен иметь навыки слесарной обработки: разметка, опиливание, сверление, нарезание резьбы, притирка, доводка, пригонка собираемых деталей в узлы с последующей регулировкой и проверкой работы конструкции, а также навыки по покраске изделий и дизайнерскому решению. Он осуществляет проверку функциональности и выполнение технических требований к изделию.</p>\n<p><strong>&nbsp;</strong></p>\n<p><strong>Название ENG: Prototype Modelling </strong></p>\n<p><strong>Статус компетенции: Основная </strong></p>\n<p><strong>Тип: WSI </strong></p>\n<p><strong>№ CIS: 45 </strong></p>\n<p><strong>Количество конкурсантов в команде: 1</strong></p>',10,2,7,37,6,1),(23,'Мобильные приложения',1,'<p>Смартфоны продаются. Компании всё больше вкладывают в развитие мобильных технологий, благодаря чему потребители из просто пользователей плавно превращаются в тех, кто использует мобильные устройства везде и всегда: на работе, дома, на отдыхе, что приводит к важным последствиям для компаний-разработчиков мобильных приложений. Спрос на mobile-программистов, способных выполнять множество профессиональных задач, растёт высокими темпами, а подходящих профессионалов на рынке сейчас мало. Основная задача специалиста по разработке мобильных приложений: создание мобильного приложения, сочетающего в себе такие обязательные качества, как безотказная работа на одной из мобильных операционных платформ, удобный пользовательский интерфейс, многофункциональность.</p>\n<p><strong>Название</strong><strong> ENG: Mobile application development </strong></p>\n<p><strong>Статус компетенции: Основная </strong></p>\n<p><strong>Тип: RU </strong></p>\n<p><strong>№ CIS: F6 </strong></p>\n<p><strong>Количество конкурсантов в команде: 1</strong></p>',3,2,8,38,7,3);
/*!40000 ALTER TABLE `skill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `skill_block`
--

DROP TABLE IF EXISTS `skill_block`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `skill_block` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `skill_block`
--

LOCK TABLES `skill_block` WRITE;
/*!40000 ALTER TABLE `skill_block` DISABLE KEYS */;
INSERT INTO `skill_block` VALUES (1,'Информационные и коммуникационные технологии'),(2,'Образование'),(3,'Производство и инженерные технологии'),(4,'Строительство и строительные технологии'),(5,'Сфера услуг'),(6,'Творчество и дизайн'),(7,'Транспорт и логистика'),(8,'Future Skills');
/*!40000 ALTER TABLE `skill_block` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `fio` varchar(50) NOT NULL,
  `gender` varchar(1) NOT NULL,
  `email` varchar(200) NOT NULL,
  `phone_number` varchar(20) NOT NULL,
  `date_of_birth` date NOT NULL,
  `password` varchar(20) NOT NULL,
  `pin` int NOT NULL,
  `role_id` int DEFAULT NULL,
  `skill_id` int DEFAULT NULL,
  `region_id` int DEFAULT NULL,
  `place` int DEFAULT NULL,
  `competition_id` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `password` (`password`),
  UNIQUE KEY `pin` (`pin`),
  KEY `fk_skill` (`skill_id`),
  KEY `fk_competition` (`competition_id`),
  KEY `fk_region` (`region_id`),
  KEY `fk_role` (`role_id`),
  CONSTRAINT `fk_competition` FOREIGN KEY (`competition_id`) REFERENCES `competition` (`id`) ON DELETE SET NULL,
  CONSTRAINT `fk_region` FOREIGN KEY (`region_id`) REFERENCES `regions` (`код`) ON DELETE SET NULL,
  CONSTRAINT `fk_role` FOREIGN KEY (`role_id`) REFERENCES `role` (`id`) ON DELETE SET NULL,
  CONSTRAINT `fk_skill` FOREIGN KEY (`skill_id`) REFERENCES `skill` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'Пересторонин Михаил Онисимович','М','miha@mai.ru','+79119153412','2000-11-21','goodpas',12345,1,1,3,NULL,1),(2,'Евгений Евгеньев Евгениевич','М','evg@mai.ru','+79217154412','1987-06-03','verygoodpas',12344,3,1,3,NULL,1);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-05-25 17:33:32

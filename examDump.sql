-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: dbshopboots
-- ------------------------------------------------------
-- Server version	8.0.39

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
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'Мужская обувь'),(2,'Женская обувь');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `images`
--

DROP TABLE IF EXISTS `images`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `images` (
  `id` int NOT NULL AUTO_INCREMENT,
  `image` varchar(255) DEFAULT NULL,
  `product_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_images_product_idx` (`product_id`),
  CONSTRAINT `fk_images_product` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `images`
--

LOCK TABLES `images` WRITE;
/*!40000 ALTER TABLE `images` DISABLE KEYS */;
INSERT INTO `images` VALUES (1,'1.jpg',1),(2,'2.jpg',2),(3,'3.jpg',3),(4,'4.jpg',4),(5,'5.jpg',5),(6,'6.jpg',6),(7,'7.jpg',7),(8,'8.jpg',8),(9,'9.jpg',9),(10,'10.jpg',10);
/*!40000 ALTER TABLE `images` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `manufacturers`
--

DROP TABLE IF EXISTS `manufacturers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `manufacturers` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manufacturers`
--

LOCK TABLES `manufacturers` WRITE;
/*!40000 ALTER TABLE `manufacturers` DISABLE KEYS */;
INSERT INTO `manufacturers` VALUES (1,'Kari'),(2,'Marco Tozzi'),(3,'Рос'),(4,'Rieker'),(5,'Alessio Nesca'),(6,'CROSBY');
/*!40000 ALTER TABLE `manufacturers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order_details`
--

DROP TABLE IF EXISTS `order_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `order_details` (
  `id` int NOT NULL AUTO_INCREMENT,
  `order_id` int NOT NULL,
  `product_id` int NOT NULL,
  `count` int NOT NULL,
  `price` double NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_order_details_product_idx` (`product_id`),
  KEY `fk_order_details_order_idx` (`order_id`),
  CONSTRAINT `fk_order_details_order` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`),
  CONSTRAINT `fk_order_details_product` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order_details`
--

LOCK TABLES `order_details` WRITE;
/*!40000 ALTER TABLE `order_details` DISABLE KEYS */;
INSERT INTO `order_details` VALUES (1,1,2,1,4990),(2,1,2,1,4990),(3,2,1,2,3244),(4,2,1,2,3244),(5,3,10,3,4499),(6,3,10,3,4499);
/*!40000 ALTER TABLE `order_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `id` int NOT NULL AUTO_INCREMENT,
  `date_order` date NOT NULL,
  `date_delivery` date NOT NULL,
  `pickup_point_id` int NOT NULL,
  `user_id` int NOT NULL,
  `code` int NOT NULL,
  `status_order` varchar(45) NOT NULL DEFAULT 'Оформлен',
  PRIMARY KEY (`id`),
  KEY `fk_orders_user_idx` (`user_id`),
  KEY `fk_orders_pickup_point_idx` (`pickup_point_id`),
  CONSTRAINT `fk_orders_pickup_point` FOREIGN KEY (`pickup_point_id`) REFERENCES `pickup_points` (`id`),
  CONSTRAINT `fk_orders_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,'2025-02-27','2025-04-20',1,4,901,'Завершен'),(2,'2022-09-28','2025-04-21',11,1,902,'Завершен'),(3,'2025-03-21','2025-04-22',2,2,903,'Завершен'),(4,'2025-02-20','2025-04-23',11,3,904,'Завершен'),(5,'2025-03-17','2025-04-24',2,4,905,'Завершен'),(6,'2025-03-01','2025-04-25',15,1,906,'Завершен'),(7,'2025-03-01','2025-04-26',3,2,907,'Завершен'),(8,'2025-03-31','2025-04-27',19,3,908,'Новый'),(9,'2025-04-02','2025-04-28',5,4,909,'Новый'),(10,'2025-04-03','2025-04-29',19,4,910,'Новый');
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pickup_points`
--

DROP TABLE IF EXISTS `pickup_points`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pickup_points` (
  `id` int NOT NULL AUTO_INCREMENT,
  `address_index` varchar(45) NOT NULL,
  `address_city` varchar(100) NOT NULL,
  `address_street` varchar(100) NOT NULL,
  `address_number_house` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pickup_points`
--

LOCK TABLES `pickup_points` WRITE;
/*!40000 ALTER TABLE `pickup_points` DISABLE KEYS */;
INSERT INTO `pickup_points` VALUES (1,'420151','г. Лесной','ул. Вишневая','32'),(2,'125061','г. Лесной','ул. Подгорная','8'),(3,'630370','г. Лесной','ул. Шоссейная','24'),(4,'400562','г. Лесной','ул. Зеленая','32'),(5,'614510','г. Лесной','ул. Маяковского','47'),(6,'410542','г. Лесной','ул. Светлая','46'),(7,'620839','г. Лесной','ул. Цветочная','8'),(8,'443890','г. Лесной','ул. Коммунистическая','1'),(9,'603379','г. Лесной','ул. Спортивная','46'),(10,'603721','г. Лесной','ул. Гоголя','41'),(11,'410172','г. Лесной','ул. Северная','13'),(12,'614611','г. Лесной','ул. Молодежная','50'),(13,'454311','г.Лесной','ул. Новая','19'),(14,'660007','г.Лесной','ул. Октябрьская','19'),(15,'603036','г. Лесной','ул. Садовая','4'),(16,'394060','г.Лесной','ул. Фрунзе','43'),(17,'410661','г. Лесной','ул. Школьная','50'),(18,'625590','г. Лесной','ул. Коммунистическая','20'),(19,'625683','г. Лесной','ул. 8 Марта',''),(20,'450983','г.Лесной','ул. Комсомольская','26'),(21,'394782','г. Лесной','ул. Чехова','3'),(22,'603002','г. Лесной','ул. Дзержинского','28'),(23,'450558','г. Лесной','ул. Набережная','30'),(24,'344288','г. Лесной','ул. Чехова','1'),(25,'614164','г.Лесной','  ул. Степная','30'),(26,'394242','г. Лесной','ул. Коммунистическая','43'),(27,'660540','г. Лесной','ул. Солнечная','25'),(28,'125837','г. Лесной','ул. Шоссейная','40'),(29,'125703','г. Лесной','ул. Партизанская','49'),(30,'625283','г. Лесной','ул. Победы','46'),(31,'614753','г. Лесной','ул. Полевая','35'),(32,'426030','г. Лесной','ул. Маяковского','44'),(33,'450375','г. Лесной','ул. Клубная','44'),(34,'625560','г. Лесной','ул. Некрасова','12'),(35,'630201','г. Лесной','ул. Комсомольская','17'),(36,'190949','г. Лесной','ул. Мичурина','26');
/*!40000 ALTER TABLE `pickup_points` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `price` double NOT NULL,
  `supplier_id` int NOT NULL,
  `manufacture_id` int NOT NULL,
  `category_id` int NOT NULL,
  `discount` int NOT NULL DEFAULT '0',
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_products_category_idx` (`category_id`),
  KEY `fk_products_supplier_idx` (`supplier_id`),
  KEY `fk_products_manufacturers_idx` (`manufacture_id`),
  CONSTRAINT `fk_products_category` FOREIGN KEY (`category_id`) REFERENCES `categories` (`id`),
  CONSTRAINT `fk_products_manufacturer` FOREIGN KEY (`manufacture_id`) REFERENCES `manufacturers` (`id`),
  CONSTRAINT `fk_products_supplier` FOREIGN KEY (`supplier_id`) REFERENCES `suppliers` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,'Ботинки',4990,1,1,2,3,'Женские Ботинки демисезонные kari'),(2,'Ботинки',3244,2,2,2,2,'Ботинки Marco Tozzi женские демисезонные, размер 39, цвет бежевый'),(3,'Туфли',4499,1,1,1,4,'Туфли kari мужские классика MYZ21AW-450A, размер 43, цвет: черный'),(4,'Ботинки',5900,1,3,1,2,'Мужские ботинки Рос-Обувь кожаные с натуральным мехом'),(5,'Ботинки',3800,2,4,1,2,'B3430/14 Полуботинки мужские Rieker'),(6,'Кроссовки',4100,2,3,1,3,'129615-4 Кроссовки мужские'),(7,'Туфли',2700,1,2,2,2,'Туфли Marco Tozzi женские летние, размер 39, цвет черный'),(8,'Полуботинки',1890,2,5,2,4,'Полуботинки Alessio Nesca женские 3-30797-47, размер 37, цвет: бордовый'),(9,'Туфли',4300,1,4,2,2,'Туфли Rieker женские демисезонные, размер 41, цвет коричневый'),(10,'Туфли',2800,1,1,2,3,'Туфли kari женские TR-YR-413017, размер 37, цвет: черный'),(11,'Полуботинки',2156,2,6,1,3,'407700/01-01 Полуботинки мужские CROSBY'),(12,'Полуботинки',1800,1,1,2,2,'Полуботинки kari женские MYZ20S-149, размер 41, цвет: черный'),(13,'Кеды',5500,2,6,1,3,'Кеды Caprice мужские демисезонные, размер 42, цвет черный'),(14,'Полуботинки',2100,2,6,1,2,'407700/01-02 Полуботинки мужские CROSBY'),(15,'Туфли',5400,2,4,2,4,'Туфли женские демисезонные Rieker артикул 55073-68/37'),(16,'Ботинки',6600,1,1,2,12,'Ботинки женские зимние ROMER арт. 893167-01 Черный'),(17,'Тапочки',500,1,1,1,13,'Тапочки мужские Арт.70701-55-67син р.41'),(18,'Ботинки',2700,2,4,1,2,'Мужские кожаные ботинки/мужские ботинки'),(19,'Туфли',6800,1,6,2,15,'Туфли женские, ARGO, размер 38'),(20,'Ботинки',10200,1,5,2,15,'Ботинки женские, ARGO, размер 40'),(21,'Ботинки',11800,2,4,2,15,'Ботинки на молнии с декоративной пряжкой FRAU'),(22,'Полуботинки',4600,1,6,2,3,'Полуботинки Ботинки черные зимние, мех'),(23,'Туфли',12400,1,1,2,16,'Туфли Luiza Belly женские Kate-lazo черные из натуральной замши'),(24,'Тапочки',9900,2,6,1,17,'Мужские кожаные тапочки \"Профиль С.Дали\" '),(25,'Полуботинки',1700,1,1,2,2,'Полуботинки kari женские WB2020SS-26, размер 38, цвет: черный'),(26,'Кроссовки',2800,2,4,1,18,'Кроссовки мужские TOFA'),(27,'Туфли',4399,2,4,2,3,'Туфли Rieker женские демисезонные, размер 36, цвет коричневый'),(28,'Сапоги',4699,1,6,2,2,'Сапоги замша Цвет: синий'),(29,'Тапочки',599,1,4,1,20,'Тапочки мужские син р.41'),(30,'Ботинки',2300,2,4,2,2,'Женские Ботинки демисезонные');
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `storages`
--

DROP TABLE IF EXISTS `storages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `storages` (
  `id` int NOT NULL AUTO_INCREMENT,
  `product_id` int NOT NULL,
  `count` int NOT NULL,
  `unit` enum('шт.') NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_storages_product_idx` (`product_id`),
  CONSTRAINT `fk_storages_product` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `storages`
--

LOCK TABLES `storages` WRITE;
/*!40000 ALTER TABLE `storages` DISABLE KEYS */;
INSERT INTO `storages` VALUES (1,1,6,'шт.'),(2,2,13,'шт.'),(3,3,5,'шт.'),(4,4,8,'шт.'),(5,5,16,'шт.'),(6,6,6,'шт.'),(7,7,14,'шт.'),(8,8,4,'шт.'),(9,9,6,'шт.'),(10,10,15,'шт.'),(11,11,6,'шт.'),(12,12,14,'шт.'),(13,13,0,'шт.'),(14,14,3,'шт.'),(15,15,18,'шт.'),(16,16,9,'шт.'),(17,17,0,'шт.'),(18,18,5,'шт.'),(19,19,15,'шт.'),(20,20,9,'шт.'),(21,21,11,'шт.'),(22,22,13,'шт.'),(23,23,5,'шт.'),(24,24,15,'шт.'),(25,25,7,'шт.'),(26,26,3,'шт.'),(27,27,12,'шт.'),(28,28,5,'шт.'),(29,29,2,'шт.'),(30,30,7,'шт.');
/*!40000 ALTER TABLE `storages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `suppliers`
--

DROP TABLE IF EXISTS `suppliers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suppliers` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `suppliers`
--

LOCK TABLES `suppliers` WRITE;
/*!40000 ALTER TABLE `suppliers` DISABLE KEYS */;
INSERT INTO `suppliers` VALUES (1,'Kari'),(2,'Обувь для вас');
/*!40000 ALTER TABLE `suppliers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `lastname` varchar(100) NOT NULL,
  `name` varchar(100) NOT NULL,
  `midname` varchar(100) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `role` enum('администратор','менеджер','клиент') NOT NULL DEFAULT 'клиент',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'Никифорова','Весения','Николаевна','94d5ous@gmail.com','uzWC67','администратор'),(2,'Сазонов','Руслан','Германович','uth4iz@mail.com','2L6KZG','администратор'),(3,'Одинцов','Серафим','Артёмович','yzls62@outlook.com','JlFRCZ','администратор'),(4,'Степанов','Михаил','Артёмович','1diph5e@tutanota.com','8ntwUp','менеджер'),(5,'Ворсин','Петр','Евгеньевич','tjde7c@yahoo.com','YOyhfR','менеджер'),(6,'Старикова','Елена','Павловна','wpmrc3do@tutanota.com','RSbvHv','менеджер'),(7,'Михайлюк','Анна','Вячеславовна','5d4zbu@tutanota.com','rwVDh9','клиент'),(8,'Ситдикова','Елена','Анатольевна','ptec8ym@yahoo.com','LdNyos','клиент'),(9,'Ворсин','Петр','Евгеньевич','1qz4kw@mail.com','gynQMT','клиент'),(10,'Старикова','Елена','Павловна','4np6se@mail.com','AtnDjr','клиент');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-01-18 21:42:53

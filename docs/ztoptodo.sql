-- --------------------------------------------------------
-- 主机:                           10.22.102.90
-- 服务器版本:                        5.1.73-community - MySQL Community Server (GPL)
-- 服务器操作系统:                      Win32
-- HeidiSQL 版本:                  9.3.0.4984
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 导出 ztoptodo 的数据库结构
CREATE DATABASE IF NOT EXISTS `ztoptodo` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ztoptodo`;


-- 导出  表 ztoptodo.aggregations 结构
CREATE TABLE IF NOT EXISTS `aggregations` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Year` int(11) NOT NULL DEFAULT '0',
  `Month` int(11) NOT NULL DEFAULT '0',
  `SubstalEE` double NOT NULL DEFAULT '0',
  `SubstalIE` double NOT NULL DEFAULT '0',
  `SubstalEP` double NOT NULL DEFAULT '0',
  `SubstalIP` double NOT NULL DEFAULT '0',
  `Time` datetime NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.aggregations 的数据：~0 rows (大约)
DELETE FROM `aggregations`;
/*!40000 ALTER TABLE `aggregations` DISABLE KEYS */;
/*!40000 ALTER TABLE `aggregations` ENABLE KEYS */;


-- 导出  表 ztoptodo.attachment 结构
CREATE TABLE IF NOT EXISTS `attachment` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FileName` varchar(128) DEFAULT NULL,
  `TaskID` int(11) DEFAULT NULL,
  `FileSize` int(11) DEFAULT NULL,
  `SavePath` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `TaskID` (`TaskID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.attachment 的数据：~0 rows (大约)
DELETE FROM `attachment`;
/*!40000 ALTER TABLE `attachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `attachment` ENABLE KEYS */;


-- 导出  表 ztoptodo.authorizes 结构
CREATE TABLE IF NOT EXISTS `authorizes` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `GroupName` varchar(1023) DEFAULT NULL,
  `Manager` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.authorizes 的数据：~0 rows (大约)
DELETE FROM `authorizes`;
/*!40000 ALTER TABLE `authorizes` DISABLE KEYS */;
/*!40000 ALTER TABLE `authorizes` ENABLE KEYS */;


-- 导出  表 ztoptodo.banks 结构
CREATE TABLE IF NOT EXISTS `banks` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Year` int(11) NOT NULL DEFAULT '0',
  `Month` int(11) NOT NULL DEFAULT '0',
  `Company` bit(3) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.banks 的数据：~3 rows (大约)
DELETE FROM `banks`;
/*!40000 ALTER TABLE `banks` DISABLE KEYS */;
INSERT INTO `banks` (`ID`, `Year`, `Month`, `Company`) VALUES
	(1, 2016, 4, b'000'),
	(2, 2016, 4, b'001'),
	(3, 2016, 1, b'000');
/*!40000 ALTER TABLE `banks` ENABLE KEYS */;


-- 导出  表 ztoptodo.bills 结构
CREATE TABLE IF NOT EXISTS `bills` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Coding` varchar(255) NOT NULL DEFAULT '0',
  `Time` datetime DEFAULT NULL,
  `Money` double NOT NULL DEFAULT '0',
  `Account` varchar(1023) NOT NULL DEFAULT '0',
  `Budget` bit(3) NOT NULL DEFAULT b'0',
  `Cost` bit(3) NOT NULL DEFAULT b'0',
  `Summary` varchar(1023) NOT NULL DEFAULT '0',
  `BID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`),
  KEY `BID` (`BID`)
) ENGINE=InnoDB AUTO_INCREMENT=138 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.bills 的数据：~41 rows (大约)
DELETE FROM `bills`;
/*!40000 ALTER TABLE `bills` DISABLE KEYS */;
INSERT INTO `bills` (`ID`, `Coding`, `Time`, `Money`, `Account`, `Budget`, `Cost`, `Summary`, `BID`) VALUES
	(80, '1', '2016-05-03 00:00:00', 5000, '6222', b'000', b'111', '海盐县平南路地块改造', 2),
	(81, '2', '2016-05-04 00:00:00', 6000, '6222', b'000', b'111', '定海岑港大涂面土地编造', 2),
	(82, '3', '2016-05-05 00:00:00', 24162, '6222', b'001', b'111', '杭州多邦办公设备', 2),
	(83, '4', '2016-05-04 00:00:00', 4000, '2222', b'000', b'111', '舟山污水处理公司方案', 2),
	(84, '5', '2017-01-06 00:00:00', 6000, '22232', b'000', b'111', '舟山国际粮油集散方案', 2),
	(85, '6', '2016-05-07 00:00:00', 4000, '2222', b'000', b'111', '舟山定海西北临港投资公司', 2),
	(86, '7', '2016-05-11 00:00:00', 4000, '2111', b'000', b'111', '舟山定海双桥农居小区配套道路', 2),
	(87, '8', '2016-05-12 00:00:00', 12000, '6222', b'000', b'111', '定海马桥标准农田置换方案', 2),
	(88, '9', '2016-05-10 00:00:00', 2500, '6122', b'000', b'101', '德清县公共资源', 2),
	(89, '10', '2016-05-11 00:00:00', 12000, '6236236', b'000', b'101', '龙游县公共资源退保证金', 2),
	(90, '11', '2016-05-02 00:00:00', 4324.5, '622', b'001', b'111', '杭州绿墅电费', 2),
	(91, '12', '2016-08-05 00:00:00', 23450.2, '6222', b'001', b'111', '杭州绿墅物管费', 2),
	(92, '13', '2016-05-10 00:00:00', 22956, '222', b'001', b'111', '浙江久加久', 2),
	(93, '14', '2016-05-10 00:00:00', 23750, '6222', b'001', b'101', '临海永久基本农田规划履约金', 2),
	(94, '15', '2016-05-12 00:00:00', 2000, '6222', b'001', b'101', '浙江成套招标代理', 2),
	(95, '16', '2016-05-03 00:00:00', 12000, '6222', b'001', b'101', '龙游县公共资源保证金', 2),
	(96, '17', '2016-05-05 00:00:00', 25, '33433', b'001', b'001', '杂费', 2),
	(97, '18', '2016-05-09 00:00:00', 125066.2, '6222', b'001', b'000', '53338.16备用', 2),
	(98, '19', '2016-05-04 00:00:00', 229435.16, '6222', b'001', b'010', '税', 2),
	(99, '20', '2016-05-07 00:00:00', 2577.61, '62111', b'001', b'011', '', 2),
	(100, '21', '2016-05-04 00:00:00', 150000, '34567', b'001', b'100', '', 2),
	(101, '123', '2016-01-01 00:00:00', 55000, '6222', b'000', b'111', '浦江县国土局', 3),
	(119, '1', '2016-05-12 00:00:00', 55000, '6226161', b'000', b'111', '浦江县国土局', 1),
	(120, '2', '2016-05-11 00:00:00', 140000, '6222', b'000', b'111', '绍兴县沪越制衣评估费', 1),
	(121, '3', '2016-05-10 00:00:00', 9000, '6222', b'000', b'101', '湖州公共资源退保证金', 1),
	(122, '4', '2016-05-09 00:00:00', 3000, '6222', b'000', b'101', '湖州公共资源退保证金', 1),
	(123, '5', '2016-05-08 00:00:00', 49000, '622', b'000', b'111', '平阳2015用地供应计划', 1),
	(124, '6', '2016-05-07 00:00:00', 40000, '6222', b'000', b'111', '秀洲区工业园区土地研究费', 1),
	(125, '7', '2016-05-06 00:00:00', 26865, '62222', b'000', b'111', '江区地产交易中心评估', 1),
	(126, '8', '2016-05-12 00:00:00', 210000, '6222', b'000', b'111', '秀洲区工业园区土地评估费', 1),
	(127, '9', '2016-09-10 00:00:00', 11550, '6222', b'001', b'111', '宁波恒康食品', 1),
	(128, '10', '2016-05-09 00:00:00', 3000, '6222', b'001', b'111', '浙江何晨信用评估', 1),
	(129, '11', '2016-05-09 00:00:00', 66338, '62222', b'001', b'111', '光楚图文设计', 1),
	(130, '12', '2016-05-10 00:00:00', 20000, '62222', b'001', b'111', '粤顺餐饮', 1),
	(131, '13', '2016-05-02 00:00:00', 350000, '62211', b'001', b'110', '杭州陆吾科技', 1),
	(132, '14', '2016-05-10 00:00:00', 25, '6222', b'001', b'001', '杂费', 1),
	(133, '15', '2016-05-12 00:00:00', 109989.95, '626262', b'001', b'000', '工资', 1),
	(134, '16', '2016-05-10 00:00:00', 142988.37, '33333', b'001', b'010', '纳税', 1),
	(135, '17', '2016-05-04 00:00:00', 5210, '63636', b'000', b'111', '开化县土地集约利用', 1),
	(136, '18', '2016-05-12 00:00:00', 160000, '6222', b'001', b'100', '备用金', 1),
	(137, '19', '2016-05-11 00:00:00', 216.56, '6266262', b'001', b'011', '', 1);
/*!40000 ALTER TABLE `bills` ENABLE KEYS */;


-- 导出  表 ztoptodo.collects 结构
CREATE TABLE IF NOT EXISTS `collects` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Summary` varchar(1023) NOT NULL DEFAULT '0',
  `Expenses` double NOT NULL DEFAULT '0',
  `Income` double NOT NULL DEFAULT '0',
  `Company` bit(3) NOT NULL DEFAULT b'0',
  `AID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`),
  KEY `AID` (`AID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.collects 的数据：~0 rows (大约)
DELETE FROM `collects`;
/*!40000 ALTER TABLE `collects` DISABLE KEYS */;
/*!40000 ALTER TABLE `collects` ENABLE KEYS */;


-- 导出  表 ztoptodo.comment 结构
CREATE TABLE IF NOT EXISTS `comment` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Content` varchar(50) NOT NULL,
  `UserID` int(11) NOT NULL,
  `TaskID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `TaskID` (`TaskID`),
  KEY `UserID` (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.comment 的数据：~0 rows (大约)
DELETE FROM `comment`;
/*!40000 ALTER TABLE `comment` DISABLE KEYS */;
/*!40000 ALTER TABLE `comment` ENABLE KEYS */;


-- 导出  表 ztoptodo.databooks 结构
CREATE TABLE IF NOT EXISTS `databooks` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `sAMAccountName` varchar(255) DEFAULT NULL,
  `GroupName` varchar(255) DEFAULT NULL,
  `CreateTime` datetime DEFAULT NULL,
  `Checker` varchar(255) DEFAULT NULL,
  `Status` int(2) DEFAULT NULL,
  `CheckTime` datetime DEFAULT NULL,
  `MaturityTime` datetime DEFAULT NULL,
  `Reason` varchar(255) DEFAULT NULL,
  `Label` bit(1) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.databooks 的数据：~0 rows (大约)
DELETE FROM `databooks`;
/*!40000 ALTER TABLE `databooks` DISABLE KEYS */;
/*!40000 ALTER TABLE `databooks` ENABLE KEYS */;


-- 导出  表 ztoptodo.errands 结构
CREATE TABLE IF NOT EXISTS `errands` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `StartTime` datetime NOT NULL,
  `EndTime` datetime NOT NULL,
  `Peoples` int(11) NOT NULL DEFAULT '0',
  `Days` int(11) NOT NULL DEFAULT '0',
  `EID` int(11) NOT NULL DEFAULT '0',
  `Users` varchar(1023) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.errands 的数据：~1 rows (大约)
DELETE FROM `errands`;
/*!40000 ALTER TABLE `errands` DISABLE KEYS */;
INSERT INTO `errands` (`ID`, `StartTime`, `EndTime`, `Peoples`, `Days`, `EID`, `Users`) VALUES
	(5, '2016-05-04 00:00:00', '2016-05-04 00:00:00', 1, 1, 1, '曾绍琴;');
/*!40000 ALTER TABLE `errands` ENABLE KEYS */;


-- 导出  表 ztoptodo.evections 结构
CREATE TABLE IF NOT EXISTS `evections` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Place` varchar(1023) DEFAULT NULL,
  `KiloMeters` double NOT NULL DEFAULT '0',
  `Traffic` double NOT NULL DEFAULT '0',
  `SubSidy` int(11) NOT NULL DEFAULT '0',
  `Hotel` double NOT NULL DEFAULT '0',
  `Mark` varchar(1023) DEFAULT NULL,
  `Other` double NOT NULL DEFAULT '0',
  `SID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.evections 的数据：~1 rows (大约)
DELETE FROM `evections`;
/*!40000 ALTER TABLE `evections` DISABLE KEYS */;
INSERT INTO `evections` (`ID`, `Place`, `KiloMeters`, `Traffic`, `SubSidy`, `Hotel`, `Mark`, `Other`, `SID`) VALUES
	(1, '浙江省嘉兴市局', 0, 363, 40, 0, NULL, 0, 1);
/*!40000 ALTER TABLE `evections` ENABLE KEYS */;


-- 导出  表 ztoptodo.gathers 结构
CREATE TABLE IF NOT EXISTS `gathers` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Company` bit(3) NOT NULL DEFAULT b'0',
  `Income` double NOT NULL DEFAULT '0',
  `MarginIncome` double NOT NULL DEFAULT '0',
  `Pay` double NOT NULL DEFAULT '0',
  `MarginPay` double NOT NULL DEFAULT '0',
  `Transfer` double NOT NULL DEFAULT '0',
  `Petty` double NOT NULL DEFAULT '0',
  `AID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`),
  KEY `AID` (`AID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.gathers 的数据：~0 rows (大约)
DELETE FROM `gathers`;
/*!40000 ALTER TABLE `gathers` DISABLE KEYS */;
/*!40000 ALTER TABLE `gathers` ENABLE KEYS */;


-- 导出  表 ztoptodo.messages 结构
CREATE TABLE IF NOT EXISTS `messages` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Sender` varchar(1023) DEFAULT NULL,
  `Info` varchar(1023) DEFAULT NULL,
  `Receiver` varchar(1023) DEFAULT NULL,
  `Check` bit(1) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `Receiver` (`Receiver`(255)),
  KEY `Sender` (`Sender`(255))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.messages 的数据：~0 rows (大约)
DELETE FROM `messages`;
/*!40000 ALTER TABLE `messages` DISABLE KEYS */;
/*!40000 ALTER TABLE `messages` ENABLE KEYS */;


-- 导出  表 ztoptodo.notification 结构
CREATE TABLE IF NOT EXISTS `notification` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ReceiverID` int(11) NOT NULL DEFAULT '0',
  `SenderID` int(11) NOT NULL DEFAULT '0',
  `InfoID` int(11) NOT NULL DEFAULT '0',
  `InfoType` int(11) NOT NULL DEFAULT '0',
  `HasRead` bit(1) NOT NULL DEFAULT b'0',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `UserID` (`ReceiverID`),
  KEY `CreateTime` (`CreateTime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.notification 的数据：~0 rows (大约)
DELETE FROM `notification`;
/*!40000 ALTER TABLE `notification` DISABLE KEYS */;
/*!40000 ALTER TABLE `notification` ENABLE KEYS */;


-- 导出  表 ztoptodo.records 结构
CREATE TABLE IF NOT EXISTS `records` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Time` datetime NOT NULL,
  `Flag` bit(1) NOT NULL,
  `Mark` varchar(1023) NOT NULL,
  `DID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.records 的数据：~0 rows (大约)
DELETE FROM `records`;
/*!40000 ALTER TABLE `records` DISABLE KEYS */;
/*!40000 ALTER TABLE `records` ENABLE KEYS */;


-- 导出  表 ztoptodo.serialnumbers 结构
CREATE TABLE IF NOT EXISTS `serialnumbers` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Number` varchar(255) NOT NULL DEFAULT '0',
  `BarCode` varchar(255) DEFAULT NULL,
  `NumberExt` int(11) NOT NULL DEFAULT '0',
  `SID` int(11) NOT NULL DEFAULT '0',
  `Name` varchar(255) NOT NULL,
  `Invoices` bit(3) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.serialnumbers 的数据：~4 rows (大约)
DELETE FROM `serialnumbers`;
/*!40000 ALTER TABLE `serialnumbers` DISABLE KEYS */;
INSERT INTO `serialnumbers` (`ID`, `Number`, `BarCode`, `NumberExt`, `SID`, `Name`, `Invoices`) VALUES
	(1, '201605', 'BarCodes\\635985726532449741.png', 1, 1, '汪建龙', b'000'),
	(2, '201605', 'BarCodes\\635985728472154393.png', 2, 0, '申屠杜平', b'000'),
	(3, '201605', 'BarCodes\\635985757508775740.png', 3, 2, '汪建龙', b'000'),
	(4, '201605', 'BarCodes\\635986589924678787.png', 4, 0, '靳小阳', b'000');
/*!40000 ALTER TABLE `serialnumbers` ENABLE KEYS */;


-- 导出  表 ztoptodo.sheets 结构
CREATE TABLE IF NOT EXISTS `sheets` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Time` datetime NOT NULL,
  `Count` int(11) NOT NULL DEFAULT '0',
  `Money` double NOT NULL DEFAULT '0',
  `Status` bit(3) NOT NULL,
  `Controler` varchar(255) DEFAULT NULL,
  `Remarks` varchar(1023) DEFAULT NULL,
  `Deleted` bit(1) NOT NULL,
  `Type` bit(3) NOT NULL,
  `Checkers` varchar(255) DEFAULT NULL,
  `CheckTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.sheets 的数据：~2 rows (大约)
DELETE FROM `sheets`;
/*!40000 ALTER TABLE `sheets` DISABLE KEYS */;
INSERT INTO `sheets` (`ID`, `Name`, `Time`, `Count`, `Money`, `Status`, `Controler`, `Remarks`, `Deleted`, `Type`, `Checkers`, `CheckTime`) VALUES
	(1, '汪建龙', '2016-05-11 00:00:00', 2, 403, b'101', '汪建龙', NULL, b'0', b'001', '蔡锋铭', '2016-05-11 14:58:50'),
	(2, '汪建龙', '2016-05-11 00:00:00', 1, 2000, b'101', '汪建龙', NULL, b'0', b'000', '彭盛状', '2016-05-11 15:03:42');
/*!40000 ALTER TABLE `sheets` ENABLE KEYS */;


-- 导出  表 ztoptodo.substancs 结构
CREATE TABLE IF NOT EXISTS `substancs` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Category` int(2) NOT NULL DEFAULT '0',
  `Details` varchar(1023) DEFAULT '0',
  `Price` double DEFAULT '0',
  `SID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.substancs 的数据：~1 rows (大约)
DELETE FROM `substancs`;
/*!40000 ALTER TABLE `substancs` DISABLE KEYS */;
INSERT INTO `substancs` (`ID`, `Category`, `Details`, `Price`, `SID`) VALUES
	(9, 0, '招待南湖国土局', 2000, 2);
/*!40000 ALTER TABLE `substancs` ENABLE KEYS */;


-- 导出  表 ztoptodo.task 结构
CREATE TABLE IF NOT EXISTS `task` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(128) NOT NULL,
  `Content` varchar(1024) NOT NULL,
  `ScheduledTime` datetime DEFAULT NULL,
  `CreatorID` int(11) NOT NULL,
  `ParentID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `Deleted` bit(1) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `CreatorID` (`CreatorID`),
  KEY `ParentID` (`ParentID`),
  KEY `ScheduledTime` (`ScheduledTime`),
  KEY `Title` (`Title`),
  KEY `CreateTime` (`CreateTime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.task 的数据：~0 rows (大约)
DELETE FROM `task`;
/*!40000 ALTER TABLE `task` DISABLE KEYS */;
/*!40000 ALTER TABLE `task` ENABLE KEYS */;


-- 导出  表 ztoptodo.task_query 结构
CREATE TABLE IF NOT EXISTS `task_query` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Content` varchar(512) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `UserID` (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.task_query 的数据：~0 rows (大约)
DELETE FROM `task_query`;
/*!40000 ALTER TABLE `task_query` DISABLE KEYS */;
/*!40000 ALTER TABLE `task_query` ENABLE KEYS */;


-- 导出  表 ztoptodo.traffic 结构
CREATE TABLE IF NOT EXISTS `traffic` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Type` bit(3) NOT NULL,
  `Cost` double NOT NULL,
  `Toll` double NOT NULL,
  `Plate` varchar(255) DEFAULT NULL,
  `Times` int(11) NOT NULL DEFAULT '0',
  `EID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.traffic 的数据：~2 rows (大约)
DELETE FROM `traffic`;
/*!40000 ALTER TABLE `traffic` DISABLE KEYS */;
INSERT INTO `traffic` (`ID`, `Type`, `Cost`, `Toll`, `Plate`, `Times`, `EID`) VALUES
	(9, b'001', 300, 0, NULL, 0, 1),
	(10, b'111', 63, 0, NULL, 2, 1);
/*!40000 ALTER TABLE `traffic` ENABLE KEYS */;


-- 导出  表 ztoptodo.user 结构
CREATE TABLE IF NOT EXISTS `user` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `GroupID` int(11) NOT NULL DEFAULT '0',
  `Username` varchar(50) NOT NULL DEFAULT '0',
  `RealName` varchar(50) DEFAULT '0',
  `LoginTimes` int(11) DEFAULT '0',
  `LastLoginTime` datetime DEFAULT '0000-00-00 00:00:00',
  `Order` int(11) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `Username` (`Username`),
  KEY `GroupID` (`GroupID`)
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.user 的数据：~51 rows (大约)
DELETE FROM `user`;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` (`ID`, `GroupID`, `Username`, `RealName`, `LoginTimes`, `LastLoginTime`, `Order`) VALUES
	(3, 2, 'zxp', '张兴平', 0, NULL, 8),
	(4, 2, 'ouy', '欧阳安蛟', 0, NULL, 9),
	(5, 2, 'shentu', '申屠杜平', 0, NULL, 5),
	(6, 4, 'jxh', '蒋新华', 0, NULL, 0),
	(7, 2, 'yy', '袁洋', 0, NULL, 6),
	(8, 2, 'chen', '陈立定', 0, NULL, 7),
	(9, 3, 'zhoujy', '周婧扬', 0, NULL, 0),
	(10, 3, 'ym', '尹淼', 0, NULL, 0),
	(11, 3, 'xzb', '徐质彬', 0, NULL, 0),
	(12, 3, 'lq', '李琪', 0, NULL, 0),
	(13, 3, 'lxx', '李纤茜', 0, NULL, 0),
	(14, 3, 'jx', '江馨', 0, NULL, 0),
	(15, 3, 'pjr', '潘锦瑞', 0, NULL, 0),
	(16, 3, 'wyf', '王艳芳', 0, NULL, 0),
	(17, 3, 'cdp', '陈鼎攀', 0, NULL, 0),
	(18, 4, 'sunxy', '孙夏阳', 0, NULL, 0),
	(20, 6, 'yhf', '俞海峰', 0, NULL, 0),
	(21, 6, 'ljj', '吕骏杰', 0, NULL, 0),
	(22, 6, 'zd', '周典', 0, NULL, 0),
	(23, 6, 'zwj', '周威俊', 0, NULL, 0),
	(24, 6, 'sxy', '宋学云', 0, NULL, 0),
	(25, 6, 'lx', '李仙', 0, NULL, 0),
	(26, 6, 'jxy', '靳小阳', 0, NULL, 0),
	(27, 6, 'hsc', '黄世超', 0, NULL, 0),
	(28, 7, 'ty', '唐尧', 0, NULL, 0),
	(29, 7, 'wjl', '汪建龙', 0, NULL, 0),
	(30, 7, 'ricepig', '赵斯思', 0, NULL, 0),
	(31, 7, 'zq', '赵泉', 0, NULL, 0),
	(32, 7, 'zlj', '郑良军', 0, NULL, 0),
	(33, 8, 'yj', '余军', 0, NULL, 0),
	(34, 8, 'lw', '刘威', 0, NULL, 0),
	(35, 8, 'lxy', '刘晓瑜', 0, NULL, 0),
	(36, 8, 'yxj', '姚雪姣', 0, NULL, 0),
	(37, 8, 'psz', '彭盛状', 0, NULL, 0),
	(38, 8, 'lxn', '李向农', 0, NULL, 0),
	(39, 8, 'ly', '李旸', 0, NULL, 0),
	(40, 8, 'lzq', '梁志卿', 0, NULL, 0),
	(41, 8, 'yxh', '袁潇涵', 0, NULL, 0),
	(42, 8, 'jmj', '金明杰', 0, NULL, 0),
	(43, 9, 'smy', '孙明月', 0, NULL, 0),
	(44, 9, 'zsq', '曾绍琴', 0, NULL, 0),
	(45, 9, 'zgh', '朱根华', 0, NULL, 0),
	(46, 9, 'zfl', '甄福雷', 0, NULL, 0),
	(47, 9, 'cfm', '蔡锋铭', 0, NULL, 0),
	(48, 9, 'xnx', '许宁馨', 0, NULL, 0),
	(49, 9, 'rxl', '阮晓利', 0, NULL, 0),
	(50, 9, 'cbl', '陈宝炉', 0, NULL, 0),
	(51, 9, 'mxd', '马旭东', 0, NULL, 0),
	(52, 9, 'gmf', '高明峰', 0, NULL, 0),
	(53, 0, 'slz', NULL, 0, NULL, 0),
	(54, 8, 'lwj', '柳文娟', 0, NULL, 0);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;


-- 导出  表 ztoptodo.user_group 结构
CREATE TABLE IF NOT EXISTS `user_group` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(128) NOT NULL DEFAULT '0',
  `Order` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.user_group 的数据：~7 rows (大约)
DELETE FROM `user_group`;
/*!40000 ALTER TABLE `user_group` DISABLE KEYS */;
INSERT INTO `user_group` (`ID`, `Name`, `Order`) VALUES
	(2, '协调组', 9),
	(3, '实习中心', 4),
	(4, '市场部', 7),
	(6, '综合部', 8),
	(7, '陆吾科技', 0),
	(8, '项目一部', 6),
	(9, '项目二部', 5);
/*!40000 ALTER TABLE `user_group` ENABLE KEYS */;


-- 导出  表 ztoptodo.user_task 结构
CREATE TABLE IF NOT EXISTS `user_task` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL DEFAULT '0',
  `TaskID` int(11) NOT NULL DEFAULT '0',
  `CreateTime` datetime NOT NULL,
  `CompletedTime` datetime DEFAULT NULL,
  `HasRead` bit(1) NOT NULL DEFAULT b'0',
  `Deleted` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`ID`),
  KEY `UserID` (`UserID`),
  KEY `TaskID` (`TaskID`),
  KEY `CreateTime` (`CreateTime`),
  KEY `CompletedTime` (`CompletedTime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.user_task 的数据：~0 rows (大约)
DELETE FROM `user_task`;
/*!40000 ALTER TABLE `user_task` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_task` ENABLE KEYS */;


-- 导出  视图 ztoptodo.user_task_view 结构
-- 创建临时表以解决视图依赖性错误
CREATE TABLE `user_task_view` (
	`ID` INT(11) NOT NULL,
	`UserID` INT(11) NOT NULL,
	`TaskID` INT(11) NOT NULL,
	`CompletedTime` DATETIME NULL,
	`CreateTime` DATETIME NOT NULL,
	`Title` VARCHAR(128) NOT NULL COLLATE 'utf8_general_ci',
	`ScheduledTime` DATETIME NULL,
	`CreatorID` INT(11) NOT NULL,
	`HasRead` BIT(1) NOT NULL
) ENGINE=MyISAM;


-- 导出  表 ztoptodo.verifys 结构
CREATE TABLE IF NOT EXISTS `verifys` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Step` int(2) NOT NULL DEFAULT '0',
  `Time` datetime DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Reason` varchar(255) DEFAULT NULL,
  `Position` int(2) NOT NULL DEFAULT '0',
  `SID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.verifys 的数据：~14 rows (大约)
DELETE FROM `verifys`;
/*!40000 ALTER TABLE `verifys` DISABLE KEYS */;
INSERT INTO `verifys` (`ID`, `Step`, `Time`, `Name`, `Reason`, `Position`, `SID`) VALUES
	(1, 0, '2016-05-11 14:57:45', '汪建龙', NULL, 0, 1),
	(2, 1, '2016-05-11 14:58:01', '蔡锋铭', NULL, 0, 1),
	(3, 2, '2016-05-11 14:58:16', '申屠杜平', NULL, 0, 1),
	(4, 3, '2016-05-11 14:58:50', '靳小阳', NULL, 0, 1),
	(5, 4, '2016-05-11 15:00:35', '申屠杜平', NULL, 0, 1),
	(6, 0, '2016-05-11 15:02:57', '汪建龙', NULL, 0, 2),
	(7, 1, '2016-05-11 15:03:12', '彭盛状', NULL, 0, 2),
	(8, 2, '2016-05-11 15:03:26', '申屠杜平', NULL, 0, 2),
	(9, 3, '2016-05-11 15:03:42', '靳小阳', NULL, 0, 2),
	(10, 4, '2016-05-11 15:04:06', '申屠杜平', NULL, 0, 2),
	(11, 4, '2016-05-11 15:06:06', '申屠杜平', NULL, 0, 2),
	(12, 4, '2016-05-11 15:07:02', '申屠杜平', NULL, 0, 2),
	(13, 4, '2016-05-11 15:07:29', '申屠杜平', NULL, 0, 2),
	(14, 4, '2016-05-11 15:09:05', '申屠杜平', NULL, 0, 2);
/*!40000 ALTER TABLE `verifys` ENABLE KEYS */;


-- 导出  视图 ztoptodo.user_task_view 结构
-- 移除临时表并创建最终视图结构
DROP TABLE IF EXISTS `user_task_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`::1` VIEW `user_task_view` AS SELECT
user_task.ID,
user_task.UserID,
user_task.TaskID,
user_task.CompletedTime,
user_task.CreateTime,
task.Title,
task.ScheduledTime,
task.CreatorID,
user_task.HasRead
FROM
task
INNER JOIN user_task ON task.ID = user_task.TaskID 
Where user_task.Deleted = 0 ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

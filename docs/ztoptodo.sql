-- --------------------------------------------------------
-- 主机:                           127.0.0.1
-- 服务器版本:                        5.1.73-community - MySQL Community Server (GPL)
-- 服务器操作系统:                      Win64
-- HeidiSQL 版本:                  9.3.0.4984
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 导出  表 ztoptodo.attachment 结构
DROP TABLE IF EXISTS `attachment`;
CREATE TABLE IF NOT EXISTS `attachment` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FileName` varchar(128) DEFAULT NULL,
  `TaskID` int(11) DEFAULT NULL,
  `FileSize` int(11) DEFAULT NULL,
  `SavePath` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `TaskID` (`TaskID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.attachment 的数据：~1 rows (大约)
DELETE FROM `attachment`;
/*!40000 ALTER TABLE `attachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `attachment` ENABLE KEYS */;


-- 导出  表 ztoptodo.authorizes 结构
DROP TABLE IF EXISTS `authorizes`;
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


-- 导出  表 ztoptodo.comment 结构
DROP TABLE IF EXISTS `comment`;
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
DROP TABLE IF EXISTS `databooks`;
CREATE TABLE IF NOT EXISTS `databooks` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
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


-- 导出  表 ztoptodo.messages 结构
DROP TABLE IF EXISTS `messages`;
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


-- 导出  表 ztoptodo.serialnumbers 结构
DROP TABLE IF EXISTS `serialnumbers`;
CREATE TABLE IF NOT EXISTS `serialnumbers` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Number` varchar(255) NOT NULL DEFAULT '0',
  `NumberExt` int(11) NOT NULL DEFAULT '0',
  `SID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.serialnumbers 的数据：~9 rows (大约)
DELETE FROM `serialnumbers`;
/*!40000 ALTER TABLE `serialnumbers` DISABLE KEYS */;
INSERT INTO `serialnumbers` (`ID`, `Number`, `NumberExt`, `SID`) VALUES
	(1, 'ZTOP20160317', 1, 0),
	(2, 'ZTOP20160317', 2, 0),
	(3, 'ZTOP20160317', 3, 0),
	(4, 'ZTOP20160317', 4, 0),
	(5, 'ZTOP20160317', 5, 0),
	(6, 'ZTOP20160317', 6, 0),
	(7, 'ZTOP20160317', 7, 0),
	(8, 'ZTOP20160317', 8, 0),
	(9, 'ZTOP20160317', 9, 0);
/*!40000 ALTER TABLE `serialnumbers` ENABLE KEYS */;


-- 导出  表 ztoptodo.sheets 结构
DROP TABLE IF EXISTS `sheets`;
CREATE TABLE IF NOT EXISTS `sheets` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) DEFAULT NULL,
  `Time` datetime NOT NULL,
  `Money` double NOT NULL DEFAULT '0',
  `Status` bit(1) NOT NULL,
  `Remarks` varchar(1023) DEFAULT NULL,
  `Deleted` bit(1) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.sheets 的数据：~0 rows (大约)
DELETE FROM `sheets`;
/*!40000 ALTER TABLE `sheets` DISABLE KEYS */;
/*!40000 ALTER TABLE `sheets` ENABLE KEYS */;


-- 导出  表 ztoptodo.substancs 结构
DROP TABLE IF EXISTS `substancs`;
CREATE TABLE IF NOT EXISTS `substancs` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Category` int(2) NOT NULL DEFAULT '0',
  `Details` varchar(1023) DEFAULT '0',
  `Price` double DEFAULT '0',
  `SID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.substancs 的数据：~0 rows (大约)
DELETE FROM `substancs`;
/*!40000 ALTER TABLE `substancs` DISABLE KEYS */;
/*!40000 ALTER TABLE `substancs` ENABLE KEYS */;


-- 导出  表 ztoptodo.task 结构
DROP TABLE IF EXISTS `task`;
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

-- 正在导出表  ztoptodo.task 的数据：~4 rows (大约)
DELETE FROM `task`;
/*!40000 ALTER TABLE `task` DISABLE KEYS */;
/*!40000 ALTER TABLE `task` ENABLE KEYS */;


-- 导出  表 ztoptodo.task_query 结构
DROP TABLE IF EXISTS `task_query`;
CREATE TABLE IF NOT EXISTS `task_query` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Content` varchar(512) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `UserID` (`UserID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.task_query 的数据：~4 rows (大约)
DELETE FROM `task_query`;
/*!40000 ALTER TABLE `task_query` DISABLE KEYS */;
INSERT INTO `task_query` (`ID`, `UserID`, `Name`, `Content`) VALUES
	(1, 0, '接收-未完成', '{"IsCreator":false,"CreatorID":0,"ReceiverID":0,"SearchKey":"","IsCompleted":false,"Order":0,"Page":null,"HasRead":null,"Days":0,"GetCreator":true,"GetReceiver":true}'),
	(2, 0, '下达-未完成', '{"IsCreator":true,"CreatorID":0,"ReceiverID":0,"SearchKey":"","IsCompleted":false,"Order":0,"Page":null,"HasRead":null,"Days":0,"GetCreator":true,"GetReceiver":true}'),
	(3, 0, '接收-已完成', '{"IsCreator":false,"CreatorID":0,"ReceiverID":0,"SearchKey":"","IsCompleted":true,"Order":0,"Page":null,"HasRead":null,"Days":0,"GetCreator":true,"GetReceiver":true}'),
	(4, 0, '下达-已完成', '{"IsCreator":true,"CreatorID":0,"ReceiverID":0,"SearchKey":"","IsCompleted":true,"Order":0,"Page":null,"HasRead":null,"Days":0,"GetCreator":true,"GetReceiver":true}');
/*!40000 ALTER TABLE `task_query` ENABLE KEYS */;


-- 导出  表 ztoptodo.user 结构
DROP TABLE IF EXISTS `user`;
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
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.user 的数据：~52 rows (大约)
DELETE FROM `user`;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` (`ID`, `GroupID`, `Username`, `RealName`, `LoginTimes`, `LastLoginTime`, `Order`) VALUES
	(3, 2, 'zxp', '张兴平', 0, NULL, 8),
	(4, 2, 'ouy', '欧阳安蛟', 0, NULL, 9),
	(5, 2, 'shentu', '申屠杜平', 0, NULL, 5),
	(6, 2, 'jxh', '蒋新华', 0, NULL, 0),
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
	(52, 9, 'gmf', '高明峰', 0, NULL, 0);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;


-- 导出  表 ztoptodo.user_group 结构
DROP TABLE IF EXISTS `user_group`;
CREATE TABLE IF NOT EXISTS `user_group` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(128) NOT NULL DEFAULT '0',
  `Order` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.user_group 的数据：~9 rows (大约)
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
DROP TABLE IF EXISTS `user_task`;
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

-- 正在导出表  ztoptodo.user_task 的数据：~4 rows (大约)
DELETE FROM `user_task`;
/*!40000 ALTER TABLE `user_task` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_task` ENABLE KEYS */;


-- 导出  视图 ztoptodo.user_task_view 结构
DROP VIEW IF EXISTS `user_task_view`;
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
DROP TABLE IF EXISTS `verifys`;
CREATE TABLE IF NOT EXISTS `verifys` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Step` int(2) NOT NULL DEFAULT '0',
  `Time` datetime DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Reason` varchar(255) DEFAULT NULL,
  `IsCheck` bit(1) NOT NULL,
  `SID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.verifys 的数据：~0 rows (大约)
DELETE FROM `verifys`;
/*!40000 ALTER TABLE `verifys` DISABLE KEYS */;
/*!40000 ALTER TABLE `verifys` ENABLE KEYS */;


-- 导出  视图 ztoptodo.user_task_view 结构
DROP VIEW IF EXISTS `user_task_view`;
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

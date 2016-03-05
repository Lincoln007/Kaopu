-- --------------------------------------------------------
-- 主机:                           127.0.0.1
-- 服务器版本:                        5.6.26 - MySQL Community Server (GPL)
-- 服务器操作系统:                      Win64
-- HeidiSQL 版本:                  9.3.0.4984
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.attachment 的数据：~4 rows (大约)
/*!40000 ALTER TABLE `attachment` DISABLE KEYS */;
INSERT INTO `attachment` (`ID`, `FileName`, `TaskID`, `FileSize`, `SavePath`) VALUES
	(1, '淮北市城市建设用地节约集约利用评价项目招标文件（三次招标）.doc', 1, 119296, '635926917087643543.doc'),
	(2, '淮北市城市建设用地节约集约利用评价项目招标文件（三次招标）.doc', 2, 119296, '635926917087643543.doc'),
	(3, '淮北市城市建设用地节约集约利用评价项目招标文件（三次招标）.doc', 6, 119296, '635926917087643543.doc'),
	(4, '淮北市城市建设用地节约集约利用评价项目招标文件（三次招标）.doc', 7, 119296, '635926917087643543.doc');
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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.comment 的数据：~3 rows (大约)
/*!40000 ALTER TABLE `comment` DISABLE KEYS */;
INSERT INTO `comment` (`ID`, `Content`, `UserID`, `TaskID`, `CreateTime`) VALUES
	(1, 'fsdf', 2, 4, '2016-03-04 13:39:31'),
	(2, 'fdsfds', 2, 4, '2016-03-04 13:39:33'),
	(3, 'fsdfds', 2, 4, '2016-03-04 13:39:34');
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
/*!40000 ALTER TABLE `messages` DISABLE KEYS */;
/*!40000 ALTER TABLE `messages` ENABLE KEYS */;


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
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.task 的数据：~12 rows (大约)
/*!40000 ALTER TABLE `task` DISABLE KEYS */;
INSERT INTO `task` (`ID`, `Title`, `Content`, `ScheduledTime`, `CreatorID`, `ParentID`, `CreateTime`, `Deleted`) VALUES
	(1, '测试yy', '测试', '2016-03-07 00:00:00', 2, 0, '2016-03-04 12:35:08', b'1'),
	(2, '测试yy', '测试', '2016-03-07 00:00:00', 2, 1, '2016-03-04 12:35:26', b'0'),
	(3, '测试yy2', 'dsds', '2016-03-11 00:00:00', 2, 0, '2016-03-04 12:36:03', b'0'),
	(4, 'cxcxc', 'xcxc', '2016-03-05 00:00:00', 2, 0, '2016-03-04 12:39:38', b'0'),
	(5, 'xczcxzc', 'xczxcxzc', '2016-03-05 00:00:00', 2, 0, '2016-03-04 12:40:16', b'0'),
	(6, '测试yy', '测试', '2016-03-07 00:00:00', 2, 1, '2016-03-04 12:41:56', b'0'),
	(7, '测试yy', '测试', '2016-03-07 00:00:00', 2, 6, '2016-03-04 12:44:13', b'0'),
	(8, 'ghhghggh', 'hgg', '2016-03-11 00:00:00', 2, 0, '2016-03-04 12:44:37', b'0'),
	(9, 'dsdds', 'dsds', '2016-03-05 00:00:00', 7, 0, '2016-03-04 13:14:53', b'0'),
	(10, 'dasds', 'dasdsad', '2016-03-05 00:00:00', 7, 0, '2016-03-04 13:53:40', b'0'),
	(11, 'dasdasdsadasd', 'dasasdasd', '2016-03-11 00:00:00', 7, 0, '2016-03-04 13:55:27', b'0'),
	(12, '测试1', '测试1', '2016-03-05 00:00:00', 8, 0, '2016-03-04 19:17:27', b'0');
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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.task_query 的数据：~0 rows (大约)
/*!40000 ALTER TABLE `task_query` DISABLE KEYS */;
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
  PRIMARY KEY (`ID`),
  KEY `Username` (`Username`),
  KEY `GroupID` (`GroupID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.user 的数据：~8 rows (大约)
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` (`ID`, `GroupID`, `Username`, `RealName`, `LoginTimes`, `LastLoginTime`) VALUES
	(1, 0, 'wjl', '汪建龙', 1, '2016-01-21 13:50:24'),
	(2, 0, 'yy', '袁洋', 0, NULL),
	(3, 0, 'ljj', '吕骏杰', 0, NULL),
	(4, 0, 'ty', '唐尧', 0, NULL),
	(5, 0, 'sunxy', '孙夏阳', 0, NULL),
	(6, 0, 'zwj', '周威俊', 0, NULL),
	(7, 0, 'slz', '测试', 0, NULL),
	(8, 0, 'liangjun', '郑良军', 0, NULL);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;


-- 导出  表 ztoptodo.user_group 结构
DROP TABLE IF EXISTS `user_group`;
CREATE TABLE IF NOT EXISTS `user_group` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(128) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.user_group 的数据：~0 rows (大约)
/*!40000 ALTER TABLE `user_group` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_group` ENABLE KEYS */;


-- 导出  表 ztoptodo.user_task 结构
DROP TABLE IF EXISTS `user_task`;
CREATE TABLE IF NOT EXISTS `user_task` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL DEFAULT '0',
  `TaskID` int(11) NOT NULL DEFAULT '0',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `CompletedTime` datetime DEFAULT NULL,
  `HasRead` bit(1) NOT NULL DEFAULT b'0',
  `Deleted` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`ID`),
  KEY `UserID` (`UserID`),
  KEY `TaskID` (`TaskID`),
  KEY `CreateTime` (`CreateTime`),
  KEY `CompletedTime` (`CompletedTime`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.user_task 的数据：~29 rows (大约)
/*!40000 ALTER TABLE `user_task` DISABLE KEYS */;
INSERT INTO `user_task` (`ID`, `UserID`, `TaskID`, `CreateTime`, `CompletedTime`, `HasRead`, `Deleted`) VALUES
	(1, 3, 1, '2016-03-04 12:35:08', NULL, b'0', b'0'),
	(2, 2, 1, '2016-03-04 12:35:08', NULL, b'1', b'0'),
	(3, 5, 1, '2016-03-04 12:35:08', NULL, b'0', b'0'),
	(4, 4, 1, '2016-03-04 12:35:08', NULL, b'0', b'0'),
	(5, 6, 1, '2016-03-04 12:35:08', NULL, b'0', b'0'),
	(6, 1, 1, '2016-03-04 12:35:08', NULL, b'0', b'0'),
	(7, 5, 2, '2016-03-04 12:35:26', NULL, b'0', b'0'),
	(8, 2, 2, '2016-03-04 12:35:26', '2016-03-04 12:38:33', b'1', b'0'),
	(9, 5, 3, '2016-03-04 12:36:03', NULL, b'0', b'0'),
	(10, 2, 3, '2016-03-04 12:36:03', '2016-03-04 12:38:06', b'1', b'0'),
	(11, 2, 4, '2016-03-04 12:39:38', '2016-03-04 12:39:45', b'1', b'0'),
	(12, 1, 5, '2016-03-04 12:40:16', NULL, b'1', b'0'),
	(13, 2, 5, '2016-03-04 12:40:16', NULL, b'1', b'0'),
	(14, 1, 6, '2016-03-04 12:41:56', NULL, b'0', b'0'),
	(15, 2, 6, '2016-03-04 12:41:56', NULL, b'1', b'0'),
	(16, 3, 7, '2016-03-04 12:44:13', NULL, b'0', b'0'),
	(17, 2, 7, '2016-03-04 12:44:13', NULL, b'1', b'0'),
	(18, 5, 7, '2016-03-04 12:44:13', NULL, b'0', b'0'),
	(19, 4, 7, '2016-03-04 12:44:13', NULL, b'0', b'0'),
	(20, 4, 8, '2016-03-04 12:44:37', NULL, b'0', b'0'),
	(21, 2, 8, '2016-03-04 12:44:37', NULL, b'1', b'0'),
	(22, 2, 9, '2016-03-04 13:14:53', NULL, b'0', b'0'),
	(23, 7, 9, '2016-03-04 13:14:53', NULL, b'0', b'0'),
	(24, 2, 10, '2016-03-04 13:53:40', NULL, b'0', b'0'),
	(25, 7, 10, '2016-03-04 13:53:40', NULL, b'0', b'0'),
	(26, 2, 11, '2016-03-04 13:55:27', NULL, b'0', b'0'),
	(27, 7, 11, '2016-03-04 13:55:27', NULL, b'0', b'0'),
	(28, 4, 12, '2016-03-04 19:17:27', NULL, b'0', b'0'),
	(29, 8, 12, '2016-03-04 19:17:27', NULL, b'1', b'0');
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

-- --------------------------------------------------------
-- 主机:                           10.22.102.3
-- 服务器版本:                        5.1.73-community - MySQL Community Server (GPL)
-- 服务器操作系统:                      Win64
-- HeidiSQL 版本:                  9.3.0.4984
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 导出 ztoptodo 的数据库结构
CREATE DATABASE IF NOT EXISTS `ztoptodo` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ztoptodo`;


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

-- 数据导出被取消选择。


-- 导出  表 ztoptodo.authorizes 结构
CREATE TABLE IF NOT EXISTS `authorizes` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `GroupName` varchar(1023) DEFAULT NULL,
  `Manager` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


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

-- 数据导出被取消选择。


-- 导出  表 ztoptodo.databooks 结构
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

-- 数据导出被取消选择。


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

-- 数据导出被取消选择。


-- 导出  表 ztoptodo.records 结构
CREATE TABLE IF NOT EXISTS `records` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Time` datetime NOT NULL,
  `Flag` bit(1) NOT NULL,
  `Mark` varchar(1023) NOT NULL,
  `DID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


-- 导出  表 ztoptodo.serialnumbers 结构
CREATE TABLE IF NOT EXISTS `serialnumbers` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Number` varchar(255) NOT NULL DEFAULT '0',
  `NumberExt` int(11) NOT NULL DEFAULT '0',
  `SID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


-- 导出  表 ztoptodo.sheets 结构
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

-- 数据导出被取消选择。


-- 导出  表 ztoptodo.substancs 结构
CREATE TABLE IF NOT EXISTS `substancs` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Category` int(2) NOT NULL DEFAULT '0',
  `Details` varchar(1023) DEFAULT '0',
  `Price` double DEFAULT '0',
  `SID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


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

-- 数据导出被取消选择。


-- 导出  表 ztoptodo.task_query 结构
CREATE TABLE IF NOT EXISTS `task_query` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `Content` varchar(512) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `UserID` (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


-- 导出  表 ztoptodo.user_group 结构
CREATE TABLE IF NOT EXISTS `user_group` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(128) NOT NULL DEFAULT '0',
  `Order` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


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

-- 数据导出被取消选择。


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
  `IsCheck` bit(1) NOT NULL,
  `SID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


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

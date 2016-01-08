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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


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

-- 数据导出被取消选择。


-- 导出  表 ztoptodo.task 结构
DROP TABLE IF EXISTS `task`;
CREATE TABLE IF NOT EXISTS `task` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(128) NOT NULL,
  `Content` varchar(1024) NOT NULL,
  `ScheduledTime` datetime DEFAULT NULL,
  `CreatorID` int(11) NOT NULL,
  `IsCompleted` bit(1) NOT NULL,
  `ParentID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `Deleted` bit(1) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `OwnerID` (`CreatorID`),
  KEY `ScheduledTime` (`ScheduledTime`),
  KEY `ParentID` (`ParentID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


-- 导出  表 ztoptodo.user 结构
DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) NOT NULL DEFAULT '0',
  `RealName` varchar(50) DEFAULT '0',
  `LoginTimes` int(11) DEFAULT '0',
  `LastLoginTime` datetime DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`ID`),
  KEY `Username` (`Username`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


-- 导出  表 ztoptodo.user_task 结构
DROP TABLE IF EXISTS `user_task`;
CREATE TABLE IF NOT EXISTS `user_task` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL DEFAULT '0',
  `TaskID` int(11) NOT NULL DEFAULT '0',
  `CreateTime` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `CompletedTime` datetime DEFAULT NULL,
  `HasRead` bit(1) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `UserID` (`UserID`),
  KEY `TaskID` (`TaskID`),
  KEY `CreateTime` (`CreateTime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


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
Where task.Deleted = 0 ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;


SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for attachment
-- ----------------------------
DROP TABLE IF EXISTS `attachment`;
CREATE TABLE `attachment` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FileName` varchar(128) DEFAULT NULL,
  `TaskID` int(11) DEFAULT NULL,
  `FileSize` int(11) DEFAULT NULL,
  `SavePath` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `TaskID` (`TaskID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for comment
-- ----------------------------
DROP TABLE IF EXISTS `comment`;
CREATE TABLE `comment` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Content` varchar(50) NOT NULL,
  `UserID` int(11) NOT NULL,
  `TaskID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `TaskID` (`TaskID`),
  KEY `UserID` (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for task
-- ----------------------------
DROP TABLE IF EXISTS `task`;
CREATE TABLE `task` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(128) NOT NULL,
  `Content` varchar(1024) NOT NULL,
  `ScheduledTime` datetime DEFAULT NULL,
  `OwnerID` int(11) NOT NULL,
  `IsCompleted` bit(1) NOT NULL,
  `ParentID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `Deleted` bit(1) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `OwnerID` (`OwnerID`),
  KEY `ScheduledTime` (`ScheduledTime`),
  KEY `ParentID` (`ParentID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) NOT NULL DEFAULT '0',
  `RealName` varchar(50) DEFAULT '0',
  `LoginTimes` int(11) DEFAULT '0',
  `LastLoginTime` datetime DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`ID`),
  KEY `Username` (`Username`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for user_task
-- ----------------------------
DROP TABLE IF EXISTS `user_task`;
CREATE TABLE `user_task` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL DEFAULT '0',
  `TaskID` int(11) NOT NULL DEFAULT '0',
  `IsCompleted` bit(1) NOT NULL DEFAULT b'0',
  `CreateTime` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`ID`),
  KEY `UserID` (`UserID`),
  KEY `TaskID` (`TaskID`),
  KEY `CreateTime` (`CreateTime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- View structure for user_task_view
-- ----------------------------
DROP VIEW IF EXISTS `user_task_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`::1` SQL SECURITY DEFINER  VIEW `user_task_view` AS SELECT
user_task.ID,
user_task.UserID,
user_task.TaskID,
user_task.IsCompleted,
user_task.CreateTime,
task.Title,
task.ScheduledTime,
task.OwnerID
FROM
task
INNER JOIN user_task ON task.ID = user_task.TaskID 
Where task.Deleted = 0 ;

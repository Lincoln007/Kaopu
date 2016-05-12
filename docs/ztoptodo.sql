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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


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

-- 数据导出被取消选择。


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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。


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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

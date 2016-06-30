-- --------------------------------------------------------
-- 主机:                           192.168.31.10
-- 服务器版本:                        5.1.73-community - MySQL Community Server (GPL)
-- 服务器操作系统:                      Win64
-- HeidiSQL 版本:                  9.3.0.5086
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
  `Balance` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.banks 的数据：~1 rows (大约)
DELETE FROM `banks`;
/*!40000 ALTER TABLE `banks` DISABLE KEYS */;
INSERT INTO `banks` (`ID`, `Year`, `Month`, `Company`, `Balance`) VALUES
	(1, 2016, 5, b'000', 0);
/*!40000 ALTER TABLE `banks` ENABLE KEYS */;

-- 导出  表 ztoptodo.bills 结构
CREATE TABLE IF NOT EXISTS `bills` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Coding` varchar(255) DEFAULT '0',
  `Time` datetime DEFAULT NULL,
  `Money` double NOT NULL DEFAULT '0',
  `Account` varchar(1023) NOT NULL DEFAULT '0',
  `Budget` bit(3) NOT NULL DEFAULT b'0',
  `Cost` bit(3) NOT NULL DEFAULT b'0',
  `Category` bit(3) DEFAULT b'0',
  `Summary` varchar(1023) NOT NULL DEFAULT '0',
  `Remark` varchar(1023) NOT NULL DEFAULT '0',
  `BID` int(11) NOT NULL DEFAULT '0',
  `Balance` double NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`),
  KEY `BID` (`BID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.bills 的数据：~5 rows (大约)
DELETE FROM `bills`;
/*!40000 ALTER TABLE `bills` DISABLE KEYS */;
INSERT INTO `bills` (`ID`, `Coding`, `Time`, `Money`, `Account`, `Budget`, `Cost`, `Category`, `Summary`, `Remark`, `BID`, `Balance`) VALUES
	(1, NULL, '2016-05-02 00:00:00', 1000, '嘉兴市国土资源局秀洲分局', b'000', b'010', NULL, '保证金退回', '保证金退回', 1, 1000),
	(2, NULL, '2016-05-03 00:00:00', 1000, '嘉兴市国土资源局南湖分局', b'000', b'010', NULL, '保证金退回', '保证金退回', 1, 2000),
	(3, NULL, '2016-05-04 00:00:00', 200, '陆吾科技', b'001', b'011', NULL, '过账', '过账', 1, 1800),
	(4, NULL, '0001-01-01 00:00:00', 0, '', b'001', b'000', NULL, '', '', 1, 0),
	(5, '636028771683630198', '2016-06-30 00:00:00', 10000, '唐尧', b'000', b'000', NULL, '汪建龙录入的到账信息', '唐尧中奖了', 0, 0);
/*!40000 ALTER TABLE `bills` ENABLE KEYS */;

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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.comment 的数据：~5 rows (大约)
DELETE FROM `comment`;
/*!40000 ALTER TABLE `comment` DISABLE KEYS */;
INSERT INTO `comment` (`ID`, `Content`, `UserID`, `TaskID`, `CreateTime`) VALUES
	(1, '123', 29, 9, '2016-05-31 09:51:14'),
	(2, '2345', 29, 9, '2016-05-31 09:51:36'),
	(3, '5678899', 29, 9, '2016-05-31 09:52:33'),
	(4, '123456789', 29, 9, '2016-05-31 09:56:56'),
	(5, '123422', 29, 9, '2016-05-31 09:57:13');
/*!40000 ALTER TABLE `comment` ENABLE KEYS */;

-- 导出  表 ztoptodo.contractfiles 结构
CREATE TABLE IF NOT EXISTS `contractfiles` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ContractID` int(11) NOT NULL DEFAULT '0',
  `SavePath` varchar(1023) DEFAULT NULL,
  `FileName` varchar(255) DEFAULT NULL,
  `FileSize` int(11) NOT NULL DEFAULT '0',
  `Remove` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`ID`),
  KEY `ContractID` (`ContractID`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.contractfiles 的数据：~26 rows (大约)
DELETE FROM `contractfiles`;
/*!40000 ALTER TABLE `contractfiles` DISABLE KEYS */;
INSERT INTO `contractfiles` (`ID`, `ContractID`, `SavePath`, `FileName`, `FileSize`, `Remove`) VALUES
	(1, 4, 'E:\\github\\Kaopu\\Ztop.Todo.Web\\ContratctFiles\\【BT天堂】【BTtiantang.com】[1080p]速度与激情.7.95GB636028322173464235.torrent', '【BT天堂】【BTtiantang.com】[1080p]速度与激情.7.95GB636028322173464235.torrent', 217921, b'0'),
	(2, 4, 'E:\\github\\Kaopu\\Ztop.Todo.Web\\ContratctFiles\\【BT天堂】【BTtiantang.com】[1080p]速度与激情2.7.96GB636028322173484260.torrent', '【BT天堂】【BTtiantang.com】[1080p]速度与激情2.7.96GB636028322173484260.torrent', 218131, b'0'),
	(3, 4, 'E:\\github\\Kaopu\\Ztop.Todo.Web\\ContratctFiles\\【BT天堂】【BTtiantang.com】[1080p]速度与激情3：东京漂移.3.41GB636028322173499278.torrent', '【BT天堂】【BTtiantang.com】[1080p]速度与激情3：东京漂移.3.41GB636028322173499278.torrent', 18839, b'0'),
	(4, 4, 'E:\\github\\Kaopu\\Ztop.Todo.Web\\ContratctFiles\\【BT天堂】【BTtiantang.com】[1080p]速度与激情5.13.17GB636028322173509292.torrent', '【BT天堂】【BTtiantang.com】[1080p]速度与激情5.13.17GB636028322173509292.torrent', 69171, b'0'),
	(5, 4, 'E:\\github\\Kaopu\\Ztop.Todo.Web\\ContratctFiles\\554636028322173519306.png', '554636028322173519306.png', 271340, b'0'),
	(6, 4, 'E:\\github\\Kaopu\\Ztop.Todo.Web\\ContratctFiles\\杭州智拓项目管理系统636028322173529317.doc', '杭州智拓项目管理系统636028322173529317.doc', 9216, b'0'),
	(7, 5, 'E:\\github\\Kaopu\\Ztop.Todo.Web\\ContratctFiles\\【BT天堂】【BTtiantang.com】[1080p]速度与激情.7.95GB636028325557926444.torrent', '【BT天堂】【BTtiantang.com】[1080p]速度与激情.7.95GB.torrent', 217921, b'0'),
	(8, 5, 'E:\\github\\Kaopu\\Ztop.Todo.Web\\ContratctFiles\\【BT天堂】【BTtiantang.com】[1080p]速度与激情2.7.96GB636028325557941553.torrent', '【BT天堂】【BTtiantang.com】[1080p]速度与激情2.7.96GB.torrent', 218131, b'0'),
	(9, 5, 'E:\\github\\Kaopu\\Ztop.Todo.Web\\ContratctFiles\\【BT天堂】【BTtiantang.com】[1080p]速度与激情3：东京漂移.3.41GB636028325557946460.torrent', '【BT天堂】【BTtiantang.com】[1080p]速度与激情3：东京漂移.3.41GB.torrent', 18839, b'0'),
	(10, 5, 'E:\\github\\Kaopu\\Ztop.Todo.Web\\ContratctFiles\\【BT天堂】【BTtiantang.com】[1080p]速度与激情5.13.17GB636028325557956486.torrent', '【BT天堂】【BTtiantang.com】[1080p]速度与激情5.13.17GB.torrent', 69171, b'0'),
	(11, 5, 'E:\\github\\Kaopu\\Ztop.Todo.Web\\ContratctFiles\\554636028325557966482.png', '554.png', 271340, b'0'),
	(12, 5, 'E:\\github\\Kaopu\\Ztop.Todo.Web\\ContratctFiles\\杭州智拓项目管理系统636028325557971489.doc', '杭州智拓项目管理系统.doc', 9216, b'0'),
	(13, 6, 'ContractFiles\\554636028336837562629.png', '554.png', 271340, b'0'),
	(14, 6, 'ContractFiles\\【BT天堂】【BTtiantang.com】[1080p]速度与激情3：东京漂移.3.41GB636028336837577647.torrent', '【BT天堂】【BTtiantang.com】[1080p]速度与激情3：东京漂移.3.41GB.torrent', 18839, b'0'),
	(15, 6, 'ContractFiles\\【BT天堂】【BTtiantang.com】[BluRay 1080p]速度与激情7.9.84GB636028336837587809.torrent', '【BT天堂】【BTtiantang.com】[BluRay 1080p]速度与激情7.9.84GB.torrent', 101631, b'0'),
	(16, 7, 'ContractFiles\\7af40ad162d9f2d309bcd489aaec8a136327cc23636028775751784235.jpg', '7af40ad162d9f2d309bcd489aaec8a136327cc23.jpg', 51538, b'0'),
	(17, 7, 'ContractFiles\\123636028775751814258.xls', '123.xls', 2248619, b'0'),
	(18, 10, 'ContractFiles\\7af40ad162d9f2d309bcd489aaec8a136327cc23636029036073759797.jpg', '7af40ad162d9f2d309bcd489aaec8a136327cc23.jpg', 51538, b'0'),
	(19, 10, 'ContractFiles\\123636029036073909908.xls', '123.xls', 2248619, b'0'),
	(20, 10, 'ContractFiles\\SKM_C364e16062816340_0003636029036073949811.jpg', 'SKM_C364e16062816340_0003.jpg', 2216255, b'0'),
	(21, 11, 'ContractFiles\\TODO636029036356233961.png', 'TODO.png', 2995, b'0'),
	(22, 13, 'ContractFiles\\7af40ad162d9f2d309bcd489aaec8a136327cc23636029042616306817.jpg', '7af40ad162d9f2d309bcd489aaec8a136327cc23.jpg', 51538, b'1'),
	(23, 13, 'ContractFiles\\123636029042616336840.xls', '123.xls', 2248619, b'0'),
	(24, 13, 'ContractFiles\\20160527袁总反馈问题636029042616366991.docx', '20160527袁总反馈问题.docx', 324738, b'0'),
	(25, 13, 'ContractFiles\\TODO636029044098546591.png', 'TODO.png', 2995, b'0'),
	(26, 8, 'ContractFiles\\[jQuery攻略].(印)哈瓦尼.扫描版636029053102373893.pdf', '[jQuery攻略].(印)哈瓦尼.扫描版.pdf', 21478998, b'0'),
	(27, 14, 'ContractFiles\\7af40ad162d9f2d309bcd489aaec8a136327cc23636029168991141069.jpg', '7af40ad162d9f2d309bcd489aaec8a136327cc23.jpg', 51538, b'0'),
	(28, 14, 'ContractFiles\\QQ截图20160610201427636029168991176016.png', 'QQ截图20160610201427.png', 67514, b'0'),
	(29, 14, 'ContractFiles\\新建文本文档636029168991181032.txt', '新建文本文档.txt', 710, b'0');
/*!40000 ALTER TABLE `contractfiles` ENABLE KEYS */;

-- 导出  表 ztoptodo.contracts 结构
CREATE TABLE IF NOT EXISTS `contracts` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Coding` varchar(255) NOT NULL,
  `Name` varchar(1023) NOT NULL,
  `Company` varchar(1023) NOT NULL,
  `Creator` varchar(255) NOT NULL,
  `ZtopCompany` bit(3) NOT NULL DEFAULT b'0',
  `Deleted` bit(1) NOT NULL DEFAULT b'0',
  `Archived` bit(1) NOT NULL DEFAULT b'0',
  `StartTime` datetime NOT NULL,
  `EndTime` datetime DEFAULT NULL,
  `Money` double NOT NULL DEFAULT '0',
  `PerformanceBond` double NOT NULL DEFAULT '0',
  `PayWay` varchar(1023) NOT NULL DEFAULT '0',
  `Department` varchar(255) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.contracts 的数据：~13 rows (大约)
DELETE FROM `contracts`;
/*!40000 ALTER TABLE `contracts` DISABLE KEYS */;
INSERT INTO `contracts` (`ID`, `Coding`, `Name`, `Company`, `Creator`, `ZtopCompany`, `Deleted`, `Archived`, `StartTime`, `EndTime`, `Money`, `PerformanceBond`, `PayWay`, `Department`) VALUES
	(1, '636028302358101647', '嘉兴市工业区评价', '嘉兴市国土资源局秀洲分局', '汪建龙', b'000', b'1', b'0', '2016-06-01 00:00:00', NULL, 100000, 1000, '第一期：\r\n第二期：\r\n第三期：', ''),
	(2, '636028314860853057', '测试多个文件', '嘉兴市国土资源局南湖分局', '汪建龙', b'000', b'1', b'0', '2016-06-01 00:00:00', '2019-10-03 00:00:00', 1000000, 2000, '第一期：100000元\r\n第二期：20000元', ''),
	(3, '636028317274933451', '测试多个文件', '嘉兴市国土资源局秀洲分局', '汪建龙', b'000', b'1', b'0', '2016-06-01 00:00:00', '2020-06-11 00:00:00', 1000000, 2000, '第一期：100000元\r\n第二期：10000元', ''),
	(4, '636028322145679633', '测试', '嘉兴市国土资源局秀洲分局', '汪建龙', b'000', b'1', b'0', '2016-06-01 00:00:00', '2020-06-11 00:00:00', 100000, 1000, '第一期：10000元\r\n第二期：1000元', ''),
	(5, '636028325535078648', '测试多个文件深深', '嘉兴市国土资源局秀洲分局', '汪建龙', b'000', b'1', b'0', '2016-06-01 00:00:00', '2021-06-10 00:00:00', 100000, 2000, '第一期：1000元\r\n第二期：10000元', ''),
	(6, '636028336835630316', '测试第二遍', '嘉兴市国土资源局秀洲分局', '汪建龙', b'000', b'1', b'0', '2016-06-01 00:00:00', '2021-06-23 00:00:00', 100000, 2000, '第一期：10000元\r\n第三期：5555元', ''),
	(7, '636028775712631579', '嘉兴市测试合同项目', '嘉兴市国土资源局', '汪建龙', b'000', b'0', b'0', '2016-06-01 00:00:00', '2020-06-12 00:00:00', 10000, 1000, '第一期：5000元\r\n第二期：5000元', ''),
	(8, '636028945247032279', '第二个项目', '嘉兴市国土资源局南湖分局', '汪建龙', b'001', b'0', b'0', '2016-06-08 00:00:00', '2020-06-04 00:00:00', 100000, 2999, '第一期：2020\r\n第二期：28282', ''),
	(9, '636028962358636678', '第三个项目', '嘉兴市国土资源局南湖分局', '汪建龙', b'000', b'0', b'0', '2016-05-31 00:00:00', '2021-06-09 00:00:00', 10000, 2000, '第一期：1000元\r\n第二期：1000元', ''),
	(10, '636029036071718234', '第三个个木木', '嘉兴市国土资源局秀洲分局', '汪建龙', b'010', b'0', b'0', '2016-06-01 00:00:00', '2020-06-10 00:00:00', 10000, 2000, '第一期：1000元\r\n第二期：10000元', ''),
	(11, '636029036356073848', '第三个个木木', '嘉兴市国土资源局秀洲分局', '汪建龙', b'010', b'0', b'0', '2016-06-01 00:00:00', '2020-06-10 00:00:00', 10000, 2000, '第一期：1000元\r\n第二期：10000元', ''),
	(12, '636029036753945389', '第三个个木木', '嘉兴市国土资源局秀洲分局', '汪建龙', b'010', b'0', b'0', '2016-06-01 00:00:00', '2020-06-10 00:00:00', 10000, 2000, '第一期：1000元\r\n第二期：10000元', ''),
	(13, '636029041953980105', '第四个木木木', '嘉兴市国土资源局南湖分局', '汪建龙', b'010', b'0', b'0', '2016-06-01 00:00:00', '2021-06-10 00:00:00', 10000, 1000, '第一期：1000元\r\n第二期：1000元', ''),
	(14, '636029168322235151', '一个可以做很多年很多年的项目', '嘉兴市国土资源局', '汪建龙', b'010', b'0', b'0', '2016-06-01 00:00:00', '2023-06-01 00:00:00', 10000000, 2000, '第一期：1000元\r\n第二期：10000元\r\n第三期：10000元', '陆吾科技');
/*!40000 ALTER TABLE `contracts` ENABLE KEYS */;

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
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.errands 的数据：~12 rows (大约)
DELETE FROM `errands`;
/*!40000 ALTER TABLE `errands` DISABLE KEYS */;
INSERT INTO `errands` (`ID`, `StartTime`, `EndTime`, `Peoples`, `Days`, `EID`, `Users`) VALUES
	(20, '2016-06-06 00:00:00', '2016-06-10 00:00:00', 4, 5, 1, '黄世超;俞海峰;李仙;靳小阳'),
	(21, '2016-06-20 00:00:00', '2016-06-22 00:00:00', 2, 3, 1, '尹淼;周婧扬'),
	(22, '2016-06-06 00:00:00', '2016-06-10 00:00:00', 5, 5, 2, '欧阳安蛟;张兴平;陈立定;袁洋;申屠杜平'),
	(23, '2016-06-13 00:00:00', '2016-06-17 00:00:00', 5, 5, 2, '唐尧;郑良军;赵斯思;汪建龙;赵泉'),
	(24, '2016-06-27 00:00:00', '2016-06-27 00:00:00', 4, 1, 3, '张兴平;陈立定;宋学云;孙夏阳'),
	(25, '2016-06-01 00:00:00', '2016-06-14 00:00:00', 6, 14, 4, '张兴平;陈立定;袁洋;周典;黄世超;俞海峰'),
	(26, '2016-06-08 00:00:00', '2016-06-09 00:00:00', 1, 2, 4, '吕骏杰'),
	(27, '2016-06-13 00:00:00', '2016-06-17 00:00:00', 3, 5, 5, '黄世超;俞海峰;靳小阳'),
	(28, '2016-06-20 00:00:00', '2016-06-24 00:00:00', 2, 5, 6, '孙夏阳;蒋新华'),
	(36, '2016-06-06 00:00:00', '2016-06-10 00:00:00', 7, 5, 7, '江馨;李琪;李纤茜;徐质彬;唐尧;郑良军;赵斯思'),
	(37, '2016-06-06 00:00:00', '2016-06-09 00:00:00', 2, 4, 7, '汪建龙;赵泉'),
	(39, '2016-06-20 00:00:00', '2016-06-24 00:00:00', 5, 5, 8, '宋学云;周典;周威俊;吕骏杰;孙夏阳');
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
  `Persons` varchar(1023) NOT NULL,
  `Way` varchar(255) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.evections 的数据：~8 rows (大约)
DELETE FROM `evections`;
/*!40000 ALTER TABLE `evections` DISABLE KEYS */;
INSERT INTO `evections` (`ID`, `Place`, `KiloMeters`, `Traffic`, `SubSidy`, `Hotel`, `Mark`, `Other`, `SID`, `Persons`, `Way`) VALUES
	(1, '浙江省嘉兴市南湖区', 0, 10000, 1040, 10000, '其他', 1000, 1, '黄世超;俞海峰;李仙;靳小阳;尹淼;周婧扬', '公共交通'),
	(2, '浙江省温州市平阳县', 0, 2000, 2000, 20000, '吃', 10000, 2, '欧阳安蛟;张兴平;陈立定;袁洋;申屠杜平;唐尧;郑良军;赵斯思;汪建龙;赵泉', '公共交通'),
	(3, '浙江省台州市临海市', 0, 480, 160, 0, '吃吃', 189, 3, '张兴平;陈立定;宋学云;孙夏阳', '自驾车'),
	(4, '浙江省嘉兴市许村镇', 0, 10000, 3440, 0, NULL, 0, 4, '张兴平;陈立定;袁洋;周典;黄世超;俞海峰;吕骏杰', '公共交通'),
	(5, '浙江省嘉兴市海盐县', 0, 1600, 600, 10000, NULL, 0, 5, '黄世超;俞海峰;靳小阳', '自驾车'),
	(6, '浙江省湖州市德清县', 0, 200, 400, 1000, '吃饭饭', 10000, 6, '孙夏阳;蒋新华', '公共交通'),
	(7, '浙江省温州市平阳县', 0, 3188, 1720, 3000, '吃吃', 2000, 7, '江馨;李琪;李纤茜;徐质彬;唐尧;郑良军;赵斯思;汪建龙;赵泉', '公共交通'),
	(8, '浙江省衢州市江山市', 0, 567, 1000, 0, NULL, 0, 8, '宋学云;周典;周威俊;吕骏杰;孙夏阳', '公共交通');
/*!40000 ALTER TABLE `evections` ENABLE KEYS */;

-- 导出  表 ztoptodo.invoicebill 结构
CREATE TABLE IF NOT EXISTS `invoicebill` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Price` double NOT NULL DEFAULT '0',
  `IID` int(11) NOT NULL DEFAULT '0',
  `BID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`),
  KEY `IID` (`IID`),
  KEY `BID` (`BID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.invoicebill 的数据：~1 rows (大约)
DELETE FROM `invoicebill`;
/*!40000 ALTER TABLE `invoicebill` DISABLE KEYS */;
INSERT INTO `invoicebill` (`ID`, `Price`, `IID`, `BID`) VALUES
	(1, 10000, 1, 5);
/*!40000 ALTER TABLE `invoicebill` ENABLE KEYS */;

-- 导出  表 ztoptodo.invoices 结构
CREATE TABLE IF NOT EXISTS `invoices` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Coding` varchar(255) NOT NULL DEFAULT '0',
  `Time` datetime NOT NULL,
  `Applicant` varchar(255) NOT NULL DEFAULT '0',
  `GroupName` varchar(255) DEFAULT '0',
  `ZtopCompany` bit(2) NOT NULL DEFAULT b'0',
  `OtherSideCompany` varchar(1023) NOT NULL DEFAULT '0',
  `Money` double NOT NULL DEFAULT '0',
  `Content` varchar(1023) NOT NULL DEFAULT '0',
  `FillTime` datetime DEFAULT NULL,
  `Number` varchar(1023) DEFAULT '0',
  `State` bit(3) NOT NULL DEFAULT b'0',
  `Remark` varchar(1023) DEFAULT '0',
  `CID` int(11) NOT NULL DEFAULT '0',
  `Deleted` bit(1) NOT NULL DEFAULT b'0',
  `Recevied` bit(3) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`ID`),
  KEY `CID` (`CID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.invoices 的数据：~4 rows (大约)
DELETE FROM `invoices`;
/*!40000 ALTER TABLE `invoices` DISABLE KEYS */;
INSERT INTO `invoices` (`ID`, `Coding`, `Time`, `Applicant`, `GroupName`, `ZtopCompany`, `OtherSideCompany`, `Money`, `Content`, `FillTime`, `Number`, `State`, `Remark`, `CID`, `Deleted`, `Recevied`) VALUES
	(1, '0', '2016-06-29 00:00:00', '汪建龙', '陆吾科技', b'00', '嘉兴市国土资源局秀洲分局', 10000, '第一期项目', NULL, NULL, b'000', NULL, 6, b'1', b'010'),
	(2, '0', '2016-06-30 00:00:00', '汪建龙', '陆吾科技', b'00', '嘉兴市国土资源局秀洲分局', 5000, '第一期项目', NULL, NULL, b'000', NULL, 7, b'0', b'000'),
	(3, '636028778685116720', '2016-06-30 00:00:00', '汪建龙', '陆吾科技', b'00', '嘉兴市国土资源局秀洲分局', 5000, '第二期项目', NULL, NULL, b'000', NULL, 7, b'0', b'000'),
	(4, '636028971595630631', '2016-06-30 00:00:00', '汪建龙', '陆吾科技', b'00', '嘉兴市国土资源局南湖分局', 5000, '第一期', NULL, NULL, b'000', NULL, 9, b'0', b'000');
/*!40000 ALTER TABLE `invoices` ENABLE KEYS */;

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
  `ContentID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`),
  KEY `UserID` (`ReceiverID`),
  KEY `CreateTime` (`CreateTime`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.notification 的数据：~18 rows (大约)
DELETE FROM `notification`;
/*!40000 ALTER TABLE `notification` DISABLE KEYS */;
INSERT INTO `notification` (`ID`, `ReceiverID`, `SenderID`, `InfoID`, `InfoType`, `HasRead`, `CreateTime`, `ContentID`) VALUES
	(1, 22, 29, 1, 1, b'0', '2016-05-19 11:22:30', 0),
	(2, 27, 29, 2, 1, b'0', '2016-05-19 11:22:30', 0),
	(3, 25, 29, 3, 1, b'0', '2016-05-19 11:22:30', 0),
	(4, 23, 29, 4, 1, b'0', '2016-05-19 11:22:30', 0),
	(5, 29, 29, 5, 1, b'1', '2016-05-19 11:22:30', 0),
	(6, 24, 29, 6, 1, b'0', '2016-05-19 14:37:06', 0),
	(7, 22, 29, 7, 1, b'0', '2016-05-19 14:37:06', 0),
	(8, 27, 29, 8, 1, b'0', '2016-05-19 14:37:06', 0),
	(9, 29, 29, 9, 1, b'1', '2016-05-19 14:37:06', 0),
	(10, 24, 29, 9, 2, b'0', '2016-05-31 09:51:14', 1),
	(11, 22, 29, 9, 2, b'0', '2016-05-31 09:51:14', 1),
	(12, 27, 29, 9, 2, b'0', '2016-05-31 09:51:14', 1),
	(13, 24, 29, 9, 2, b'0', '2016-05-31 09:56:56', 4),
	(14, 22, 29, 9, 2, b'0', '2016-05-31 09:56:56', 4),
	(15, 27, 29, 9, 2, b'0', '2016-05-31 09:56:56', 4),
	(16, 24, 29, 9, 2, b'0', '2016-05-31 09:57:13', 5),
	(17, 22, 29, 9, 2, b'0', '2016-05-31 09:57:13', 5),
	(18, 27, 29, 9, 2, b'0', '2016-05-31 09:57:13', 5);
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.serialnumbers 的数据：~10 rows (大约)
DELETE FROM `serialnumbers`;
/*!40000 ALTER TABLE `serialnumbers` DISABLE KEYS */;
INSERT INTO `serialnumbers` (`ID`, `Number`, `BarCode`, `NumberExt`, `SID`, `Name`, `Invoices`) VALUES
	(1, '201606', 'BarCodes\\636027094817009400.png', 1, 1, '汪建龙', b'000'),
	(2, '201606', 'BarCodes\\636027191130042610.png', 2, 2, '汪建龙', b'000'),
	(3, '201606', 'BarCodes\\636027193942198461.png', 3, 3, '汪建龙', b'000'),
	(4, '201606', 'BarCodes\\636027202395388821.png', 4, 4, '汪建龙', b'000'),
	(5, '201606', 'BarCodes\\636027209110154951.png', 5, 5, '汪建龙', b'000'),
	(6, '201606', 'BarCodes\\636027210464900034.png', 6, 6, '汪建龙', b'000'),
	(7, '201606', 'BarCodes\\636027226238982516.png', 7, 7, '彭盛状', b'000'),
	(8, '201606', 'BarCodes\\636027226983840384.png', 8, 8, '彭盛状', b'000'),
	(9, '201606', 'BarCodes\\636027305938831208.png', 9, 0, '彭盛状', b'000'),
	(10, '201606', 'BarCodes\\636027875285894109.png', 10, 0, '靳小阳', b'000');
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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.sheets 的数据：~8 rows (大约)
DELETE FROM `sheets`;
/*!40000 ALTER TABLE `sheets` DISABLE KEYS */;
INSERT INTO `sheets` (`ID`, `Name`, `Time`, `Count`, `Money`, `Status`, `Controler`, `Remarks`, `Deleted`, `Type`, `Checkers`, `CheckTime`) VALUES
	(1, '汪建龙', '2016-06-28 00:00:00', 10, 22040, b'011', '靳小阳', NULL, b'0', b'001', '彭盛状', NULL),
	(2, '汪建龙', '2016-06-28 00:00:00', 10, 34000, b'011', '靳小阳', NULL, b'0', b'001', '彭盛状', NULL),
	(3, '汪建龙', '2016-06-28 00:00:00', 2, 829, b'011', '靳小阳', NULL, b'0', b'001', '彭盛状', NULL),
	(4, '汪建龙', '2016-06-28 00:00:00', 10, 13440, b'011', '靳小阳', NULL, b'0', b'001', '彭盛状', NULL),
	(5, '汪建龙', '2016-06-28 00:00:00', 10, 12200, b'011', '靳小阳', NULL, b'0', b'001', '彭盛状', NULL),
	(6, '汪建龙', '2016-06-28 00:00:00', 5, 11600, b'011', '靳小阳', NULL, b'0', b'001', '彭盛状', NULL),
	(7, '彭盛状', '2016-06-28 00:00:00', 5, 9908, b'011', '靳小阳', NULL, b'0', b'001', '彭盛状', NULL),
	(8, '彭盛状', '2016-06-28 00:00:00', 4, 1567, b'100', '申屠杜平', NULL, b'0', b'001', '彭盛状', '2016-06-28 17:52:32');
/*!40000 ALTER TABLE `sheets` ENABLE KEYS */;

-- 导出  表 ztoptodo.substancs 结构
CREATE TABLE IF NOT EXISTS `substancs` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Category` int(2) NOT NULL DEFAULT '0',
  `SecondCategory` int(2) DEFAULT '0',
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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.task 的数据：~2 rows (大约)
DELETE FROM `task`;
/*!40000 ALTER TABLE `task` DISABLE KEYS */;
INSERT INTO `task` (`ID`, `Title`, `Content`, `ScheduledTime`, `CreatorID`, `ParentID`, `CreateTime`, `Deleted`) VALUES
	(1, '手机页面测试使用下达一个任务', '测试手机', '2016-05-20 00:00:00', 29, 0, '2016-05-19 11:22:30', b'1'),
	(2, '899', '就卡打', '2016-05-20 00:00:00', 29, 0, '2016-05-19 14:37:06', b'0');
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
  `Type` bit(10) NOT NULL,
  `Cost` double NOT NULL,
  `Toll` double NOT NULL,
  `Petrol` double NOT NULL,
  `Driver` bit(3) NOT NULL,
  `CarPetty` double NOT NULL,
  `Plate` varchar(255) DEFAULT NULL,
  `Times` int(11) NOT NULL DEFAULT '0',
  `KiloMeters` double DEFAULT '0',
  `EID` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.traffic 的数据：~11 rows (大约)
DELETE FROM `traffic`;
/*!40000 ALTER TABLE `traffic` DISABLE KEYS */;
INSERT INTO `traffic` (`ID`, `Type`, `Cost`, `Toll`, `Petrol`, `Driver`, `CarPetty`, `Plate`, `Times`, `KiloMeters`, `EID`) VALUES
	(16, b'00000001', 10000, 0, 0, b'000', 0, NULL, 0, 0, 1),
	(17, b'00000001', 1000, 0, 0, b'000', 0, NULL, 0, 0, 2),
	(18, b'00000010', 1000, 0, 0, b'000', 0, NULL, 0, 0, 2),
	(19, b'00000111', 480, 300, 100, b'001', 80, '浙AZN022', 0, 400, 3),
	(20, b'00000001', 10000, 0, 0, b'000', 0, NULL, 0, 0, 4),
	(21, b'00000111', 1600, 500, 1000, b'001', 100, '浙AZN022', 0, 500, 5),
	(22, b'00000010', 200, 0, 0, b'000', 0, NULL, 0, 0, 6),
	(32, b'00000001', 2345, 0, 0, b'000', 0, NULL, 0, 0, 7),
	(33, b'00000010', 300, 0, 0, b'000', 0, NULL, 0, 0, 7),
	(34, b'00000011', 543, 0, 0, b'000', 0, NULL, 0, 0, 7),
	(36, b'00000001', 567, 0, 0, b'000', 0, NULL, 0, 0, 8);
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

-- 导出  视图 ztoptodo.user_group_view 结构
-- 创建临时表以解决视图依赖性错误
CREATE TABLE `user_group_view` (
	`ID` INT(11) NOT NULL,
	`RealName` VARCHAR(50) NULL COLLATE 'utf8_general_ci',
	`Name` VARCHAR(128) NOT NULL COLLATE 'utf8_general_ci'
) ENGINE=MyISAM;

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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.user_task 的数据：~9 rows (大约)
DELETE FROM `user_task`;
/*!40000 ALTER TABLE `user_task` DISABLE KEYS */;
INSERT INTO `user_task` (`ID`, `UserID`, `TaskID`, `CreateTime`, `CompletedTime`, `HasRead`, `Deleted`) VALUES
	(1, 22, 1, '2016-05-19 11:22:30', NULL, b'0', b'0'),
	(2, 27, 1, '2016-05-19 11:22:30', NULL, b'0', b'0'),
	(3, 25, 1, '2016-05-19 11:22:30', NULL, b'0', b'0'),
	(4, 23, 1, '2016-05-19 11:22:30', NULL, b'0', b'0'),
	(5, 29, 1, '2016-05-19 11:22:30', NULL, b'1', b'1'),
	(6, 24, 2, '2016-05-19 14:37:06', NULL, b'0', b'0'),
	(7, 22, 2, '2016-05-19 14:37:06', NULL, b'0', b'0'),
	(8, 27, 2, '2016-05-19 14:37:06', NULL, b'0', b'0'),
	(9, 29, 2, '2016-05-19 14:37:06', NULL, b'1', b'0');
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
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8;

-- 正在导出表  ztoptodo.verifys 的数据：~25 rows (大约)
DELETE FROM `verifys`;
/*!40000 ALTER TABLE `verifys` DISABLE KEYS */;
INSERT INTO `verifys` (`ID`, `Step`, `Time`, `Name`, `Reason`, `Position`, `SID`) VALUES
	(1, 0, '2016-06-28 13:59:44', '汪建龙', NULL, 0, 2),
	(2, 0, '2016-06-28 14:00:04', '汪建龙', NULL, 0, 1),
	(3, 0, '2016-06-28 14:32:10', '汪建龙', NULL, 0, 6),
	(4, 0, '2016-06-28 14:38:18', '汪建龙', NULL, 0, 3),
	(5, 0, '2016-06-28 14:38:31', '汪建龙', NULL, 0, 4),
	(6, 0, '2016-06-28 14:38:39', '汪建龙', NULL, 0, 5),
	(7, 1, '2016-06-28 14:49:11', '彭盛状', NULL, 0, 1),
	(8, 1, '2016-06-28 14:53:04', '彭盛状', NULL, 0, 2),
	(9, 1, '2016-06-28 14:53:10', '彭盛状', NULL, 0, 3),
	(10, 1, '2016-06-28 14:53:15', '彭盛状', NULL, 0, 4),
	(11, 1, '2016-06-28 14:53:21', '彭盛状', NULL, 0, 5),
	(12, 1, '2016-06-28 14:53:26', '彭盛状', NULL, 0, 6),
	(13, 2, '2016-06-28 14:56:31', '申屠杜平', NULL, 0, 1),
	(14, 2, '2016-06-28 14:56:36', '申屠杜平', NULL, 0, 2),
	(15, 2, '2016-06-28 14:56:41', '申屠杜平', NULL, 0, 3),
	(16, 2, '2016-06-28 14:56:45', '申屠杜平', NULL, 0, 4),
	(17, 2, '2016-06-28 14:56:50', '申屠杜平', NULL, 0, 5),
	(18, 2, '2016-06-28 14:56:54', '申屠杜平', NULL, 0, 6),
	(19, 0, '2016-06-28 14:59:27', '彭盛状', NULL, 0, 7),
	(20, 1, '2016-06-28 14:59:44', '彭盛状', NULL, 0, 7),
	(21, 0, '2016-06-28 14:59:52', '彭盛状', NULL, 0, 8),
	(22, 1, '2016-06-28 15:00:15', '彭盛状', NULL, 0, 8),
	(23, 2, '2016-06-28 17:11:13', '申屠杜平', NULL, 0, 7),
	(24, 2, '2016-06-28 17:11:18', '申屠杜平', NULL, 0, 8),
	(25, 3, '2016-06-28 17:52:32', '靳小阳', NULL, 0, 8);
/*!40000 ALTER TABLE `verifys` ENABLE KEYS */;

-- 导出  视图 ztoptodo.user_group_view 结构
-- 移除临时表并创建最终视图结构
DROP TABLE IF EXISTS `user_group_view`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` VIEW `user_group_view` AS SELECT user.ID,user.RealName,user_group.Name from user inner join user_group on user.GroupID=user_group.ID ;

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

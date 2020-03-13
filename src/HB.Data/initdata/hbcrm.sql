/*
SQLyog Ultimate v12.09 (64 bit)
MySQL - 5.6.17 : Database - hbcrm
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`hbcrm` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `hbcrm`;

/*Table structure for table `__efmigrationshistory` */

DROP TABLE IF EXISTS `__efmigrationshistory`;

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `__efmigrationshistory` */

LOCK TABLES `__efmigrationshistory` WRITE;

insert  into `__efmigrationshistory`(`MigrationId`,`ProductVersion`) values ('20191022021302_init','2.2.6-servicing-10079');

UNLOCK TABLES;

/*Table structure for table `meb_user` */

DROP TABLE IF EXISTS `meb_user`;

CREATE TABLE `meb_user` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(50) NOT NULL,
  `Password` varchar(50) NOT NULL,
  `NickName` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

/*Data for the table `meb_user` */

LOCK TABLES `meb_user` WRITE;

insert  into `meb_user`(`Id`,`UserName`,`Password`,`NickName`) values (1,'lily','e10adc3949ba59abbe56e057f20f883e','莉莉');

UNLOCK TABLES;

/*Table structure for table `sys_admin` */

DROP TABLE IF EXISTS `sys_admin`;

CREATE TABLE `sys_admin` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreateBy` int(11) NOT NULL,
  `CreatebyName` varchar(50) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `LastUpdateBy` int(11) NOT NULL,
  `LastUpdateByName` varchar(50) NOT NULL,
  `LastUpdateDate` datetime(6) NOT NULL,
  `Guid` varchar(50) NOT NULL,
  `UserName` varchar(50) NOT NULL,
  `Password` varchar(50) NOT NULL,
  `NickName` varchar(50) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `MobilePhone` varchar(50) DEFAULT NULL,
  `QQ` varchar(50) DEFAULT NULL,
  `WeChar` varchar(50) DEFAULT NULL,
  `Status` int(11) NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;

/*Data for the table `sys_admin` */

LOCK TABLES `sys_admin` WRITE;

insert  into `sys_admin`(`Id`,`CreateBy`,`CreatebyName`,`CreateDate`,`LastUpdateBy`,`LastUpdateByName`,`LastUpdateDate`,`Guid`,`UserName`,`Password`,`NickName`,`Email`,`MobilePhone`,`QQ`,`WeChar`,`Status`) values (1,0,'init','2019-09-23 11:02:15.000000',0,'init','2019-09-23 11:02:15.000000','6fa8a7b3-782a-4ca4-b6ca-a49494746de9','Admin','e10adc3949ba59abbe56e057f20f883e','老黑','2485414172@qq.com','18682023172','2485414172','2485414172',1),(2,1,'Admin','2019-09-23 11:05:37.000000',1,'Admin','2019-09-23 11:05:37.000000','cabfda64-acb6-4899-b802-59b0aee72ff5','lily','e10adc3949ba59abbe56e057f20f883e','老黑','2485414172@qq.com','18682023172','2485414172','2485414172',1),(3,1,'Admin','2019-09-23 11:05:37.000000',1,'Admin','2019-11-21 15:38:48.232346','cabfda64-acb6-4899-b802-59b0aee72ff5','lily001','e10adc3949ba59abbe56e057f20f883e','丽丽','2485414172@qq.com','18682023172','2485414172','2485414172',1),(4,1,'Admin','2019-09-23 11:05:37.000000',1,'Admin','2019-09-23 11:05:37.000000','cabfda64-acb6-4899-b802-59b0aee72ff5','lily002','e10adc3949ba59abbe56e057f20f883e','莉莉','2485414172@qq.com','18682023172','2485414172','2485414172',1),(5,1,'Admin','2019-09-23 11:05:37.000000',1,'Admin','2019-09-23 11:05:37.000000','cabfda64-acb6-4899-b802-59b0aee72ff5','lily003','e10adc3949ba59abbe56e057f20f883e','莉莉','2485414172@qq.com','18682023172','2485414172','2485414172',1),(6,1,'Admin','2019-09-23 11:05:37.000000',1,'Admin','2019-09-23 11:05:37.000000','cabfda64-acb6-4899-b802-59b0aee72ff5','lily004','e10adc3949ba59abbe56e057f20f883e','莉莉','2485414172@qq.com','18682023172','2485414172','2485414172',1),(7,1,'Admin','2019-09-23 11:05:37.000000',1,'Admin','2019-09-23 11:05:37.000000','cabfda64-acb6-4899-b802-59b0aee72ff5','lily005','e10adc3949ba59abbe56e057f20f883e','莉莉','2485414172@qq.com','18682023172','2485414172','2485414172',1),(8,1,'Admin','2019-09-23 11:05:37.000000',1,'Admin','2019-09-23 11:05:37.000000','cabfda64-acb6-4899-b802-59b0aee72ff5','lily006','e10adc3949ba59abbe56e057f20f883e','莉莉','2485414172@qq.com','18682023172','2485414172','2485414172',1),(9,1,'Admin','2019-09-23 11:05:37.000000',1,'Admin','2019-09-23 11:05:37.000000','cabfda64-acb6-4899-b802-59b0aee72ff5','lily007','e10adc3949ba59abbe56e057f20f883e','莉莉','2485414172@qq.com','18682023172','2485414172','2485414172',1),(10,1,'Admin','2019-09-23 11:05:37.000000',1,'Admin','2019-09-23 11:05:37.000000','cabfda64-acb6-4899-b802-59b0aee72ff5','lily008','e10adc3949ba59abbe56e057f20f883e','莉莉','2485414172@qq.com','18682023172','2485414172','2485414172',1),(11,1,'Admin','2019-09-23 11:05:37.000000',1,'Admin','2019-09-23 11:05:37.000000','cabfda64-acb6-4899-b802-59b0aee72ff5','lily009','e10adc3949ba59abbe56e057f20f883e','莉莉','2485414172@qq.com','18682023172','2485414172','2485414172',1),(12,1,'Admin','2019-09-23 11:05:37.000000',1,'Admin','2019-09-23 11:05:37.000000','cabfda64-acb6-4899-b802-59b0aee72ff5','lily010','e10adc3949ba59abbe56e057f20f883e','莉莉','2485414172@qq.com','18682023172','2485414172','2485414172',1),(16,1,'Admin','2019-11-21 16:13:31.861523',1,'Admin','2019-11-21 16:14:18.658200','b0871b35-2b93-4d27-ac9b-e785276cfb1d','lily10','e10adc3949ba59abbe56e057f20f883e','丽丽','2485414172@qq.com','2485414172','2485414172','2485414172',0),(17,1,'Admin','2019-11-21 16:37:49.447892',1,'Admin','2019-11-21 16:37:49.447892','14d5cf0d-5ff7-4986-9096-e0f5ab830c29','lily011','e10adc3949ba59abbe56e057f20f883e','丽丽','2485414172@qq.com','2485414172','2485414172','2485414172',0);

UNLOCK TABLES;

/*Table structure for table `sys_adminrole` */

DROP TABLE IF EXISTS `sys_adminrole`;

CREATE TABLE `sys_adminrole` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreateBy` int(11) NOT NULL,
  `CreatebyName` varchar(50) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `LastUpdateBy` int(11) NOT NULL,
  `LastUpdateByName` varchar(50) NOT NULL,
  `LastUpdateDate` datetime(6) NOT NULL,
  `AdminId` int(11) NOT NULL,
  `RoleId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_sys_adminrole_AdminId` (`AdminId`),
  KEY `IX_sys_adminrole_RoleId` (`RoleId`),
  CONSTRAINT `FK_sys_adminrole_sys_admin_AdminId` FOREIGN KEY (`AdminId`) REFERENCES `sys_admin` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_sys_adminrole_sys_role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `sys_role` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8;

/*Data for the table `sys_adminrole` */

LOCK TABLES `sys_adminrole` WRITE;

insert  into `sys_adminrole`(`Id`,`CreateBy`,`CreatebyName`,`CreateDate`,`LastUpdateBy`,`LastUpdateByName`,`LastUpdateDate`,`AdminId`,`RoleId`) values (1,1,'Admin','2019-09-23 11:07:39.000000',1,'Admin','2019-09-23 11:07:39.000000',1,1),(2,1,'Admin','2019-09-23 11:07:39.000000',1,'Admin','2019-09-23 11:07:39.000000',2,1),(11,1,'Admin','2019-10-25 16:30:35.565254',1,'Admin','2019-10-25 16:30:35.565254',1,3),(20,1,'Admin','2019-11-21 15:38:48.232346',1,'Admin','2019-11-21 15:38:48.232346',3,1),(21,1,'Admin','2019-11-21 15:38:48.232346',1,'Admin','2019-11-21 15:38:48.232346',3,2),(26,1,'Admin','2019-11-21 16:14:18.658200',1,'Admin','2019-11-21 16:14:18.658200',16,1),(27,1,'Admin','2019-11-21 16:37:49.447892',1,'Admin','2019-11-21 16:37:49.447892',17,1),(28,1,'Admin','2019-11-21 16:37:49.447892',1,'Admin','2019-11-21 16:37:49.447892',17,2),(29,1,'Admin','2019-11-21 16:37:49.447892',1,'Admin','2019-11-21 16:37:49.447892',17,3);

UNLOCK TABLES;

/*Table structure for table `sys_apiuser` */

DROP TABLE IF EXISTS `sys_apiuser`;

CREATE TABLE `sys_apiuser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AppId` int(11) NOT NULL,
  `UserKey` varchar(50) NOT NULL,
  `IpAddress` varchar(1000) DEFAULT NULL,
  `Remark` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

/*Data for the table `sys_apiuser` */

LOCK TABLES `sys_apiuser` WRITE;

insert  into `sys_apiuser`(`Id`,`AppId`,`UserKey`,`IpAddress`,`Remark`) values (1,123456,'sfdsafsdf',NULL,NULL);

UNLOCK TABLES;

/*Table structure for table `sys_menu` */

DROP TABLE IF EXISTS `sys_menu`;

CREATE TABLE `sys_menu` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreateBy` int(11) NOT NULL,
  `CreatebyName` varchar(50) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `LastUpdateBy` int(11) NOT NULL,
  `LastUpdateByName` varchar(50) NOT NULL,
  `LastUpdateDate` datetime(6) NOT NULL,
  `MenuName` varchar(50) NOT NULL,
  `MenuSystermName` varchar(255) NOT NULL,
  `MenuUrl` varchar(255) DEFAULT NULL,
  `ParentMenuId` int(11) NOT NULL DEFAULT '0',
  `Type` int(11) NOT NULL DEFAULT '1',
  `MenuIcon` varchar(50) DEFAULT NULL,
  `MenuSort` int(11) NOT NULL DEFAULT '0',
  `MenuRemark` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8;

/*Data for the table `sys_menu` */

LOCK TABLES `sys_menu` WRITE;

insert  into `sys_menu`(`Id`,`CreateBy`,`CreatebyName`,`CreateDate`,`LastUpdateBy`,`LastUpdateByName`,`LastUpdateDate`,`MenuName`,`MenuSystermName`,`MenuUrl`,`ParentMenuId`,`Type`,`MenuIcon`,`MenuSort`,`MenuRemark`) values (1,1,'Admin','2019-09-25 16:24:19.000000',1,'Admin','2019-09-25 16:24:19.000000','设置','Setting','#',0,1,'fas fa-th',0,NULL),(2,1,'Admin','2019-09-25 16:24:19.000000',1,'Admin','2019-09-25 16:24:19.000000','账号权限','Setting.Admin','#',1,1,'fas fa-th',0,NULL),(3,1,'Admin','2019-09-25 16:24:19.000000',1,'Admin','2019-09-25 16:24:19.000000','账号管理','Setting.Admin.Account','/Account/Index',2,1,'fas fa-th',0,NULL),(4,1,'Admin','2019-09-25 16:24:19.000000',1,'Admin','2019-10-24 13:31:43.467050','菜单管理（高级）','Setting.Admin.Menu','/Menu/Index',2,1,'fas fa-th',2,NULL),(5,1,'Admin','2019-10-12 14:51:29.589437',1,'Admin','2019-10-12 14:51:29.589437','角色管理','Setting.Admin.Role','/Role/Index',2,1,'fas fa-th',1,''),(7,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','查看菜单','MenuView','#',4,2,'fas fa-th',0,NULL),(8,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','新增菜单','MenuAdd','#',4,2,'fas fa-th',1,NULL),(9,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','修改菜单','MenuEdit','#',4,2,'fas fa-th',2,NULL),(10,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','删除菜单','MenuDelete','#',4,2,'fas fa-th',3,NULL),(11,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','查看账号','AdminView','#',3,2,'fas fa-th',0,NULL),(12,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','新增账号','AdminAdd','#',3,2,'fas fa-th',1,NULL),(13,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','修改账号','AdminEdit','#',3,2,'fas fa-th',2,NULL),(14,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','删除账号','AdminDelete','#',3,2,'fas fa-th',3,NULL),(15,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','查看角色','RoleView','#',5,2,'fas fa-th',0,NULL),(16,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','新增角色','RoleAdd','#',5,2,'fas fa-th',1,NULL),(17,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','修改角色','RoleEdit','#',5,2,'fas fa-th',2,NULL),(18,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','删除角色','RoleDelete','#',5,2,'fas fa-th',3,NULL),(19,1,'Admin','2019-10-23 10:52:14.996169',1,'Admin','2019-10-23 10:52:14.996169','运维','Operations','#',0,1,'fas fa-th',0,'运维管理'),(20,1,'Admin','2019-10-23 11:03:36.585509',1,'Admin','2019-10-23 11:03:36.585509','订单管理','Operations.Order','#',19,1,'fas fa-th',0,'各类订单管理'),(21,1,'Admin','2019-10-23 11:10:54.780756',1,'Admin','2019-10-23 11:10:54.780756','主订单管理','Operations.Order.MainOrder','#',20,1,'fas fa-th',0,'主订单的管理'),(22,1,'Admin','2019-10-23 11:14:11.221413',1,'Admin','2019-10-23 11:14:11.221413','欠费订单管理','Operations.Order.ArrearsOrder','#',20,1,'fas fa-th',0,'欠费订单管理'),(24,1,'Admin','2019-10-23 15:26:36.225742',1,'Admin','2019-10-23 16:11:54.485217','主订单查看','Operations.Order.MainOrder.View','#',21,2,'fas fa-th',0,'主订单查看权限2'),(25,1,'Admin','2019-10-24 09:05:52.089684',1,'Admin','2019-10-24 09:05:52.089684','角色权限分配','RolePermission','#',5,2,'fas fa-th',4,'给角色分配对应的权限'),(26,1,'Admin','2019-09-23 17:12:32.000000',1,'Admin','2019-09-23 17:12:32.000000','查看权限','AdminPermission','#',3,2,'fas fa-th',4,'查看账号拥有的权限');

UNLOCK TABLES;

/*Table structure for table `sys_menurole` */

DROP TABLE IF EXISTS `sys_menurole`;

CREATE TABLE `sys_menurole` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreateBy` int(11) NOT NULL,
  `CreatebyName` varchar(50) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `LastUpdateBy` int(11) NOT NULL,
  `LastUpdateByName` varchar(50) NOT NULL,
  `LastUpdateDate` datetime(6) NOT NULL,
  `MenuId` int(11) NOT NULL,
  `RoleId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_sys_menurole_MenuId` (`MenuId`),
  KEY `IX_sys_menurole_RoleId` (`RoleId`),
  CONSTRAINT `FK_sys_menurole_sys_menu_MenuId` FOREIGN KEY (`MenuId`) REFERENCES `sys_menu` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_sys_menurole_sys_role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `sys_role` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8;

/*Data for the table `sys_menurole` */

LOCK TABLES `sys_menurole` WRITE;

insert  into `sys_menurole`(`Id`,`CreateBy`,`CreatebyName`,`CreateDate`,`LastUpdateBy`,`LastUpdateByName`,`LastUpdateDate`,`MenuId`,`RoleId`) values (1,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',1,1),(2,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',2,1),(3,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',3,1),(4,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',4,1),(5,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',5,1),(7,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',7,1),(8,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',8,1),(9,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',9,1),(10,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',10,1),(11,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',11,1),(12,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',12,1),(13,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',13,1),(14,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',14,1),(15,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',15,1),(16,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',16,1),(18,1,'Admin','2019-09-25 16:25:44.000000',1,'Admin','2019-09-25 16:25:44.000000',25,1),(19,1,'Admin','2019-10-24 13:26:27.441975',1,'Admin','2019-10-24 13:26:27.441975',18,1),(21,1,'Admin','2019-10-24 13:28:23.647621',1,'Admin','2019-10-24 13:28:23.647621',17,1),(26,1,'Admin','2019-10-24 13:28:23.647621',1,'Admin','2019-10-24 13:28:23.647621',26,1),(27,1,'Admin','2019-10-25 17:31:27.492132',1,'Admin','2019-10-25 17:31:27.492132',19,1),(28,1,'Admin','2019-10-25 17:31:27.492132',1,'Admin','2019-10-25 17:31:27.492132',20,1),(29,1,'Admin','2019-10-25 17:31:27.492132',1,'Admin','2019-10-25 17:31:27.492132',21,1),(30,1,'Admin','2019-10-25 17:31:27.492132',1,'Admin','2019-10-25 17:31:27.492132',22,1),(31,1,'Admin','2019-10-25 17:31:27.492132',1,'Admin','2019-10-25 17:31:27.492132',24,1);

UNLOCK TABLES;

/*Table structure for table `sys_role` */

DROP TABLE IF EXISTS `sys_role`;

CREATE TABLE `sys_role` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CreateBy` int(11) NOT NULL,
  `CreatebyName` varchar(50) NOT NULL,
  `CreateDate` datetime(6) NOT NULL,
  `LastUpdateBy` int(11) NOT NULL,
  `LastUpdateByName` varchar(50) NOT NULL,
  `LastUpdateDate` datetime(6) NOT NULL,
  `RoleName` varchar(50) NOT NULL,
  `Status` int(11) NOT NULL DEFAULT '1',
  `RoleRemark` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

/*Data for the table `sys_role` */

LOCK TABLES `sys_role` WRITE;

insert  into `sys_role`(`Id`,`CreateBy`,`CreatebyName`,`CreateDate`,`LastUpdateBy`,`LastUpdateByName`,`LastUpdateDate`,`RoleName`,`Status`,`RoleRemark`) values (1,1,'Admin','2019-09-23 11:06:27.000000',1,'Admin','2019-10-25 17:31:27.492132','超级管理员',1,NULL),(2,1,'Admin','2019-10-24 08:44:29.210307',1,'Admin','2019-10-24 08:44:29.210307','订单管理专员',1,'专门管理订单'),(3,1,'Admin','2019-10-25 16:30:35.565254',1,'Admin','2019-10-25 16:30:35.565254','财务',1,'查看报表'),(5,1,'Admin','2019-10-25 16:31:26.932192',1,'Admin','2019-10-25 17:23:05.565424','2222',2,'2222');

UNLOCK TABLES;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

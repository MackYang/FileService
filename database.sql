-- --------------------------------------------------------
-- 主机:                           127.0.0.1
-- 服务器版本:                        10.0.17-MariaDB - mariadb.org binary distribution
-- 服务器操作系统:                      Win64
-- HeidiSQL 版本:                  9.1.0.4867
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 导出 file_server 的数据库结构
CREATE DATABASE IF NOT EXISTS `file_server` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `file_server`;


-- 导出  表 file_server.biz_file 结构
CREATE TABLE IF NOT EXISTS `biz_file` (
  `id` varchar(36) NOT NULL COMMENT '主键',
  `file_name` varchar(100) NOT NULL COMMENT '文件名称',
  `file_type` varchar(36) NOT NULL COMMENT '文件类型',
  `file_size` bigint(20) NOT NULL,
  `content_type` varchar(100) NOT NULL COMMENT '内容类型',
  `ref_id` varchar(36) DEFAULT NULL COMMENT '引用记录的ID,如果上传的文件已存在,直接引用首次上传的记录ID',
  `url` varchar(255) NOT NULL DEFAULT '0' COMMENT '文件路径,不含服务器前缀,直接从目录开始',
  `md5` varchar(32) NOT NULL COMMENT '文件的MD5值,通过此值判断文件是否已存在',
  `width` smallint(6) NOT NULL COMMENT '图片宽度,单位像素,非图片填写0',
  `height` smallint(6) NOT NULL COMMENT '图片高度,单位像素,非图片填写0',
  `create_id` varchar(36) DEFAULT NULL,
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `biz_system_id` varchar(36) NOT NULL COMMENT '业务系统ID',
  `is_temp` tinyint(4) NOT NULL DEFAULT '0' COMMENT '是否是临时文件',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- 数据导出被取消选择。


-- 导出  表 file_server.biz_system 结构
CREATE TABLE IF NOT EXISTS `biz_system` (
  `id` varchar(50) NOT NULL,
  `name` varchar(50) NOT NULL COMMENT '业务系统名称',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `enable` tinyint(4) NOT NULL DEFAULT '1' COMMENT '是否启用',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- 数据导出被取消选择。


-- 导出  表 file_server.sys_log 结构
CREATE TABLE IF NOT EXISTS `sys_log` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `PLATFORM` varchar(10) DEFAULT NULL,
  `LOGTIME` varchar(25) DEFAULT NULL,
  `THREAD` varchar(10) DEFAULT NULL,
  `LOGLEVEL` varchar(10) DEFAULT NULL,
  `LOGGER` varchar(2000) DEFAULT NULL,
  `LOGMESSAGE` text,
  KEY `ID` (`ID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

-- 数据导出被取消选择。
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

USE [master]
GO
/****** 对象:  Database [AppSystem]    脚本日期: 03/01/2014 12:37:31 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'AppSystem')
BEGIN
CREATE DATABASE [AppSystem] ON  PRIMARY 
( NAME = N'AppSystem', FILENAME = N'D:\sql\EAMS\AppSystem.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AppSystem_log', FILENAME = N'D:\sql\EAMS\AppSystem_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'AppSystem', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AppSystem].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [AppSystem] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [AppSystem] SET ANSI_NULLS OFF
GO
ALTER DATABASE [AppSystem] SET ANSI_PADDING OFF
GO
ALTER DATABASE [AppSystem] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [AppSystem] SET ARITHABORT OFF
GO
ALTER DATABASE [AppSystem] SET AUTO_CLOSE ON
GO
ALTER DATABASE [AppSystem] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [AppSystem] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [AppSystem] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [AppSystem] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [AppSystem] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [AppSystem] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [AppSystem] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [AppSystem] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [AppSystem] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [AppSystem] SET  DISABLE_BROKER
GO
ALTER DATABASE [AppSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [AppSystem] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [AppSystem] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [AppSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [AppSystem] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [AppSystem] SET  READ_WRITE
GO
ALTER DATABASE [AppSystem] SET RECOVERY SIMPLE
GO
ALTER DATABASE [AppSystem] SET  MULTI_USER
GO
ALTER DATABASE [AppSystem] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [AppSystem] SET DB_CHAINING OFF
GO
USE [AppSystem]
GO
/****** 对象:  ForeignKey [FK__Logs__iActionId__21B6055D]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Logs__iActionId__21B6055D]') AND parent_object_id = OBJECT_ID(N'[dbo].[Logs]'))
ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [FK__Logs__iActionId__21B6055D]
GO
/****** 对象:  ForeignKey [FK_Logs_Functions]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Logs_Functions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Logs]'))
ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [FK_Logs_Functions]
GO
/****** 对象:  ForeignKey [FK_Logs_User]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Logs_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Logs]'))
ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [FK_Logs_User]
GO
/****** 对象:  ForeignKey [FK__Permissions__iId__2B3F6F97]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Permissions__iId__2B3F6F97]') AND parent_object_id = OBJECT_ID(N'[dbo].[Permissions]'))
ALTER TABLE [dbo].[Permissions] DROP CONSTRAINT [FK__Permissions__iId__2B3F6F97]
GO
/****** 对象:  ForeignKey [FK__Permissions__iId__2C3393D0]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Permissions__iId__2C3393D0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Permissions]'))
ALTER TABLE [dbo].[Permissions] DROP CONSTRAINT [FK__Permissions__iId__2C3393D0]
GO
/****** 对象:  ForeignKey [FK_RolePermissions_Actions]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolePermissions_Actions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Permissions]'))
ALTER TABLE [dbo].[Permissions] DROP CONSTRAINT [FK_RolePermissions_Actions]
GO
/****** 对象:  ForeignKey [FK_RolePermissions_Functions]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolePermissions_Functions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Permissions]'))
ALTER TABLE [dbo].[Permissions] DROP CONSTRAINT [FK_RolePermissions_Functions]
GO
/****** 对象:  ForeignKey [FK_UserExt_User]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserExt_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserExt]'))
ALTER TABLE [dbo].[UserExt] DROP CONSTRAINT [FK_UserExt_User]
GO
/****** 对象:  ForeignKey [UserRoleRefRoleId]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[UserRoleRefRoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRoleRef]'))
ALTER TABLE [dbo].[UserRoleRef] DROP CONSTRAINT [UserRoleRefRoleId]
GO
/****** 对象:  ForeignKey [UserRoleRefUserId]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[UserRoleRefUserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRoleRef]'))
ALTER TABLE [dbo].[UserRoleRef] DROP CONSTRAINT [UserRoleRefUserId]
GO
/****** 对象:  Table [dbo].[Permissions]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Permissions]') AND type in (N'U'))
DROP TABLE [dbo].[Permissions]
GO
/****** 对象:  Table [dbo].[UserExt]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserExt]') AND type in (N'U'))
DROP TABLE [dbo].[UserExt]
GO
/****** 对象:  Table [dbo].[UserRoleRef]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRoleRef]') AND type in (N'U'))
DROP TABLE [dbo].[UserRoleRef]
GO
/****** 对象:  Table [dbo].[Logs]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Logs]') AND type in (N'U'))
DROP TABLE [dbo].[Logs]
GO
/****** 对象:  Table [dbo].[Roles]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
DROP TABLE [dbo].[Roles]
GO
/****** 对象:  Table [dbo].[Functions]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Functions]') AND type in (N'U'))
DROP TABLE [dbo].[Functions]
GO
/****** 对象:  Table [dbo].[Actions]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Actions]') AND type in (N'U'))
DROP TABLE [dbo].[Actions]
GO
/****** 对象:  Table [dbo].[User]    脚本日期: 03/01/2014 12:37:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
DROP TABLE [dbo].[User]
GO
/****** 对象:  Table [dbo].[Functions]    脚本日期: 03/01/2014 12:37:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Functions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Functions](
	[iFunctionId] [int] IDENTITY(1,1) NOT NULL,
	[cFunctionLevel] [nvarchar](24) NOT NULL,
	[cFunctionName] [nvarchar](64) NOT NULL,
	[cFunctionDescription] [nvarchar](256) NULL,
	[cFunctionCommandGo] [nvarchar](max) NULL,
	[bLog] [bit] NOT NULL CONSTRAINT [DF_Functions_bLog]  DEFAULT ((0)),
 CONSTRAINT [PK_Functions] PRIMARY KEY CLUSTERED 
(
	[iFunctionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [FunctionName] UNIQUE NONCLUSTERED 
(
	[cFunctionName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Functions', N'COLUMN',N'iFunctionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Functions', @level2type=N'COLUMN',@level2name=N'iFunctionId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Functions', N'COLUMN',N'cFunctionLevel'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能级别(菜单,命令,服务)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Functions', @level2type=N'COLUMN',@level2name=N'cFunctionLevel'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Functions', N'COLUMN',N'cFunctionName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能名称,显示在相应的菜单或命令上' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Functions', @level2type=N'COLUMN',@level2name=N'cFunctionName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Functions', N'COLUMN',N'cFunctionDescription'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Functions', @level2type=N'COLUMN',@level2name=N'cFunctionDescription'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Functions', N'COLUMN',N'cFunctionCommandGo'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'命令指向' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Functions', @level2type=N'COLUMN',@level2name=N'cFunctionCommandGo'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Functions', N'COLUMN',N'bLog'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否记录历史变更,默认为否.(如果都为是,日志表将增至极大).' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Functions', @level2type=N'COLUMN',@level2name=N'bLog'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Functions', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能(菜单,命令,服务)表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Functions'
GO
/****** 对象:  Table [dbo].[Actions]    脚本日期: 03/01/2014 12:37:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Actions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Actions](
	[iActionId] [int] IDENTITY(1,1) NOT NULL,
	[cActionName] [nvarchar](64) NOT NULL,
	[cActionDescription] [nvarchar](256) NULL,
 CONSTRAINT [PK_Actions] PRIMARY KEY CLUSTERED 
(
	[iActionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [ActionName] UNIQUE NONCLUSTERED 
(
	[cActionName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Actions', N'COLUMN',N'iActionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操(动)作ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Actions', @level2type=N'COLUMN',@level2name=N'iActionId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Actions', N'COLUMN',N'cActionName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操(动)作名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Actions', @level2type=N'COLUMN',@level2name=N'cActionName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Actions', N'COLUMN',N'cActionDescription'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操(动)作描述.增,删,改,查,打印,输入,输出' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Actions', @level2type=N'COLUMN',@level2name=N'cActionDescription'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Actions', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操(动)作表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Actions'
GO
/****** 对象:  Table [dbo].[Roles]    脚本日期: 03/01/2014 12:37:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Roles](
	[iRoleId] [int] IDENTITY(1,1) NOT NULL,
	[cRoleName] [nvarchar](64) NOT NULL,
	[cRoleDescription] [nvarchar](256) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[iRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [RoleName] UNIQUE NONCLUSTERED 
(
	[cRoleName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Roles', N'COLUMN',N'iRoleId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Roles', @level2type=N'COLUMN',@level2name=N'iRoleId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Roles', N'COLUMN',N'cRoleName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Roles', @level2type=N'COLUMN',@level2name=N'cRoleName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Roles', N'COLUMN',N'cRoleDescription'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Roles', @level2type=N'COLUMN',@level2name=N'cRoleDescription'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Roles', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色表，保存角色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Roles'
GO
/****** 对象:  Table [dbo].[User]    脚本日期: 03/01/2014 12:37:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[iUserId] [int] IDENTITY(1,1) NOT NULL,
	[cUserCode] [nvarchar](128) NOT NULL,
	[cUserName] [nvarchar](64) NULL,
	[cUserPassWord] [nvarchar](64) NULL,
	[cUserEMail] [nvarchar](64) NULL,
	[cUserMobile] [nvarchar](16) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[iUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'User', N'COLUMN',N'iUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户id,表检索' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'iUserId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'User', N'COLUMN',N'cUserCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户编码，可用于登录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'cUserCode'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'User', N'COLUMN',N'cUserName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名,用于界面显示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'cUserName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'User', N'COLUMN',N'cUserPassWord'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户登录密码，加密后保存' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'cUserPassWord'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'User', N'COLUMN',N'cUserEMail'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户EMail, 可用于登录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'cUserEMail'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'User', N'COLUMN',N'cUserMobile'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机号码，用于登录及其它认证操作' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'cUserMobile'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'User', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户表，保存基本信息，用于登录验证' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User'
GO
/****** 对象:  Table [dbo].[Permissions]    脚本日期: 03/01/2014 12:37:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Permissions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Permissions](
	[iPermissionId] [int] NOT NULL,
	[iActionId] [int] NOT NULL,
	[iFunctionId] [int] NOT NULL,
	[iId] [int] NOT NULL,
	[cType] [varchar](1) NOT NULL,
 CONSTRAINT [PK__RolePermissions__2A4B4B5E] PRIMARY KEY CLUSTERED 
(
	[iPermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Permissions', N'COLUMN',N'iPermissionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'iPermissionId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Permissions', N'COLUMN',N'iActionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'动作ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'iActionId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Permissions', N'COLUMN',N'iFunctionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单,命令ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'iFunctionId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Permissions', N'COLUMN',N'iId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对象Id,用户id或角色id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'iId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Permissions', N'COLUMN',N'cType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对象类型_用户or角色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'cType'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Permissions', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对象命令权限,类型:用户User,角色Role' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions'
GO
/****** 对象:  Table [dbo].[Logs]    脚本日期: 03/01/2014 12:37:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Logs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Logs](
	[iLogId] [int] IDENTITY(1,1) NOT NULL,
	[dLogDate] [datetime] NOT NULL,
	[iUserID] [int] NULL,
	[cWorkStation] [nvarchar](128) NULL,
	[cIP] [nvarchar](24) NULL,
	[iFunctionId] [int] NULL,
	[iActionId] [int] NULL,
	[cLastValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[iLogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Logs', N'COLUMN',N'iLogId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Logs', @level2type=N'COLUMN',@level2name=N'iLogId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Logs', N'COLUMN',N'dLogDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Logs', @level2type=N'COLUMN',@level2name=N'dLogDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Logs', N'COLUMN',N'iUserID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Logs', @level2type=N'COLUMN',@level2name=N'iUserID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Logs', N'COLUMN',N'cWorkStation'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户工作站' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Logs', @level2type=N'COLUMN',@level2name=N'cWorkStation'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Logs', N'COLUMN',N'cIP'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Logs', @level2type=N'COLUMN',@level2name=N'cIP'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Logs', N'COLUMN',N'iFunctionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Logs', @level2type=N'COLUMN',@level2name=N'iFunctionId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Logs', N'COLUMN',N'iActionId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'动作Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Logs', @level2type=N'COLUMN',@level2name=N'iActionId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Logs', N'COLUMN',N'cLastValue'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改后的值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Logs', @level2type=N'COLUMN',@level2name=N'cLastValue'
GO
/****** 对象:  Table [dbo].[UserRoleRef]    脚本日期: 03/01/2014 12:37:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRoleRef]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserRoleRef](
	[iRoleId] [int] NOT NULL,
	[iUserId] [int] NOT NULL,
	[iURRefId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK__UserRoleRef__29572725] PRIMARY KEY CLUSTERED 
(
	[iURRefId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'UserRoleRef', N'COLUMN',N'iRoleId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoleRef', @level2type=N'COLUMN',@level2name=N'iRoleId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'UserRoleRef', N'COLUMN',N'iUserId'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoleRef', @level2type=N'COLUMN',@level2name=N'iUserId'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'UserRoleRef', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户角色关联表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoleRef'
GO
/****** 对象:  Table [dbo].[UserExt]    脚本日期: 03/01/2014 12:37:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserExt]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserExt](
	[iUserId] [int] NOT NULL,
	[cUserAddress] [nvarchar](128) NULL,
	[cUserPhone] [nvarchar](128) NULL,
	[cUserIM] [nvarchar](64) NULL,
	[cUserMasterPage] [nvarchar](128) NULL
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'UserExt', N'COLUMN',N'cUserAddress'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserExt', @level2type=N'COLUMN',@level2name=N'cUserAddress'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'UserExt', N'COLUMN',N'cUserIM'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IM' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserExt', @level2type=N'COLUMN',@level2name=N'cUserIM'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'UserExt', N'COLUMN',N'cUserMasterPage'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主页' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserExt', @level2type=N'COLUMN',@level2name=N'cUserMasterPage'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'UserExt', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户扩展表，保存用户的其它信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserExt'
GO
/****** 对象:  ForeignKey [FK__Logs__iActionId__21B6055D]    脚本日期: 03/01/2014 12:37:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Logs__iActionId__21B6055D]') AND parent_object_id = OBJECT_ID(N'[dbo].[Logs]'))
ALTER TABLE [dbo].[Logs]  WITH CHECK ADD  CONSTRAINT [FK__Logs__iActionId__21B6055D] FOREIGN KEY([iActionId])
REFERENCES [dbo].[Actions] ([iActionId])
GO
ALTER TABLE [dbo].[Logs] CHECK CONSTRAINT [FK__Logs__iActionId__21B6055D]
GO
/****** 对象:  ForeignKey [FK_Logs_Functions]    脚本日期: 03/01/2014 12:37:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Logs_Functions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Logs]'))
ALTER TABLE [dbo].[Logs]  WITH CHECK ADD  CONSTRAINT [FK_Logs_Functions] FOREIGN KEY([iFunctionId])
REFERENCES [dbo].[Functions] ([iFunctionId])
GO
ALTER TABLE [dbo].[Logs] CHECK CONSTRAINT [FK_Logs_Functions]
GO
/****** 对象:  ForeignKey [FK_Logs_User]    脚本日期: 03/01/2014 12:37:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Logs_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[Logs]'))
ALTER TABLE [dbo].[Logs]  WITH CHECK ADD  CONSTRAINT [FK_Logs_User] FOREIGN KEY([iUserID])
REFERENCES [dbo].[User] ([iUserId])
GO
ALTER TABLE [dbo].[Logs] CHECK CONSTRAINT [FK_Logs_User]
GO
/****** 对象:  ForeignKey [FK__Permissions__iId__2B3F6F97]    脚本日期: 03/01/2014 12:37:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Permissions__iId__2B3F6F97]') AND parent_object_id = OBJECT_ID(N'[dbo].[Permissions]'))
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK__Permissions__iId__2B3F6F97] FOREIGN KEY([iId])
REFERENCES [dbo].[Roles] ([iRoleId])
GO
ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK__Permissions__iId__2B3F6F97]
GO
/****** 对象:  ForeignKey [FK__Permissions__iId__2C3393D0]    脚本日期: 03/01/2014 12:37:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Permissions__iId__2C3393D0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Permissions]'))
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK__Permissions__iId__2C3393D0] FOREIGN KEY([iId])
REFERENCES [dbo].[User] ([iUserId])
GO
ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK__Permissions__iId__2C3393D0]
GO
/****** 对象:  ForeignKey [FK_RolePermissions_Actions]    脚本日期: 03/01/2014 12:37:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolePermissions_Actions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Permissions]'))
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Actions] FOREIGN KEY([iActionId])
REFERENCES [dbo].[Actions] ([iActionId])
GO
ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_RolePermissions_Actions]
GO
/****** 对象:  ForeignKey [FK_RolePermissions_Functions]    脚本日期: 03/01/2014 12:37:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolePermissions_Functions]') AND parent_object_id = OBJECT_ID(N'[dbo].[Permissions]'))
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermissions_Functions] FOREIGN KEY([iFunctionId])
REFERENCES [dbo].[Functions] ([iFunctionId])
GO
ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_RolePermissions_Functions]
GO
/****** 对象:  ForeignKey [FK_UserExt_User]    脚本日期: 03/01/2014 12:37:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserExt_User]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserExt]'))
ALTER TABLE [dbo].[UserExt]  WITH CHECK ADD  CONSTRAINT [FK_UserExt_User] FOREIGN KEY([iUserId])
REFERENCES [dbo].[User] ([iUserId])
GO
ALTER TABLE [dbo].[UserExt] CHECK CONSTRAINT [FK_UserExt_User]
GO
/****** 对象:  ForeignKey [UserRoleRefRoleId]    脚本日期: 03/01/2014 12:37:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[UserRoleRefRoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRoleRef]'))
ALTER TABLE [dbo].[UserRoleRef]  WITH CHECK ADD  CONSTRAINT [UserRoleRefRoleId] FOREIGN KEY([iRoleId])
REFERENCES [dbo].[Roles] ([iRoleId])
GO
ALTER TABLE [dbo].[UserRoleRef] CHECK CONSTRAINT [UserRoleRefRoleId]
GO
/****** 对象:  ForeignKey [UserRoleRefUserId]    脚本日期: 03/01/2014 12:37:31 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[UserRoleRefUserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRoleRef]'))
ALTER TABLE [dbo].[UserRoleRef]  WITH CHECK ADD  CONSTRAINT [UserRoleRefUserId] FOREIGN KEY([iUserId])
REFERENCES [dbo].[User] ([iUserId])
GO
ALTER TABLE [dbo].[UserRoleRef] CHECK CONSTRAINT [UserRoleRefUserId]
GO

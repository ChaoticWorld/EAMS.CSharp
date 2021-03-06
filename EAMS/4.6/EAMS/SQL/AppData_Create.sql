USE [master]
GO
/****** 对象:  Database [AppData]    脚本日期: 03/01/2014 12:39:54 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'AppData')
BEGIN
CREATE DATABASE [AppData] ON  PRIMARY 
( NAME = N'AppData', FILENAME = N'D:\sql\EAMS\AppData.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AppData_log', FILENAME = N'D:\sql\EAMS\AppData_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'AppData', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AppData].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [AppData] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [AppData] SET ANSI_NULLS OFF
GO
ALTER DATABASE [AppData] SET ANSI_PADDING OFF
GO
ALTER DATABASE [AppData] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [AppData] SET ARITHABORT OFF
GO
ALTER DATABASE [AppData] SET AUTO_CLOSE ON
GO
ALTER DATABASE [AppData] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [AppData] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [AppData] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [AppData] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [AppData] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [AppData] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [AppData] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [AppData] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [AppData] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [AppData] SET  DISABLE_BROKER
GO
ALTER DATABASE [AppData] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [AppData] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [AppData] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [AppData] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [AppData] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [AppData] SET  READ_WRITE
GO
ALTER DATABASE [AppData] SET RECOVERY SIMPLE
GO
ALTER DATABASE [AppData] SET  MULTI_USER
GO
ALTER DATABASE [AppData] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [AppData] SET DB_CHAINING OFF
GO
USE [AppData]
GO
/****** 对象:  ForeignKey [FK__QC_Detail__QC_Ma__0F975522]    脚本日期: 03/01/2014 12:39:55 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__QC_Detail__QC_Ma__0F975522]') AND parent_object_id = OBJECT_ID(N'[dbo].[QC_Details]'))
ALTER TABLE [dbo].[QC_Details] DROP CONSTRAINT [FK__QC_Detail__QC_Ma__0F975522]
GO
/****** 对象:  ForeignKey [FK__QC_Detail__QC_RC__108B795B]    脚本日期: 03/01/2014 12:39:55 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__QC_Detail__QC_RC__108B795B]') AND parent_object_id = OBJECT_ID(N'[dbo].[QC_Details]'))
ALTER TABLE [dbo].[QC_Details] DROP CONSTRAINT [FK__QC_Detail__QC_RC__108B795B]
GO
/****** 对象:  ForeignKey [FK__QC_Replys__QC_Ma__117F9D94]    脚本日期: 03/01/2014 12:39:55 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__QC_Replys__QC_Ma__117F9D94]') AND parent_object_id = OBJECT_ID(N'[dbo].[QC_Replys]'))
ALTER TABLE [dbo].[QC_Replys] DROP CONSTRAINT [FK__QC_Replys__QC_Ma__117F9D94]
GO
/****** 对象:  Table [dbo].[sqlQuery]    脚本日期: 03/01/2014 12:39:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sqlQuery]') AND type in (N'U'))
DROP TABLE [dbo].[sqlQuery]
GO
/****** 对象:  Table [dbo].[QC_Replys]    脚本日期: 03/01/2014 12:39:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QC_Replys]') AND type in (N'U'))
DROP TABLE [dbo].[QC_Replys]
GO
/****** 对象:  Table [dbo].[QC_Details]    脚本日期: 03/01/2014 12:39:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QC_Details]') AND type in (N'U'))
DROP TABLE [dbo].[QC_Details]
GO
/****** 对象:  Table [dbo].[Logs]    脚本日期: 03/01/2014 12:39:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Logs]') AND type in (N'U'))
DROP TABLE [dbo].[Logs]
GO
/****** 对象:  Table [dbo].[QC_Main]    脚本日期: 03/01/2014 12:39:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QC_Main]') AND type in (N'U'))
DROP TABLE [dbo].[QC_Main]
GO
/****** 对象:  Table [dbo].[QC_DetailClass]    脚本日期: 03/01/2014 12:39:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QC_DetailClass]') AND type in (N'U'))
DROP TABLE [dbo].[QC_DetailClass]
GO
/****** 对象:  Table [dbo].[sqlQuery]    脚本日期: 03/01/2014 12:39:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sqlQuery]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[sqlQuery](
	[autoid] [int] IDENTITY(1,1) NOT NULL,
	[sQueryName] [nvarchar](128) NULL,
	[sQueryMain] [nvarchar](max) NULL,
	[sQueryWhere] [nvarchar](max) NULL,
	[sQueryGroup] [nvarchar](max) NULL,
	[sQueryOrder] [nvarchar](max) NULL,
	[sMemo] [nvarchar](max) NULL,
 CONSTRAINT [PK__sqlQuery__7E6CC920] PRIMARY KEY CLUSTERED 
(
	[autoid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [QueryName] UNIQUE NONCLUSTERED 
(
	[sQueryName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'sqlQuery', N'COLUMN',N'autoid'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'序号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sqlQuery', @level2type=N'COLUMN',@level2name=N'autoid'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'sqlQuery', N'COLUMN',N'sQueryName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'查询名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sqlQuery', @level2type=N'COLUMN',@level2name=N'sQueryName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'sqlQuery', N'COLUMN',N'sQueryMain'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'查寻主句, 替换关键字{0}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sqlQuery', @level2type=N'COLUMN',@level2name=N'sQueryMain'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'sqlQuery', N'COLUMN',N'sQueryWhere'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'查寻条件, 替换关键字{0}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sqlQuery', @level2type=N'COLUMN',@level2name=N'sQueryWhere'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'sqlQuery', N'COLUMN',N'sQueryGroup'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'查询分组' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sqlQuery', @level2type=N'COLUMN',@level2name=N'sQueryGroup'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'sqlQuery', N'COLUMN',N'sQueryOrder'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'查询排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sqlQuery', @level2type=N'COLUMN',@level2name=N'sQueryOrder'
GO
/****** 对象:  Table [dbo].[QC_Main]    脚本日期: 03/01/2014 12:39:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QC_Main]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[QC_Main](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[cCode] [nvarchar](24) NULL,
	[cCreateMan] [nvarchar](16) NULL,
	[dCreateDate] [datetime] NULL,
	[cCusManager] [nvarchar](16) NULL,
	[cModifyMan] [nvarchar](16) NULL,
	[dModifyDate] [datetime] NULL,
	[cCusCode] [nvarchar](24) NULL,
	[cCusName] [nvarchar](64) NULL,
	[cCusPerson] [nvarchar](16) NULL,
	[dExpDate] [datetime] NULL,
	[dEffDate] [datetime] NULL,
	[cMemo] [nvarchar](1024) NULL,
 CONSTRAINT [PK__QC_Main__0EA330E9] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Main', N'COLUMN',N'cCode'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'签呈编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Main', @level2type=N'COLUMN',@level2name=N'cCode'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Main', N'COLUMN',N'cCreateMan'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Main', @level2type=N'COLUMN',@level2name=N'cCreateMan'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Main', N'COLUMN',N'dCreateDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Main', @level2type=N'COLUMN',@level2name=N'dCreateDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Main', N'COLUMN',N'cCusManager'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户经理' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Main', @level2type=N'COLUMN',@level2name=N'cCusManager'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Main', N'COLUMN',N'cModifyMan'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Main', @level2type=N'COLUMN',@level2name=N'cModifyMan'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Main', N'COLUMN',N'dModifyDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Main', @level2type=N'COLUMN',@level2name=N'dModifyDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Main', N'COLUMN',N'dExpDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'失效期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Main', @level2type=N'COLUMN',@level2name=N'dExpDate'
GO
/****** 对象:  Table [dbo].[QC_DetailClass]    脚本日期: 03/01/2014 12:39:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QC_DetailClass]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[QC_DetailClass](
	[QC_RCName] [nvarchar](64) NULL,
	[QC_RCDescription] [nvarchar](128) NULL,
	[QC_RCIndex] [int] NULL,
	[QC_RCID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK__QC_DetailClass__07F6335A] PRIMARY KEY CLUSTERED 
(
	[QC_RCID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_DetailClass', N'COLUMN',N'QC_RCName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'签呈分类名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_DetailClass', @level2type=N'COLUMN',@level2name=N'QC_RCName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_DetailClass', N'COLUMN',N'QC_RCDescription'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'签呈分类描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_DetailClass', @level2type=N'COLUMN',@level2name=N'QC_RCDescription'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_DetailClass', N'COLUMN',N'QC_RCIndex'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序权重' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_DetailClass', @level2type=N'COLUMN',@level2name=N'QC_RCIndex'
GO
/****** 对象:  Table [dbo].[Logs]    脚本日期: 03/01/2014 12:39:54 ******/
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
/****** 对象:  Table [dbo].[QC_Replys]    脚本日期: 03/01/2014 12:39:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QC_Replys]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[QC_Replys](
	[AutoId] [int] IDENTITY(1,1) NOT NULL,
	[QC_MainID] [int] NULL,
	[QC_SubmitMan] [nvarchar](16) NULL,
	[QC_SubmitDate] [datetime] NULL,
	[QC_ReplyMan] [nvarchar](16) NULL,
	[QC_ReplyDate] [datetime] NULL,
	[QC_ReplyContent] [nvarchar](128) NULL CONSTRAINT [DF__QC_Replys__QC_Re__0519C6AF]  DEFAULT (''),
	[QC_ReplyPass] [bit] NULL CONSTRAINT [DF__QC_Replys__QC_Re__060DEAE8]  DEFAULT ((0)),
	[QC_NextID] [int] NULL CONSTRAINT [DF__QC_Replys__QC_Ne__07020F21]  DEFAULT (''),
 CONSTRAINT [PK__QC_Replys__0DAF0CB0] PRIMARY KEY CLUSTERED 
(
	[AutoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Replys', N'COLUMN',N'QC_MainID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'签呈主ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Replys', @level2type=N'COLUMN',@level2name=N'QC_MainID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Replys', N'COLUMN',N'QC_SubmitMan'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提交人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Replys', @level2type=N'COLUMN',@level2name=N'QC_SubmitMan'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Replys', N'COLUMN',N'QC_SubmitDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提交日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Replys', @level2type=N'COLUMN',@level2name=N'QC_SubmitDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Replys', N'COLUMN',N'QC_ReplyMan'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'批复人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Replys', @level2type=N'COLUMN',@level2name=N'QC_ReplyMan'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Replys', N'COLUMN',N'QC_ReplyDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'批复日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Replys', @level2type=N'COLUMN',@level2name=N'QC_ReplyDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Replys', N'COLUMN',N'QC_ReplyContent'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'批复内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Replys', @level2type=N'COLUMN',@level2name=N'QC_ReplyContent'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Replys', N'COLUMN',N'QC_ReplyPass'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'签呈是否通过' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Replys', @level2type=N'COLUMN',@level2name=N'QC_ReplyPass'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Replys', N'COLUMN',N'QC_NextID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'转交他人批复的AutoID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Replys', @level2type=N'COLUMN',@level2name=N'QC_NextID'
GO
/****** 对象:  Table [dbo].[QC_Details]    脚本日期: 03/01/2014 12:39:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QC_Details]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[QC_Details](
	[QC_Autoid] [int] IDENTITY(1,1) NOT NULL,
	[QC_RCID] [int] NULL,
	[QC_OrderCode] [nvarchar](24) NULL,
	[cInvCode] [nvarchar](24) NULL,
	[cInvName] [nvarchar](32) NULL,
	[cInvStd] [nvarchar](64) NULL,
	[iQuentity] [numeric](18, 0) NULL,
	[iPrice] [money] NULL,
	[iSum] [money] NULL,
	[cMemo] [nvarchar](512) NULL,
	[bDaiShouKuan] [bit] NULL CONSTRAINT [DF__QC_Detail__bDaiS__1273C1CD]  DEFAULT ((0)),
	[QC_MainID] [int] NULL,
	[dSaleSumStartDate] [datetime] NULL,
	[dSaleSumFinishDate] [datetime] NULL,
 CONSTRAINT [PK__QC_Records__03317E3D] PRIMARY KEY CLUSTERED 
(
	[QC_Autoid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Details', N'COLUMN',N'QC_Autoid'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'签呈记录id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Details', @level2type=N'COLUMN',@level2name=N'QC_Autoid'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'QC_Details', N'COLUMN',N'cMemo'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Memo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'QC_Details', @level2type=N'COLUMN',@level2name=N'cMemo'
GO
/****** 对象:  ForeignKey [FK__QC_Detail__QC_Ma__0F975522]    脚本日期: 03/01/2014 12:39:55 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__QC_Detail__QC_Ma__0F975522]') AND parent_object_id = OBJECT_ID(N'[dbo].[QC_Details]'))
ALTER TABLE [dbo].[QC_Details]  WITH CHECK ADD  CONSTRAINT [FK__QC_Detail__QC_Ma__0F975522] FOREIGN KEY([QC_MainID])
REFERENCES [dbo].[QC_Main] ([Id])
GO
ALTER TABLE [dbo].[QC_Details] CHECK CONSTRAINT [FK__QC_Detail__QC_Ma__0F975522]
GO
/****** 对象:  ForeignKey [FK__QC_Detail__QC_RC__108B795B]    脚本日期: 03/01/2014 12:39:55 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__QC_Detail__QC_RC__108B795B]') AND parent_object_id = OBJECT_ID(N'[dbo].[QC_Details]'))
ALTER TABLE [dbo].[QC_Details]  WITH NOCHECK ADD  CONSTRAINT [FK__QC_Detail__QC_RC__108B795B] FOREIGN KEY([QC_RCID])
REFERENCES [dbo].[QC_DetailClass] ([QC_RCID])
GO
ALTER TABLE [dbo].[QC_Details] NOCHECK CONSTRAINT [FK__QC_Detail__QC_RC__108B795B]
GO
/****** 对象:  ForeignKey [FK__QC_Replys__QC_Ma__117F9D94]    脚本日期: 03/01/2014 12:39:55 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__QC_Replys__QC_Ma__117F9D94]') AND parent_object_id = OBJECT_ID(N'[dbo].[QC_Replys]'))
ALTER TABLE [dbo].[QC_Replys]  WITH CHECK ADD  CONSTRAINT [FK__QC_Replys__QC_Ma__117F9D94] FOREIGN KEY([QC_MainID])
REFERENCES [dbo].[QC_Main] ([Id])
GO
ALTER TABLE [dbo].[QC_Replys] CHECK CONSTRAINT [FK__QC_Replys__QC_Ma__117F9D94]
GO

USE [UFDATA_007_2015]
GO
/****** Object:  Trigger [dbo].[SoDetailsValidStrategy_INSTrig]    Script Date: 2015-12-04 15:54:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dst
-- Create date: 20150603
-- Description:	销售订单 插入 策略库检查
-- =============================================
ALTER TRIGGER [dbo].[SoDetailsValidStrategy_INSTrig]
   ON  [dbo].[SO_SODetails]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	declare @id INT
	declare @invcode nvarchar(63)
	declare @iquantity DECIMAL(18,6)
	declare @iTaxUnitPrice DECIMAL(18,6)
	declare @valid nvarchar(255)
	set @valid = null

		select @id = id, @invcode = ''+cinvcode+'',@iquantity = iquantity,@iTaxUnitPrice = iTaxUnitPrice
			 from Inserted
	insert into AppData.dbo.strategyLogs (dLogDate,iUserID,cWorkStation,cModule,cFunction,cParams,cReturn,cUserName)
	VALUES(GETDATE(),-999,'server1','SqlValidStrategy','SqlValidStrategy'
	,'Insert-销售订单:'+cast(@id as nvarchar)+';'+@invcode+','+cast(@iTaxUnitPrice as nvarchar)+','+cast(@iquantity as nvarchar)
	,@valid,'sa')
		exec saleOrder_validStrategy @id,@invcode,@iquantity,@iTaxUnitPrice,@valid output

END


/******************************************************/
USE [UFDATA_007_2015]
GO
/****** Object:  Trigger [dbo].[SoDetailsValidStrategy_UPSTrig]    Script Date: 2015-12-04 15:57:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dst
-- Create date: 20150603
-- Description:	销售订单 更新 策略库检查
-- =============================================
ALTER TRIGGER [dbo].[SoDetailsValidStrategy_UPSTrig]
   ON  [dbo].[SO_SODetails]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	declare @id INT
	declare @invcode nvarchar(63)
	declare @iquantity DECIMAL(18,6)
	declare @iTaxUnitPrice DECIMAL(18,6)
	declare @valid nvarchar(255)
	set @valid = null
	IF (UPDATE(cinvcode) or UPDATE(iquantity) or UPDATE(iTaxUnitPrice) )
		begin
			select @id = id,@invcode = ''+cinvcode+'',@iquantity = iquantity,@iTaxUnitPrice = iTaxUnitPrice
				 from Inserted
	insert into AppData.dbo.strategyLogs (dLogDate,iUserID,cWorkStation,cModule,cFunction,cParams,cReturn,cUserName)
	VALUES(GETDATE(),-999,'server1','SqlValidStrategy','SqlValidStrategy'
	,'Update-销售订单:'+cast(@id as nvarchar)+';'+@invcode+','+cast(@iTaxUnitPrice as nvarchar)+','+cast(@iquantity as nvarchar)
	,@valid,'sa')
			exec saleOrder_validStrategy @id,@invcode,@iquantity,@iTaxUnitPrice,@valid output
		end
END


/****************************************************************/
USE [UFDATA_007_2015]
GO
/****** Object:  Trigger [dbo].[validStrategy_INSTrig]    Script Date: 2015-12-04 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dst
-- Create date: 20150603
-- Description:	销售发货单 插入 策略库检查
-- =============================================
ALTER TRIGGER [dbo].[validStrategy_INSTrig]
   ON  [dbo].[DispatchLists]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	declare @id INT
	declare @invcode nvarchar(63)
	declare @iquantity DECIMAL(18,6)
	declare @iTaxUnitPrice DECIMAL(18,6)
	declare @valid nvarchar(255)
	set @valid = null

		select @id = dlid,@invcode = ''+cinvcode+'',@iquantity = iquantity,@iTaxUnitPrice = iTaxUnitPrice
			 from Inserted
		insert into AppData.dbo.strategyLogs (dLogDate,iUserID,cWorkStation,cModule,cFunction,cParams,cReturn,cUserName)
		VALUES(GETDATE(),-999,'server1','SqlValidStrategy','SqlValidStrategy'
		,'Insert-发货单:'+cast(@id as nvarchar)+';'+@invcode+','+cast(@iTaxUnitPrice as nvarchar)+','+cast(@iquantity as nvarchar)
		,@valid,'sa')
		exec saleFHD_validStrategy @id,@invcode,@iquantity,@iTaxUnitPrice,@valid output

END


/*******************************************************/
USE [UFDATA_007_2015]
GO
/****** Object:  Trigger [dbo].[validStrategy_UPSTrig]    Script Date: 2015-12-04 15:58:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		dst
-- Create date: 20150603
-- Description:	销售发货单 更新 策略库检查
-- =============================================
ALTER TRIGGER [dbo].[validStrategy_UPSTrig]
   ON  [dbo].[DispatchLists]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	declare @id INT
	declare @invcode nvarchar(63)
	declare @iquantity DECIMAL(18,6)
	declare @iTaxUnitPrice DECIMAL(18,6)
	declare @valid nvarchar(255)
	set @valid = null
	IF (UPDATE(cinvcode) or UPDATE(iquantity) or UPDATE(iTaxUnitPrice) )
		begin
			select @id = dlid,@invcode = ''+cinvcode+'',@iquantity = iquantity,@iTaxUnitPrice = iTaxUnitPrice
				 from Inserted
			insert into AppData.dbo.strategyLogs (dLogDate,iUserID,cWorkStation,cModule,cFunction,cParams,cReturn,cUserName)
			VALUES(GETDATE(),-999,'server1','SqlValidStrategy','SqlValidStrategy'
			,'Update-发货单:'+cast(@id as nvarchar)+';'+@invcode+','+cast(@iTaxUnitPrice as nvarchar)+','+cast(@iquantity as nvarchar)
			,@valid,'sa')
			exec saleFHD_validStrategy @id,@invcode,@iquantity,@iTaxUnitPrice,@valid output
		end
END


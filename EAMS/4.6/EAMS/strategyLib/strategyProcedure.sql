--存储过程
--策略库验证

--触发:DispatchLists 条件触发
ALTER PROCEDURE [dbo].[saleFHD_validStrategy]
@id INT,
@invcode nvarchar(63),
@iquantity DECIMAL(18,6),
@iTaxUnitPrice DECIMAL(18,6),
@valid nvarchar(255) OUTPUT
as 
BEGIN
	declare @cuscode nvarchar(63)
	declare @dcName nvarchar(63)
	declare @vouchCode nvarchar(31)
  DECLARE @dDate datetime
	declare @rvalid nvarchar(255)
	--declare @invcode nvarchar(63)
	--取主表信息
	select @vouchCode = cdlcode,@cuscode=ccuscode,@dDate = dDate
	from DispatchList
	where dlid = @id
	select @dcName = DistrictClass.cDCName
	from Customer left join DistrictClass on Customer.cDCCode = DistrictClass.cDCCode
	where 1 = 1
	and Customer.cCusCode = @cuscode

	DECLARE @cSourceType nvarchar(63)
	DECLARE @cSourceCode nvarchar(127)
	DECLARE @Aprice DECIMAL(18,6)
	DECLARE @AQuantity DECIMAL(18,6)
	DECLARE @Bcode nvarchar(63)
	DECLARE @Bprice DECIMAL(18,6)
	DECLARE @BQuantity DECIMAL(18,6)

	DECLARE @tmptable nvarchar(64)
	set @tmptable = N'[##'+ CAST(@id AS NVARCHAR) + @invcode + 'at'
			+ REPLACE(REPLACE(REPLACE(CONVERT( nvarchar,GETDATE() ,120), ' ', '')
				,'-',''),':','')+']'	
	if exists (select * from tempdb.dbo.sysobjects where name = @tmptable and type='U')
	exec('drop table tempdb.dbo.'+@tmptable)

	DECLARE @sql1 NVARCHAR(4000)
	SET @sql1 = '
			SELECT top 1 invAprice,invAQuantity
			, cinvBcode, invBprice,invBQuantity
			,cSourceType,cSourceCode,id=identity(int)
			INTO '+@tmptable
	+ N' from AppData.dbo.vw_strategy
			where 1 = 1 and cinvACode = @invcode
			and (cVouchCode is null or cVouchCode = @vouchCode)
			and (cVouchType is null or cVouchType = ''Dispatch'' or cVouchType = ''发货单'')
			and (cDWCode is null or cDWCode =@cuscode)
			and (cDCName is null or cDCName LIKE ''%@dcName%'')
			and (deffdate <= @dDate and @dDate <= dexpdate or dExpdate is null)
			order by cLevel desc,cVouchCode desc,cDWCode desc,cDCName desc,deffdate,dexpdate desc'

--		insert into AppData.dbo.strategyLogs (dLogDate,iUserID,cWorkStation,cModule,cFunction,cParams,cReturn,cUserName)
--			VALUES(GETDATE(),-999,'server1','SqlValidStrategy','SqlValidStrategy','','sql1:'+@sql1,'sa')

	exec  sp_executesql @sql1
			,N'@invcode nvarchar(63), @vouchCode nvarchar(31),@cuscode nvarchar(63),@dcName nvarchar(63),@dDate datetime'
			,@invcode, @vouchCode,@cuscode,@dcName,@dDate

	DECLARE @SQL2 NVARCHAR(4000)
	--@Aprice,@AQuantity ,@Bcode,@Bprice,@BQuantity,@cSourceType,@cSourceCode
	SET @SQL2 = 'select @Aprice = invAprice, @AQuantity = invAQuantity
			, @Bcode = cinvBcode, @Bprice = invBprice,@BQuantity=invBQuantity
			, @cSourceType = cSourceType,@cSourceCode = cSourceCode from '+@tmptable+' where id = @i '

	declare @sql3 nvarchar(128)
	 set @sql3 = N'SELECT @cnt = COUNT(id) from '+@tmptable

	declare @cnt int
	exec sp_executesql @sql3,N'@cnt int output',@cnt OUTPUT
--			insert into AppData.dbo.strategyLogs (dLogDate,iUserID,cWorkStation,cModule,cFunction,cParams,cReturn,cUserName)
--			VALUES(GETDATE(),-999,'server1','SqlValidStrategy','SqlValidStrategy' ,''
--			,'策略数量:'+cast(@cnt as nvarchar),'sa')
	DECLARE @i int 
	set @i = 1
	if (@cnt<=0) 
		begin
			insert into AppData.dbo.strategyLogs (dLogDate,iUserID,cWorkStation,cModule,cFunction,cParams,cReturn,cUserName)
			VALUES(GETDATE(),-999,'server1','SqlValidStrategy','SqlValidStrategy'
			,'发货单:'+@vouchCode+';'+@invcode+','+cast(@iTaxUnitPrice as nvarchar)+','+cast(@iquantity as nvarchar)
			,'无策略库记录','sa')
			SET @rvalid= '无策略库记录'
		end
	WHILE(@i<=@cnt)
		BEGIN
			declare @isValid bit --0是假,1是真
			set @isValid = 0
			exec sp_executesql @sql2,N'@Aprice decimal(18,6) output ,@AQuantity  decimal(18,6) output 
			 ,@Bcode nvarchar(32) output,@Bprice decimal(18,6) output ,@BQuantity decimal(18,6) output 
			,@cSourceType nvarchar(32) output,@cSourceCode nvarchar(32) output, @i int '
			,@Aprice OUTPUT,@AQuantity OUTPUT 
			,@Bcode OUTPUT,@Bprice OUTPUT,@BQuantity OUTPUT
			,@cSourceType OUTPUT,@cSourceCode OUTPUT,@i
			SET @rvalid= ''

			if (@iTaxUnitPrice >= @Aprice and (@iquantity > 0 and @iquantity >= @AQuantity) or @iquantity <= 0 )--@iquantity >= @AQuantity AND 退货单问题-退货数量为负数
				BEGIN
					set @rvalid = '合法' + ISNULL(@cSourceType,'')+'-'+ISNULL(@cSourceCode,'')
					set @isValid = 1
					if (@Bcode is not null or @Bcode <> '')
						BEGIN
							declare @erpBCode nvarchar(63)
							declare @erpBQuantity nvarchar(63)
							declare @erpBPrice nvarchar(63)

							select top 1 @erpBCode = cinvcode,@erpBQuantity = iQuantity
											,@erpBPrice = iTaxUnitPrice
							from DispatchLists
							where 1 = 1 and dlid = @id
								and cInvCode = @BCode
	
							if ((@iquantity / @AQuantity) = (@erpBQuantity / @BQuantity)
										and (@Bprice <= @erpBPrice))
								BEGIN
									set @rvalid = '合法' + ISNULL(@cSourceType,'')+'-'+ISNULL(@cSourceCode,'')
									set @isValid = 1
								END
							ELSE							
								BEGIN
									set @rvalid = '非法!配比赠送部分!InvCode:'+@Bcode+';'+ ISNULL(@cSourceType,'')+'-'+ISNULL(@cSourceCode,'')
									set @isValid = 0
								END							
						END
				END
			else 
				BEGIN
					set @rvalid = '非法!,'+ ISNULL(@cSourceType,'')+'-'+ISNULL(@cSourceCode,'')
					set @isValid = 0
			insert into AppData.dbo.strategyLogs (dLogDate,iUserID,cWorkStation,cModule,cFunction,cParams,cReturn,cUserName)
			VALUES(GETDATE(),-999,'server1','SqlValidStrategy','SqlValidStrategy'
			,'发货单:'+@vouchCode+';'+@invcode+','+cast(@iTaxUnitPrice as nvarchar)+','+cast(@iquantity as nvarchar)
			,'非法!','sa')

				END
			
			insert into AppData.dbo.strategyLogs (dLogDate,iUserID,cWorkStation,cModule,cFunction,cParams,cReturn,cUserName)
			VALUES(GETDATE(),-999,'server1','SqlValidStrategy','SqlValidStrategy'
			,'发货单:'+@vouchCode+';'+@invcode+','+cast(@iTaxUnitPrice as nvarchar)+','+cast(@iquantity as nvarchar)
			,@rvalid,'sa')
			--if (@isValid = 0) break			
			if (@isValid = 0) RAISERROR(N'价格非法！请检查。',16,1)/*-- 抛出自定义的异常*/
			set @i=@i+1

		END
	select @valid = @rvalid+'  发货单:'+@vouchCode+'-'+@invcode
	--select @valid
	exec('drop table '+@tmptable)
END



/***************************/

--存储过程
--策略库验证

--触发:SO_SODetails 条件触发
ALTER PROCEDURE [dbo].[saleOrder_validStrategy]
@id INT,
--@dlsid INT,
@invcode nvarchar(63),
@iquantity DECIMAL(18,6),
@iTaxUnitPrice DECIMAL(18,6),
@valid nvarchar(255) OUTPUT
as 
BEGIN
	declare @cuscode nvarchar(63)
	declare @dcName nvarchar(63)
	declare @vouchCode nvarchar(31)
  declare @dDate datetime
	declare @rvalid nvarchar(255)
	--declare @invcode nvarchar(63)
	--取主表信息
	select @vouchCode = csocode,@cuscode=ccuscode,@dDate = dDate
	from so_somain
	where id = @id
	select @dcName = DistrictClass.cDCName
	from Customer left join DistrictClass on Customer.cDCCode = DistrictClass.cDCCode
	where 1 = 1
	and Customer.cCusCode = @cuscode

	DECLARE @cSourceType nvarchar(63)
	DECLARE @cSourceCode nvarchar(127)
	DECLARE @Aprice DECIMAL(18,6)
	DECLARE @AQuantity DECIMAL(18,6)
	DECLARE @Bcode nvarchar(63)
	DECLARE @Bprice DECIMAL(18,6)
	DECLARE @BQuantity DECIMAL(18,6)

	DECLARE @tmptable nvarchar(64)
	set @tmptable = N'[##'+ CAST(@ID AS NVARCHAR) + @invcode + 'at'
			+ REPLACE(REPLACE(REPLACE(CONVERT( nvarchar,GETDATE() ,120), ' ', '')
				,'-',''),':','')+']'	
	if exists (select * from tempdb.dbo.sysobjects where name = @tmptable and type='U')
	exec('drop table tempdb.dbo.'+@tmptable)

	DECLARE @sql1 NVARCHAR(4000)
	SET @sql1 = '
			SELECT top 1 invAprice,invAQuantity
			, cinvBcode, invBprice,invBQuantity
			,cSourceType,cSourceCode,id=identity(int)
			INTO '+@tmptable
	+ N' from AppData.dbo.vw_strategy
			where 1 = 1 and cinvACode = @invcode
			and (cVouchCode is null or cVouchCode = @vouchCode)
			and (cVouchType is null or cVouchType = ''SaleOrder'' or cVouchType = ''销售订单'')
			and (cDWCode is null or cDWCode =@cuscode)
			and (cDCName is null or cDCName LIKE ''%@dcName%'')
			and (deffdate <= @dDate and @dDate <= dexpdate or dExpdate is null)
			order by cLevel desc,cVouchCode desc,cDWCode desc,cDCName desc,deffdate,dexpdate desc'

--		insert into AppData.dbo.strategyLogs (dLogDate,iUserID,cWorkStation,cModule,cFunction,cParams,cReturn,cUserName)
--			VALUES(GETDATE(),-999,'server1','SqlValidStrategy','SqlValidStrategy','','sql1:'+@sql1,'sa')

	exec  sp_executesql @sql1
			,N'@invcode nvarchar(63), @vouchCode nvarchar(31),@cuscode nvarchar(63),@dcName nvarchar(63),@dDate datetime'
			,@invcode, @vouchCode,@cuscode,@dcName,@dDate

	DECLARE @SQL2 NVARCHAR(4000)
	--@Aprice,@AQuantity ,@Bcode,@Bprice,@BQuantity,@cSourceType,@cSourceCode
	SET @SQL2 = 'select @Aprice = invAprice, @AQuantity = invAQuantity
			, @Bcode = cinvBcode, @Bprice = invBprice,@BQuantity=invBQuantity
			, @cSourceType = cSourceType,@cSourceCode = cSourceCode from '+@tmptable+' where id = @i '

	declare @sql3 nvarchar(128)
	 set @sql3 = N'SELECT @cnt = COUNT(id) from '+@tmptable

	declare @cnt int
	exec sp_executesql @sql3,N'@cnt int output',@cnt OUTPUT

	DECLARE @i int 
	set @i = 1
	if (@cnt<=0) 
		begin
			insert into AppData.dbo.strategyLogs (dLogDate,iUserID,cWorkStation,cModule,cFunction,cParams,cReturn,cUserName)
			VALUES(GETDATE(),-999,'server1','SqlValidStrategy','SqlValidStrategy'
			,'销售订单:'+@vouchCode+';'+@invcode+','+cast(@iTaxUnitPrice as nvarchar)+','+cast(@iquantity as nvarchar)
			,'无策略库记录','sa')
			SET @rvalid= '无策略库记录'
		end
	WHILE(@i<=@cnt)
		BEGIN
			declare @isValid bit --0是假,1是真
			set @isValid = 0
			exec sp_executesql @sql2,N'@Aprice decimal(18,6) output ,@AQuantity  decimal(18,6) output 
			 ,@Bcode nvarchar(32) output,@Bprice decimal(18,6) output ,@BQuantity decimal(18,6) output 
			,@cSourceType nvarchar(32) output,@cSourceCode nvarchar(32) output, @i int '
			,@Aprice OUTPUT,@AQuantity OUTPUT 
			,@Bcode OUTPUT,@Bprice OUTPUT,@BQuantity OUTPUT
			,@cSourceType OUTPUT,@cSourceCode OUTPUT,@i
			SET @rvalid= ''
			if ( @iTaxUnitPrice >= @Aprice and @iquantity >=@AQuantity )
				BEGIN
					set @rvalid = '合法' +  ISNULL(@cSourceType,'')+'-'+ISNULL(@cSourceCode,'')
					set @isValid = 1
					if (@Bcode is not null or @Bcode <> '')
						BEGIN
							declare @erpBCode nvarchar(63)
							declare @erpBQuantity nvarchar(63)
							declare @erpBPrice nvarchar(63)

							select top 1 @erpBCode = cinvcode,@erpBQuantity = iQuantity
											,@erpBPrice = iTaxUnitPrice
							from So_Sodetails
							where 1 = 1 and id = @id
								and cInvCode = @BCode
	
							if ((@iquantity / @AQuantity) = (@erpBQuantity / @BQuantity)
										and (@Bprice <= @erpBPrice))
								BEGIN
									set @rvalid = '合法' + ISNULL(@cSourceType,'')+'-'+ISNULL(@cSourceCode,'')
									set @isValid = 1
								END
							ELSE							
								BEGIN
									set @rvalid = '非法!配比赠送部分!InvCode:'+@Bcode+';'+ ISNULL(@cSourceType,'')+'-'+ISNULL(@cSourceCode,'')
									set @isValid = 0
								END							
						END
				END
			else 
				BEGIN
					set @rvalid = '非法!,'+ ISNULL(@cSourceType,'')+'-'+ISNULL(@cSourceCode,'')
					set @isValid = 0
				END

			insert into AppData.dbo.strategyLogs (dLogDate,iUserID,cWorkStation,cModule,cFunction,cParams,cReturn,cUserName)
			VALUES(GETDATE(),-999,'server1','SqlValidStrategy','SqlValidStrategy'
			,'销售订单:'+@vouchCode+';'+@invcode+','+cast(@iTaxUnitPrice as nvarchar)+','+cast(@iquantity as nvarchar)
			,@rvalid,'sa')

			--if (@isValid = 0) break
			if (@isValid = 0) print '中止单据处理'
			set @i=@i+1

			END
	select @valid = @rvalid+'  销售订单:'+@vouchCode+'-'+@invcode
	--select @valid
	exec('drop table '+@tmptable)
END

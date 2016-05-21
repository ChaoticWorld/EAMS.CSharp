use [AppData]
set Identity_Insert [QC_DetailClass] on
INSERT INTO [QC_DetailClass] ([QC_RCName], [QC_RCDescription], [QC_RCIndex], [QC_RCID]) VALUES ('销售订单', 'ERP中的销售订单参照', 1, 1)
INSERT INTO [QC_DetailClass] ([QC_RCName], [QC_RCDescription], [QC_RCIndex], [QC_RCID]) VALUES ('销售发货单', 'ERP中的销售发货单参照', 2, 2)
INSERT INTO [QC_DetailClass] ([QC_RCName], [QC_RCDescription], [QC_RCIndex], [QC_RCID]) VALUES ('累计销售', '累计销售', 3, 3)
INSERT INTO [QC_DetailClass] ([QC_RCName], [QC_RCDescription], [QC_RCIndex], [QC_RCID]) VALUES ('配赠内容', '配赠内容', 4, 4)
INSERT INTO [QC_DetailClass] ([QC_RCName], [QC_RCDescription], [QC_RCIndex], [QC_RCID]) VALUES ('折款金额', '折款金额，此单申请折款', 5, 5)
INSERT INTO [QC_DetailClass] ([QC_RCName], [QC_RCDescription], [QC_RCIndex], [QC_RCID]) VALUES ('运费', '运费金额，是否我司付款', 6, 6)
INSERT INTO [QC_DetailClass] ([QC_RCName], [QC_RCDescription], [QC_RCIndex], [QC_RCID]) VALUES ('代收货款', '货运是否代收货款', 7, 7)
INSERT INTO [QC_DetailClass] ([QC_RCName], [QC_RCDescription], [QC_RCIndex], [QC_RCID]) VALUES ('其它信息', '其它信息', 8, 8)

set Identity_Insert [QC_DetailClass] off
GO
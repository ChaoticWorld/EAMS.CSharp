use [AppSystem]
--User
set Identity_Insert [User] on
INSERT INTO [User] ([iUserId], [cUserCode], [cUserName], [cUserPassWord], [cUserEMail], [cUserMobile]) VALUES (1, 'admin', 'admin', '6fd742a61bd034804c00c49b18045020', NULL, NULL);
set Identity_Insert [User] off
GO

--Roles
set Identity_Insert [Roles] on
INSERT INTO [Roles] ([iRoleId], [cRoleName], [cRoleDescription]) VALUES (1, 'Admin', '管理员');
INSERT INTO [Roles] ([iRoleId], [cRoleName], [cRoleDescription]) VALUES (2, 'Employee', '员工');
INSERT INTO [Roles] ([iRoleId], [cRoleName], [cRoleDescription]) VALUES (4, 'Customer', '客户');
INSERT INTO [Roles] ([iRoleId], [cRoleName], [cRoleDescription]) VALUES (3, '客户经理', '客户经理');
set Identity_Insert [Roles] off
GO

--UserRoleRef
set Identity_Insert [UserRoleRef] on
INSERT INTO [UserRoleRef] ([iRoleId], [iUserId], [iURRefId]) VALUES (1, 1, 1);
--INSERT INTO [UserRoleRef] ([iRoleId], [iUserId], [iURRefId]) VALUES (2, 1, 2);
set Identity_Insert [UserRoleRef] off
GO

--Actions
set Identity_Insert [Actions] on
INSERT INTO [Actions] ([iActionId], [cActionName], [cActionDescription]) VALUES (1, 'Action1', 'ActionDescritption1');
set Identity_Insert [Actions] off
GO

--Functions
set Identity_Insert [Functions] on
INSERT INTO [Functions] ([iFunctionId], [cFunctionLevel], [cFunctionName], [cFunctionDescription], [cFunctionCommandGo], [bLog]) VALUES (1, 'Menu', 'Menu1', 'menu description', 'cmd', '1');
set Identity_Insert [Functions] off
GO

--Permissions

--Logs
set Identity_Insert [Logs] on
INSERT INTO [Logs] ([iLogId], [dLogDate], [iUserID], [cWorkStation], [cIP], [iFunctionId], [iActionId], [cLastValue]) VALUES (1, '2013-9-23 16:49:54', 1, NULL, NULL, 1, 1, NULL);
INSERT INTO [Logs] ([iLogId], [dLogDate], [iUserID], [cWorkStation], [cIP], [iFunctionId], [iActionId], [cLastValue]) VALUES (2, '2013-9-23 16:52:34', 1, NULL, NULL, 1, 1, NULL);
set Identity_Insert [Logs] on
GO

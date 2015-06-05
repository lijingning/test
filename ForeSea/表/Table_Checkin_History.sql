﻿CREATE TABLE [dbo].[Table_Checkin_History]
(
	[GUID] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	[Term] CHAR(2) NOT NULL,
	[ID] CHAR(9) NOT NULL,
    [Date] DATE NOT NULL DEFAULT CONVERT(DATE,GETDATE()), 
	[Lesson] NCHAR(2) NOT NULL,
    [Signin] TIME(0) NULL DEFAULT CONVERT(TIME(0),GETDATE()), 
    [Signout] TIME(0) NULL, 
    [Change] TIME(0) NULL, 
    [Keep] TIME(0) NULL, 
    [Signin_State] INT NULL, 
    [State] INT NOT NULL DEFAULT 0
)

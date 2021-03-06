USE [FencingDB]
GO
/****** Object:  Table [dbo].[Category_Master]    Script Date: 05/20/2018 12:10:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Category_Master](
	[Category_Id] [int] IDENTITY(1,1) NOT NULL,
	[Category_Name] [varchar](50) NULL,
	[User_Id] [int] NULL,
	[Entry_Date] [date] NULL,
 CONSTRAINT [PK_Category_Master] PRIMARY KEY CLUSTERED 
(
	[Category_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Category_Master] ON
INSERT [dbo].[Category_Master] ([Category_Id], [Category_Name], [User_Id], [Entry_Date]) VALUES (1, N'FENCE MESH DAIMOND', NULL, NULL)
INSERT [dbo].[Category_Master] ([Category_Id], [Category_Name], [User_Id], [Entry_Date]) VALUES (2, N'FENCE POST CORNER', NULL, NULL)
INSERT [dbo].[Category_Master] ([Category_Id], [Category_Name], [User_Id], [Entry_Date]) VALUES (3, N'FENCE STAY SUPPORTED', NULL, NULL)
INSERT [dbo].[Category_Master] ([Category_Id], [Category_Name], [User_Id], [Entry_Date]) VALUES (4, N'FENCE GATE', NULL, NULL)
INSERT [dbo].[Category_Master] ([Category_Id], [Category_Name], [User_Id], [Entry_Date]) VALUES (5, N'FENCE STANDARD [Y]', NULL, NULL)
INSERT [dbo].[Category_Master] ([Category_Id], [Category_Name], [User_Id], [Entry_Date]) VALUES (6, N'FENCE DROPPER', NULL, NULL)
INSERT [dbo].[Category_Master] ([Category_Id], [Category_Name], [User_Id], [Entry_Date]) VALUES (7, N'FENCE WIRE', 1, CAST(0x413E0B00 AS Date))
INSERT [dbo].[Category_Master] ([Category_Id], [Category_Name], [User_Id], [Entry_Date]) VALUES (8, N'FENCE BOLT & NUTT', 1, CAST(0x413E0B00 AS Date))
INSERT [dbo].[Category_Master] ([Category_Id], [Category_Name], [User_Id], [Entry_Date]) VALUES (9, N'FENCE', 1, CAST(0x413E0B00 AS Date))
SET IDENTITY_INSERT [dbo].[Category_Master] OFF

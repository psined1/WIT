USE [WIT]
GO

/****** Object:  Table [dbo].[LItemType]    Script Date: 11/13/2017 10:24:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LItemType](
	[ItemTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](250) NULL,
	[MenuHide] [bit] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](50) NULL,
	CONSTRAINT [PK_LItemType] PRIMARY KEY CLUSTERED 
	(
		[ItemTypeID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LItemType] ADD  CONSTRAINT [DF_LItemType_MenuHide]  DEFAULT ((0)) FOR [MenuHide]
GO

ALTER TABLE [dbo].[LItemType] ADD  CONSTRAINT [DF_LItemType_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[LItemType] ADD  CONSTRAINT [DF_LItemType_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO


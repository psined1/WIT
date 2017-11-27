USE [WIT]
GO

/****** Object:  Table [dbo].[LItem]    Script Date: 11/27/2017 11:05:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LItem](
	[ItemID] [bigint] IDENTITY(1,1) NOT NULL,
	[ItemTypeID] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_LItem] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LItem]  WITH CHECK ADD  CONSTRAINT [FK_LItem_LItemType] FOREIGN KEY([ItemTypeID])
REFERENCES [dbo].[LItemType] ([ItemTypeID])
GO

ALTER TABLE [dbo].[LItem] CHECK CONSTRAINT [FK_LItem_LItemType]
GO

ALTER TABLE [dbo].[LItem] ADD  CONSTRAINT [DF_LItem_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[LItem] ADD  CONSTRAINT [DF_LItem_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO



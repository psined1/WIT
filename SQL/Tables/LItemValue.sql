USE [WIT]
GO

/****** Object:  Table [dbo].[LItemValue]    Script Date: 11/15/2017 12:27:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LItemValue](
	[ItemValueID] [bigint] IDENTITY(1,1) NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[ItemPropID] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_LItemValue] PRIMARY KEY CLUSTERED 
(
	[ItemValueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LItemValue]  WITH CHECK ADD  CONSTRAINT [FK_LItemValue_LItem] FOREIGN KEY([ItemID])
REFERENCES [dbo].[LItem] ([ItemID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LItemValue] CHECK CONSTRAINT [FK_LItemValue_LItem]
GO

ALTER TABLE [dbo].[LItemValue]  WITH CHECK ADD  CONSTRAINT [FK_LItemValue_LItemProp] FOREIGN KEY([ItemPropID])
REFERENCES [dbo].[LItemProp] ([ItemPropID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LItemValue] CHECK CONSTRAINT [FK_LItemValue_LItemProp]
GO

ALTER TABLE [dbo].[LItemValue] ADD  CONSTRAINT [DF_LItemValue_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[LItemValue] ADD  CONSTRAINT [DF_LItemValue_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO



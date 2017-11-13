USE [WIT]
GO

/****** Object:  Table [dbo].[LItemProp]    Script Date: 11/13/2017 10:42:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LItemProp](
	[ItemPropID] [bigint] NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](250) NULL,
	[PropType] [int] NOT NULL,
	[ItemTypeID] [int] NULL,
	[GridHide] [bit] NOT NULL,
	[ValueRule] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_LItemProp] PRIMARY KEY CLUSTERED 
(
	[ItemPropID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LItemProp]  WITH CHECK ADD  CONSTRAINT [FK_LItemProp_LItem] FOREIGN KEY([ItemID])
REFERENCES [dbo].[LItem] ([ItemID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LItemProp] CHECK CONSTRAINT [FK_LItemProp_LItem]
GO

ALTER TABLE [dbo].[LItemProp]  WITH CHECK ADD  CONSTRAINT [FK_LItemProp_LItemType] FOREIGN KEY([ItemTypeID])
REFERENCES [dbo].[LItemType] ([ItemTypeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LItemProp] CHECK CONSTRAINT [FK_LItemProp_LItemType]
GO

ALTER TABLE [dbo].[LItemProp] ADD  CONSTRAINT [DF_LItemProp_PropType]  DEFAULT ((0)) FOR [PropType]
GO

ALTER TABLE [dbo].[LItemProp] ADD  CONSTRAINT [DF_LItemProp_ValueRule]  DEFAULT ((0)) FOR [ValueRule]
GO


ALTER TABLE [dbo].[LItemProp] ADD  CONSTRAINT [DF_LItemProp_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[LItemProp] ADD  CONSTRAINT [DF_LItemProp_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO


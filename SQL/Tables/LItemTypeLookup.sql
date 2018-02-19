USE [WIT]
GO

/****** Object:  Table [dbo].[LItemTypeLookup]    Script Date: 02/19/2018 06:26:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LItemTypeLookup](
	[ItemTypeID] [int] NOT NULL,
	[ItemPropID] [int] NOT NULL,
	[Nill] [int] NULL,
 CONSTRAINT [PK_LItemTypeLookup] PRIMARY KEY CLUSTERED 
(
	[ItemTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[LItemTypeLookup]  WITH CHECK ADD  CONSTRAINT [FK_LItemTypeLookup_LItemProp] FOREIGN KEY([ItemPropID])
REFERENCES [dbo].[LItemProp] ([ItemPropID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LItemTypeLookup] CHECK CONSTRAINT [FK_LItemTypeLookup_LItemProp]
GO

ALTER TABLE [dbo].[LItemTypeLookup]  WITH CHECK ADD  CONSTRAINT [FK_LItemTypeLookup_LItemType] FOREIGN KEY([ItemTypeID])
REFERENCES [dbo].[LItemType] ([ItemTypeID])
GO

ALTER TABLE [dbo].[LItemTypeLookup] CHECK CONSTRAINT [FK_LItemTypeLookup_LItemType]
GO



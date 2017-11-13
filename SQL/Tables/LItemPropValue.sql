USE [WIT]
GO

/****** Object:  Table [dbo].[LItemPropValue]    Script Date: 11/13/2017 10:52:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LItemPropValue](
	[ItemPropValueID] [bigint] IDENTITY(1,1) NOT NULL,
	[ItemPropID] [bigint] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_LItemPropValue] PRIMARY KEY CLUSTERED 
(
	[ItemPropValueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LItemPropValue]  WITH CHECK ADD  CONSTRAINT [FK_LItemPropValue_LItemProp] FOREIGN KEY([ItemPropID])
REFERENCES [dbo].[LItemProp] ([ItemPropID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LItemPropValue] CHECK CONSTRAINT [FK_LItemPropValue_LItemProp]
GO

ALTER TABLE [dbo].[LItemPropValue] ADD  CONSTRAINT [DF_LItemPropValue_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[LItemPropValue] ADD  CONSTRAINT [DF_LItemPropValue_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO



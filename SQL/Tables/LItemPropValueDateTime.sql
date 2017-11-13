USE [WIT]
GO

/****** Object:  Table [dbo].[LItemPropValueDateTime]    Script Date: 11/13/2017 11:14:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LItemPropValueDateTime](
	[ItemPropValueID] [bigint] NOT NULL,
	[Value] [datetime] NOT NULL,
 CONSTRAINT [PK_LItemPropValueDateTime] PRIMARY KEY CLUSTERED 
(
	[ItemPropValueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LItemPropValueDateTime]  WITH CHECK ADD  CONSTRAINT [FK_LItemPropValueDateTime_LItemPropValue] FOREIGN KEY([ItemPropValueID])
REFERENCES [dbo].[LItemPropValue] ([ItemPropValueID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LItemPropValueDateTime] CHECK CONSTRAINT [FK_LItemPropValueDateTime_LItemPropValue]
GO



USE [WIT]
GO

/****** Object:  Table [dbo].[LItemPropValueDecimal]    Script Date: 11/13/2017 11:12:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LItemPropValueDecimal](
	[ItemPropValueID] [bigint] NOT NULL,
	[Value] [numeric](30,8) NOT NULL,
 CONSTRAINT [PK_LItemPropValueDecimal] PRIMARY KEY CLUSTERED 
(
	[ItemPropValueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LItemPropValueDecimal]  WITH CHECK ADD  CONSTRAINT [FK_LItemPropValueDecimal_LItemPropValue] FOREIGN KEY([ItemPropValueID])
REFERENCES [dbo].[LItemPropValue] ([ItemPropValueID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LItemPropValueDecimal] CHECK CONSTRAINT [FK_LItemPropValueDecimal_LItemPropValue]
GO



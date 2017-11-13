USE [WIT]
GO

/****** Object:  Table [dbo].[LItemPropValueString]    Script Date: 11/13/2017 11:08:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LItemPropValueString](
	[ItemPropValueID] [bigint] NOT NULL,
	[Value] [varchar](1000) NOT NULL,
 CONSTRAINT [PK_LItemPropValueString] PRIMARY KEY CLUSTERED 
(
	[ItemPropValueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LItemPropValueString]  WITH CHECK ADD  CONSTRAINT [FK_LItemPropValueString_LItemPropValue] FOREIGN KEY([ItemPropValueID])
REFERENCES [dbo].[LItemPropValue] ([ItemPropValueID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LItemPropValueString] CHECK CONSTRAINT [FK_LItemPropValueString_LItemPropValue]
GO



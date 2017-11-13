USE [WIT]
GO

/****** Object:  Table [dbo].[LItemPropValueInteger]    Script Date: 11/13/2017 11:10:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LItemPropValueInteger](
	[ItemPropValueID] [bigint] NOT NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_LItemPropValueInteger] PRIMARY KEY CLUSTERED 
(
	[ItemPropValueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LItemPropValueInteger]  WITH CHECK ADD  CONSTRAINT [FK_LItemPropValueInteger_LItemPropValue] FOREIGN KEY([ItemPropValueID])
REFERENCES [dbo].[LItemPropValue] ([ItemPropValueID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LItemPropValueInteger] CHECK CONSTRAINT [FK_LItemPropValueInteger_LItemPropValue]
GO



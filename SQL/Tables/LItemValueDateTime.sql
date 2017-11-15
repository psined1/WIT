USE [WIT]
GO

/****** Object:  Table [dbo].[LItemValueDateTime]    Script Date: 11/13/2017 11:14:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LItemValueDateTime](
	[ItemValueID] [bigint] NOT NULL,
	[Value] [datetime] NOT NULL,
 CONSTRAINT [PK_LItemValueDateTime] PRIMARY KEY CLUSTERED 
(
	[ItemValueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LItemValueDateTime]  WITH CHECK ADD  CONSTRAINT [FK_LItemValueDateTime_LItemValue] FOREIGN KEY([ItemValueID])
REFERENCES [dbo].[LItemValue] ([ItemValueID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LItemValueDateTime] CHECK CONSTRAINT [FK_LItemValueDateTime_LItemValue]
GO


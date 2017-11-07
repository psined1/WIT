USE [WIT]
GO

/****** Object:  Table [dbo].[ProductFeature]    Script Date: 11/06/2017 23:40:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ProductFeature](
	[ProductFeatureID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](10) NOT NULL,
	[Name] [varchar](100) NULL,
	[Description] [varchar](250) NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_ProductFeature] PRIMARY KEY CLUSTERED 
(
	[ProductFeatureID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ProductFeature] ADD  CONSTRAINT [DF_ProductFeature_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[ProductFeature] ADD  CONSTRAINT [DF_ProductFeature_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO



USE [WIT]
GO

/****** Object:  Table [dbo].[Product]    Script Date: 11/06/2017 23:38:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductCode] [varchar](10) NOT NULL,
	[ProductName] [varchar](100) NULL,
	[Description] [varchar](250) NULL,
	[ProductFeatureID] [int] NULL,
	[ProductClassID] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductClass] FOREIGN KEY([ProductClassID])
REFERENCES [dbo].[ProductClass] ([ProductClassID])
ON UPDATE CASCADE
ON DELETE SET NULL
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductClass]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductFeature] FOREIGN KEY([ProductFeatureID])
REFERENCES [dbo].[ProductFeature] ([ProductFeatureID])
ON UPDATE CASCADE
ON DELETE SET NULL
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductFeature]
GO

ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO



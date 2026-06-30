USE [RTFT]
GO

/****** Object:  Table [dbo].[StdNaming_AttMap]    Script Date: 04/18/2013 08:46:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StdNaming_AttMap](
	[StdNamingColumnName] [nvarchar](50) NOT NULL,
	[COE_TESTENV] [nvarchar](50) NOT NULL,
	[COE] [nvarchar](50) NULL
) ON [PRIMARY]

GO


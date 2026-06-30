USE [RTFT]
GO

/****** Object:  Table [dbo].[StdNaming_Program]    Script Date: 04/18/2013 08:47:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StdNaming_Program](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProgramName] [nvarchar](255) NOT NULL
) ON [PRIMARY]

GO


USE [RTFT]
GO

/****** Object:  Table [dbo].[StdNaming]    Script Date: 04/18/2013 08:46:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StdNaming](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocumentType] [nvarchar](max) NULL,
	[Discipline] [nvarchar](255) NULL,
	[Std_Folders] [nvarchar](max) NULL,
	[RegionalProcess] [nvarchar](255) NULL,
	[Program] [nvarchar](50) NULL,
	[ArchivedToHPTRIM] [nvarchar](3) NULL,
	[DocTypeDescription] [nvarchar](max) NULL
) ON [PRIMARY]

GO


USE [Demo]
GO

/****** Object:  Table [dbo].[UserData]    Script Date: 14.5.2022 г. 19:46:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserData](
	[Id] [int] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Age] [tinyint] NULL,
 CONSTRAINT [PK_UserData_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


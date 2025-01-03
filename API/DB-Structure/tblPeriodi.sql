CREATE TABLE [dbo].[tblPeriodi](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Mese] [int] NOT NULL,
	[Anno] [int] NOT NULL,
	[Descrizione] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_tblPeriodi] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
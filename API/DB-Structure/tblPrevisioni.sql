CREATE TABLE [dbo].[tblPrevisioni](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RifPeriodo] [int] NOT NULL,
	[Giorno] [int] NOT NULL,
	[Descrizione] [varchar](250) NOT NULL,
	[Costo] [money] NOT NULL,
	[AddebitoAutomatico] [int] NOT NULL,
	[RifTipoAttivita] [int] NOT NULL,
	[Gestita] [int] NOT NULL,
 CONSTRAINT [PK_tblPrevisioni] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblPrevisioni] ADD  CONSTRAINT [DF_tblPrevisioni_AddebitoAutomatico]  DEFAULT ((0)) FOR [AddebitoAutomatico]
GO

ALTER TABLE [dbo].[tblPrevisioni] ADD  CONSTRAINT [DF_tblPrevisioni_Gestita]  DEFAULT ((0)) FOR [Gestita]
GO

ALTER TABLE [dbo].[tblPrevisioni]  WITH CHECK ADD  CONSTRAINT [FK_tblPrevisioni_tblPeriodi] FOREIGN KEY([RifPeriodo])
REFERENCES [dbo].[tblPeriodi] ([ID])
GO

ALTER TABLE [dbo].[tblPrevisioni] CHECK CONSTRAINT [FK_tblPrevisioni_tblPeriodi]
GO

ALTER TABLE [dbo].[tblPrevisioni]  WITH CHECK ADD  CONSTRAINT [FK_tblPrevisioni_tblTipoAttivita] FOREIGN KEY([RifTipoAttivita])
REFERENCES [dbo].[tblTipoAttivita] ([ID])
GO

ALTER TABLE [dbo].[tblPrevisioni] CHECK CONSTRAINT [FK_tblPrevisioni_tblTipoAttivita]
GO


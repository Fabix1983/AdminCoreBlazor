CREATE PROCEDURE [dbo].[spPrevisione_Nuova] (
	@iGiorno int,
	@RifPeriodo int, 
	@Descrizione varchar(250), 
	@Tipologia varchar(50),
	@Costo money
)AS

IF @Tipologia = 'Costo'
BEGIN 
	IF @Costo > 0
	BEGIN
		SET @Costo = @Costo * -1
	END
END
ELSE 
BEGIN
	IF @Costo < 0
	BEGIN
		SET @Costo = @Costo * -1
	END
END

SET @Descrizione = LTRIM(RTRIM(@Descrizione))

SET XACT_ABORT ON
BEGIN TRAN tr

	INSERT INTO tblPrevisioni(Giorno, RifPeriodo, Descrizione, Costo)
	VALUES(@iGiorno, @RifPeriodo, @Descrizione, @Costo)

COMMIT TRAN tr
GO



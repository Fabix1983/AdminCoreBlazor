CREATE PROCEDURE [dbo].[spInizialize] AS

SET XACT_ABORT ON
BEGIN TRAN trc

	DECLARE @RifPeriodo int 
	DECLARE @Giorno int
	DECLARE @Descrizione varchar(250)
	DECLARE @Costo money
	DECLARE @RifTipoAttivita int 
	DECLARE @ID int

	DECLARE Prev_Cursor_Dst CURSOR
	FOR
		-- Previsioni di Spesa con Addebito Automatico
		SELECT 
			Prev.RifPeriodo, 
			Prev.Giorno, 
			Prev.Descrizione, 
			Prev.Costo, 
			Prev.RifTipoAttivita, 
			Prev.ID 
		FROM 
			tblPrevisioni Prev
			INNER JOIN tblPeriodi Per ON Per.ID = Prev.RifPeriodo
		WHERE 
			ISNULL(Prev.AddebitoAutomatico, 0) = 1 AND 
			ISNULL(Prev.Gestita, 0) = 0 AND
			Per.Anno = YEAR(GETDATE()) AND
			Per.Mese = MONTH(GETDATE()) AND
			Prev.Giorno <= DAY(GETDATE())

	OPEN Prev_Cursor_Dst

	FETCH NEXT FROM Prev_Cursor_Dst
	INTO @RifPeriodo, @Giorno, @Descrizione, @Costo, @RifTipoAttivita, @ID 

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC spAttivita_Nuova
			@Giorno, 
			@RifPeriodo, 
			@RifTipoAttivita, 
			@Descrizione, 
			@Costo	

		DELETE FROM tblPrevisioni WHERE ID = @ID  

		FETCH NEXT FROM Prev_Cursor_Dst
		INTO  @RifPeriodo, @Giorno, @Descrizione, @Costo, @RifTipoAttivita, @ID 
	END
		
	CLOSE Prev_Cursor_Dst
	DEALLOCATE Prev_Cursor_Dst

COMMIT TRAN trc





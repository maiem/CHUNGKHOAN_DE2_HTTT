USE [DE2]
GO
/****** Object:  Trigger [dbo].[TRG_DELETE_LENHDAT]    Script Date: 5/16/2021 3:57:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[TRG_DELETE_LENHDAT]
ON [dbo].[LENHDAT] AFTER DELETE
AS
BEGIN
	DECLARE @LOAIGD_Del NCHAR(1),
			@MACP_Del NCHAR(7),
			@NGAYDAT_Del DATETIME,
			@GIADAT_Del FLOAT
	
	DECLARE @CurrentDay DATETIME
	SET @CurrentDay = GETDATE()

	SELECT @LOAIGD_Del = deleted.LOAIGD,
		   @MACP_Del = deleted.MACP,
		   @NGAYDAT_Del = deleted.NGAYDAT,
		   @GIADAT_Del = deleted.GIADAT
	FROM deleted 
	IF (  DAY(@CurrentDay) = DAY(@NGAYDAT_Del) AND MONTH(@CurrentDay)=MONTH(@NGAYDAT_Del) AND YEAR(@CurrentDay) = YEAR(@NGAYDAT_Del))
	BEGIN
		EXEC SP_HIENTHI_BGTT @LOAIGD_Del, @MACP_Del, @NGAYDAT_Del, @GIADAT_Del	
	END
	
END

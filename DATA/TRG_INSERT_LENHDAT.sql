USE [DE2]
GO
/****** Object:  Trigger [dbo].[TRG_INSERT_LENHDAT]    Script Date: 5/16/2021 3:20:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[TRG_INSERT_LENHDAT]
ON [dbo].[LENHDAT] AFTER INSERT 
AS
BEGIN
	DECLARE @LOAIGD_Ins NCHAR(1),
			@MACP_Ins NCHAR(7),
			@NGAYDAT_Ins DATETIME,
			@GIADAT_Ins FLOAT

	SELECT @LOAIGD_Ins = inserted.LOAIGD,
		   @MACP_Ins = inserted.MACP,
		   @NGAYDAT_Ins = inserted.NGAYDAT,
		   @GIADAT_Ins = inserted.GIADAT
	FROM inserted 
	EXEC SP_HIENTHI_BGTT @LOAIGD_Ins, @MACP_Ins, @NGAYDAT_Ins, @GIADAT_Ins

END

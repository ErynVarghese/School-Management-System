USE [SMS]
GO
/****** Object:  StoredProcedure [dbo].[proc_fee]    Script Date: 7/1/2025 9:39:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[proc_fee]
	-- Add the parameters for the stored procedure here

(

@FeeId int = null,
@ClassId int = null,
@TotalFee decimal(7,3) = null,
@Installment1 decimal(7,3) = null,
@Installment2 decimal(7,3) = null,
@Installment3 decimal(7,3) = null,
@mode int

)


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Not needed (0)



	-- needed
	IF @Mode = 0
    BEGIN
        SELECT FeeId,ClassId,TotalFee, Installment1,Installment2 , Installment3
        FROM  Feestructure
    END

	IF @Mode = 1
    BEGIN
        SELECT FeeId,ClassId,TotalFee, Installment1,Installment2 , Installment3
        FROM  Feestructure
		where feeid = @FeeId;
    END

	IF @Mode = 2
	BEGIN
    -- Insert into FeeStructure
		INSERT INTO FeeStructure (ClassId, TotalFee, Installment1, Installment2, Installment3)
		VALUES (@ClassId, @TotalFee, @Installment1, @Installment2, @Installment3);
	END

	IF @MODE = 3
	BEGIN
			UPDATE feestructure
			SET 
			classId = @ClassId,
			TotalFee = @TotalFee,
			Installment1 = @Installment1,
			Installment2 = @Installment2,
			Installment3 = @Installment3
			WHERE feeid = @feeid;
	END


	if @mode = 4
	begin
		delete from feestructure where feeid=@feeid;
	end


	IF @Mode = 5
	BEGIN
		SELECT MAX(FeeId) + 1  AS MaxFeeId FROM feestructure;
	END

	IF @Mode = 6
	BEGIN
		SELECT TotalFee from feestructure 
		where classid =@classid;
	END

	IF @MODE = 7
	BEGIN
			UPDATE feestructure
			SET 
			classId = @ClassId,
			TotalFee = @TotalFee,
			Installment1 = @Installment1,
			Installment2 = @Installment2,
			Installment3 = @Installment3
			WHERE classid = @classid;
	END

	IF @MODE = 8
	BEGIN
		  select installment1 from feestructure where classid = @classid;
	END

	IF @MODE = 9
	BEGIN
		  select installment2 from feestructure where classid = @classid;
	END

	IF @MODE = 10
	BEGIN
		  select installment3 from feestructure where classid = @classid;
	END


END

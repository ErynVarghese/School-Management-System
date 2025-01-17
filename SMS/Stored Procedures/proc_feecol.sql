USE [SMS]
GO
/****** Object:  StoredProcedure [dbo].[proc_feecol]    Script Date: 7/1/2025 9:39:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[proc_feecol]
	-- Add the parameters for the stored procedure here

(

@FeeColId int = null,
@StudentId int = null,
@ClassId int = null,
@Installment1 varchar(100) = null,
@Installment2 varchar(100) = null,
@Installment3 varchar(100) = null,
@FeeStatus varchar(100) = null,
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
        SELECT FeeColId,StudentId,ClassId, Installment1,Installment2 , Installment3 , FeeStatus
        FROM  feecollection 
    END

	IF @Mode = 1
    BEGIN
        SELECT FeeColId,StudentId,ClassId, Installment1,Installment2 , Installment3 , FeeStatus
        FROM  feecollection
		where feecolid = @FeeColId;
    END




	IF @Mode = 2
	BEGIN
  
    -- Set FeeStatus based on installment values
    IF @Installment3 = 1
    BEGIN
        SET @FeeStatus = 'Paid Installment 1,2,3'
    END
    ELSE IF @Installment2 = 1
    BEGIN
        SET @FeeStatus = 'Paid Installment 1,2'
    END
    ELSE IF @Installment1 = 1
    BEGIN
        SET @FeeStatus = 'Paid Installment 1'
    END
    ELSE
    BEGIN
        SET @FeeStatus = 'Pending' 
    END

	    IF EXISTS (SELECT 1 FROM feecollection WHERE StudentId = @StudentId AND ClassId = @ClassId)
    BEGIN
        -- Update the record if StudentId exists
        UPDATE feecollection
        SET Installment1 = @Installment1,
            Installment2 = @Installment2,
            Installment3 = @Installment3,
            FeeStatus = @FeeStatus
			
        WHERE StudentId = @StudentId AND ClassId = @ClassId;
    END
    ELSE
    BEGIN

    -- Insert into FeeCollection
    INSERT INTO feecollection (StudentId, ClassId, Installment1, Installment2, Installment3, FeeStatus)
    VALUES (@StudentId, @ClassId, @Installment1, @Installment2, @Installment3, @FeeStatus);

	END
END

	IF @MODE = 3
	BEGIN

	IF @Installment3 = 1 
    BEGIN
        SET @FeeStatus = 'Paid Installment 1,2,3'
    END
    ELSE IF @Installment2 = 1
    BEGIN
        SET @FeeStatus = 'Paid Installment 1,2'
    END
    ELSE IF @Installment1 = 1
    BEGIN
        SET @FeeStatus = 'Paid Installment 1'
    END
    ELSE
    BEGIN
        SET @FeeStatus = 'Pending' 
    END

			UPDATE feecollection
			SET 
			studentid = @studentid,
			classId = @ClassId,
			Installment1 = @Installment1,
			Installment2 = @Installment2,
			Installment3 = @Installment3,
			feestatus = @feestatus
			WHERE FeeColId = @FeeColId;
	END


	if @mode = 4
	begin
		delete from feecollection where feecolid=@feecolid;
	end


  IF @Mode = 5
	BEGIN
		SELECT MAX(feecolid) + 1  AS MaxFeeColId FROM feecollection;
	END

	IF @MODE = 6
	BEGIN
		  select installment1 from FeeCollection where StudentId = @StudentId;
	END

	IF @MODE = 7
	BEGIN
		  select installment2 from FeeCollection where StudentId = @StudentId;
	END

	IF @MODE = 8 
	BEGIN
		  select installment3 from FeeCollection where StudentId = @StudentId;
	END


END


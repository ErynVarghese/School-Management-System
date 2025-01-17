USE [SMS]
GO
/****** Object:  StoredProcedure [dbo].[proc_class]    Script Date: 7/1/2025 9:37:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[proc_class]
	-- Add the parameters for the stored procedure here
(
@ClassId int = null,
@ClassName varchar(100) = null,
@ClassSize int = null,
@ClassFee decimal(7,3) = null,
@InstallmentNo int = null,
@mode int
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @Mode = 0
    BEGIN
        SELECT ClassId, ClassName, ClassSize, ClassFee ,InstallmentNo
        FROM Class;
    END

	IF @Mode = 1
    BEGIN
        SELECT ClassId,ClassName,ClassSize, ClassFee,InstallmentNo
        FROM Class
		where ClassId = @ClassId;
    END

	IF @Mode = 2
    BEGIN
        INSERT INTO Class (ClassName , ClassSize, ClassFee, InstallmentNo)
        VALUES (@ClassName , @ClassSize, @ClassFee, @InstallmentNo)

    END

	IF @MODE = 3
	BEGIN
			UPDATE class
			SET 
			classname = @ClassName,
			ClassSize = @ClassSize,
			ClassFee = @ClassFee,
			InstallmentNo = @InstallmentNo
			WHERE classId = @ClassId;
	END


	if @mode = 4
	begin
		delete from Class where ClassId=@ClassId;
	end

	IF @Mode = 5
	BEGIN
		SELECT MAX(ClassId) + 1  AS MaxClassId FROM class;
	END

	if @mode = 6
	begin
	SELECT COUNT(*) FROM Class WHERE LOWER(ClassName) = LOWER(@ClassName)
	end
	
    if @mode = 7
	begin
	SELECT COUNT(*) FROM Class WHERE classid = @classid
	end

	if @mode = 8
	begin
	SELECT className from class where classid = @classid 
	end


END

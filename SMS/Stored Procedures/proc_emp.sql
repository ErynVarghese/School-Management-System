USE [SMS]
GO
/****** Object:  StoredProcedure [dbo].[proc_emp]    Script Date: 7/1/2025 9:38:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[proc_emp] 
	-- Add the parameters for the stored procedure here
(
@EmpId int = null,
@EmpName varchar(100) = null,
@ContactNo int = null,
@Email varchar(100) = null,
@DOB date = null,
@DeptId int = null,
@DOJ date = null,
@EmpUserName varchar(100) = null,
@EmpPassword int = null,
@Mode int,
@Imagebase64 varchar(max) = null,
@FileExtension varchar(100) = null
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- SELECT FOR MAIN PAGE DISPLAY 
    IF @Mode = 0
    BEGIN
        SELECT EmpId, EmpName, ContactNo, Email, DOB, DeptId , DOJ , EmpUserName , EmpPassword
        FROM Employer;
    END

	IF @Mode = 1
    BEGIN
        SELECT EmpId, EmpName, ContactNo, Email, DOB, DeptId , DOJ , EmpUserName , EmpPassword
        FROM Employer
		where EmpId = @EmpId;
    END

	IF @Mode = 2
    BEGIN
        INSERT INTO Employer (EmpName, ContactNo, Email, DOB, DeptId, DOJ, EmpUsername ,EmpPassword)
        VALUES (@EmpName, @ContactNo, @Email, @DOB, @DeptId, @DOJ, @EmpUsername, @EmpPassword)

	IF @MODE = 3
	BEGIN
			UPDATE Employer
			SET EmpName = @EmpName,
				ContactNo = @ContactNo,
				Email = @Email,
				DOB = @DOB,
				DeptId = @DeptId,
				DOJ = @DOJ,
				EmpUsername = @EmpUsername,
				EmpPassword = @EmpPassword
			WHERE EmpId = @EmpId;
	END



    END

	if @mode = 4
	begin
		delete from employer where EmpId=@EmpId;
	end



	IF @Mode = 5
	BEGIN
		SELECT MAX(EmpId) + 1  AS MaxEmpId FROM Employer;
	END

	IF @Mode = 6
    BEGIN
        SELECT EmpId, EmpName, ContactNo, Email, DOB, DeptId , DOJ , EmpUserName , EmpPassword
        FROM Employer
		where EmpUserName = @EmpUserName;
    END

	If @Mode = 8
	begin
		select Imagebase64, fileextension from employer where EmpId = @empid;
	end

	If @Mode = 7
	begin
		UPDATE EMPLOYER
		SET IMAGEBASE64 = @Imagebase64,
			Fileextension = @FileExtension
			where EmpId =@EmpId
	end



END

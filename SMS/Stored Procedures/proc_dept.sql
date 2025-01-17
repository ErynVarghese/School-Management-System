USE [SMS]
GO
/****** Object:  StoredProcedure [dbo].[proc_dept]    Script Date: 7/1/2025 9:38:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[proc_dept]
	-- Add the parameters for the stored procedure here
(
@DeptId int = null,
@DeptName varchar(100) = null,
@mode int
)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @Mode = 0
    BEGIN
        SELECT DeptId, Deptname
        FROM Department;
    END

	IF @Mode = 1
    BEGIN
        SELECT DeptId,DeptName
        FROM department
		where DeptId = @DeptId;
    END

	IF @Mode = 2
    BEGIN
        INSERT INTO Department (DeptName)
        VALUES (@DeptName)

    END

	IF @MODE = 3
	BEGIN
			UPDATE department
			SET 
			deptname = @Deptname
			WHERE DeptId = @DeptId;
	END



	if @mode = 4
	begin
		delete from department where DeptId=@DeptId;
	end

	IF @Mode = 5
	BEGIN
		SELECT MAX(DeptId) + 1  AS MaxDeptId FROM Department;
	END

    if @mode = 6
	begin
		SELECT COUNT(*) FROM Department WHERE LOWER(DeptName) = LOWER(@DeptName)
	end



		

END

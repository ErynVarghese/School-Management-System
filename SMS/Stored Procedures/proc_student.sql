USE [SMS]
GO
/****** Object:  StoredProcedure [dbo].[proc_student]    Script Date: 7/1/2025 9:40:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[proc_student]
	-- Add the parameters for the stored procedure here

(
@StudentId int = null,
@StudentName varchar(100) = null,
@DOB date = null,
@ClassId int = null,
@SectionId int = null,
@FatherName varchar(100) = null,
@ContactNo int = null,
@StudentAddress varchar(100) = null,
@StudentUsername varchar(100) = null,
@StudentPassword int = null,
@StudentFee decimal(7,3) = null,
@ImageName varchar(100) = null,
@Mode int
)


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	    IF @Mode = 0
    BEGIN
        SELECT StudentId, StudentName, DOB, ClassId, SectionId, FatherName , ContactNo , StudentAddress ,StudentUsername
		 , StudentPassword , StudentFee , ImageName
        FROM Student;
    END

	IF @Mode = 1
    BEGIN
        SELECT StudentId, StudentName, DOB, ClassId,  SectionId, FatherName , ContactNo , StudentAddress ,StudentUsername
		 , StudentPassword , StudentFee , ImageName
        FROM Student
		where StudentId = @StudentId;
    END


	IF @Mode = 2
    BEGIN
        INSERT INTO Student (StudentName, DOB, ClassId, SectionId, FatherName, ContactNo, StudentAddress, StudentUsername ,StudentPassword , StudentFee ,ImageName)
        VALUES (@StudentName, @DOB, @ClassId,  @SectionId, @FatherName, @ContactNo, @StudentAddress, @StudentUsername ,@StudentPassword , @StudentFee ,@ImageName)

    END

	IF @MODE = 3
	BEGIN
			UPDATE Student
			SET StudentName = @StudentName,
				DOB = @DOB,
				ClassId = @ClassId,
				SectionId = @SectionId,
				FatherName = @FatherName,
				ContactNo = @ContactNo,
				StudentAddress = @StudentAddress,
				StudentUsername = @StudentUsername,
				StudentPassword = @StudentPassword,
				StudentFee = @StudentFee,
				ImageName = @ImageName
			WHERE StudentId = @StudentId;
	END

	if @mode = 4
	begin
		delete from student where StudentId=@StudentId;
	end



	IF @Mode = 5
	BEGIN
		SELECT MAX(StudentId) + 1  AS MaxStudId FROM Student;
	END




    IF @Mode = 6
    BEGIN
        SELECT StudentId, StudentName, DOB, ClassId, SectionId, FatherName , ContactNo , StudentAddress ,StudentUsername
		 , StudentPassword , StudentFee ,ImageName
        FROM Student
		where StudentUsername = @StudentUsername;
    END


	IF @mode = 7
    BEGIN
		SELECT COUNT(*) FROM student 
		WHERE studentid = @studentid

	END		

	IF @mode = 8
    BEGIN
		SELECT studentid, studentname from student 
		where classid = @classid;

	END		

	if @mode= 9
	begin
		UPDATE Student
    SET ImageName = @ImageName
    WHERE StudentId = @StudentId;
	end

	if @mode= 10
	begin
		select imagename from student
    WHERE StudentId = @StudentId;
	end


END

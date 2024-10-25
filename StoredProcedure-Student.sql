USE [SMS]
GO
/****** Object:  StoredProcedure [dbo].[proc_student]    Script Date: 2 Oct 2024 4:03:03 PM ******/
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
		 , StudentPassword , StudentFee
        FROM Student;
    END

	IF @Mode = 1
    BEGIN
        SELECT StudentId, StudentName, DOB, ClassId,  SectionId, FatherName , ContactNo , StudentAddress ,StudentUsername
		 , StudentPassword , StudentFee
        FROM Student
		where StudentId = @StudentId;
    END

	IF @MODE = 2
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
				StudentFee = @StudentFee
			WHERE StudentId = @StudentId;
	END

	IF @Mode = 3
    BEGIN
        INSERT INTO Student (StudentName, DOB, ClassId, SectionId, FatherName, ContactNo, StudentAddress, StudentUsername ,StudentPassword , StudentFee)
        VALUES (@StudentName, @DOB, @ClassId,  @SectionId, @FatherName, @ContactNo, @StudentAddress, @StudentUsername ,@StudentPassword , @StudentFee)

    END

	if @mode = 4
	begin
		delete from student where StudentId=@StudentId;
	end

	if @mode = 5
	begin
		select StudentId, StudentName from student;
	end

	IF @Mode = 6
    BEGIN
        SELECT StudentId, StudentName, DOB, ClassId, SectionId, FatherName , ContactNo , StudentAddress ,StudentUsername
		 , StudentPassword , StudentFee
        FROM Student
		where StudentUsername = @StudentUsername;
    END

	IF @Mode = 7
	BEGIN
		SELECT MAX(StudentId) + 1  AS MaxStudId FROM Student;
	END

	IF @Mode = 8
	BEGIN
		SELECT studentid, classid  from student;
	END

	if @mode = 9
	begin
		select StudentId, StudentName from student
		where ClassId = @ClassId;
	end

	if @mode = 10
	begin
		select StudentName from student 
		where studentid = @studentid;
	end

	if @mode = 11
	begin

        SELECT s.StudentId, s.StudentName
        FROM Student s
        INNER JOIN FeeCollection f ON s.StudentId = f.StudentId
 
	end

END

USE [SMS]
GO
/****** Object:  StoredProcedure [dbo].[proc_admin]    Script Date: 7/1/2025 9:35:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[proc_admin]
	-- Add the parameters for the stored procedure here

(
@AdminId int = null,
@AdminUsername varchar(100) = null,
@AdminName varchar(100) = null,
@AdminPassword int = null,
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
        SELECT AdminId, AdminUsername, AdminName, AdminPassword
        FROM Admin;
    END

	IF @Mode = 1
    BEGIN
        SELECT AdminId, AdminUsername, AdminName, AdminPassword
        FROM Admin
		where adminid = @AdminId;
    END

	IF @Mode = 2
    BEGIN
        INSERT INTO admin (AdminUsername, AdminName, AdminPassword)
        VALUES (@AdminUsername, @AdminName, @AdminPassword)

    END

	IF @MODE = 3
	BEGIN
			UPDATE admin
			SET adminusername = @adminUsername,
				adminname = @AdminName,
				adminpassword = @AdminPassword
			WHERE adminid = @AdminId;
	END


	if @mode = 4
	begin
		delete from admin where adminid=@AdminId;
	end


	IF @Mode = 5
	BEGIN
		SELECT MAX(AdminId) + 1  AS MaxAdminId FROM admin;
	END

	IF @Mode = 6
    BEGIN
        SELECT AdminId, AdminUsername, AdminName, AdminPassword
        FROM admin
		where adminusername = @AdminUsername;
    END



END

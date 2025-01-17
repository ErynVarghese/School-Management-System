USE [SMS]
GO
/****** Object:  StoredProcedure [dbo].[proc_section]    Script Date: 7/1/2025 9:39:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[proc_section]
	-- Add the parameters for the stored procedure here
(
@SectionId int = null,
@SectionName varchar(100) = null,
@ClassId int = null,
@TotalSpace int = null,
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
        SELECT SectionId, SectionName, ClassId , TotalSpace
        FROM section;
    END

	IF @Mode = 1
    BEGIN
        SELECT SectionId, SectionName, ClassId , TotalSpace
        FROM section
		where SectionId = @SectionID;
    END

	IF @Mode = 2
    BEGIN
        INSERT INTO section (SectionName, ClassId , TotalSpace)
        VALUES (@SectionName, @ClassId , @TotalSpace)

    END

	IF @MODE = 3
	BEGIN
			UPDATE section
			SET 
			Sectionname = @SectionName,
			ClassId = @ClassId,
			TotalSpace = @TotalSpace
			WHERE sectionId = @SectionId;
	END



	if @mode = 4
	begin
		delete from section where sectionId=@SectionId;
	end


	IF @Mode = 5
	BEGIN
		SELECT MAX(sectionid) + 1  AS MaxSectionId FROM Section;
	END


	if @mode = 6
	begin
	Select SectionId,SectionName from section where ClassId=@ClassId
	end

	IF @mode = 7
    BEGIN
		SELECT COUNT(*) FROM Section 
		WHERE LOWER(SectionName) = LOWER(@SectionName)
		AND ClassId = @ClassId 
		AND (@SectionId IS NULL OR SectionId <> @SectionId)
	END		

	IF @mode = 8
    BEGIN
		SELECT COUNT(*) FROM Section 
		WHERE sectionid = @sectionid

	END		




END

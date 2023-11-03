--Populate the database categories table with default data
USE [WebApiDatabase];

BEGIN TRY
	BEGIN TRANSACTION ADDTABLE_Categories_Content
	    DECLARE @date DateTime = GETUTCDATE();

		IF (EXISTS(
				SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
			   WHERE TABLE_NAME = N'Categories') AND (SELECT COUNT(CategoryID) FROM [Categories]) = 0)
		BEGIN
			PRINT N'Creating table Categories content';
			INSERT INTO [Categories] (Category, SubCategory, CreatedDate, CreatedBy, LastUpdatedDate, LastUpdatedBy, isDeleted)
			VALUES
			(N'Science', N'Biological', @date, 0, null, null , 0),
			(N'Science', N'Chemical', @date, 0, null, null , 0),
			(N'Science', N'Physical', @date, 0, null, null , 0),
			(N'Science', N'Mathematical', @date, 0, null, null , 0),
			(N'Science', N'Social', @date, 0, null, null , 0),
			(N'Science', N'Logical', @date, 0, null, null , 0),
			(N'Science', N'Engineering', @date, 0, null, null , 0),
			(N'Science', N'Medicine', @date, 0, null, null , 0),
			('History', N'Social', @date, 0, null, null , 0),
			('History', N'Political', @date, 0, null, null , 0),
			('History', N'Economic', @date, 0, null, null , 0),
			('History', N'Diplomatic', @date, 0, null, null , 0),
			('History', N'Art', @date, 0, null, null , 0),
			('History', N'Science', @date, 0, null, null , 0),
			('History', N'Cultural', @date, 0, null, null , 0),
			('Arts', N'Visual', @date, 0, null, null , 0),
			('Arts', N'Literature', @date, 0, null, null , 0),
			('Arts', N'Decorative', @date, 0, null, null , 0),
			('Arts', N'Performing', @date, 0, null, null , 0),
			('Arts', N'Music', @date, 0, null, null , 0),
			('Arts', N'Architecture', @date, 0, null, null , 0),
			('Arts', N'Film', @date, 0, null, null , 0),
			('Entertainment', N'Food', @date, 0, null, null , 0),
			('Entertainment', N'Drink', @date, 0, null, null , 0),
			('Entertainment', N'Ludology', @date, 0, null, null , 0),
			('Entertainment', N'Performance', @date, 0, null, null , 0),
			('Entertainment', N'Events', @date, 0, null, null , 0),
			('Entertainment', N'Television', @date, 0, null, null , 0),
			('Entertainment', N'Radio', @date, 0, null, null , 0),
			('Entertainment', N'HHGTTG', @date, 0, null, null , 1),
			('Geography', N'Human', @date, 0, null, null , 0),
			('Geography', N'Physical', @date, 0, null, null , 0),
			('Geography', N'Cartography', @date, 0, null, null , 0),
			('Sport', N'Olympics', @date, 0, null, null , 0),
			('Sport', N'Aquatic', @date, 0, null, null , 0),
			('Sport', N'Team', @date, 0, null, null , 0),
			('Sport', N'Individual', @date, 0, null, null , 0),
			('Sport', N'Winter', @date, 0, null, null , 0),
			('Sport', N'Extreme', @date, 0, null, null , 0),
			('Sport', N'Motor', @date, 0, null, null , 0)


			PRINT N'Finished creating table Categories content'
		END
		ELSE
			PRINT N'Categories table content already exists'

		COMMIT TRANSACTION ADDTABLE_Categories_Content
END TRY
BEGIN CATCH
		PRINT N'';
		PRINT N'';
		PRINT N'FINISHED Create table Categories content , WITH ERRORS, ROLLING BACK';
		PRINT ERROR_MESSAGE();
		ROLLBACK TRANSACTION ADDTABLE_Categories_Content
END CATCH
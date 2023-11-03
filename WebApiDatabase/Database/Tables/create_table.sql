--Create the database categories table
USE [WebApiDatabase];

BEGIN TRY
	BEGIN TRANSACTION ADDTABLE_Categories

	-- BELOW FOR DEVELOPMENT RECREATE TABLE EACH RUN
	    IF (EXISTS(
				SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
			   WHERE TABLE_NAME = N'Categories'))
			BEGIN
				DROP TABLE [Categories];
			END

		IF (NOT EXISTS(
				SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
			   WHERE TABLE_NAME = N'Categories'))
			BEGIN
					PRINT N'Creating table Categories';

					CREATE TABLE [Categories](
                    	[CategoryID] [UniqueIdentifier] NOT NULL DEFAULT NEWID(),
                    	[Category] [nvarchar](25) NOT NULL,
                    	[SubCategory] [nvarchar](25) NOT NULL,
                    	[CreatedDate] [datetimeoffset](7) NOT NULL,
                    	[CreatedBy] [int] NOT NULL,
                    	[LastUpdatedDate] [datetimeoffset](7) NULL,
                    	[LastUpdatedBy] [int] NULL,
                    	[IsDeleted] [bit] NOT NULL,

						CONSTRAINT pk_categoryId PRIMARY KEY([CategoryID])
                    );

				    EXEC sp_addextendedproperty  
					@name = N'Description' 
					,@value = N'The Category Identifier' 
					,@level0type = N'Schema', @level0name = 'dbo' 
					,@level1type = N'Table',  @level1name = 'Categories' 
					,@level2type = N'Column', @level2name = 'CategoryID'

					EXEC sp_addextendedproperty  
					@name = N'Description' 
					,@value = N'The Primary Category' 
					,@level0type = N'Schema', @level0name = 'dbo' 
				 	,@level1type = N'Table',  @level1name = 'Categories' 
					,@level2type = N'Column', @level2name = 'Category'

					EXEC sp_addextendedproperty  
					@name = N'Description' 
					,@value = N'The Sub Category' 
					,@level0type = N'Schema', @level0name = 'dbo' 
				 	,@level1type = N'Table',  @level1name = 'Categories' 
					,@level2type = N'Column', @level2name = 'SubCategory'

					EXEC sp_addextendedproperty  
					@name = N'Description' 
					,@value = N'The Date of Creation of the record' 
					,@level0type = N'Schema', @level0name = 'dbo' 
				 	,@level1type = N'Table',  @level1name = 'Categories' 
					,@level2type = N'Column', @level2name = 'CreatedDate'

					EXEC sp_addextendedproperty  
					@name = N'Description' 
					,@value = N'The user creating the record' 
					,@level0type = N'Schema', @level0name = 'dbo' 
				 	,@level1type = N'Table',  @level1name = 'Categories' 
					,@level2type = N'Column', @level2name = 'CreatedBy'

					EXEC sp_addextendedproperty  
					@name = N'Description' 
					,@value = N'The date the record was last  updated' 
					,@level0type = N'Schema', @level0name = 'dbo' 
				 	,@level1type = N'Table',  @level1name = 'Categories' 
					,@level2type = N'Column', @level2name = 'LastUpdatedDate'

					EXEC sp_addextendedproperty  
					@name = N'Description' 
					,@value = N'The user who last updated the record' 
					,@level0type = N'Schema', @level0name = 'dbo' 
				 	,@level1type = N'Table',  @level1name = 'Categories' 
					,@level2type = N'Column', @level2name = 'LastUpdatedBy'

					EXEC sp_addextendedproperty  
					@name = N'Description' 
					,@value = N'Identifies whether the record has been soft deleted' 
					,@level0type = N'Schema', @level0name = 'dbo' 
				 	,@level1type = N'Table',  @level1name = 'Categories' 
					,@level2type = N'Column', @level2name = 'IsDeleted'

					CREATE UNIQUE NONCLUSTERED INDEX [Index_Category_SubCategory]
					ON [Categories]([Category] ASC, [SubCategory] ASC);

			PRINT N'Finished creating table Categories'
		END
		ELSE
			PRINT N'Table Categories already exists'

		COMMIT TRANSACTION ADDTABLE_Categories
END TRY
BEGIN CATCH
		PRINT N'';
		PRINT N'';
		PRINT N'FINISHED Create table Categories , WITH ERRORS, ROLLING BACK';
		PRINT ERROR_MESSAGE();
		ROLLBACK TRANSACTION ADDTABLE_Categories
END CATCH
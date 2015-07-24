-- 1. Create a database
CREATE DATABASE SharkTreeTest
 ON  PRIMARY 
( NAME = N'SharkTreeTest', FILENAME = N'D:\data\SharkTreeTest.mdf', SIZE = 6080KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SharkTreeTest_log', FILENAME = N'D:\data\SharkTreeTest_log.ldf' , SIZE = 20608KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

USE SharkTreeTest

-- 1. Create a temporary table
CREATE TABLE [dbo].[TreeData]([ID] [int] NOT NULL,[Term] [varchar](100) NULL,[ParentId] [int] NULL,
CONSTRAINT [PK_TreeData_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- 2. Add stored procedure to the database
CREATE PROCEDURE usp_CreateNode
(
	@ID int, 
	@Term varchar(100), 
	@ParentID int, 
	@Deep int,
	@Count int
)
AS
BEGIN
	INSERT INTO TreeData VALUES
	(
		@ID,
		@Term + CAST(@ID as varchar(10)),
		@ParentID
	)
	
	IF @ParentID > 0
	BEGIN
		DECLARE @TempCount int = @Count
		DECLARE @TempID int		
		WHILE @TempCount > 0
		BEGIN
			SELECT @TempID = MAX(ID) + 1 FROM TreeData
			INSERT INTO TreeData VALUES
			(
				@TempID,
				@Term + CAST(@TempID as varchar(10)),
				@ParentID
			)	
			SET @TempCount = @TempCount - 1
		END
	END
	
	DECLARE @D int = 1	
	DECLARE @TotalDeep int	
	DECLARE @TempDeep int = @Deep
	WHILE @TempDeep > 0
	BEGIN
		SET @TotalDeep = @Deep - @D				
		DECLARE @ParentIDTemp int = @ID		
		SET @ID = @ID  + 1	
		SELECT @ID = MAX(ID) + 1 FROM TreeData
		EXECUTE usp_CreateNode @ID, @Term, @ParentIDTemp, @TotalDeep, @Count		
		SET @TempDeep = 0
	END	
END
GO

-- 3. You can change @Deep, @Count, @Term. 
-- This script repeats (@I) generating a tree with depth (@Depth) of 10, every level has (@Count) 10 items - results with a 10 866 records.
DECLARE @Term varchar(15) = 'Node' 
DECLARE @Count int = 10
DECLARE @Deep int = 10

DECLARE @I int = 240
DECLARE @ID int = 1
DECLARE @ParentID int = NULL

WHILE @I > 0
BEGIN
	EXECUTE usp_createNode @ID, @Term, @ParentId, @Deep, @Count
	
	SET @I = @I - 1
	SElECT @ID = MAX(ID) + 1 FROM TreeData
	SET @Deep = 4
	SET @ParentID = 0
END

--DELETE TreeData
--SELECT * FROM TreeData
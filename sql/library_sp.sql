/**************FETCH BOOKS*********************/


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FETCH_BOOKS]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FETCH_BOOKS]

GO

CREATE PROCEDURE [dbo].[FETCH_BOOKS]  (@ID AS INT =NULL,
                                       @NAME AS VARCHAR(100)=NULL,
									   @AUTHOR AS VARCHAR(100)=NULL,
									   @IS_AVAILABLE BIT = NULL,
									   @ISBN AS VARCHAR(100)=NULL,
									   @DEBUG AS bit = 0)
AS
BEGIN

DECLARE @sql AS nvarchar(max)
DECLARE @paramlist AS nvarchar(max)

SELECT @sql = '	SELECT ID ID ,
			    NAME BOOK_NAME,
				AUTHOR AUTHOR_NAME,
				IS_AVAILABLE IS_AVAILABLE,
				ISBN BOOK_ISBN
			    FROM BOOKS
				WHERE 1 = 1 '
				
				
	
IF @ID  IS NOT NULL
	SELECT @sql = @sql + '	AND ID = @ID '

			
IF @NAME IS NOT NULL
SELECT @sql = @sql  + ' AND (NAME LIKE ''%'' + @NAME + ''%'') ' 

IF @AUTHOR IS NOT NULL
SELECT @sql = @sql  + ' AND (AUTHOR LIKE ''%'' + @AUTHOR + ''%'') ' 

IF @IS_AVAILABLE IS NOT NULL
SELECT @sql = @sql + '	AND  IS_AVAILABLE = @IS_AVAILABLE '		

IF @ISBN IS NOT NULL
SELECT @sql = @sql + '	AND  ISBN = @ISBN '		


IF @debug = 1                                                      
   PRINT @sql                                                      

SELECT @paramlist = '	@ID AS INT ,
                        @NAME AS VARCHAR(100),
						@AUTHOR AS VARCHAR(100),
						@IS_AVAILABLE AS BIT,
						@ISBN AS VARCHAR(100),
						@DEBUG AS BIT '

EXEC sp_executesql @sql, @paramlist,                               
                   @ID,
				   @NAME,
				   @AUTHOR,
				   @IS_AVAILABLE,
				   @ISBN,
				   @DEBUG

END
GO

/**************INSERT BORROWING*********************/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INSERT_BORROWING]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[INSERT_BORROWING]

GO

CREATE PROCEDURE [dbo].[INSERT_BORROWING]   (@BOOK_ID AS INT,
                                             @USER_ID AS INT)
                                                        
AS
BEGIN 
INSERT INTO  [dbo].[BORROWINGS] (BOOK_ID,
                                 USER_ID)
	                                     
 VALUES (@BOOK_ID,
         @USER_ID)

SELECT @@IDENTITY AS ID

END 
GO

/**************FETCH BORROWINGS*********************/


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FETCH_BORROWINGS]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FETCH_BORROWINGS]

GO

CREATE PROCEDURE [dbo].[FETCH_BORROWINGS] (@ID AS INT =NULL,
										   @USER_ID AS INT =NULL,
										   @BOOK_ID AS INT =NULL,
										   @BOOK_ISBN AS VARCHAR(100)=NULL,
										   @DEBUG AS bit = 0)
AS
BEGIN

DECLARE @sql AS nvarchar(max)
DECLARE @paramlist AS nvarchar(max)

SELECT @sql = ' SELECT BORROWING.ID ,
			    USR.ID USER_ID,
				BOOK.ID BOOK_ID,
				BOOK.ISBN BOOK_ISBN,
				BOOK.AUTHOR BOOK_AUTHOR,
			    BOOK.IS_AVAILABLE BOOK_IS_AVAILABLE,
				BOOK.NAME BOOK_NAME,
				USR.NAME USER_NAME,
				USR.EMAIL USER_EMAIL
			    FROM BORROWINGS AS BORROWING
				INNER JOIN BOOKS AS BOOK ON BOOK.ID = BORROWING.BOOK_ID
				INNER JOIN USERS AS USR ON USR.ID = BORROWING.USER_ID
				WHERE 1 = 1 '
				
				
	
IF @ID  IS NOT NULL
	SELECT @sql = @sql + '	AND ID = @ID '

			
IF @BOOK_ID IS NOT NULL
SELECT @sql = @sql + '	AND  BOOK_ID  = @BOOK_ID '	

IF @USER_ID IS NOT NULL
SELECT @sql = @sql + '	AND  USER_ID  = @USER_ID '

IF @BOOK_ISBN IS NOT NULL
SELECT @sql = @sql + '	AND  BOOK.ISBN = @BOOK_ISBN '		


IF @debug = 1                                                      
   PRINT @sql                                                      

SELECT @paramlist = '	@BOOK_ID AS INT ,
                        @USER_ID AS INT,
						@BOOK_ISBN AS VARCHAR,
						@DEBUG AS BIT '

EXEC sp_executesql @sql, @paramlist,                               
                   @BOOK_ID,
				   @USER_ID,
				   @BOOK_ISBN,
				   @DEBUG

END
GO


/**************DELETE BORROWING*********************/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DELETE_BORROWING]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DELETE_BORROWING]
GO
CREATE PROCEDURE [DBO].[DELETE_BORROWING]	@USER_ID INT,
											@BOOK_ID INT
AS
BEGIN
DELETE FROM [BORROWINGS] 
WHERE BOOK_ID = @BOOK_ID
AND USER_ID = @USER_ID
END
GO


/**************RETURNING BOOK*********************/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RETURNING_BOOK]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[RETURNING_BOOK]
GO
CREATE PROCEDURE [DBO].[RETURNING_BOOK]	@BOOK_ID INT,
                                         @USER_ID INT
AS
BEGIN
EXECUTE DELETE_BORROWING
@BOOK_ID = @BOOK_ID,
@USER_ID = @USER_ID

UPDATE BOOKS SET IS_AVAILABLE = 1
WHERE ID = @BOOK_ID

END
GO


/**************BORROWING BOOK*********************/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BORROWING_BOOK]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BORROWING_BOOK]
GO
CREATE PROCEDURE [DBO].[BORROWING_BOOK]	@BOOK_ID INT,
                                        @USER_ID INT
AS
BEGIN
EXECUTE INSERT_BORROWING 
@BOOK_ID = @BOOK_ID,
@USER_ID = @USER_ID

UPDATE BOOKS SET IS_AVAILABLE = 0
WHERE ID = @BOOK_ID

END
GO


/**************FETCH USERS*********************/


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FETCH_USERS]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FETCH_USERS]

GO

CREATE PROCEDURE [dbo].[FETCH_USERS]  (@ID AS INT =NULL,
                                       @NAME AS VARCHAR(100)=NULL,
									   @EMAIL AS VARCHAR(100)=NULL,
									   @DEBUG AS bit = 0)
AS
BEGIN

DECLARE @sql AS nvarchar(max)
DECLARE @paramlist AS nvarchar(max)

SELECT @sql = '	SELECT ID ID ,
			    NAME USER_NAME,
				EMAIL USER_EMAIL
			    FROM USERS
				WHERE 1 = 1 '
				
				
	
IF @ID  IS NOT NULL
	SELECT @sql = @sql + '	AND ID = @ID '

			
IF @NAME IS NOT NULL
SELECT @sql = @sql + '	AND  NAME = @NAME  '		


IF @debug = 1                                                      
   PRINT @sql                                                      

SELECT @paramlist = '	@ID AS INT ,
                        @NAME AS VARCHAR(100),
						@EMAIL AS VARCHAR(100),
						@DEBUG AS BIT '

EXEC sp_executesql @sql, @paramlist,                               
                   @ID,
				   @NAME,
				   @EMAIL,
				   @DEBUG

END
GO





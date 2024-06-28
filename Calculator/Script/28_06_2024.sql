CREATE TABLE [dbo].[Users](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](200) NULL,
	[LastName] [nvarchar](200) NULL,
	[EmailId] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](max) NULL,
	[PasswordSalt] [nvarchar](max) NULL,
	[JWTToken] [nvarchar](max) NULL,
	[TokenCreatedDate] [datetime] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

----------------------------------------------------------------------------------
CREATE TABLE [dbo].[Calculate_History](
	[CalculateId] [bigint] IDENTITY(1,1) NOT NULL,
	[Details] [nvarchar](max) NULL,
	[FirstValue] float NULL,
	[SecondValue] float null,
	[CalculateValue] float null,
	[Operator] char(1),
	[CreatedDate] [datetime] NULL
 CONSTRAINT [PK_Calculate_History] PRIMARY KEY CLUSTERED 
(
	[CalculateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
----------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[sp_GetUserSaltByEmail]
@EmailId Nvarchar(400)
AS
BEGIN
		Select PasswordSalt,Password from users where EmailId = @EmailId
END
GO
-----------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[sp_UserLogin] @EmailId VARCHAR(150) = '',
@Password VARCHAR(MAX) = '',
@IsSuperAdminUser BIGINT = 0
AS
BEGIN
  SET NOCOUNT ON;
    SELECT
      userid
     ,u.FirstName + ' ' + u.LastName AS FullName
     ,u.EmailId
    FROM Users u
    WHERE  EmailId = @EmailId
    AND Password = @Password
END
GO
-----------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[SP_UpdateLoginToken]    
@UserId bigint ,
@Token nvarchar(max)
AS    
BEGIN    
     UPDATE Users    
       SET    JWTToken = @Token,   
           TokenCreatedDate =GETUTCDATE()    
         WHERE  UserId = @UserId 
     SELECT 1 
END  
GO
-----------------------------------------------------------------------------------
Create PROCEDURE [dbo].[sp_ValidateToken]          
@UserId bigint      
as
BEGIN   
   IF(Select COUNT(JWTToken) from Users where UserId=@UserId)=1      
   BEGIN      
		SELECT 1
   END      
  ELSE      
      BEGIN      
        SELECT 0      
	END      
END  
-----------------------------------------------------------------------------------  
Go
INSERT [dbo].[Users] ([FirstName], [LastName], [EmailId], [Password], [PasswordSalt]) VALUES ( N'Admin', N'Patel', N'admin@gmail.com', N'CmU1YSjLlwEz3PdOCnvy7TDxKJViyBk22HOlngAprMkmbxhCCOVjlR4Q33ZIypP8tDcgfj+BwGl/2dzk236vQNzcx/AeJPL8SVH9Ra7bvsB/sTO6azqg0kOEnghwzYoVkAN/FZ7pWbRd9Sj0g1Rnj2JaZeHB+FugFg/lTy744BwWianJg0Itcx75aKcg3VZw', 
N'aaDkQ0Pyf4WpwMdyQmk8/iGt7xBnYJcxwB99o3+47wDn3wBZoyd15/6RPOTDUUJog/bqCnsWweG7dUiYjoOb8bMlZvQBfu8SjOJUxvyMVQE=')
Go
-----------------------------------------------------------------------------------
Create PROCEDURE [dbo].[SP_Insert_Calculate_History]    
@FirstValue float ,
@SecondValue float ,
@CalculateValue float,
@Operator char(1)
AS    
BEGIN    
    insert into Calculate_History (FirstValue,SecondValue,CalculateValue,Operator,CreatedDate)
	values (@FirstValue,@SecondValue,@CalculateValue,@Operator,GETUTCDATE())
    SELECT 1 as IsSuccess,'Saved Successfully' as Result 

END  
GO

-----------------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[SP_Get_Calculate_History]    
AS    
BEGIN    
    select FirstValue,SecondValue,CalculateValue,Operator,CreatedDate From Calculate_History
	order by CreatedDate desc
END  
GO
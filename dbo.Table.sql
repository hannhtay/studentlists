CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NULL, 
    [FatherName] VARCHAR(50) NULL, 
    [DOB] DATETIME NULL, 
    [NRC] VARCHAR(50) NULL, 
    [Gender] INT NULL, 
    [Class] VARCHAR(50) NULL, 
    [Remark] TEXT NULL, 
    [Created_at] DATETIME NULL
)

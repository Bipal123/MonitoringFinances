/* Use this script to seed some data into application ONLY AFTER users and roles have been created */
USE [MonitoringFinancesDb-Dev-Local]; /* Change database name to correct db name */

/* Get User ID's for both accounts */
DECLARE @AdminUserId nvarchar(450), @StandardUserId nvarchar(450);
SET @AdminUserId = (SELECT Id FROM AspNetUsers WHERE UserName = 'admin@admin.com');
SET @StandardUserId = (SELECT Id FROM AspNetUsers WHERE UserName = 'user@user.com');

/* Seed CategoryType Entity */
IF(NOT EXISTS(SELECT 1 FROM dbo.CategoryType))
	BEGIN
		INSERT INTO [CategoryType]
		VALUES ('Income'), ('Expense');
	END

DECLARE @IncomeId int, @ExpenseId int;
SET @IncomeId = (SELECT Id FROM CategoryType WHERE Name = 'Income');
SET @ExpenseId = (SELECT Id FROM CategoryType WHERE Name = 'Expense');

/* Seed Category Entity for Admin and Standard User*/
IF(NOT EXISTS(SELECT 1 FROM dbo.Category))
	BEGIN
		INSERT INTO Category
		VALUES 
		('Restaurant', @AdminUserId, @ExpenseId), 
		('Transportation', @AdminUserId, @ExpenseId), 
		('Grocery', @AdminUserId, @ExpenseId), 
		('Utility Bills', @AdminUserId, @ExpenseId), 
		('Maintenance', @AdminUserId, @ExpenseId), 
		('Education', @AdminUserId, @ExpenseId),
		('Entertainment', @AdminUserId, @ExpenseId), 
		('Other Expenditure', @AdminUserId, @ExpenseId),
		('Salary', @AdminUserId, @IncomeId),
		('Business', @AdminUserId, @IncomeId),
		('Investment', @AdminUserId, @IncomeId),
		('Other Income', @AdminUserId, @IncomeId),
		
		('Restaurant', @StandardUserId, @ExpenseId), 
		('Transportation', @StandardUserId, @ExpenseId), 
		('Grocery', @StandardUserId, @ExpenseId), 
		('Utility Bills', @StandardUserId, @ExpenseId), 
		('Maintenance', @StandardUserId, @ExpenseId), 
		('Education', @StandardUserId, @ExpenseId),
		('Entertainment', @StandardUserId, @ExpenseId), 
		('Other Expenditure', @StandardUserId, @ExpenseId),
		('Salary', @StandardUserId, @IncomeId),
		('Business', @StandardUserId, @IncomeId),
		('Investment', @StandardUserId, @IncomeId),
		('Other Income', @StandardUserId, @IncomeId);
	END


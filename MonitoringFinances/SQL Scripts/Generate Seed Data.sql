/* Use this script to seed some data into application ONLY AFTER users and roles have been created */
USE [MonitoringFinancesDb-Local]; /* Change database name to correct db name */

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

/* Seed Category Entity for Admin and Standard User */
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

DECLARE @RestaurantAdminId int, @TransportationAdminId int,
		@GroceryAdminId int, @SalaryAdminId int;

SET @RestaurantAdminId = (SELECT Id FROM Category WHERE Name = 'Restaurant' AND UserId = @AdminUserId);
SET @TransportationAdminId = (SELECT Id FROM Category WHERE Name = 'Transportation' AND UserId = @AdminUserId);
SET @GroceryAdminId = (SELECT Id FROM Category WHERE Name = 'Grocery' AND UserId = @AdminUserId);
SET @SalaryAdminId = (SELECT Id FROM Category WHERE Name = 'Salary' AND UserId = @AdminUserId);

/* Seed Transaction Entity for Admin User */
IF(NOT EXISTS(SELECT 1 FROM dbo.Record))
	BEGIN
		INSERT INTO dbo.Record
		VALUES
		(6.4, '2020-05-07', 'Krishna Indian Restaurant', @RestaurantAdminId),
		(16.2, '2020-05-12', 'Burger King w. Tyusha', @RestaurantAdminId),
		(8.81, '2020-05-15', 'Lyft to volunteering', @TransportationAdminId),
		(22.4, '2020-05-01', NULL, @TransportationAdminId),
		(30.42, '2020-05-01', 'Kroger', @GroceryAdminId),
		(60, '2020-05-08', 'Kroger', @GroceryAdminId),
		(160, '2020-05-08', 'Kroger', @GroceryAdminId),
		(450, '2020-05-01', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-05-15', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-05-08', 'University of Cincinnati', @SalaryAdminId),
		
		(6.4, '2020-06-07', 'Krishna Indian Restaurant', @RestaurantAdminId),
		(16.2, '2020-06-12', 'Burger King w. Tyusha', @RestaurantAdminId),
		(8.81, '2020-06-15', 'Lyft to volunteering', @TransportationAdminId),
		(22.4, '2020-06-01', NULL, @TransportationAdminId),
		(30.42, '2020-06-01', 'Kroger', @GroceryAdminId),
		(60, '2020-06-08', 'Kroger', @GroceryAdminId),
		(160, '2020-06-08', 'Kroger', @GroceryAdminId),
		(450, '2020-06-01', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-06-15', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-06-08', 'University of Cincinnati', @SalaryAdminId),
		
		(6.4, '2020-07-07', 'Krishna Indian Restaurant', @RestaurantAdminId),
		(16.2, '2020-07-12', 'Burger King w. Tyusha', @RestaurantAdminId),
		(8.81, '2020-07-15', 'Lyft to volunteering', @TransportationAdminId),
		(22.4, '2020-07-01', NULL, @TransportationAdminId),
		(30.42, '2020-07-01', 'Kroger', @GroceryAdminId),
		(60, '2020-07-08', 'Kroger', @GroceryAdminId),
		(160, '2020-07-08', 'Kroger', @GroceryAdminId),
		(450, '2020-07-01', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-07-15', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-07-08', 'University of Cincinnati', @SalaryAdminId),
		
		(6.4, '2020-08-07', 'Krishna Indian Restaurant', @RestaurantAdminId),
		(16.2, '2020-08-12', 'Burger King w. Tyusha', @RestaurantAdminId),
		(8.81, '2020-08-15', 'Lyft to volunteering', @TransportationAdminId),
		(22.4, '2020-08-01', NULL, @TransportationAdminId),
		(30.42, '2020-08-01', 'Kroger', @GroceryAdminId),
		(60, '2020-08-08', 'Kroger', @GroceryAdminId),
		(160, '2020-08-08', 'Kroger', @GroceryAdminId),
		(450, '2020-08-01', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-08-15', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-08-08', 'University of Cincinnati', @SalaryAdminId),
		
		(6.4, '2020-09-07', 'Krishna Indian Restaurant', @RestaurantAdminId),
		(16.2, '2020-09-12', 'Burger King w. Tyusha', @RestaurantAdminId),
		(8.81, '2020-09-15', 'Lyft to volunteering', @TransportationAdminId),
		(22.4, '2020-09-01', NULL, @TransportationAdminId),
		(30.42, '2020-09-01', 'Kroger', @GroceryAdminId),
		(60, '2020-09-08', 'Kroger', @GroceryAdminId),
		(160, '2020-09-08', 'Kroger', @GroceryAdminId),
		(450, '2020-09-01', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-09-15', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-09-08', 'University of Cincinnati', @SalaryAdminId),
		
		(6.4, '2020-10-07', 'Krishna Indian Restaurant', @RestaurantAdminId),
		(16.2, '2020-10-12', 'Burger King w. Tyusha', @RestaurantAdminId),
		(8.81, '2020-10-15', 'Lyft to volunteering', @TransportationAdminId),
		(22.4, '2020-10-01', NULL, @TransportationAdminId),
		(30.42, '2020-10-01', 'Kroger', @GroceryAdminId),
		(60, '2020-10-08', 'Kroger', @GroceryAdminId),
		(160, '2020-10-08', 'Kroger', @GroceryAdminId),
		(450, '2020-10-01', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-10-15', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-10-08', 'University of Cincinnati', @SalaryAdminId),
		
		(6.4, '2020-11-07', 'Krishna Indian Restaurant', @RestaurantAdminId),
		(16.2, '2020-11-12', 'Burger King w. Tyusha', @RestaurantAdminId),
		(8.81, '2020-11-15', 'Lyft to volunteering', @TransportationAdminId),
		(22.4, '2020-11-01', NULL, @TransportationAdminId),
		(30.42, '2020-11-01', 'Kroger', @GroceryAdminId),
		(60, '2020-11-08', 'Kroger', @GroceryAdminId),
		(160, '2020-11-08', 'Kroger', @GroceryAdminId),
		(450, '2020-11-01', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-11-15', 'University of Cincinnati', @SalaryAdminId),
		(160, '2020-11-08', 'University of Cincinnati', @SalaryAdminId);
 	END

USE [MonitoringFinancesDb-Local]
GO
SELECT Record.Id, Record.Amount, Record.Date, Record.Description, Category.Name, CategoryType.Name, AspNetUsers.UserName
FROM dbo.Record 
	INNER JOIN dbo.Category ON Record.CategoryId = Category.Id
	INNER JOIN dbo.AspNetUsers ON Category.UserId = AspNetUsers.Id
	INNER JOIN dbo.CategoryType ON Category.CategoryTypeId = CategoryType.Id
ORDER BY Record.Id ASC
GO
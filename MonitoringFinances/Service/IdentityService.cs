using MonitoringFinances.Data;
using MonitoringFinances.Models;
using MonitoringFinances.Models.AdminModels;
using MonitoringFinances.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringFinances.Service
{ 
    public class IdentityService
    {
        public List<Category> NewUserCategories(ApplicationUser user, IEnumerable<CategoryType> categoryTypes)
        {
            int incomeId = categoryTypes.Where(u => u.Name.Equals("Income")).FirstOrDefault().Id;
            int expenseId = categoryTypes.Where(u => u.Name.Equals("Expense")).FirstOrDefault().Id;
            List<Category> newCategories = new List<Category>();
            newCategories.Add(new Category()
            {
                Name = "Restaurant",
                CategoryTypeId = expenseId,
                UserId = user.Id
            });
            newCategories.Add(new Category()
            {
                Name = "Transportation",
                CategoryTypeId = expenseId,
                UserId = user.Id
            });
            newCategories.Add(new Category()
            {
                Name = "Grocery",
                CategoryTypeId = expenseId,
                UserId = user.Id
            });
            newCategories.Add(new Category()
            {
                Name = "Utility Bills",
                CategoryTypeId = expenseId,
                UserId = user.Id
            });
            newCategories.Add(new Category()
            {
                Name = "Maintenance",
                CategoryTypeId = expenseId,
                UserId = user.Id
            });
            newCategories.Add(new Category()
            {
                Name = "Education",
                CategoryTypeId = expenseId,
                UserId = user.Id
            });
            newCategories.Add(new Category()
            {
                Name = "Entertainment",
                CategoryTypeId = expenseId,
                UserId = user.Id
            });
            newCategories.Add(new Category()
            {
                Name = "Other Expenditure",
                CategoryTypeId = expenseId,
                UserId = user.Id
            });
            newCategories.Add(new Category()
            {
                Name = "Salary",
                CategoryTypeId = incomeId,
                UserId = user.Id
            });
            newCategories.Add(new Category()
            {
                Name = "Business",
                CategoryTypeId = incomeId,
                UserId = user.Id
            });
            newCategories.Add(new Category()
            {
                Name = "Investment",
                CategoryTypeId = incomeId,
                UserId = user.Id
            });
            newCategories.Add(new Category()
            {
                Name = "Other Income",
                CategoryTypeId = incomeId,
                UserId = user.Id
            });
            return newCategories;
        } 
    }
}

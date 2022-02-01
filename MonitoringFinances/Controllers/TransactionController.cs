using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonitoringFinances.Data;
using MonitoringFinances.Models;
using MonitoringFinances.Models.AdminModels;
using MonitoringFinances.Models.Identity;
using MonitoringFinances.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static MonitoringFinances.Models.ViewModel.TransactionAllVM;

namespace MonitoringFinances.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public TransactionController
            (ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> IndexAsync(string id)
        {
            if (id == null || (!id.Equals("Income") && !id.Equals("Expense")))
            {
                return NotFound();
            }
            string categoryType = id;
            //Get current user
            ApplicationUser currentUser = (ApplicationUser) await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            //Get records by category type
            IEnumerable<Transaction> transactionsForCurUser = _db.Transaction.Include(u => u.Category).Where(u => u.Category.ApplicationUser.Id == currentUser.Id);
            foreach (Transaction transaction in transactionsForCurUser)
            {
                transaction.Category.CategoryType = _db.CategoryType.Where(u => u.Id == transaction.Category.CategoryTypeId).FirstOrDefault();
            }
            IEnumerable<Transaction> recordsByType = transactionsForCurUser.Where(u => u.Category.CategoryType.Name.Equals(categoryType));
            
            IEnumerable<Category> categories = _db.Category.Where(u => u.ApplicationUser.Id == currentUser.Id).Where(u => u.CategoryType.Name.Equals(categoryType));

            //Get Pie Chart Data for month
            List<Transaction> recordsForThisMonth = new List<Transaction>();
            foreach(Transaction record in recordsByType)
            {
                DateTime date = (DateTime) record.Date;
                if (date.Month.Equals(DateTime.Now.Month))
                {
                    recordsForThisMonth.Add(record);
                }
            }
            List<PieChartData> pieChartDataByMonth = new List<PieChartData>();
            decimal totalSumByMonth = recordsForThisMonth.Sum(u => u.Amount);
            if (totalSumByMonth != 0)
            {
                foreach (Category category in categories)
                {
                    decimal sum = recordsForThisMonth.Where(u => u.Category.Id.Equals(category.Id)).Sum(u => u.Amount);
                    if (sum != 0)
                    {
                        //xValue is category name, yValue is total sum
                        pieChartDataByMonth.Add(new PieChartData()
                        {
                            xValue = category.Name,
                            yValue = sum,
                            text = (sum / totalSumByMonth).ToString("p0")
                        });
                    }
                }
            }

            //Get this year transaction
            List<Transaction> recordsForThisYear = new List<Transaction>();
            foreach (Transaction record in recordsByType)
            {
                DateTime date = (DateTime) record.Date;
                if (date.Year.Equals(DateTime.Now.Year))
                {
                    recordsForThisYear.Add(record);
                }
            }

            //this year pie chart data by category
            List<PieChartData> pieChartDataByYear = new List<PieChartData>();
            decimal totalSumByYear = recordsForThisYear.Sum(u => u.Amount);
            if (totalSumByYear != 0)
            {
                foreach (Category category in categories)
                {
                    decimal sum = recordsForThisYear.Where(u => u.Category.Id.Equals(category.Id)).Sum(u => u.Amount);
                    if (sum != 0)
                    {
                        //xValue is category name, yValue is total sum, text is sum percentage
                        pieChartDataByYear.Add(new PieChartData()
                        {
                            xValue = category.Name,
                            yValue = sum,
                            text = (sum / totalSumByYear).ToString("p0")
                        });
                    }
                }
            }

            //this year column chart data by month
            List<ColumnChartData> columnChartData = new List<ColumnChartData>();
            if (totalSumByYear != 0)
            {
                for (int month=1; month <= DateTime.Now.Month; month++)
                {
                    decimal sum = recordsForThisYear.Where(u => DateTime.Parse(u.Date.ToString()).Month.Equals(month)).Sum(u => u.Amount);
                    //xValue is month name, yValue is sum
                    columnChartData.Add(new ColumnChartData()
                    {
                        xAxisText = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month),
                        yValue = sum,
                        text = sum.ToString("C2")
                    });
                    
                }
            }

            //List<ColumnChartData> testColumnData = new List<ColumnChartData>();
            //testColumnData.Add(new ColumnChartData()
            //{
            //    xAxisText = "Jan",
            //    yValue = 600,
            //    text = "8%"
            //});
            //testColumnData.Add(new ColumnChartData()
            //{
            //    xAxisText = "Feb",
            //    yValue = 680,
            //    text = "10%"
            //});
            

            TransactionAllVM transactionAllVM = new TransactionAllVM()
            {
                recordsByType = recordsByType,
                pieChartDataForMonth = pieChartDataByMonth,
                pieChartDataForYear = pieChartDataByYear,
                categoryType = categoryType,
                columnChartData = columnChartData
                //columnChartData = testColumnData
            };
            return View(transactionAllVM);
        }

        [HttpGet]
        public async Task<IActionResult> DetailAsync(int? id)
        {
            if (id == null || id == 0)
            {
                return StatusCode(500);
            }
            else
            {
                ApplicationUser currentUser = (ApplicationUser)await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                Transaction transaction = _db.Transaction.Include(u => u.Category).Where(u => u.Category.ApplicationUser.Id == currentUser.Id).Where(u => u.Id == id).FirstOrDefault();
                transaction.Category.CategoryType = _db.CategoryType.Find(transaction.Category.CategoryTypeId);
                if (transaction == null)
                {
                    return NotFound();
                }
                return PartialView("~/Views/Transaction/_Detail.cshtml", transaction);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpSertAsync(int? id, bool? isIncome)
        {
            ApplicationUser currentUser = (ApplicationUser)await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (id == null || isIncome == null)
            {
                return StatusCode(500);
            }
            IEnumerable<SelectListItem> subCategories;
            if ((bool) isIncome)
            {
                
                subCategories = _db.Category.Where(u => u.ApplicationUser.Id == currentUser.Id).Where(u => u.CategoryType.Name.Equals("Income")).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            else
            {
                subCategories = _db.Category.Where(u => u.ApplicationUser.Id == currentUser.Id).Where(u => u.CategoryType.Name.Equals("Expense")).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            TransactionSingleVM transactionVM = new TransactionSingleVM()
            {
                Transaction = new Transaction(),
                SubCategories = subCategories
            };
            if (id == 0)
            {
                return PartialView("~/Views/Transaction/_Upsert.cshtml", transactionVM);
            }
            else
            {
                transactionVM.Transaction = _db.Transaction.Find(id);
                if (transactionVM.Transaction == null)
                {
                    return NotFound();
                }
                return PartialView("~/Views/Transaction/_Upsert.cshtml", transactionVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if (transaction.Id == 0)
                {
                    _db.Transaction.Add(transaction);
                }
                else
                {
                    _db.Transaction.Update(transaction);
                }
                _db.SaveChanges();

                Category currentCategory = _db.Category.Find(transaction.CategoryId);
                CategoryType currentCategoryType = _db.CategoryType.Find(currentCategory.CategoryTypeId);

                return RedirectToAction(nameof(Index), nameof(Transaction), new {id=currentCategoryType.Name});
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return StatusCode(500);
            }
            else
            {
                Transaction transaction = _db.Transaction.Find(id);
                if (transaction == null)
                {
                    return NotFound();
                }
                return PartialView("~/Views/Transaction/_Delete.cshtml", transaction);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Transaction.Find(id);
            _db.Transaction.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

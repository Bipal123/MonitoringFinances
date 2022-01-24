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
            
            //Get records by record type
            IEnumerable<Transaction> transactionsForCurUser = _db.Transaction.Include(u => u.Category).Where(u => u.Category.ApplicationUser.Id == currentUser.Id);
            foreach (Transaction transaction in transactionsForCurUser)
            {
                transaction.Category.CategoryType = _db.CategoryType.Where(u => u.Id == transaction.Category.CategoryTypeId).FirstOrDefault();
            }
            IEnumerable<Transaction> recordsByType = transactionsForCurUser.Where(u => u.Category.CategoryType.Name.Equals(categoryType));

            //Get Pie Chart Data
            List<Transaction> recordsForThisMonth = new List<Transaction>();
            foreach(Transaction record in recordsByType)
            {
                DateTime date = (DateTime) record.Date;
                if (date.Month.Equals(DateTime.Now.Month))
                {
                    recordsForThisMonth.Add(record);
                }
            }

            List<PieChartData> pieChartData = new List<PieChartData>();
            IEnumerable<Category> categories = _db.Category.Where(u => u.ApplicationUser.Id == currentUser.Id).Where(u => u.CategoryType.Name.Equals(categoryType));
            decimal totalSum = recordsForThisMonth.Sum(u => u.Amount);
            if (totalSum != 0)
            {
                foreach (Category category in categories)
                {
                    decimal sum = recordsForThisMonth.Where(u => u.Category.Id.Equals(category.Id)).Sum(u => u.Amount);
                    if (sum != 0)
                    {
                        pieChartData.Add(new PieChartData()
                        {
                            xValue = category.Name,
                            yValue = recordsForThisMonth.Where(u => u.Category.Id.Equals(category.Id)).Sum(u => u.Amount),
                            text = (sum / totalSum).ToString("P", CultureInfo.InvariantCulture)
                        });
                    }
                }
            }

            TransactionAllVM transactionAllVM = new TransactionAllVM()
            {
                recordsByType = recordsByType,
                pieChartData = pieChartData,
                categoryType = categoryType
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

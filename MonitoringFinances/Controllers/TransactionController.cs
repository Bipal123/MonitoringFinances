using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonitoringFinances.Data;
using MonitoringFinances.Models;
using MonitoringFinances.Models.Identity;
using MonitoringFinances.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringFinances.Controllers
{
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
        public async Task<IActionResult> IndexAsync()
        {
            //Get current user
            ApplicationUser currentUser = (ApplicationUser) await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            IEnumerable<Transaction> transactionsForCurUser = _db.Transaction.Include(u => u.Category).Where(u => u.Category.ApplicationUser.Id == currentUser.Id);
            foreach (Transaction transaction in transactionsForCurUser)
            {
                transaction.Category.CategoryType = _db.CategoryType.Where(u => u.Id == transaction.Category.CategoryTypeId).FirstOrDefault();
            };
            IEnumerable<Transaction> incomeRecords = transactionsForCurUser.Where(u => u.Category.CategoryType.Name.Equals("Income"));
            IEnumerable<Transaction> expenseRecords = transactionsForCurUser.Where(u => u.Category.CategoryType.Name.Equals("Expense"));
            TransactionAllVM transactionAllVM = new TransactionAllVM()
            {
                incomeRecords = incomeRecords,
                expenseRecords = expenseRecords
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
        public IActionResult UpSert(int? id, bool? isIncome)
        {
            if (id == null || isIncome == null)
            {
                return StatusCode(500);
            }
            IEnumerable<SelectListItem> subCategories;
            if ((bool) isIncome)
            {
                subCategories = _db.Category.Where(u => u.CategoryType.Name.Equals("Income")).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            else
            {
                subCategories = _db.Category.Where(u => u.CategoryType.Name.Equals("Expense")).Select(i => new SelectListItem
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
                return RedirectToAction(nameof(Index));
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

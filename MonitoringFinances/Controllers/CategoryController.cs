using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CategoryController
            (ApplicationDbContext db, 
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            //Get current user
            ApplicationUser currentUser = (ApplicationUser) await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            IEnumerable<Category> categoriesForCurUser = _db.Category.Include(u => u.ApplicationUser).Where(u => u.ApplicationUser.Id == currentUser.Id).Include(u => u.CategoryType);
            return View(categoriesForCurUser);
        }
        
        [HttpGet]
        public IActionResult UpSert(int id)
        {
            CategoryVM categoryVM = new CategoryVM()
            {
                Category = new Category(),
                CategoryTypeSelectList = _db.CategoryType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            }; 
            if (id == 0)
            {
                return PartialView("~/Views/Category/_Upsert.cshtml", categoryVM);
            }
            else
            {
                categoryVM.Category = _db.Category.Find(id);
                if (categoryVM.Category == null)
                {
                    return NotFound();
                }
                return PartialView("~/Views/Category/_Upsert.cshtml", categoryVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpSert(Category category)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = (ApplicationUser) await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }
                category.UserId = currentUser.Id;
                if (category.Id == 0)
                {
                    _db.Category.Add(category);
                }
                else
                {
                    _db.Category.Update(category);
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
                Category category = _db.Category.Find(id);
                if (category == null)
                {
                    return NotFound();
                }
                return PartialView("~/Views/Category/_Delete.cshtml", category);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Category.Find(id);
            _db.Category.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

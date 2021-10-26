using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> IndexAsync()
        {
            //Get current user
            ApplicationUser currentUser = (ApplicationUser)await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            IEnumerable<Category> categoriesForCurUser = _db.Category.Include(u => u.ApplicationUser).Where(u => u.ApplicationUser.Id == currentUser.Id);

            CategoryVM categoryVM = new CategoryVM()
            {
                categories = categoriesForCurUser,
                currentUser = currentUser,
                upSertCategory = new Category()
            }; 
            return View(categoryVM);
        }
    }
}

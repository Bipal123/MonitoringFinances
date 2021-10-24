using Microsoft.AspNetCore.Mvc;
using MonitoringFinances.Data;
using MonitoringFinances.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringFinances.Controllers
{
    public class PredefinedCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PredefinedCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<PredefinedCategory> predefinedCategoryList = _db.PredefinedCategory;
            return View(predefinedCategoryList);
        }

        //Get - Insert and Update
        [HttpGet]
        public IActionResult UpSert(int? id)
        {
            if (id == null || id == 0)
            {
                return PartialView("~/Views/PredefinedCategory/_Upsert.cshtml");
            }
            else
            {
                PredefinedCategory predefinedCategory = _db.PredefinedCategory.Find(id);
                if (predefinedCategory == null)
                {
                    return NotFound();
                }
                return PartialView("~/Views/PredefinedCategory/_Upsert.cshtml", predefinedCategory);
            }
        }

        [HttpPost]
        public IActionResult Upsert(PredefinedCategory predefinedCategory)
        {
            if (ModelState.IsValid)
            {
                if (predefinedCategory.Id == 0)
                {
                    _db.PredefinedCategory.Add(predefinedCategory);
                }
                else
                {
                    _db.PredefinedCategory.Update(predefinedCategory);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return PartialView("~/Views/PredefinedCategory/_Upsert.cshtml", predefinedCategory);
            }
        }
    }
}

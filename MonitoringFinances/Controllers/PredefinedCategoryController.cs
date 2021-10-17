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

        ////Get - Insert and Update
        //public IActionResult UpSert()
        //{

        //}
    }
}

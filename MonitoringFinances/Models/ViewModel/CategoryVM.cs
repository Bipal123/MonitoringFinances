using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringFinances.Models.ViewModel
{
    public class CategoryVM
    {
        public Category Category { get; set; }
        public IEnumerable<SelectListItem> CategoryTypeSelectList { get; set; }
    }
}

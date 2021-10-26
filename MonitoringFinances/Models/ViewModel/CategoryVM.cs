using MonitoringFinances.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringFinances.Models.ViewModel
{
    public class CategoryVM
    {
        public IEnumerable<Category> categories { get; set; }
        public ApplicationUser currentUser { get; set; }
        public Category upSertCategory { get; set; }
    }
}

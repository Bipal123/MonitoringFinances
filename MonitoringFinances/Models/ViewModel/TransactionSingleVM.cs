using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringFinances.Models.ViewModel
{
    public class TransactionSingleVM
    {
        public Transaction Transaction { get; set; }
        public IEnumerable<SelectListItem> SubCategories { get; set; }
    }
}

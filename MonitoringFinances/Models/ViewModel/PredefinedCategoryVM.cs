using MonitoringFinances.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringFinances.Models.ViewModel
{
    public class PredefinedCategoryVM
    {
        IEnumerable<PredefinedCategory> predefinedCategoryList { get; set; }

        PredefinedCategory upSertPredefinedCategory { get; set; }

        bool isUpSertModelStateValid { get; set; }
    }
}

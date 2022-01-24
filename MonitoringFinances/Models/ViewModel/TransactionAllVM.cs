using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringFinances.Models.ViewModel
{
    public class TransactionAllVM
    {
        public class PieChartData
        {
            public string xValue;
            public decimal yValue;
            public string text;
        }

        public IEnumerable<Transaction> recordsByType;

        public IList<PieChartData> pieChartData;

    }
}

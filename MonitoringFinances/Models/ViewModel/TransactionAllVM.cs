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

        public class ColumnChartData
        {
            public string xAxisText;
            public decimal yValue;
            public string text;
        }

        public IEnumerable<Transaction> recordsByType;

        public List<PieChartData> pieChartDataForMonth;

        public List<PieChartData> pieChartDataForYear;

        public string categoryType;

        public List<ColumnChartData> columnChartData;
    }
}

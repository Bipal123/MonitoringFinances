using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringFinances.Models.ViewModel
{
    public class TransactionAllVM
    {
        public IEnumerable<Transaction> incomeRecords;
        public IEnumerable<Transaction> expenseRecords;
    }
}

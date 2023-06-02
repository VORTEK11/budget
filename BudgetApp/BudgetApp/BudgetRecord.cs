using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace BudgetApp
{
    public class BudgetRecord
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public bool IsIncome { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
    }
}

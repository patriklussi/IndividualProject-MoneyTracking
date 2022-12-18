using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject_MoneyTracking.Classes
{
    internal class ExpenseItem : Account
    {
        public ExpenseItem(string title, DateTime month, double amount,string itemtype) : base(title, month, amount,itemtype)
        {
        }
    }
}

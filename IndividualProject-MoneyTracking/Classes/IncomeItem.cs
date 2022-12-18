using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject_MoneyTracking.Classes
{
    internal class IncomeItem : Account
    {
        public IncomeItem(string title, DateTime month, double amount, string itemtype) : base(title, month, amount, itemtype)
        {
        }
    } 
}

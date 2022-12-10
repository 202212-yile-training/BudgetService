#region

using System;

#endregion

namespace BudgetProject
{
    public class Budget
    {
        public Budget(string yearMonth, int amount)
        {
            YearMonth = yearMonth;
            Amount = amount;
        }

        public int Amount { get; }
        public string YearMonth { get; }

        public DateTime GetFirstDay()
        {
            return DateTime.ParseExact(YearMonth, "yyyyMM", null);
        }
    }
}
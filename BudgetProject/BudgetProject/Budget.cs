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

        public DateTime GetLastDay()
        {
            var firstDay = GetFirstDay();
            var daysInMonth = DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
            return DateTime.ParseExact(YearMonth + daysInMonth, "yyyyMMdd", null);
        }

        public Period CreatePeriod()
        {
            return new Period(GetFirstDay(), GetLastDay());
        }

        public int GetDailyAmount()
        {
            return Amount / GetLastDay().Day;
        }
    }
}
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

        private int Amount { get; }
        private string YearMonth { get; }

        public int GetOverlappingAmount(Period period)
        {
            return GetDailyAmount() * period.GetOverlappingDays(CreatePeriod());
        }

        private Period CreatePeriod()
        {
            return new Period(GetFirstDay(), GetLastDay());
        }

        private int GetDailyAmount()
        {
            return Amount / GetLastDay().Day;
        }

        private DateTime GetFirstDay()
        {
            return DateTime.ParseExact(YearMonth, "yyyyMM", null);
        }

        private DateTime GetLastDay()
        {
            var firstDay = GetFirstDay();
            var daysInMonth = DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
            return DateTime.ParseExact(YearMonth + daysInMonth, "yyyyMMdd", null);
        }
    }
}
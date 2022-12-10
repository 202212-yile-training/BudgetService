#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace BudgetProject
{
    public class BudgetService
    {
        private readonly IBudgetRepo _budgetRepo;
        private Dictionary<string, int> _yearMonthBudget;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return 0;
            }

            var budgets = _budgetRepo.getAll();
            _yearMonthBudget = budgets.ToDictionary(budget => budget.YearMonth, budget => budget.Amount);

            if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
            {
                //同年同月
                return GetSingleDayBudgetInMonth(start.Year, start.Month) *
                    GetSameMonthDays(start, end);
            }

            DateTime valueDateTime = new DateTime(start.Year, start.Month, 1);
            //DateTime valueDateTime = startTime;
            int total = 0;
            while (valueDateTime <= end)
            {
                int defineYear = valueDateTime.Year;
                int defineMonth = valueDateTime.Month;

                if (valueDateTime.ToString("yyyyMM") == start.ToString("yyyyMM"))
                {
                    int remainDay = DateTime.DaysInMonth(defineYear, defineMonth);
                    Console.WriteLine(remainDay);
                    total += GetSingleDayBudgetInMonth(defineYear, defineMonth) * GetSameMonthDays(start, new DateTime(start.Year, start.Month, remainDay));
                }
                else if (valueDateTime.ToString("yyyyMM") == end.ToString("yyyyMM"))
                {
                    total += GetSingleDayBudgetInMonth(defineYear, defineMonth) * GetSameMonthDays(new DateTime(end.Year, end.Month, 1), end);
                }
                else
                {
                    total += GetBudgetByYearMonth(new DateTime(defineYear, defineMonth, 1));
                }

                valueDateTime = valueDateTime.AddMonths(1);
            }

            return total;
        }

        //起訖錯誤

        int GetBudgetByYearMonth(DateTime time)
        {
            string yyyymm = time.Year.ToString() + time.Month.ToString("00");
            if (!_yearMonthBudget.ContainsKey(yyyymm))
            {
                return 0;
            }

            return _yearMonthBudget[yyyymm];
        }

        int GetSameMonthDays(DateTime startTime, DateTime endDateTime)
        {
            return endDateTime.Day - startTime.Day + 1;
        }

        int GetSingleDayBudgetInMonth(int year, int month)
        {
            var monnthBudget = GetBudgetByYearMonth(new DateTime(year, month, 1));

            return monnthBudget / GetTotalDayInMonth(year, month);
        }

        int GetTotalDayInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }
    }
}
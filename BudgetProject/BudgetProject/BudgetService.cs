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
        private Dictionary<string, Budget> _yearMonthBudget;

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
            _yearMonthBudget = budgets.ToDictionary(budget => budget.YearMonth, budget => budget);

            if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
            {
                //同年同月
                return GetSingleDayBudgetInMonth(start.Year, start.Month) *
                    GetSameMonthDays(start, end);
            }

            var currentMonth = new DateTime(start.Year, start.Month, 1);
            var total = 0;
            while (currentMonth <= end)
            {
                if (_yearMonthBudget.ContainsKey(currentMonth.ToString("yyyyMM")))
                {
                    var budget = _yearMonthBudget[currentMonth.ToString("yyyyMM")];
                    DateTime overlappingStart;
                    DateTime overlappingEnd;
                    if (budget.YearMonth == start.ToString("yyyyMM"))
                    {
                        overlappingStart = start;
                        overlappingEnd = budget.GetLastDay();
                    }
                    else if (budget.YearMonth == end.ToString("yyyyMM"))
                    {
                        overlappingStart = budget.GetFirstDay();
                        overlappingEnd = end;
                    }
                    else
                    {
                        overlappingStart = budget.GetFirstDay();
                        overlappingEnd = budget.GetLastDay();
                    }

                    var overlappingDays = overlappingEnd.Day - overlappingStart.Day + 1;
                    total += GetSingleDayBudgetInMonth(currentMonth.Year, currentMonth.Month) * overlappingDays;
                }

                currentMonth = currentMonth.AddMonths(1);
            }

            return total;
        }

        int GetBudgetByYearMonth(DateTime time)
        {
            var yyyymm = time.Year.ToString() + time.Month.ToString("00");
            if (!_yearMonthBudget.ContainsKey(yyyymm))
            {
                return 0;
            }

            return _yearMonthBudget[yyyymm].Amount;
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
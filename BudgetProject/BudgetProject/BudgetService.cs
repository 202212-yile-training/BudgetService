﻿#region

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

            var currentMonth = new DateTime(start.Year, start.Month, 1);
            var total = 0;
            while (currentMonth <= end)
            {
                if (currentMonth.ToString("yyyyMM") == start.ToString("yyyyMM"))
                {
                    var overlappingStart = start;
                    var overlappingEnd = new DateTime(start.Year, start.Month, DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month));
                    total += GetSingleDayBudgetInMonth(currentMonth.Year, currentMonth.Month) * GetSameMonthDays(overlappingStart, overlappingEnd);
                }
                else if (currentMonth.ToString("yyyyMM") == end.ToString("yyyyMM"))
                {
                    var overlappingStart = new DateTime(end.Year, end.Month, 1);
                    var overlappingEnd = end;
                    total += GetSingleDayBudgetInMonth(currentMonth.Year, currentMonth.Month) * GetSameMonthDays(overlappingStart, overlappingEnd);
                }
                else
                {
                    var overlappingStart = new DateTime(currentMonth.Year, currentMonth.Month, 1);
                    var overlappingEnd = new DateTime(currentMonth.Year, currentMonth.Month, DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month));
                    total += GetSingleDayBudgetInMonth(currentMonth.Year, currentMonth.Month) * GetSameMonthDays(overlappingStart, overlappingEnd);
                    // total += GetBudgetByYearMonth(new DateTime(currentMonth.Year, currentMonth.Month, 1));
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
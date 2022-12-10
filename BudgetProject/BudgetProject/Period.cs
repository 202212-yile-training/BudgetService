#region

using System;

#endregion

namespace BudgetProject
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime End { get; }
        public DateTime Start { get; }

        public int GetOverlappingDays(Budget budget)
        {
            DateTime overlappingStart;
            DateTime overlappingEnd;
            if (budget.YearMonth == Start.ToString("yyyyMM"))
            {
                overlappingStart = Start;
                overlappingEnd = budget.GetLastDay();
            }
            else if (budget.YearMonth == End.ToString("yyyyMM"))
            {
                overlappingStart = budget.GetFirstDay();
                overlappingEnd = End;
            }
            else
            {
                overlappingStart = budget.GetFirstDay();
                overlappingEnd = budget.GetLastDay();
            }

            var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            return overlappingDays;
        }
    }
}
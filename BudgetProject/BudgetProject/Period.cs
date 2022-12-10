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

        private DateTime End { get; }
        private DateTime Start { get; }

        public int GetOverlappingDays(Budget budget)
        {
            DateTime overlappingStart = Start > budget.GetFirstDay()
                ? Start
                : budget.GetFirstDay();
            DateTime overlappingEnd = End < budget.GetLastDay()
                ? End
                : budget.GetLastDay();
            if (budget.YearMonth == Start.ToString("yyyyMM"))
            {
                // overlappingStart = Start;
                // overlappingEnd = budget.GetLastDay();
            }
            else if (budget.YearMonth == End.ToString("yyyyMM"))
            {
                // overlappingStart = budget.GetFirstDay();
                // overlappingEnd = End;
            }
            else
            {
                // overlappingStart = budget.GetFirstDay();
                // overlappingEnd = budget.GetLastDay();
            }

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}
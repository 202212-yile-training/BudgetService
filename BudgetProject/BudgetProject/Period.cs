﻿#region

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

        public int GetOverlappingDays(Period another)
        {
            var overlappingStart = Start > another.Start
                ? Start
                : another.Start;

            var overlappingEnd = End < another.End
                ? End
                : another.End;

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}
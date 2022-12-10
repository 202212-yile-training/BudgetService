#region

using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

#endregion

namespace BudgetProject
{
    [TestFixture]
    class BudgetServiceTest
    {
        private IBudgetRepo _budgetRepo;
        private BudgetService _budgetService;

        [SetUp]
        public void SetUp()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();
            _budgetService = new BudgetService(_budgetRepo);
            // _budgetService = new BudgetService(new TDDRepo());
            // var budgets = new List<Budget>()
            //            {
            //                new Budget("202201",31),
            //                new Budget("202202",280),
            //                new Budget("202203",62),
            //                new Budget("202204",3000),
            //                new Budget("202205",62000)
            //            };
        }

        [Test]
        public void query_single_day()
        {
            GivenBudgets(new Budget("202201", 31));

            DateTime day = new DateTime(2022, 1, 1);

            decimal result = _budgetService.Query(day, day);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void query_partial_month()
        {
            GivenBudgets(new Budget("202201", 31));
            DateTime startDay = new DateTime(2022, 1, 1);
            DateTime endDay = new DateTime(2022, 1, 13);
            decimal result = _budgetService.Query(startDay, endDay);

            Assert.AreEqual(13, result);
        }

        [Test]
        public void query_whole_month()
        {
            GivenBudgets(new Budget("202201", 31));
            DateTime startDay = new DateTime(2022, 1, 1);
            DateTime endDay = new DateTime(2022, 1, 31);
            decimal result = _budgetService.Query(startDay, endDay);

            Assert.AreEqual(31, result);
        }

        [Test]
        public void cross_2_full_month()
        {
            GivenBudgets(
                new Budget("202201", 31),
                new Budget("202202", 280)
            );
            DateTime startDay = new DateTime(2022, 1, 1);
            DateTime endDay = new DateTime(2022, 2, 28);
            decimal result = _budgetService.Query(startDay, endDay);

            Assert.AreEqual(31 + 280, result);
        }

        [Test]
        public void cross_year()
        {
            GivenBudgets(
                new Budget("202201", 31),
                new Budget("202202", 280),
                new Budget("202203", 62),
                new Budget("202204", 3000),
                new Budget("202205", 62000)
            );
            DateTime startDay = new DateTime(2022, 1, 1);
            DateTime endDay = new DateTime(2023, 2, 28);
            decimal result = _budgetService.Query(startDay, endDay);

            Assert.AreEqual(31 + 280 + 62 + 3000 + 62000, result);
        }

        private void GivenBudgets(params Budget[] budgets)
        {
            _budgetRepo.getAll()
                       .Returns(budgets.ToList());
        }
    }
}
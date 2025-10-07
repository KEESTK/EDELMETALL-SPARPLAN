using System;
using System.Linq;
using Xunit;
using Sparplan.Domain.Entities;

namespace Sparplan.Domain.Tests
{
    public class DepotTests
    {
        [Fact]
        public void AddSparplan_ShouldAddToDepot()
        {
            var depot = new Depot();
            var sparplan = new SparplanClass(MetalType.Gold, 100, depot);

            depot.AddSparplan(sparplan);

            Assert.Single(depot.Sparplaene);
            Assert.Equal(sparplan, depot.Sparplaene.First());
        }

        [Fact]
        public void GetTotalBalance_ShouldReturnSumOfAllPlans()
        {
            var depot = new Depot();
            var sp1 = new SparplanClass(MetalType.Gold, 100, depot);
            var sp2 = new SparplanClass(MetalType.Silver, 200, depot);

            sp1.AddContribution(1m, 500m);
            sp2.AddContribution(2m, 1000m);

            depot.AddSparplan(sp1);
            depot.AddSparplan(sp2);

            Assert.Equal(3m, depot.GetTotalBalanceInBars());
        }

        [Fact]
        public void GetActivePlans_ShouldReturnOnlyActive()
        {
            var depot = new Depot();
            var activePlan = new SparplanClass(MetalType.Gold, 100, depot);
            var closedPlan = new SparplanClass(MetalType.Silver, 200, depot);

            activePlan.AddContribution(1m, 500m);
            closedPlan.AddContribution(1m, 500m);
            closedPlan.Close(1m);

            depot.AddSparplan(activePlan);
            depot.AddSparplan(closedPlan);

            var active = depot.GetActivePlans().ToList();

            Assert.Single(active);
            Assert.Equal(activePlan.Id, active[0].Id);
        }
    }
}

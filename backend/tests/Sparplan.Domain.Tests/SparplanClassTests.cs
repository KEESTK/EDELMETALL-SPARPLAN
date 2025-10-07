using System;
using Xunit;
using Sparplan.Domain.Entities;

namespace Sparplan.Domain.Tests
{
    public class SparplanClassTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDepot()
        {
            var depot = new Depot();
            var sparplan = new SparplanClass(MetalType.Gold, 200, depot);

            Assert.Equal(MetalType.Gold, sparplan.Metal);
            Assert.Equal(200m, sparplan.MonthlyRate);
            Assert.True(sparplan.IsActive);
            Assert.Equal(depot.Id, sparplan.DepotId);
            Assert.Empty(sparplan.Transactions);
        }

        [Fact]
        public void AddContribution_ShouldIncreaseBalance_AndCreateTransaction()
        {
            var depot = new Depot();
            var sparplan = new SparplanClass(MetalType.Gold, 200, depot);

            sparplan.AddContribution(1.5m, 1000m);

            Assert.Equal(1.5m, sparplan.BalanceInBars);
            Assert.Single(sparplan.Transactions);
            Assert.Equal(TransactionType.Deposit, sparplan.Transactions[0].Type);
            Assert.Equal(1000m, sparplan.Transactions[0].Amount);
        }

        [Fact]
        public void AddContribution_ShouldThrow_WhenPlanIsClosed()
        {
            var depot = new Depot();
            var sparplan = new SparplanClass(MetalType.Silver, 100, depot);
            sparplan.Close(0);

            Assert.Throws<InvalidOperationException>(() => sparplan.AddContribution(1m, 500m));
        }

        [Fact]
        public void DeductFee_ShouldReduceBalance_AndAddTransaction()
        {
            var depot = new Depot();
            var sparplan = new SparplanClass(MetalType.Gold, 200, depot);
            sparplan.AddContribution(2m, 1000m);

            sparplan.DeductFee(0.5m);

            Assert.Equal(1.5m, sparplan.BalanceInBars);
            Assert.Equal(TransactionType.Fee, sparplan.Transactions[1].Type);
            Assert.Equal(0.5m, sparplan.Transactions[1].Amount);
        }

        [Fact]
        public void DeductFee_ShouldThrow_WhenBalanceIsTooLow()
        {
            var depot = new Depot();
            var sparplan = new SparplanClass(MetalType.Gold, 200, depot);

            Assert.Throws<InvalidOperationException>(() => sparplan.DeductFee(0.1m));
        }

        [Fact]
        public void Close_ShouldMarkPlanInactive_AndAddPayoutTransaction()
        {
            var depot = new Depot();
            var sparplan = new SparplanClass(MetalType.Silver, 100, depot);
            sparplan.AddContribution(2m, 500m);

            sparplan.Close(1.5m);

            Assert.False(sparplan.IsActive);
            Assert.Equal(0.5m, sparplan.BalanceInBars);
            Assert.Equal(TransactionType.Payout, sparplan.Transactions[1].Type);
        }

        [Fact]
        public void Close_ShouldThrow_WhenBalanceTooLow()
        {
            var depot = new Depot();
            var sparplan = new SparplanClass(MetalType.Gold, 200, depot);
            sparplan.AddContribution(0.5m, 200m);

            Assert.Throws<InvalidOperationException>(() => sparplan.Close(1m));
        }
    }
}

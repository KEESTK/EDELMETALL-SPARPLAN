using System;
using Xunit;
using Sparplan.Domain.Entities;

namespace Sparplan.Domain.Tests
{
    public class TransactionTests
    {
        [Fact]
        public void Transaction_ShouldHaveIdAndDate()
        {
            var depot = new Depot();
            var sparplan = new SparplanClass(MetalType.Gold, 100, depot);

            var tx = new Transaction(TransactionType.Deposit, 1000m, sparplan);

            Assert.NotEqual(Guid.Empty, tx.Id);
            Assert.True((DateTime.UtcNow - tx.Date).TotalSeconds < 2);
            Assert.Equal(TransactionType.Deposit, tx.Type);
            Assert.Equal(1000m, tx.Amount);
            Assert.Equal(sparplan.Id, tx.Sparplan.Id);
        }
    }
}

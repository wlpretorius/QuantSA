using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantSA.Core.Products.SAMarket;
using QuantSA.CoreExtensions.SAMarket;
using QuantSA.Shared.Dates;
using QuantSA.Solution.Test;

namespace QuantSA.CoreExtensions.Test.SAMarket
{
    [TestClass]
    public class BesaJseCPIBondTest
    {
        [TestMethod]
        public void TestAllInCPIPrice()
        {
            var settleDate = new Date(2020, 12, 23);
            var maturityDate = new Date(2025, 1, 31);
            var notional = 1000000.0;
            var annualCouponRate = 0.02;
            var couponMonth1 = 1;
            var couponDay1 = 31;
            var couponMonth2 = 7;
            var couponDay2 = 31;
            var booksCloseDateDays = 21;
            var zaCalendar = new Calendar("Test");
            var bondI2025 = new BesaJseBond(maturityDate, notional, annualCouponRate, couponMonth1,
                couponDay1, couponMonth2, couponDay2, booksCloseDateDays, zaCalendar, TestHelpers.ZAR);

            var ytm = 0.027;

            var issueDate = new Date(2012, 07, 04);

            double[] cpiIssue = { 77.6, 77.9 };
            double[] cpiSettlement = { 116.6, 116.8 };

            var cpiBondI2025 = new BesaJseCPIBond(bondI2025, settleDate, issueDate, cpiSettlement, cpiIssue);

            var results = BesaJseCPIBondEx.GetCPISpotMeasures(cpiBondI2025, cpiIssue, ytm, cpiSettlement);
            Assert.AreEqual(147.50232, (double)results.GetScalar(BesaJseCPIBondEx.Keys.GetCPISpotMeasures), 1e-6);
        }
    }
}

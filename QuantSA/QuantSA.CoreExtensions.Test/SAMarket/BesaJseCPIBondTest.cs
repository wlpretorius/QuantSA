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
        public void TestAllInPrice()
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

            var results = bondI2025.GetSpotMeasures(settleDate, ytm);
            Assert.AreEqual(98.08354, (double)results.GetScalar(BesaJseBondEx.Keys.RoundedAip), 1e-8);
        }

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

            var baseCpi = 77.62806717;
            var cpiM4 = 116.6;
            var cpiM3 = 116.8;

            var cpiBondI2025 = new BesaJseCPIBond(bondI2025, baseCpi, cpiM4, cpiM3, settleDate);

            var results = BesaJseCPIBondEx.GetCPISpotMeasures(cpiBondI2025, baseCpi, ytm);
            Assert.AreEqual(147.50415, (double)results.GetScalar(BesaJseCPIBondEx.Keys.GetCPISpotMeasures), 1e-8);
        }
    }
}

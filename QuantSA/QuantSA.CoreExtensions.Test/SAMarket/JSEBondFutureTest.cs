using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantSA.Core.Products.SAMarket;
using QuantSA.CoreExtensions.SAMarket;
using QuantSA.Shared.Dates;
using QuantSA.Solution.Test;

namespace QuantSA.CoreExtensions.Test.SAMarket
{
    [TestClass]
    public class JSEBondFutureTest
    {
        [TestMethod]
        public void TestAllInPrice()
        {
            var settleDate = new Date(2006, 6, 8);
            var maturityDate = new Date(2026, 12, 21);
            var notional = 1000000.0;
            var annualCouponRate = 0.105;
            var couponMonth1 = 6;
            var couponDay1 = 21;
            var couponMonth2 = 12;
            var couponDay2 = 21;
            var booksCloseDateDays = 10;
            var zaCalendar = new Calendar("Test");
            var bondR186 = new BesaJseBond(maturityDate, notional, annualCouponRate, couponMonth1,
                couponDay1, couponMonth2, couponDay2, booksCloseDateDays, zaCalendar, TestHelpers.ZAR);

            var ytm = 0.0715;
            var results = bondR186.GetSpotMeasures(settleDate, ytm);
            Assert.AreEqual(140.65075443, (double)results.GetScalar(BesaJseBondEx.Keys.UnroundedAip), 1e-8);
            Assert.AreEqual(140.65075, (double)results.GetScalar(BesaJseBondEx.Keys.RoundedAip), 1e-8);


        }

    }
}

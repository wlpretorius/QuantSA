using System;
using System.Collections.Generic;
using System.Linq;
using QuantSA.Core.Products.SAMarket;
using QuantSA.Shared;
using QuantSA.Shared.Dates;

namespace QuantSA.CoreExtensions.SAMarket
{
    public static class JSEBondFutureEx
    {
        // Get coupon dates for books close dates that lie between settlement date and expiey date
        private static List<Date> GetCouponDates(this BesaJseBond bond, Date settleDate, Date expiryDate)
        {
            var BooksCloseDates = new List<Date>();
            var CouponDates = new List<Date>();

            var yr = settleDate.Year;
            while (yr >= settleDate.Year && yr < expiryDate.Year + 1)
            {
                var BCD1 = (new Date(yr, bond.couponMonth1, bond.couponDay1).AddDays(-bond.booksCloseDateDays));
                var BCD2 = (new Date(yr, bond.couponMonth2, bond.couponDay2).AddDays(-bond.booksCloseDateDays));

                if (BCD1 > settleDate && BCD1 < expiryDate)
                {
                    CouponDates.Add(new Date(yr, bond.couponMonth1, bond.couponDay1));
                    BooksCloseDates.Add(BCD1);
                }

                if (BCD2 > settleDate && BCD2 < expiryDate)
                {
                    CouponDates.Add(new Date(yr, bond.couponMonth2, bond.couponDay2));
                    BooksCloseDates.Add(BCD2);
                }

                yr += 1;
            }

            return CouponDates;

        }
        public static ResultStore FuturePrice(this JSEBondFuture bondfuture, Date settleDate, double ytm, double repo)
        {
            var N = 100.0;

            var couponamount = N * bondfuture.underlyingBond.annualCouponRate / 2;
            var expiryDate = bondfuture.expiryDate;

            if (settleDate > expiryDate)
                throw new ArgumentException("settlement date must be before expiry date.");

            // get all-in price of underlying bond
            var results = bondfuture.underlyingBond.GetSpotMeasures(settleDate, ytm);
            var AIP = (double)results.GetScalar(BesaJseBondEx.Keys.RoundedAip);

            // calculate Unadjusted Forward Price
            var dt = (expiryDate - settleDate) / 365;
            var FuturePrice = AIP + AIP * (1 + repo * dt);

            // get coupon dates between settlement and forward date and calculate equivalent value function
            var CouponDates = GetCouponDates(bondfuture.underlyingBond, settleDate, expiryDate);

            var EV = new List<double>();

            double AdjustedFuturePrice = 0;

            if (CouponDates.Any())
            {
                foreach (var date in CouponDates)
                {
                    if (date <= expiryDate)
                    {
                        EV.Add(1 + repo * (expiryDate - date) / 365);
                    }
                    else
                    {
                        EV.Add(Math.Pow(1 + repo * (date - expiryDate) / 365, -1));
                    }
                }

                AdjustedFuturePrice = FuturePrice - couponamount * EV.Sum();
            }
            else
            {
                AdjustedFuturePrice = FuturePrice;
            }

            var resultStore = new ResultStore();
            resultStore.Add(Keys.FuturePrice, AdjustedFuturePrice);
            return resultStore;
        }
        public static class Keys
        {
            public const string FuturePrice = "AdjustedFuturePrice";
        }




    }
}

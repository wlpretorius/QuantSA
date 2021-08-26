using ExcelDna.Integration;
using QuantSA.Excel.Shared;
using QuantSA.Shared.Dates;
using QuantSA.Shared.Primitives;
using QuantSA.Core.Products.SAMarket;
using QuantSA.CoreExtensions.SAMarket;


namespace QuantSA.ExcelFunctions
{
    public class XLSAMarket
    {
        [QuantSAExcelFunction(
            Description = "Create a Besa JSE Bond.",
            Name = "QSA.CreateBesaJSEBond",
            HasGeneratedVersion = true,
            ExampleSheet = "BesaJSEBond.xlsx",
            Category = "QSA.SAMarket",
            IsHidden = false,
            HelpTopic = "")]

        public static BesaJseBond CreateBesaJseBond(
            [ExcelArgument(Description = "The maturity date of the bond.")]
            Date maturityDate,
            [ExcelArgument(Description = "The notional amount of the bond.")]
            double notional,
            [ExcelArgument(Description = "The annual coupon rate of the bond.")]
            double annualCouponRate,
            [ExcelArgument(Description = "The month the first bond coupon is paid.")]
            int couponMonth1,
            [ExcelArgument(Description = "The day the first bond coupon is paid.")]
            int couponDay1,
            [ExcelArgument(Description = "The month the second bond coupon is paid.")]
            int couponMonth2,
            [ExcelArgument(Description = "The day the second bond coupon is paid.")]
            int couponDay2,
            [ExcelArgument(Description = "The books close date days of the bond.")]
            int bookscloseDateDays,
            [ExcelArgument(Description = "The currency of the cashflows.")]
            Currency currency)

        {
            return new BesaJseBond(maturityDate, notional, annualCouponRate, couponMonth1, couponDay1, couponMonth2, couponDay2, bookscloseDateDays,
                new Calendar("ZA"), currency);
        }

        [QuantSAExcelFunction(
            Description = "Returns all key output parameters of a Besa JSE Bond.",
            Name = "QSA.BesaJseBondResults",
            HasGeneratedVersion = true,
            ExampleSheet = "BesaJSEBond.xlsx",
            Category = "QSA.SAMarket",
            IsHidden = false,
            HelpTopic = "")]

        public static string[,] BesaJseBondResults(

            [ExcelArgument(Description = "The underlying bond.")]
            BesaJseBond besaJseBond,
            [ExcelArgument(Description = "The settlement date of the bond.")]
            Date settleDate,
            [ExcelArgument(Description = "The yield to maturity of the bond.")]
            double ytm)

        {
            var results = besaJseBond.GetSpotMeasures(settleDate, ytm);
            string[,] measures = { {"roundedAip", results.GetScalar(BesaJseBondEx.Keys.RoundedAip).ToString()}, {"unroundedAip", results.GetScalar(BesaJseBondEx.Keys.UnroundedAip).ToString()},
                {"roundedClean", results.GetScalar(BesaJseBondEx.Keys.RoundedClean).ToString()},{"unroundedClean", results.GetScalar(BesaJseBondEx.Keys.UnroundedClean).ToString()},
                {"unroundedAccrued", results.GetScalar(BesaJseBondEx.Keys.UnroundedAccrued).ToString()} };

            return measures;
        }
    }
}
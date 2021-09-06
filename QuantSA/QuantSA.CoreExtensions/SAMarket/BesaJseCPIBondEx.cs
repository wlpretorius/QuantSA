using QuantSA.Core.Products.SAMarket;
using QuantSA.Shared.Dates;
using QuantSA.Shared;
using System;

namespace QuantSA.CoreExtensions.SAMarket
{
    public static class BesaJseCPIBondEx
    {
        private static double GetCPIAtSettlementDate(BesaJseCPIBond besaJseCPIBond, double cpiM4, double cpiM3)
        {
            var theSettlementDate = new Date(besaJseCPIBond.settleDate.Year, besaJseCPIBond.settleDate.Month, besaJseCPIBond.settleDate.Day);
            var actualDayInMonth = theSettlementDate.Day;
            var noDaysInMonth = Date.DaysInMonth(besaJseCPIBond.settleDate.Year, besaJseCPIBond.settleDate.Month);

            var cpiAtSettlementDate_M4 = (double)(noDaysInMonth - actualDayInMonth + 1) / noDaysInMonth * cpiM4;
            var cpiAtSettlementDate_M3 = (double)(actualDayInMonth - 1) / noDaysInMonth * cpiM3;
            var cpiAtSettlementDate = cpiAtSettlementDate_M4 + cpiAtSettlementDate_M3;

            return cpiAtSettlementDate;
        }

        public static ResultStore GetCPISpotMeasures(BesaJseCPIBond besaJseCPIBond, double baseCpi, double ytm)
        {
            // Get the all-in price of underlying bond
            var settleDate = besaJseCPIBond.settleDate;
            var results = besaJseCPIBond.underlyingBond.GetSpotMeasures(settleDate, ytm);
            var Aip = (double)results.GetScalar(BesaJseBondEx.Keys.RoundedAip);

            // Calculate index-ratio
            var cpiM4 = besaJseCPIBond.cpiM4;
            var cpiM3 = besaJseCPIBond.cpiM3;
            var indexRatio = (double)GetCPIAtSettlementDate(besaJseCPIBond, cpiM4, cpiM3) / baseCpi;

            // Get the all-in price of underlying index-linked bond
            var AipCpi = Math.Round(Aip * indexRatio, 5);

            var resultStore = new ResultStore();
            resultStore.Add(Keys.GetCPISpotMeasures, AipCpi);
            return resultStore;
        }

        public static class Keys
        {
            public const string GetCPISpotMeasures = "AipCpi";
        }
    }
}

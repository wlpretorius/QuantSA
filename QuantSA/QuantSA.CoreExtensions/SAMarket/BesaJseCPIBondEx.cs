using QuantSA.Core.Products.SAMarket;
using QuantSA.Shared.Dates;
using QuantSA.Shared;
using System;

namespace QuantSA.CoreExtensions.SAMarket
{
    public static class BesaJseCPIBondEx
    {
        private static double GetCPIAtIssueDate(BesaJseCPIBond besaJseCPIBond, double[] cpiIssue)
        {
            var theIssueDate = new Date(besaJseCPIBond.issueDate.Year, besaJseCPIBond.issueDate.Month, besaJseCPIBond.issueDate.Day);
            var actualDayInMonth = theIssueDate.Day;
            var noDaysInMonth = Date.DaysInMonth(besaJseCPIBond.issueDate.Year, besaJseCPIBond.issueDate.Month);

            var cpiAtIssue_M4 = (double)(noDaysInMonth - actualDayInMonth + 1) / noDaysInMonth * cpiIssue[0];
            var cpiAtIssue_M3 = (double)(actualDayInMonth - 1) / noDaysInMonth * cpiIssue[1];
            var cpiAtIssueDate = cpiAtIssue_M4 + cpiAtIssue_M3;

            return cpiAtIssueDate;
        }
        private static double GetCPIAtSettlementDate(BesaJseCPIBond besaJseCPIBond, double[] cpiSettlement)
        {
            var theSettlementDate = new Date(besaJseCPIBond.settleDate.Year, besaJseCPIBond.settleDate.Month, besaJseCPIBond.settleDate.Day);
            var actualDayInMonth = theSettlementDate.Day;
            var noDaysInMonth = Date.DaysInMonth(besaJseCPIBond.settleDate.Year, besaJseCPIBond.settleDate.Month);

            var cpiAtSettlementDate_M4 = (double)(noDaysInMonth - actualDayInMonth + 1) / noDaysInMonth * cpiSettlement[0];
            var cpiAtSettlementDate_M3 = (double)(actualDayInMonth - 1) / noDaysInMonth * cpiSettlement[1];
            var cpiAtSettlementDate = cpiAtSettlementDate_M4 + cpiAtSettlementDate_M3;

            return cpiAtSettlementDate;
        }

        public static ResultStore GetCPISpotMeasures(BesaJseCPIBond besaJseCPIBond, double[] cpiIssue, double ytm, double[] cpiSettlement)
        {
            // Get the all-in price of underlying bond
            var settleDate = besaJseCPIBond.settleDate;
            var results = besaJseCPIBond.underlyingBond.GetSpotMeasures(settleDate, ytm);
            var Aip = (double)results.GetScalar(BesaJseBondEx.Keys.RoundedAip);

            // Calculate index-ratio
            var indexRatio = (double)GetCPIAtSettlementDate(besaJseCPIBond, cpiSettlement) / GetCPIAtIssueDate(besaJseCPIBond, cpiIssue);

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

using System;
using System.Collections.Generic;
using System.Text;
using QuantSA.Shared.Dates;
using QuantSA.Shared.Primitives;

namespace QuantSA.Core.Products.SAMarket
{
    /// <summary>
    /// The market standard inflation-linked (CPI) bond traded on the JSE.
    /// </summary>
    public class BesaJseCPIBond : ProductWrapper
    {

        public BesaJseBond underlyingBond;

        /// <summary>
        /// The inflation index (CPI) for the required Settlement date. The inflation figure applicable to any
        /// particular settlement date will be based on the interpolated CPI figures looking back 4 months
        /// and 3 months respectively.The CPI value published for any given month, 
        /// represents the index value on the first day of the month 4 months later. 
        /// This is known as lagged indexation and is needed so that cash flows on payment dates can be computed. 
        /// [0] = Month 4
        /// [1] = Month 3
        /// </summary>
        public double[] cpiSettlement;

        /// <summary>
        /// The inflation index (CPI) for the required Issue date. 
        /// The methodology for determining the Reference CPI for the inflation linked bond on issue
        /// date is exactly the same as that for any other date.
        /// The Index Ratio is calculated by dividing the Reference CPI for the settlement date by the
        /// Reference CPI for the issue date. This ratio is multiplied by the BesaJseBondPrice to obtain the final Inflation-Linked price.
        /// [0] = Month 4
        /// [1] = Month 3
        /// </summary>
        public double[] cpiIssue;

        public Date settleDate;

        public Date issueDate;

        public BesaJseCPIBond(BesaJseBond besaJseBond, Date settleDate, Date issueDate, double[] cpiSettlement, double[] cpiIssue)
        {

            underlyingBond = besaJseBond;
            this.settleDate = settleDate;
            this.issueDate = issueDate;
            this.cpiSettlement = cpiSettlement;
            this.cpiIssue = cpiIssue;
        }

        public override List<Cashflow> GetCFs()
        {
            throw new NotImplementedException();
        }
    }
}

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
        /// The inflation index (CPI) for the required date. This will be CPI(m-4) and CPI(m-3) according 
        /// to the JSE CPI Pricing Formula. The Base CPI Value is given by the JSE Calculator and the CPI index
        /// values are given by STATSSA. The user will provide the 3 CPI values. From this the CPI at settlement date
        /// will be calculator in order to price the index-linked bond. The CPI value published for any given month, 
        /// represents the index value on the first day of the month 4 months later. 
        /// This is known as lagged indexation and is needed so that cash flows on payment dates can be computed. 
        /// The index value for dates between the first of a month (CPI at settlement date) are obtained using linear interpolation.
        /// </summary>
        public double cpiM4, cpiM3;

        /// <summary>
        /// The CPI value for a particular month is only published in the following month. 
        /// For example, the July CPI index number is only published in August and the August number in September. 
        /// </summary>
        public double baseCpi;

        public Date settleDate;

        public BesaJseCPIBond(BesaJseBond besaJseBond, double baseCpi, double cpiM4, double cpiM3, Date settleDate)
        {

            underlyingBond = besaJseBond;
            this.cpiM4 = cpiM4;
            this.cpiM3 = cpiM3;
            this.baseCpi = baseCpi;
            this.settleDate = settleDate;
        }

        public override List<Cashflow> GetCFs()
        {
            throw new NotImplementedException();
        }
    }
}

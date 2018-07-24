﻿using ExcelDna.Integration;
using QuantSA.General;
using QuantSA.Valuation.Models;
using QuantSA.Valuation;
using System.Linq;
using QuantSA.Core.MarketData;
using QuantSA.Excel.Shared;
using QuantSA.Shared.Dates;
using QuantSA.Shared.MarketData;
using QuantSA.Shared.MarketObservables;
using QuantSA.Shared.Primitives;
using QuantSA.Valuation.Models.Rates;

namespace QuantSA.ExcelFunctions
{
    public class XLFX
    {
        [QuantSAExcelFunction(Description = "Create a curve to be used for FX rate forecasting.",
            Name = "QSA.CreateFXForecastCurve",
            HasGeneratedVersion = true,
            Category = "QSA.FX",
            ExampleSheet = "GeneralSwap.xlsx",
            IsHidden = false,
            HelpTopic = "http://www.quantsa.org/CreateFXForecastCurve.html")]
        public static FXForecastCurve CreateFXForecastCurve(
            [ExcelArgument(Description = "The currency pair that the curve forecasts.")]
            CurrencyPair currencyPair,
            [ExcelArgument(Description = "The rate at the anchor date of the two curves.")]
            double fxRateAtAnchorDate,
            [ExcelArgument(Description = "A curve that will be used to obtain forward rates.")]
            IDiscountingSource baseCurrencyFXBasisCurve,
            [ExcelArgument(Description = "A curve that will be used to obtain forward rates.")]
            IDiscountingSource counterCurrencyFXBasisCurve)
        {
            return new FXForecastCurve(currencyPair, fxRateAtAnchorDate, baseCurrencyFXBasisCurve,
                counterCurrencyFXBasisCurve);
        }

        [QuantSAExcelFunction(Description = "Get the FX rate at a date.  There is no spot settlement adjustment.",
            Name = "QSA.GetFXRate",
            HasGeneratedVersion = true,
            ExampleSheet = "Introduction.xlsx",
            Category = "QSA.FX",
            IsHidden = false,
            HelpTopic = "http://www.quantsa.org/GetFXRate.html")]
        public static double GetFXRate([ExcelArgument(Description = "Name of FX curve")]IFXSource fxCurve,
            [ExcelArgument(Description = "Date on which FX rate is required.")]Date date)
        {
            return fxCurve.GetRate(date);
        }


        [QuantSAExcelFunction(Description = "A sample model that simulates FX processes according to geometric Brownian motion and short rates according to Hull White.",
            Name = "QSA.CreateMultiHWAndFXToy",
            HasGeneratedVersion = true,
            ExampleSheet = "PFE.xlsx",
            Category = "QSA.FX",
            IsHidden = false,
            HelpTopic = "http://www.quantsa.org/CreateMultiHWAndFXToy.html")]
        public static NumeraireSimulator CreateMultiHWAndFXToy(
            [ExcelArgument(Description = "The date from which the model applies")]
            Date anchorDate,
            [QuantSAExcelArgument(Description = "The currency into which all valuations will be converted.")]
            Currency numeraireCcy,
            [QuantSAExcelArgument(Description = "Hull White simulators for each of the currencies")]
            HullWhite1F[] rateSimulators,
            [QuantSAExcelArgument(Description = "The list of other currencies pairs to be simulated, they must all have " +
                                                "the numeraire currency as their counter currency.")]
            CurrencyPair[] currencyPairs,
            [QuantSAExcelArgument(Description = "The initial values for the FX processes at the anchor date.  These would " +
                                                "actually need to be discounted spot rates.")]
            double[] spots,
            [QuantSAExcelArgument(Description = "The volatilities for the FX processes.")]
            double[] vols,
            [QuantSAExcelArgument(Description = "A correlation matrix for the FX processes, rows and columns must be in the " +
                                                "order of the currencies in 'currencies'")]
            double[,] correlations)            
        {
            return new MultiHWAndFXToy(anchorDate, numeraireCcy, rateSimulators, currencyPairs, spots, vols, correlations);
        }
    }
}

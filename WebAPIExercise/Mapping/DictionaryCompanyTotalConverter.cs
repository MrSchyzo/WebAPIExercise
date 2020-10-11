using System;
using System.Collections.Generic;

namespace WebAPIExercise.Mapping
{
    public class DictionaryCompanyTotalConverter : ICompanyTotalConverter
    {
        private static readonly IDictionary<string, Func<double, double>> companyTable = new Dictionary<string, Func<double, double>>()
        {
            ["COMPANY_1"] = current => 1.0002 * current,
            ["COMPANY_2"] = current => 1 + current
        };

        public double ComputeTotalFor(string companyCode, double currentTotal)
        {
            return companyTable[companyCode].Invoke(currentTotal);
        }
    }
}
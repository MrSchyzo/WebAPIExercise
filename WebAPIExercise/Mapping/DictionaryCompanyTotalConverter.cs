using AutoMapper.Internal;
using System;
using System.Collections.Generic;

namespace WebAPIExercise.Mapping
{
    /// <summary>
    /// <inheritdoc cref="ICompanyTotalConverter"/>
    /// <para>Implements the conversion through an associative array of functions with the identity function as default</para>
    /// </summary>
    public class DictionaryCompanyTotalConverter : ICompanyTotalConverter
    {
        private static readonly Func<double, double> identity = x => x;

        private static readonly IDictionary<string, Func<double, double>> companyTable = new Dictionary<string, Func<double, double>>()
        {
            ["COMPANY_1"] = current => 1.0002 * current,
            ["COMPANY_2"] = current => 1 + current
        };

        /// <summary>
        /// <inheritdoc cref="ICompanyTotalConverter.ComputeTotalFor(string, double)"/>
        /// </summary>
        /// <param name="companyCode">Company code</param>
        /// <param name="currentTotal">Total to compute from</param>
        /// <returns>Definitive total amount</returns>
        public double ComputeTotalFor(string companyCode, double currentTotal)
        {
            Func<double, double> toApply = companyTable.GetOrDefault(companyCode) ?? identity;
            return toApply.Invoke(currentTotal);
        }
    }
}
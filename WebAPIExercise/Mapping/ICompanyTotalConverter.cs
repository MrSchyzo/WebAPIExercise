namespace WebAPIExercise.Mapping
{
    /// <summary>
    /// Converts the total cost
    /// </summary>
    public interface ICompanyTotalConverter
    {
        /// <summary>
        /// Computes the definitive amount of the order by using the provided company code
        /// </summary>
        /// <param name="companyCode">Company code</param>
        /// <param name="currentTotal">Total to compute from</param>
        /// <returns>Definitive total amount</returns>
        public double ComputeTotalFor(string companyCode, double currentTotal);
    }
}
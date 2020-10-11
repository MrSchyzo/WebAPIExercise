namespace WebAPIExercise.Mapping
{
    public interface ICompanyTotalConverter
    {
        public double ComputeTotalFor(string companyCode, double currentTotal);
    }
}
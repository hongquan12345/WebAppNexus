
namespace NexusApp.ModelDTOs
{
    public class DashboardViewModel
    {
        public Dictionary<string, decimal>? MonthlyData { get; set; }
        public Dictionary<string, decimal>? LastMonthData { get; set; }
        public Dictionary<string, decimal>? YearlyData { get; set; }
        public Dictionary<string, decimal>? LastYearData { get; set; }
        public decimal? YearDifference { get; set; }
        public decimal? MonthDifference { get; set; }
        public List<TransactionViewModel>? Transactions { get; set; }
        public class TransactionViewModel
        {
            public DateTime Time { get; set; }
            public string? Type { get; set; }
            public string? Description { get; set; }
            public string? ReferenceNumber { get; set; }
        }
    }
   
}

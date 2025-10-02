using CashFlow.Communication.Enums;

namespace CashFlow.Communication.Requests
{
    public class ReportRequest
    {
        public ReportType ReportType { get; set; }
        public DateOnly Month { get; set; }
    }
}

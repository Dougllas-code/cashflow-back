using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities
{
    public class ReportRequests
    {
        public Guid Id { get; set; }
        public ReportStatus Status { get; set; }
        public ReportType Type { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ConclusionDate { get; set; }
        public DateOnly Month { get; set; }

        public long UserId { get; set; }
        public User User { get; set; } = default!;
    }
}

using System;

namespace MaxInsight.Mobile.Domain
{
    public class CalenderPlans : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string SDate { get; set; }
        public string EDate { get; set; }
        public string Type { get; set; }
        public DateTime? InDateTime { get; set; }
        public int InUserId { get; set; }
        public int ModiUserId { get; set; }
        public DateTime? ModiDateTime { get; set; }
        public int DisId { get; set; }
        public int DepartId { get; set; }
    }
}

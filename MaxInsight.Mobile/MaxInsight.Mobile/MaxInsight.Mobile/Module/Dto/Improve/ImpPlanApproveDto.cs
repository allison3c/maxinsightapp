using System.Collections.Generic;

namespace MaxInsight.Mobile.Module.Dto.Improve
{
    public class ImpPlanApproveDto
    {
        public string ImprovementContent { get; set; }
        public string ExpectedTime { get; set; }
        public int ImprovementId { get; set; }
        public int InUserId { get; set; }
        public List<AttachDto> AttachList { get; set; }
        public string SaveStatus { get; set; }

    }
}

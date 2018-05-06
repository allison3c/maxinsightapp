using System.Collections.Generic;

namespace MaxInsight.Mobile.Module.Dto.Improve
{
    public class ImpResultApproveDto
    {
        public string ImprovementId { get; set; }
        public string ImpResultId { get; set; }
        public string ResultStatus { get; set; }
        public string ResultContent { get; set; }
        public string InUserId { get; set; }
        public List<AttachDto> AttachList { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Case
{
    public class CasesParamDto
    {
        public int Id { get; set; }
        public string CaseType { get; set; }
        public string CasePoint { get; set; }
        public string LossRemark { get; set; }
        public string ImproveRemark { get; set; }
        public int InUserId { get; set; }
        public string CaseTitle { get; set; }
        public List<AttachDto> AttachList { get; set; }
    }
}

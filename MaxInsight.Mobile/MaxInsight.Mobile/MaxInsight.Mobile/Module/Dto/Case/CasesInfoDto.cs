using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Case
{
    public class CasesInfoDto
    {
        public CasesInfoDto()
        {
            AttachList = new List<AttachDto>();
        }
        public int Id { get; set; }
        public string CaseNo { get; set; }
        public string CaseType { get; set; }
        public string CaseTypeName { get; set; }
        public string CaseTitle { get; set; }
        public string CasePoint { get; set; }
        public string LossRemark { get; set; }
        public string ImproveRemark { get; set; }
        public List<AttachDto> AttachList { get; set; }
    }
}

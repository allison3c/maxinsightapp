using MaxInsight.Mobile.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Case
{
    public class CasesListDto
    {
        public int Id { get; set; }
        public string CaseNo { get; set; }
        public string CaseType { get; set; }
        public string CaseTypeName { get; set; }
        public string CaseTitle { get; set; }
        public string CaseRegDate { get; set; }
        public string CaseRegUserName { get; set; }
    }
    public class NameValueObject: BaseEntity
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
}

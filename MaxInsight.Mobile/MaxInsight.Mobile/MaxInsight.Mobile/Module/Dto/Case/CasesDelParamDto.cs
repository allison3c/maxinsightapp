using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Case
{
    public class CasesDelParamDto
    {
        public int InUserId { get; set; }
        public List<IdParamDto> IdList { get; set; }
    }
    public class IdParamDto
    {
        public int Id { get; set; }
    }
}

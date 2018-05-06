using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Improve
{
   public class ImprovementParamDto
    {
        public int tPId { get; set; }
        public int itemId { get; set; }
        public int departmentId { get; set; }
        public string allocateYN { get; set; }
        public string improvementCaption { get; set; }
        public string lostDescription { get; set; }
        public int inUserId { get; set; }
    }
}

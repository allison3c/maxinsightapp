using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.ReviewPlans
{
   public  class ReviewPlansDto
    {
        public string Id { get; set; }
        public string PStatus { get; set; }
        public string RejectReason { get; set; }

        public string UserId { get; set; }
    }
}

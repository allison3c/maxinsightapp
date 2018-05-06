using MaxInsight.Mobile.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Shops
{
   public  class PlanDto: BaseEntity
    {
        public int PId { get; set; }
        public string PTitle { get; set; }

    }
}

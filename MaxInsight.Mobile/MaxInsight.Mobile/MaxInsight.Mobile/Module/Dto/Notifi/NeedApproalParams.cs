using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Notifi
{
    public class NeedApproalParams
    {
        public int NoticeReaderId { get; set; }
        public bool PassYN { get; set; }
        public string ReplyContent { get; set; }
        public int UserId { get; set; }
    }
}

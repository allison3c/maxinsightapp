using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Notifi
{
    public class NeedApprovalDto
    {
        public int NoticeReaderId { get; set; }
        public string NoticeNo { get; set; }
        public string Title { get; set; }
        public string ReaderName { get; set; }
        public string FeedBackDate { get; set; }
    }
}

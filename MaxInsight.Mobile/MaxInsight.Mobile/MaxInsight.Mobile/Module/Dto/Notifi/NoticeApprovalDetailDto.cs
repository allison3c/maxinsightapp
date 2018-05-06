using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Notifi
{
    public class NoticeApprovalDetailDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ReplyContent { get; set; }
        public string Status { get; set; }
        public string FeedbackContent { get; set; }
        public List<AttachDto> AttachList { get; set; }
        public int NoticeId { get; set; }
    }
}

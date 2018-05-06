using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Notifi
{
   public class NoticeFeedBackDtlDto
    {
        public string NoticeNo { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string ReplyContent { get; set; }
        public string NoticeReaderId { get; set; }
        public string FeedbackContent { get; set; }
        public string PassYN { get; set; }
        public string EditYN { get; set; }
        public List<AttachDto> AttachList { get; set; }
        public List<AttachDto> NoticeAttachList { get; set; }

    }
}

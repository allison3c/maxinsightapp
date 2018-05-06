using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Notifi
{
   public class FeedBackInfoDto
    {
        public int NoticeId { get; set; }
        public int UserId { get; set; }
        public string ReplyContent { get; set; }
        public string Status { get; set; }
        public List<AttachDto> list { get; set; }
    }
}

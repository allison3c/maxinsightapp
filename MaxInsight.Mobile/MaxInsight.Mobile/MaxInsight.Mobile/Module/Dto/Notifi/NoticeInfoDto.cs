using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Notifi
{
    //保存参数传递用
    public class NoticeInfoDto
    {
        public string Title { get; set; }//通知标题
        public string SDate { get; set; }//通知有效期开始日期
        public string EDate { get; set; }//通知有效期结束日期
        public string NeedReply { get; set; }//是否反馈
        public string Content { get; set; }//通知内容
        public string Status { get; set; }//状态
        public int InUserId { get; set; }
        public int NoticeId { get; set; }
        public string NoticeReaders { get; set; }
        public List<AttachDto> AttachList { get; set; }
    }
}

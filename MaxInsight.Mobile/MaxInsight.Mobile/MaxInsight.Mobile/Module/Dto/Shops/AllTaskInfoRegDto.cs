using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxInsight.Mobile.Module.Dto.Shops
{
    public class AllTaskInfoRegDto
    {
        public string TPType { get; set; }
        public CustomizedTaskDto CustomizedTask { get; set; }
        public ScoreCheckResultParam ScoreCheckResult { get; set; }
    }
    public class AllTaskInfoRegLstDto
    {
        public List<AllTaskInfoRegDto> TaskInfoRegLstDto { get; set; }
    }
}

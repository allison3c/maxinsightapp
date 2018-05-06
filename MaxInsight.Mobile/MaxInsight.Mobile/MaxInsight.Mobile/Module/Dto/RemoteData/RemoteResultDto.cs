using MaxInsight.Mobile.Domain;
using System.Collections.Generic;

namespace MaxInsight.Mobile.Module.Dto.RemoteData
{
    public class RemoteResultDto
    {
        public RemoteResultDto()
        {
            PlansLst = new List<Plans>();
            DistributorLst = new List<Distributor>();
            EmployeeLst = new List<Employee>();
            TaskOfPlanLst = new List<TaskOfPlan>();
            TaskCardLst = new List<TaskCard>();
            TaskItemLst = new List<TaskItem>();
            ScoreLst = new List<Score>();
            CheckStandardLst = new List<Domain.CheckStandard>();
            CheckResultLst = new List<CheckResult>();
            StandardPicLst = new List<Domain.StandardPic>();
            PictureStandardLst = new List<Domain.PictureStandard>();
            HiddenCodeLst = new List<CodeHidden>();
        }
        public List<Plans> PlansLst { get; set; }
        public List<Distributor> DistributorLst { get; set; }
        public List<Employee> EmployeeLst { get; set; }
        public List<TaskOfPlan> TaskOfPlanLst { get; set; }
        public List<TaskCard> TaskCardLst { get; set; }
        public List<TaskItem> TaskItemLst { get; set; }
        public List<Score> ScoreLst { get; set; }
        public List<Domain.CheckStandard> CheckStandardLst { get; set; }
        public List<CheckResult> CheckResultLst { get; set; }
        public List<Domain.StandardPic> StandardPicLst { get; set; }
        public List<Domain.PictureStandard> PictureStandardLst { get; set; }
        public List<CodeHidden> HiddenCodeLst { get; set; }
    }
}

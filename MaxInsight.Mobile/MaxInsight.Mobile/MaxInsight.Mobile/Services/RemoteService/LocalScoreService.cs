using MaxInsight.Mobile.Data;
using MaxInsight.Mobile.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLabs.Ioc;
using System;
using MaxInsight.Mobile.Module.Dto.Shops;
using MaxInsight.Mobile.Module.Dto.Case;

namespace MaxInsight.Mobile.Services.RemoteService
{
    public interface ILocalScoreService
    {
        Task<List<Distributor>> SearchDistributors();
        Task<List<TaskOfPlanDto>> GetTaskOfPlans(int disId);
        Task<List<LoaclItemOfTaskDto>> SearchTaskItem(string taskId, string operation);
        Task<List<CheckStandard>> SearchCheckStandard(string tpId, string TIId);
        Task<List<StandardPic>> SearchStandardPic(string tpId, string TIId);
        Task<List<PictureStandard>> SearchPictureStandard(string tpId, string TIId);
        bool CheckLocalModiYN();
        void SaveDataToLocal(Score score, List<CheckResult> SCLst, List<Domain.StandardPic> StandardPicLst, List<Domain.StandardPic> PictureStandardLst);
        Task<List<CustomizedTaskDto>> LocalGetCustomizedTaskByTaskId(string taskId, string operation);
        int LocalCustomizedTaskCheck(CustomizedTaskDto dto);
        bool CheckTaskStatus(int disId);

        //自定义改善
        Task<List<PlanDto>> SearchPlanList(int disId);
        int SaveCustImprove(TaskCard tc, TaskItem ti, TaskOfPlan tp, Score score);
        Task<List<NameValueObject>> LocalGetTypeFromHiddenCode(string groupCode);
    }
    public class LocalScoreService : ILocalScoreService
    {
        private readonly IRepository<Distributor> _distributorRepository;
        private readonly IRepository<TaskOfPlanDto> _taskOfPlanDtoRepository;
        private readonly IRepository<LoaclItemOfTaskDto> _taskitemRepository;
        private readonly IRepository<CheckStandard> _checkstandardRepository;
        private readonly IRepository<StandardPic> _standardpicRepository;
        private readonly IRepository<PictureStandard> _picturestandardRepository;
        private readonly IRepository<Score> _scoreRepository;
        private readonly IRepository<Score> _scoreRegRepository;
        private readonly IRepository<TaskOfPlan> _taskOfPlanRepository;
        private readonly IRepository<CustomizedTaskDto> _customizedTaskDtoRepository;
        private readonly IRepository<PlanDto> _plansRepository;
        private readonly IRepository<NameValueObject> _typeRepository;
        public LocalScoreService()
        {
            _distributorRepository = Resolver.Resolve<IRepository<Distributor>>();
            _taskOfPlanDtoRepository = Resolver.Resolve<IRepository<TaskOfPlanDto>>();
            _taskitemRepository = Resolver.Resolve<IRepository<LoaclItemOfTaskDto>>();
            _checkstandardRepository = Resolver.Resolve<IRepository<CheckStandard>>();
            _standardpicRepository = Resolver.Resolve<IRepository<StandardPic>>();
            _picturestandardRepository = Resolver.Resolve<IRepository<PictureStandard>>();
            _scoreRepository = Resolver.Resolve<IRepository<Score>>();
            _scoreRegRepository = Resolver.Resolve<IRepository<Score>>();
            _taskOfPlanRepository = Resolver.Resolve<IRepository<TaskOfPlan>>();
            _customizedTaskDtoRepository = Resolver.Resolve<IRepository<CustomizedTaskDto>>();
            _plansRepository = Resolver.Resolve<IRepository<PlanDto>>();
            _typeRepository = Resolver.Resolve<IRepository<NameValueObject>>();
        }

        public Task<List<Distributor>> SearchDistributors()
        {
            List<string> parameters = new List<string>();
            string sql = @"SELECT * FROM Distributor";
            return _distributorRepository.QueryListForTaskAsync(sql, parameters.ToArray());
        }

        public Task<List<TaskOfPlanDto>> GetTaskOfPlans(int disId)
        {
            List<string> parameters = new List<string>();
            parameters.Add(disId.ToString());

            /*string sql = @"SELECT TP.Id AS TPId
                            ,IFNULL(TC.TCCode,TP.TPCode) AS TCCode
                            ,IFNULL(TC.TCTitle,Tp.TPTitle) AS TCTitle
                            ,TP.[Status] AS TPStatus
                            ,CASE WHEN TP.TCId =0 THEN 'C' ELSE 'S' END AS TPType
                            ,CH.Name AS SourceType
                            FROM  Plans P
                            JOIN TaskOfPlan TP
                            ON P.Id=  TP.PId
                            LEFT JOIN TaskCard TC 
                            ON TP.TCId = TC.Id
							LEFT JOIN CodeHidden CH
							ON TC.SourceType = CH.Code
							AND CH.GroupCode ='15'
                            WHERE P.DistributorId =?
                            AND P.PStatus = 'P'
                            AND IFNULL(TP.[Status],'S') = 'S'
                            AND IFNULL(TC.SourceType,'D') ='D'
                            ORDER BY TP.GRUD DESC,P.ModiDateTime DESC,TC.SourceType";*/

            string sql = @"SELECT TP.Id AS TPId
                            ,IFNULL(TC.TCCode,TP.TPCode) AS TCCode
                            ,P.Title AS PTitle
                            ,IFNULL(TC.TCTitle,Tp.TPTitle) AS TCTitle
                            ,TP.[Status] AS TPStatus
                            ,CASE WHEN TP.TCId =0 THEN 'C' ELSE 'S' END AS TPType
                            ,CH.Name AS SourceType
							,E.UserName
							,P.InUserId 
                            FROM  Plans P
                            JOIN TaskOfPlan TP
                            ON P.Id=  TP.PId
                            LEFT JOIN TaskCard TC 
                            ON TP.TCId = TC.Id
							LEFT JOIN CodeHidden CH
							ON TC.SourceType = CH.Code
							AND CH.GroupCode ='15'
							JOIN Employee E
							ON P.InUserId = E.UserId
                            WHERE P.DistributorId =?
                            AND P.PStatus = 'P'
                            AND IFNULL(TP.[Status],'S') <> 'C'
                            AND IFNULL(TC.SourceType,'D') ='D'
							AND P.InUserId  in (SELECT userid FROM Employee)
                            ORDER BY TP.GRUD DESC,P.ModiDateTime DESC,TC.SourceType";
            return _taskOfPlanDtoRepository.QueryListForTaskAsync(sql, parameters.ToArray());
        }
        public bool CheckLocalModiYN()
        {
            List<string> parameters = new List<string>();
            string sql = @"SELECT * FROM SCORE WHERE IFNULL(GRUD,'') <> ''";
            Score score = _scoreRepository.GetAsyncConnection().QueryAsync<Score>(sql, parameters.ToArray()).Result.FirstOrDefault();

            string sql2 = @"SELECT * FROM TaskOfPlan WHERE IFNULL(GRUD,'') <> ''";
            TaskOfPlan taskOfPlan = _taskOfPlanRepository.GetAsyncConnection().QueryAsync<TaskOfPlan>(sql2, parameters.ToArray()).Result.FirstOrDefault();

            if (null != score || taskOfPlan != null)
            {
                return true;
            }
            return false;
        }
        public Task<List<LoaclItemOfTaskDto>> SearchTaskItem(string taskId, string operation)
        {
            List<string> parameters2 = new List<string>();
            parameters2.Add(taskId.ToString());
            var conn = Resolver.Resolve<ISQLite>().GetConnection();
            string sql2 = @"select * from taskOfPlan where Id =?";
            var taskOfPlan = _taskOfPlanRepository.QueryDtoForTask(sql2, parameters2.ToArray());
            if (operation == "S" && (taskOfPlan.Status == "" || taskOfPlan.Status == null))
            {
                taskOfPlan.SDateTime = DateTime.Now;
                taskOfPlan.Status = "S";
                taskOfPlan.GRUD = "I";
            }
            else if (operation == "C")
            {
                if (taskOfPlan.GRUD == "N")
                {
                    taskOfPlan.Status = "C";
                }
                else
                {
                    taskOfPlan.Status = "C";
                    taskOfPlan.GRUD = "I";
                }
            }
            else if (operation == "E")
            {
                List<string> parameters3 = new List<string>();
                parameters3.Add(taskId.ToString());
                string sql3 = @"SELECT S.PassYN
	                           FROM TaskOfPlan TP
	                           JOIN TaskCard TC
	                           ON TP.TCId = TC.Id
	                           JOIN TaskItem TI
	                           ON TC.Id = TI.TCId
	                           LEFT JOIN Score S
	                           ON  TP.Id = S.TPId
	                           AND S.ItemId = TI.Id
	                           WHERE TP.Id = ?";
                var storeLst = _scoreRepository.QueryListForTask(sql3, parameters3.ToArray());

                if ((from a in storeLst where a.PassYN == "" || a.PassYN == null select a).Count() > 0)
                {
                    throw new Exception("存在没有打分的体系，不能结束");
                }
                taskOfPlan.EDateTime = DateTime.Now;
                taskOfPlan.Status = "E";
                if (taskOfPlan.GRUD == "N")
                {

                }
                else
                {
                    taskOfPlan.GRUD = "I";
                }

            }
            conn.InsertOrReplace(taskOfPlan);

            List<string> parameters = new List<string>();
            string sql = @"SELECT TI.Title,TI.SeqNo,TI.Id AS TIId,CASE WHEN LENGTH(TI.Id)=36 THEN 1 ELSE TI.Id end AS  LCTIId,IFNULL(S.PlanApproalYN,0) AS PlanApproalYN,
                           IFNULL(S.ResultApproalYN,1) AS ResultApproalYN,
                           S.PassYN,
                           IFNULL(S.Scoreval,1) AS Score,
                           CH.Name AS ExeMode,TI.ScoreStandard,S.Remarks,
                           CASE WHEN ?='A' THEN S.PlanFinishDate ELSE IFNULL(S.PlanFinishDate,datetime('now','start of day','+7 day')) END AS PlanFinishDate,
                           CASE WHEN ?='A' THEN S.ResultFinishDate ELSE IFNULL(S.ResultFinishDate,datetime('now','start of day','+7 day')) END AS ResultFinishDate,
                           TP.Id AS TPId,
                           S.Id AS StrScoreId
                           FROM TaskItem  TI
                           JOIN TaskOfPlan TP ON TP.TCId=TI.Tcid
                           LEFT JOIN Score S ON S.ItemId=TI.Id AND S.TPId=TP.Id
                           LEFT JOIN CodeHidden CH
                           ON CH.Code = TI.ExeMode
                           AND CH.GroupCode =13
                           WHERE TP.Id=?";
            parameters.Add(operation);
            parameters.Add(operation);
            parameters.Add(taskId.ToString());
            return _taskitemRepository.QueryListForTaskAsync(sql, parameters.ToArray());
        }
        public Task<List<CheckStandard>> SearchCheckStandard(string tpId, string TIId)
        {
            List<string> parameters = new List<string>();
            string sql = @"SELECT TP.Id AS TPId,TI.Id AS TIId,TI.SeqNo,CS.Id AS CSID,CS.CContent,IFNULL(CR.Result,0)AS IsCheck
                            ,CR.Id AS StrCRId
                            FROM CheckStandard CS
                            JOIN TaskItem TI ON CS.TIId = TI.Id
                            JOIN TaskOfPlan TP ON TP.Id=?
                            LEFT JOIN CheckResult CR ON CR.TIId = CS.TIId AND CR.CSId = CS.Id AND CR.TPId =TP.Id
                            WHERE CS.TIId=?";
            parameters.Add(tpId.ToString());
            parameters.Add(TIId.ToString());
            return _checkstandardRepository.QueryListForTaskAsync(sql, parameters.ToArray());
        }
        public Task<List<StandardPic>> SearchStandardPic(string tpId, string TIId)
        {
            List<string> parameters = new List<string>();
            string sql = @"SELECT SP.Id AS StrPicId,TP.Id AS TPId,TI.Id AS TIId,TI.SeqNo
	                       ,CASE WHEN PicName ='' THEN 'default' ELSE PicName END AS PicName
	                       ,Url, SP.[Type] AS PicType
	                        FROM StandardPic SP
                              JOIN TaskItem TI ON SP.TIId = TI.Id
                              JOIN TaskOfPlan TP ON TP.Id=?
	                          WHERE SP.[Type] = 'L'
                              AND IFNULL(SP.DelChk,'0') <> '1'
                              AND SP.TIId=? AND SP.TPId=?";
            parameters.Add(tpId.ToString());
            parameters.Add(TIId.ToString());
            parameters.Add(tpId.ToString());
            return _standardpicRepository.QueryListForTaskAsync(sql, parameters.ToArray());
        }
        public Task<List<PictureStandard>> SearchPictureStandard(string tpId, string TIId)
        {
            List<string> parameters = new List<string>();
            string sql = @"SELECT PS.Id AS StandardPicId,TP.Id AS TPId,TI.Id AS TIId,TI.SeqNo,PS.StandardPicName
	                       ,SP.Url
                           ,SP.Id AS StrPicId
		                   ,CASE WHEN IFNULL(SP.Url,'')!='' THEN  'icon_success' ELSE '' END  AS SuccessImage
		                    FROM PictureStandard PS
		                    JOIN TaskItem TI ON PS.TIId = TI.Id
		                    JOIN TaskOfPlan TP ON TP.Id=?
		                    LEFT JOIN StandardPic SP
		                      ON SP.PSId = PS.Id
		                      AND SP.TIId = PS.TIId
		                      AND SP.TPId = TP.Id 
		                    WHERE TI.Id=?";
            parameters.Add(tpId.ToString());
            parameters.Add(TIId.ToString());
            return _picturestandardRepository.QueryListForTaskAsync(sql, parameters.ToArray());
        }

        public void SaveDataToLocal(Score score, List<CheckResult> SCLst, List<Domain.StandardPic> StandardPicLst, List<Domain.StandardPic> PictureStandardLst)
        {
            var conn = Resolver.Resolve<ISQLite>().GetConnection();

            List<string> parameters = new List<string>();
            string sql3 = @"select * from score 
                           where itemId =? 
                           and TPid = ?";
            parameters.Add(score.ItemId.ToString());
            parameters.Add(score.TPId.ToString());
            var list3 = _scoreRepository.QueryListForTask(sql3, parameters.ToArray());
            foreach (var item in list3)
            {
                _scoreRepository.Delete(item);
            }
            conn.InsertOrReplace(score);
            conn.InsertOrReplaceAll(SCLst, typeof(CheckResult));

            string sql = @"Update StandardPic
                         SET DelChk ='1'
                         ,GRUD ='I'
                         WHERE TIId =?
                         AND TPId =?
                         AND IFNULL(PSId,0) = 0";

            var list = _standardpicRepository.Update(sql, parameters.ToArray());

            conn.InsertOrReplaceAll(StandardPicLst, typeof(Domain.StandardPic));

            string sql2 = @"SELECT *  
                         FROM StandardPic S
                         WHERE TIId =?
                         AND TPId =?
                         AND PSId >0";
            var list2 = _standardpicRepository.QueryListForTask(sql2, parameters.ToArray());
            foreach (var item in list2)
            {
                _standardpicRepository.Delete(item);
            }
            conn.InsertOrReplaceAll(PictureStandardLst, typeof(Domain.StandardPic));
        }

        public Task<List<CustomizedTaskDto>> LocalGetCustomizedTaskByTaskId(string taskId, string operation)
        {
            List<string> parameters = new List<string>();
            parameters.Add(taskId.ToString());

            string sql2 = @"select * from taskOfPlan where Id =?";
            var taskOfPlan = _taskOfPlanRepository.QueryDtoForTask(sql2, parameters.ToArray());
            if (operation == "S" && (taskOfPlan.Status == "" || taskOfPlan.Status == null))
            {
                taskOfPlan.SDateTime = DateTime.Now;
                taskOfPlan.Status = "S";
                taskOfPlan.GRUD = "I";
            }
            else if (operation == "C")
            {
                taskOfPlan.Status = "C";
                taskOfPlan.GRUD = "I";
            }
            else if (operation == "E" && taskOfPlan.Status == "S")
            {
                string sqls = @"SELECT 1 FROM SCORE WHERE TPID = ? AND ITEMID = 0";
                var scoreLst = _scoreRepository.QueryListForTask(sqls, parameters.ToArray());
                if (scoreLst.Count == 0)
                {
                    throw new Exception("没有输入检查内容,不能结束");
                }

                taskOfPlan.EDateTime = DateTime.Now;
                taskOfPlan.Status = "E";
                taskOfPlan.GRUD = "I";
            }
            var conn = Resolver.Resolve<ISQLite>().GetConnection();
            conn.InsertOrReplace(taskOfPlan);

            string sql = @"SELECT TP.Id AS TPId,TP.TCId,IFNULL(S.Id,0) AS ScoreId,S.Remarks 
		                        FROM TaskOfPlan TP
		                        LEFT JOIN Score S
		                        ON TP.Id = S.TPId
		                        AND TP.TCId = 0
		                        WHERE TP.Id = ?";

            return _customizedTaskDtoRepository.QueryListForTaskAsync(sql, parameters.ToArray());

        }

        public int LocalCustomizedTaskCheck(CustomizedTaskDto dto)
        {
            var conn = Resolver.Resolve<ISQLite>().GetConnection();
            List<string> parameters = new List<string>();
            string sql = @"select * from score
                            where TPId =?
                            AND ItemId =?";
            parameters.Add(dto.TPId.ToString());
            parameters.Add(dto.TCId.ToString());
            var list = _scoreRepository.QueryListForTask(sql, parameters.ToArray());
            foreach (var item in list)
            {
                _scoreRepository.Delete(item);
            }
            Score score = new Score()
            {
                TPId = dto.TPId.ToString(),
                ItemId = "0",
                Remarks = dto.Remarks,
                InUserId = int.Parse(CommonContext.Account.UserId),
                InDateTime = DateTime.Now,
                Id = Guid.NewGuid().ToString(),
                GRUD = "I"
            };
            return conn.InsertOrReplace(score);
        }

        public Task<List<PlanDto>> SearchPlanList(int disId)
        {
            List<string> parameters = new List<string>();
            parameters.Add(disId.ToString());

            string sql = @"SELECT  P.Id AS PId,P.Title AS PTitle FROM Plans P
                                                     WHERE DistributorId =?
                                                     AND PStatus ='P'
                                                     AND P.InUserId  in (SELECT userid FROM Employee)";
            //AND ((SELECT COUNT(1) FROM  TaskOfPlan WHERE P. Id = PId )> (SELECT COUNT(1) FROM  TaskOfPlan WHERE P. Id = PId AND [Status] IN('E','C')))";
            var planLst = _plansRepository.QueryListForTaskAsync(sql, parameters.ToArray());
            return planLst;
        }

        public int SaveCustImprove(TaskCard tc, TaskItem ti, TaskOfPlan tp, Score score)
        {
            var conn = Resolver.Resolve<ISQLite>().GetConnection();

            conn.InsertOrReplace(tc);
            conn.InsertOrReplace(ti);
            conn.InsertOrReplace(tp);
            return conn.InsertOrReplace(score);
        }

        public Task<List<NameValueObject>> LocalGetTypeFromHiddenCode(string groupCode)
        {
            List<string> parameters = new List<string>();
            parameters.Add(groupCode.ToString());

            string sql = @"SELECT Code AS Value
                                                   ,Name FROM CodeHidden
                                                 WHERE GroupCode=?
                                                 ORDER BY DisSeq";
            var typeList = _typeRepository.QueryListForTaskAsync(sql, parameters.ToArray());
            return typeList;
        }

        public bool CheckTaskStatus(int disId)
        {
            List<string> parameters = new List<string>();
            parameters.Add(disId.ToString());

            string sql = @"SELECT TP.Id AS TPId
                            ,IFNULL(TC.TCCode,TP.TPCode) AS TCCode
                            ,P.Title AS PTitle
                            ,IFNULL(TC.TCTitle,Tp.TPTitle) AS TCTitle
                            ,TP.[Status] AS TPStatus
                            ,CASE WHEN TP.TCId =0 THEN 'C' ELSE 'S' END AS TPType
                            ,CH.Name AS SourceType
							,E.UserName
							,P.InUserId 
                            FROM  Plans P
                            JOIN TaskOfPlan TP
                            ON P.Id=  TP.PId
                            LEFT JOIN TaskCard TC 
                            ON TP.TCId = TC.Id
							LEFT JOIN CodeHidden CH
							ON TC.SourceType = CH.Code
							AND CH.GroupCode ='15'
							JOIN Employee E
							ON P.InUserId = E.UserId
                            WHERE P.DistributorId =?
                            AND P.PStatus = 'P'
                            AND IFNULL(TP.[Status],'S') = 'S'
                            AND IFNULL(TC.SourceType,'D') ='D'
							AND P.InUserId  in (SELECT userid FROM Employee)";

            List<TaskOfPlanDto> taskOfPlan = _taskOfPlanDtoRepository.GetConnection().Query<TaskOfPlanDto>(sql, parameters.ToArray());

            string sql2 = @"SELECT TP.Id AS TPId
                            ,IFNULL(TC.TCCode,TP.TPCode) AS TCCode
                            ,P.Title AS PTitle
                            ,IFNULL(TC.TCTitle,Tp.TPTitle) AS TCTitle
                            ,TP.[Status] AS TPStatus
                            ,CASE WHEN TP.TCId =0 THEN 'C' ELSE 'S' END AS TPType
                            ,CH.Name AS SourceType
							,E.UserName
							,P.InUserId 
                            FROM  Plans P
                            JOIN TaskOfPlan TP
                            ON P.Id=  TP.PId
                            LEFT JOIN TaskCard TC 
                            ON TP.TCId = TC.Id
							LEFT JOIN CodeHidden CH
							ON TC.SourceType = CH.Code
							AND CH.GroupCode ='15'
							JOIN Employee E
							ON P.InUserId = E.UserId
                            WHERE P.DistributorId =?
                            AND P.PStatus = 'P'
                            AND IFNULL(TP.[Status],'S') = 'S'
                            AND IFNULL(TC.SourceType,'D') ='D'
                            AND (TP.Status ='E' OR TP.Status ='C')
							AND P.InUserId  in (SELECT userid FROM Employee)";

            List<TaskOfPlanDto> taskOfPlan2 = _taskOfPlanDtoRepository.GetConnection().Query<TaskOfPlanDto>(sql2, parameters.ToArray());

            if (taskOfPlan != null && taskOfPlan2 != null && taskOfPlan.Count == taskOfPlan2.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

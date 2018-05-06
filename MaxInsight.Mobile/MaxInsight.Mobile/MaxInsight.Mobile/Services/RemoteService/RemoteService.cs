using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module;
using System.Collections.Generic;
using System.Threading.Tasks;
using XLabs.Ioc;
using System;
using MaxInsight.Mobile.Data;
using MaxInsight.Mobile.Domain;
using MaxInsight.Mobile.Module.Dto.Shops;

namespace MaxInsight.Mobile.Services.RemoteService
{
    public interface IRemoteService
    {
        Task<APIResult> GetAllDataForLocalDB(string inUserId);
        Task<APIResult> UploadScoreInfo(int TPId);
    }
    public class RemoteService : IRemoteService
    {
        CommonHelper _commonHelper;
        Config.Config _config;
        string baseUrl = "";

        private readonly IRepository<Score> _scoreRegRepository;
        private readonly IRepository<Domain.StandardPic> _standardpicRepository;
        private readonly IRepository<CheckResult> _checkResultRepository;
        private readonly IRepository<TaskOfPlan> _taskOfPlanRepository;
        private readonly IRepository<CustImproveItemDB> _custImproveRepository;
        private readonly IRepository<TaskCard> _taskCardRepository;
        private readonly IRepository<TaskItem> _taskItemRepository;
        public RemoteService(Config.Config config)
        {
            _commonHelper = Resolver.Resolve<CommonHelper>();
            _config = config;
            baseUrl = _config.Get<string>(Config.Config.Endpoints_BaseUrl);

            _scoreRegRepository = Resolver.Resolve<IRepository<Score>>();
            _standardpicRepository = Resolver.Resolve<IRepository<Domain.StandardPic>>();
            _checkResultRepository = Resolver.Resolve<IRepository<CheckResult>>();
            _taskOfPlanRepository = Resolver.Resolve<IRepository<TaskOfPlan>>();
            _custImproveRepository = Resolver.Resolve<IRepository<CustImproveItemDB>>();
            _taskCardRepository = Resolver.Resolve<IRepository<TaskCard>>();
            _taskItemRepository = Resolver.Resolve<IRepository<TaskItem>>();
        }
        public async Task<APIResult> GetAllDataForLocalDB(string inUserId)
        {
            List<RequestParameter> param = new List<RequestParameter> {
                new RequestParameter { Name = "UserId", Value = inUserId }
            };
            APIResult info = await _commonHelper.HttpGet<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.GetAllDataForLocalDB"), param);
            return info;
        }

        //public async Task<APIResult> UploadScoreInfo(int TPId)
        //{

        //    LocalDBUploadParams scrorDto = new LocalDBUploadParams();
        //    scrorDto.Score = new List<ScoreLDB>();
        //    scrorDto.CheckResult = new List<CheckResultLDB>();
        //    scrorDto.StandardPic = new List<StandardPicLDB>();

        //    //得分
        //    List<string> parameters = new List<string>();
        //    parameters.Add(TPId.ToString());
        //    string sql = @"SELECT *
        //                 FROM score 
        //                WHERE TPid =?
        //                AND GRUD ='I'";

        //    List<Score> storeLst = await _scoreRegRepository.QueryListForTaskAsync(sql, parameters.ToArray());
        //    foreach (var item in storeLst)
        //    {
        //        ScoreLDB scoreLDB = new ScoreLDB();
        //        scoreLDB.TPId = item.TPId;
        //        scoreLDB.ItemId = item.ItemId;
        //        scoreLDB.Scoreval = item.Scoreval;
        //        scoreLDB.PlanApproalYN = item.PlanApproalYN;
        //        scoreLDB.PlanFinishDate = item.PlanFinishDate;
        //        scoreLDB.ResultApproalYN = item.ResultApproalYN;
        //        scoreLDB.ResultFinishDate = item.ResultFinishDate;
        //        scoreLDB.PassYN = item.PassYN;
        //        scoreLDB.Remarks = item.Remarks;
        //        scoreLDB.InUserId = item.InUserId;
        //        scoreLDB.InDateTime = item.InDateTime;
        //        scrorDto.Score.Add(scoreLDB);

        //    }

        //    //CheckResult
        //    string sql2 = @"Select * From checkResult
        //                    where TPId = ?
        //                    and GRUD ='I'";

        //    List<CheckResult> checkResultLst = await _checkResultRepository.QueryListForTaskAsync(sql2, parameters.ToArray());
        //    foreach (var item2 in checkResultLst)
        //    {
        //        CheckResultLDB checkResultLDB = new CheckResultLDB();
        //        checkResultLDB.TPId = item2.TPId;
        //        checkResultLDB.TIId = item2.TIId;
        //        checkResultLDB.CSId = item2.CSId;
        //        checkResultLDB.Result = item2.Result;
        //        scrorDto.CheckResult.Add(checkResultLDB);
        //    }
        //    //图片
        //    string sql3 = @"Select * From StandardPic
        //                    where TPId = ?
        //                    and GRUD ='I'";

        //    List<Domain.StandardPic> standardPicLst = await _standardpicRepository.QueryListForTaskAsync(sql3, parameters.ToArray());
        //    foreach (var item3 in standardPicLst)
        //    {
        //        StandardPicLDB standardPicLDB = new StandardPicLDB();
        //        standardPicLDB.TPId = item3.TPId;
        //        standardPicLDB.TIId = item3.TIId;
        //        standardPicLDB.PSId = item3.PSId;
        //        standardPicLDB.Url = item3.Url;
        //        standardPicLDB.Type = item3.Type;
        //        standardPicLDB.PicName = item3.PicName;
        //        scrorDto.StandardPic.Add(standardPicLDB);
        //    }

        //    var info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.UploadLocalDB"), scrorDto);

        //    return info;
        //}

        public async Task<APIResult> UploadScoreInfo(int TPId)
        {
            LocalDBUploadParams scrorDto = new LocalDBUploadParams();
            scrorDto.Score = new List<ScoreLDB>();
            scrorDto.CheckResult = new List<CheckResultLDB>();
            scrorDto.StandardPic = new List<StandardPicLDB>();
            scrorDto.TaskOfPlan = new List<TaskOfPlanLDB>();
            scrorDto.CustImproveItem = new List<CustImproveItemDB>();

            string sql = "";
            string sql2 = "";
            string sql3 = "";
            string sql4 = "";
            string sql5 = "";
            string sql6 = "";
            string sql7 = "";
            string sql8 = "";
            string sql9 = @"delete from StandardPic where Delchk ='1'";
            string sql10 = @"SELECT P.Id AS PId,TC.TCTitle,TC.TCDescription,TC.UseYN,TC.SourceType,TC.TCKind,TI.SeqNo,TI.Title AS TiTitle,TI.ScoreStandard,TP.Status,TP.TPTitle
                                            ,S.Scoreval,S.PlanApproalYN,S.PlanFinishDate,S.ResultApproalYN,S.ResultFinishDate,TP.InUserId
                                            ,S.PassYN,S.Remarks
                                             FROM Plans P
                                            JOIN TaskOfPlan TP
                                            ON P.Id = TP.PId
                                            JOIN TaskCard TC
                                            ON TP.TCId = TC.Id
                                            JOIN TaskItem TI
                                            ON TI.TCId = TC.Id
                                            JOIN Score S
                                            ON S.TPId = TP.Id
                                            AND S.ItemId = TI.Id
                                            WHERE TP.GRUD ='N'
                                            AND TC.GRUD ='N'
                                            AND TI.GRUD ='N'
                                            AND S.GRUD ='N'";
            string sql11 = @"update TaskCard set GRUD = ''";
            string sql12 = @"update TaskItem set GRUD = ''";

            List<string> parameters = new List<string>();
            if (TPId != 0)
            {
                sql = @"SELECT *
                         FROM score 
                        WHERE TPid =?
                        AND GRUD ='I'";
                sql2 = @"Select * From checkResult
                            where TPId = ?
                            and GRUD ='I'";
                sql3 = @"Select * From StandardPic
                            where TPId = ?
                            and GRUD ='I'";
                sql7 = @"select * from taskofPlan where id=? and GRUD ='I'";

                sql4 = @"update score set GRUD = '' where TPid =?";
                sql5 = @"update checkResult set GRUD = '' where TPid =?";
                sql6 = @"update StandardPic set GRUD = '' where TPid =?";
                sql8 = @"update taskofPlan set GRUD = '' where Id =?";

                parameters.Add(TPId.ToString());
            }
            else if (TPId == 0)
            {
                sql = @"SELECT *
                         FROM score 
                        WHERE GRUD ='I'";
                sql2 = @"Select * From checkResult
                            where GRUD ='I'";
                sql3 = @"Select * From StandardPic
                            where GRUD ='I'";
                sql7 = @"select * from taskofPlan where GRUD ='I'";

                sql4 = @"update score set GRUD = ''";
                sql5 = @"update checkResult set GRUD = ''";
                sql6 = @"update StandardPic set GRUD = ''";
                sql8 = @"update taskofPlan set GRUD = ''";
            }


            List<Score> storeLst = await _scoreRegRepository.QueryListForTaskAsync(sql, parameters.ToArray());
            foreach (var item in storeLst)
            {
                ScoreLDB scoreLDB = new ScoreLDB();
                scoreLDB.TPId = item.TPId;
                scoreLDB.ItemId = item.ItemId;
                scoreLDB.Scoreval = item.Scoreval;
                scoreLDB.PlanApproalYN = item.PlanApproalYN;
                scoreLDB.PlanFinishDate = item.PlanFinishDate;
                scoreLDB.ResultApproalYN = item.ResultApproalYN;
                scoreLDB.ResultFinishDate = item.ResultFinishDate;
                scoreLDB.PassYN = item.PassYN;
                scoreLDB.Remarks = item.Remarks;
                scoreLDB.InUserId = item.InUserId;
                scoreLDB.InDateTime = item.InDateTime;
                scrorDto.Score.Add(scoreLDB);

            }
            List<CheckResult> checkResultLst = await _checkResultRepository.QueryListForTaskAsync(sql2, parameters.ToArray());
            foreach (var item2 in checkResultLst)
            {
                CheckResultLDB checkResultLDB = new CheckResultLDB();
                checkResultLDB.TPId = item2.TPId;
                checkResultLDB.TIId = item2.TIId;
                checkResultLDB.CSId = item2.CSId;
                checkResultLDB.Result = item2.Result;
                scrorDto.CheckResult.Add(checkResultLDB);
            }

            List<Domain.StandardPic> standardPicLst = await _standardpicRepository.QueryListForTaskAsync(sql3, parameters.ToArray());
            foreach (var item3 in standardPicLst)
            {
                StandardPicLDB standardPicLDB = new StandardPicLDB();
                standardPicLDB.TPId = item3.TPId;
                standardPicLDB.TIId = item3.TIId;
                standardPicLDB.PSId = item3.PSId;
                standardPicLDB.Url = item3.Url;
                standardPicLDB.Type = item3.Type;
                standardPicLDB.PicName = item3.PicName;
                standardPicLDB.DelChk = item3.DelChk;
                standardPicLDB.Id = item3.Id;
                scrorDto.StandardPic.Add(standardPicLDB);
            }
            List<Domain.TaskOfPlan> taskOfPlanLst = await _taskOfPlanRepository.QueryListForTaskAsync(sql7, parameters.ToArray());
            foreach (var item4 in taskOfPlanLst)
            {
                TaskOfPlanLDB taskOfPlanLDB = new TaskOfPlanLDB();
                taskOfPlanLDB.Id = Convert.ToInt32(item4.Id);
                taskOfPlanLDB.Status = item4.Status;
                taskOfPlanLDB.SDateTime = item4.SDateTime;
                taskOfPlanLDB.EDateTime = item4.EDateTime;
                scrorDto.TaskOfPlan.Add(taskOfPlanLDB);
            }
            var custImproveItemList = await _custImproveRepository.QueryListForTaskAsync(sql10, parameters.ToArray());
            scrorDto.CustImproveItem.AddRange(custImproveItemList);

            var info = await _commonHelper.HttpPOST<APIResult>(baseUrl, _config.Get<string>($"{Config.Config.Endpoints_Paths}.UploadLocalDB"), scrorDto);
            if (info != null && info.ResultCode == ResultType.Success)
            {
                _scoreRegRepository.Update(sql4, parameters.ToArray());
                _checkResultRepository.Update(sql5, parameters.ToArray());
                _standardpicRepository.Update(sql6, parameters.ToArray());
                _taskOfPlanRepository.Update(sql8, parameters.ToArray());
                _standardpicRepository.Update(sql9, parameters.ToArray());
                _taskCardRepository.Update(sql11, parameters.ToArray());
                _taskItemRepository.Update(sql12, parameters.ToArray());
            }


            return info;
        }
    }
}

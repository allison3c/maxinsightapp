using MaxInsight.Mobile.Data;
using MaxInsight.Mobile.Domain;
using MaxInsight.Mobile.Module.Dto.RemoteData;
using MaxInsight.Mobile.Services.RemoteService;
using Newtonsoft.Json;
using SQLite.Net;
using System;
using System.Threading.Tasks;
using XLabs.Ioc;

namespace MaxInsight.Mobile.Helpers
{
    public class MasterDataDownloadHelper
    {
        public static async Task<string> DownloadData(string type = "")
        {
            string retMsg = "";
            var commonFun = Resolver.Resolve<ICommonFun>();
            IRemoteService remoteService = Resolver.Resolve<IRemoteService>();
            CommonHelper commonHelper = Resolver.Resolve<CommonHelper>();
            var conn = Resolver.Resolve<ISQLite>().GetConnection();
            var localScoreService = Resolver.Resolve<ILocalScoreService>();
            if (commonHelper.IsNetWorkConnected() == true)
            {
                try
                {
                    if (type == "")
                    {
                        commonFun.ShowLoading("正在下载数据，请稍等。");
                    }
                    else
                    {
                        commonFun.ShowLoading("正在同步数据...");
                    }
                    var result = await remoteService.GetAllDataForLocalDB(CommonContext.Account.UserId);
                    if (result.ResultCode == Module.ResultType.Success)
                    {
                        RemoteResultDto resultList = JsonConvert.DeserializeObject<RemoteResultDto>(result.Body);

                        #region table
                        var plansInfo = conn.GetTableInfo("Plans");
                        var distributorInfo = conn.GetTableInfo("Distributor");
                        var employeeInfo = conn.GetTableInfo("Employee");
                        var taskOfPlanInfo = conn.GetTableInfo("TaskOfPlan");
                        var taskCardInfo = conn.GetTableInfo("TaskCard");
                        var taskItemInfo = conn.GetTableInfo("TaskItem");
                        var scoreInfo = conn.GetTableInfo("Score");
                        var checkStandardInfo = conn.GetTableInfo("CheckStandard");
                        var checkResultInfo = conn.GetTableInfo("CheckResult");
                        var standardPicInfo = conn.GetTableInfo("StandardPic");
                        var pictureStandardInfo = conn.GetTableInfo("PictureStandard");
                        var hiddenCodeInfo = conn.GetTableInfo("CodeHidden");
                        if (plansInfo == null || plansInfo.Count == 0)
                        {
                            conn.CreateTable<Plans>();
                        }
                        if (distributorInfo == null || distributorInfo.Count == 0)
                        {
                            conn.CreateTable<Distributor>();
                        }
                        if (employeeInfo == null || employeeInfo.Count == 0)
                        {
                            conn.CreateTable<Employee>();
                        }
                        if (taskOfPlanInfo == null || taskOfPlanInfo.Count == 0)
                        {
                            conn.CreateTable<TaskOfPlan>();
                        }
                        if (taskCardInfo == null || taskCardInfo.Count == 0)
                        {
                            conn.CreateTable<TaskCard>();
                        }
                        if (taskItemInfo == null || taskItemInfo.Count == 0)
                        {
                            conn.CreateTable<TaskItem>();
                        }
                        if (scoreInfo == null || scoreInfo.Count == 0)
                        {
                            conn.CreateTable<Score>();
                        }
                        if (checkStandardInfo == null || checkStandardInfo.Count == 0)
                        {
                            conn.CreateTable<Domain.CheckStandard>();
                        }
                        if (checkResultInfo == null || checkResultInfo.Count == 0)
                        {
                            conn.CreateTable<CheckResult>();
                        }
                        if (standardPicInfo == null || standardPicInfo.Count == 0)
                        {
                            conn.CreateTable<Domain.StandardPic>();
                        }
                        if (pictureStandardInfo == null || pictureStandardInfo.Count == 0)
                        {
                            conn.CreateTable<PictureStandard>();
                        }
                        if (hiddenCodeInfo == null || hiddenCodeInfo.Count == 0)
                        {
                            conn.CreateTable<CodeHidden>();
                        }
                        #endregion

                        if (resultList != null)
                        {
                            if (localScoreService.CheckLocalModiYN())
                            {
                                if (App.SysOS == "IOS")
                                {
                                    commonFun.HideLoading();
                                }
                                //if (await commonFun.Confirm("手机中有未上传的数据，同步数据的话将覆盖手机数据，是否继续？"))
                                //{
                                //    UpdateLocalData(conn, resultList);
                                //    commonFun.ShowToast("数据同步完毕。");
                                //}
                                //else
                                //{
                                //    commonFun.ShowToast("同步数据请求已被取消。");
                                //}
                            }
                            else
                            {
                                UpdateLocalData(conn, resultList);
                                if (type == "")
                                {
                                    commonFun.ShowToast("数据同步完毕。");
                                }
                            }
                        }
                        else
                        {
                            commonFun.ShowToast("没有需要同步的数据。");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    commonFun.HideLoading();
                    commonFun.AlertLongText("请求超时.请手动下载数据。");
                }
                catch (Exception)
                {
                    commonFun.AlertLongText("同步异常，请手动下载数据。");
                    commonFun.HideLoading();
                }
                finally
                {
                    commonFun.HideLoading();
                }
            }
            else
            {
                commonFun.AlertLongText("网络不给力。请检查网络");
            }
            return retMsg;
        }

        private static void UpdateLocalData(SQLiteConnection conn, RemoteResultDto resultList)
        {
            conn.Execute("Delete from Plans");
            conn.InsertOrReplaceAll(resultList.PlansLst, typeof(Plans));
            conn.Execute("Delete from Distributor");
            conn.InsertOrReplaceAll(resultList.DistributorLst, typeof(Distributor));
            conn.Execute("Delete from Employee");
            conn.InsertOrReplaceAll(resultList.EmployeeLst, typeof(Employee));
            conn.Execute("Delete from TaskOfPlan");
            conn.InsertOrReplaceAll(resultList.TaskOfPlanLst, typeof(TaskOfPlan));
            conn.Execute("Delete from TaskCard");
            conn.InsertOrReplaceAll(resultList.TaskCardLst, typeof(TaskCard));
            conn.Execute("Delete from TaskItem");
            conn.InsertOrReplaceAll(resultList.TaskItemLst, typeof(TaskItem));
            conn.Execute("Delete from Score");
            conn.InsertOrReplaceAll(resultList.ScoreLst, typeof(Score));
            conn.Execute("Delete from CheckStandard");
            conn.InsertOrReplaceAll(resultList.CheckStandardLst, typeof(Domain.CheckStandard));
            conn.Execute("Delete from CheckResult");
            conn.InsertOrReplaceAll(resultList.CheckResultLst, typeof(CheckResult));
            conn.Execute("Delete from StandardPic");
            conn.InsertOrReplaceAll(resultList.StandardPicLst, typeof(Domain.StandardPic));
            conn.Execute("Delete from PictureStandard");
            conn.InsertOrReplaceAll(resultList.PictureStandardLst, typeof(Domain.PictureStandard));
            conn.Execute("Delete from CodeHidden");
            conn.InsertOrReplaceAll(resultList.HiddenCodeLst, typeof(Domain.CodeHidden));
        }
    }
}

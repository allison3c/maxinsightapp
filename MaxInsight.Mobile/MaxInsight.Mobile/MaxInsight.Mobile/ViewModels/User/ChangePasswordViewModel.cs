using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Services;
using System;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;
using XLabs.Ioc;

namespace MaxInsight.Mobile.ViewModels.User
{
    public class ChangePasswordViewModel : ViewModel
    {
        IAccountService _accountService;
        ICommonFun _commonFun;
        CommonHelper _commonHelper;
        public ChangePasswordViewModel()
        {
            try
            {
                _accountService = Resolver.Resolve<IAccountService>();
                _commonFun = Resolver.Resolve<ICommonFun>();
                _commonHelper = Resolver.Resolve<CommonHelper>();
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ChangePasswordViewModel");
                return;
            }
        }

        #region properties
        string _oldPassword;
        public string OldPassword
        {
            get { return _oldPassword; }
            set { SetProperty(ref _oldPassword, value); }
        }

        string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { SetProperty(ref _newPassword, value); }
        }

        string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetProperty(ref _confirmPassword, value); }
        }
        #endregion

        #region Command
        public Command SavePswCommand
        {
            get
            {
                return new Command(() =>
                {
                    SavePsw();
                });
            }
        }
        #endregion

        #region private
        private async void SavePsw()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(OldPassword))
                {
                    _commonFun.AlertLongText("请输入旧密码。");
                    return;
                }
                else if (OldPassword != CommonContext.Account.Password)
                {
                    _commonFun.AlertLongText("旧密码输入有误，请重新输入。");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(NewPassword))
                {
                    _commonFun.AlertLongText("请输入新密码。");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    _commonFun.AlertLongText("请输入确认密码。");
                    return;
                }
                else if (NewPassword != ConfirmPassword)
                {
                    _commonFun.AlertLongText("新密码与确认密码不一致，请确认。");
                    return;
                }
                if (await _commonFun.Confirm("确认修改密码吗？"))
                {
                    if (_commonHelper.IsNetWorkConnected() == true)
                    {
                        try
                        {
                            _commonFun.ShowLoading("变更中...");

                            var result = await _accountService.UpdatePsw(CommonContext.Account.UserId, NewPassword);
                            if (result.ResultCode == Module.ResultType.Success)
                            {
                                _commonFun.HideLoading();
                                //_commonFun.AlertLongText("变更完毕。 ");
                                //_commonFun.ExistSystem();
                                OldPassword = "";
                                NewPassword = "";
                                ConfirmPassword = "";
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await Navigation.PopAsync();
                                    _commonFun.AlertLongText("变更完毕。");
                                });
                            }
                            else
                            {
                                _commonFun.HideLoading();
                                _commonFun.AlertLongText("提交失败，请重试。 " + result.Msg);
                                return;
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            _commonFun.HideLoading();
                            _commonFun.AlertLongText("请求超时。");
                            return;
                        }
                        catch (Exception)
                        {
                            _commonFun.HideLoading();
                            return;
                        }
                        finally
                        {
                            _commonFun.HideLoading();
                        }
                    }
                    else
                    {
                        _commonFun.AlertLongText("网络连接异常。");
                        return;
                    }
                }
            }
            catch (Exception)
            {
                _commonFun.AlertLongText("操作异常,请重试。-->ChangePasswordViewModel");
                return;
            }
        }
        #endregion
    }
}

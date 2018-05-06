using MaxInsight.Mobile.Helpers;
using MaxInsight.Mobile.Module.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Ioc;

namespace MaxInsight.Mobile
{
    public class CommonContext
    {
        public static AccountInfo Account { get; set; } = new AccountInfo();

		public static string USERNAMEKEY = "UserNameKey";
		public static string FIRSTINSTALLYN = "FirstInstallYN";
        public static string ImpPlanStatus { get; set; }
        public static bool IsUserLoggedIn
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Account.UserId))
                    return false;

                var session_lifetime = Resolver.Resolve<Config.Config>().Get<int>(Config.Config.SessionLifetimeMinute);
                if (DateTime.UtcNow.Subtract(Account.LoggedInAt).TotalMinutes > session_lifetime)
                    return false;

                return true;
            }
        }
    }
}

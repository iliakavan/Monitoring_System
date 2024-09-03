using Hangfire.Dashboard;

namespace wallet_api.Util
{
    public class HangFireExtensions : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
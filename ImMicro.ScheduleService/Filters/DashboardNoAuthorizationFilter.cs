using Hangfire.Dashboard;

namespace ImMicro.ScheduleService.Filters
{
    public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext dashboardContext)
        {
            return true;
        }
    }

}
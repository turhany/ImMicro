using System;
using Hangfire;
using ImMicro.Business.Services.Concrete;
using Microsoft.Extensions.Configuration;

namespace ImMicro.ScheduleService.Schedules
{
    public static class RecurringJobs
    {
        public static void UserInitJob(IConfiguration configuration)
        {
            var itemLimit = int.Parse(configuration["QueueItemLimit"]);
            
            RecurringJob.RemoveIfExists(nameof(UserInitJob));
            RecurringJob.AddOrUpdate<UserService>(nameof(UserInitJob), job => job.SendQueueForInit(itemLimit), "*/1 * * * *", TimeZoneInfo.Local);
        }
    }
}
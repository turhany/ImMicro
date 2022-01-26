using Hangfire; 

namespace ImMicro.ScheduleService.Schedules
{
    public static class RecurringJobs
    {
        public static void SampleJob()
        {
            RecurringJob.RemoveIfExists(nameof(SampleJob));
            //RecurringJob.AddOrUpdate<SampleService>(nameof(SampleJob), job => job.SampleJob(), "*/1 * * * *", TimeZoneInfo.Local);
        }
    }
}
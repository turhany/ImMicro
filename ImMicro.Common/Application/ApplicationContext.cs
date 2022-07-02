using System;
using System.Linq;
using System.Security.Claims;
using ImMicro.Common.Constans;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ImMicro.Common.Application
{
  public class ApplicationContext
    {
        private static ApplicationContext _instance;
        public static ApplicationContext Instance => _instance ??= new ApplicationContext();
        
        public static IHttpContextAccessor Context { get; set; }
        
        private static CurrentUser WorkerServiceCurrentUser;
        private static bool IsWorkerService;
        
        
        private ApplicationContext()
        {
            
        }
        
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            Context = httpContextAccessor;
        }
        
        public static void ConfigureWorkerServiceUser(Guid userId)
        {
            IsWorkerService = true;

            WorkerServiceCurrentUser = new CurrentUser
            {
                Id = userId
            };
        }
        
        public static void ConfigureThreadPool(IConfiguration configuration)
        {
            var workerThreads = Convert.ToInt32(configuration["ThreadPool:WorkerThreads"]);
            var completionPortThreads = Convert.ToInt32(configuration["ThreadPool:CompletionPortThreads"]);

            System.Threading.ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);
            System.Threading.ThreadPool.GetMinThreads(out var minWorkerThreads, out var minCompletionPortThreads);

            workerThreads = Math.Max(workerThreads, minWorkerThreads);
            workerThreads = Math.Min(workerThreads, maxWorkerThreads);
            completionPortThreads = Math.Max(completionPortThreads, minCompletionPortThreads);
            completionPortThreads = Math.Min(completionPortThreads, maxCompletionPortThreads);

            System.Threading.ThreadPool.SetMinThreads(workerThreads, completionPortThreads);
        }

        public CurrentUser CurrentUser
        {
            get
            {
                if (IsWorkerService)
                {
                    return WorkerServiceCurrentUser;
                }

                if (Context?.HttpContext?.User == null)
                {
                    return null;
                }

                var user = Context.HttpContext.User;

                return new CurrentUser
                {
                    Id = Guid.Parse(user.Claims.FirstOrDefault(claim => claim.Type == AppConstants.ClaimTypesId)?.Value),
                    Email = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value,
                    Name = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value,
                    Role = user.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value
                };
            }
        }
    }
}
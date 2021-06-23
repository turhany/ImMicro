using ImMicro.Data.Repositories.Abstract;
using ImMicro.Model;

namespace ImMicro.Data.Repositories.Concrete
{
    public class RequestLogRepository : EfGenericRepository<RequestLog>, IRequestLogRepository
    {
        public RequestLogRepository(DataContext context) : base(context)
        {
        }
    }
}
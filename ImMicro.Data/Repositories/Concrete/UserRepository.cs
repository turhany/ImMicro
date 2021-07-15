using ImMicro.Data.Repositories.Abstract;

namespace ImMicro.Data.Repositories.Concrete
{
    public class UserRepository  : EfGenericRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }
    }
}
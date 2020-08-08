using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<ICollection<User>> GetUsersAsync(int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string userName, string passwordHash)
        {
            return Task.FromResult(new User(userName, passwordHash, passwordHash, passwordHash, true));
        }

        public Task<int> GetCountAsync()
        {
            throw new NotImplementedException();
        }

        public void Add(User user)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            
        }
    }
}
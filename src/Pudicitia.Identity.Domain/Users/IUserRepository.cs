using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pudicitia.Common.Domain;

namespace Pudicitia.Identity.Domain.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<ICollection<User>> GetUsersAsync(int offset, int limit);

        Task<User> GetUserAsync(Guid userId);

        Task<User> GetUserAsync(string userName, string password);

        Task<int> GetCountAsync();

        void Add(User user);

        void Update(User user);

        void Remove(User user);
    }
}
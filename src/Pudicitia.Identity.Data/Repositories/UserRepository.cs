using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Domain;
using Pudicitia.Common.Utilities;
using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityContext context;
        private readonly DbSet<User> users;

        public UserRepository(IdentityContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            users = context.Set<User>();
        }

        public async Task<ICollection<User>> GetUsersAsync(int offset, int limit)
        {
            var result = await users
                .Include(x => x.UserRoles)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return result;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var result = await users
                .Include(x => x.UserRoles)
                .Include(x => x.UserRefreshTokens)
                .SingleOrDefaultAsync(x => x.Id == userId) ??
                throw new EntityNotFoundException(typeof(User), userId);

            return result;
        }

        public async Task<User> GetUserAsync(string userName, string password)
        {
            var result = await users
                .Include(x => x.UserRoles)
                .Include(x => x.UserRefreshTokens)
                .SingleOrDefaultAsync(x => x.UserName == userName) ??
                throw new EntityNotFoundException();
            if (result.PasswordHash != CryptographyUtility.Hash(password.Trim(), result.Salt))
                throw new EntityNotFoundException();

            return result;
        }

        public async Task<int> GetCountAsync()
        {
            var result = await users
                .CountAsync();

            return result;
        }

        public void Add(User user)
        {
            users.Add(user);
        }

        public void Update(User user)
        {
            users.Update(user);
        }

        public void Remove(User user)
        {
            users.Remove(user);
        }
    }
}
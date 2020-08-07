using System;
using Pudicitia.Common.Domain;
using Pudicitia.Common.Exceptions;

namespace Pudicitia.Identity.Domain.Users
{
    public class UserRefreshToken : Entity
    {
        private UserRefreshToken()
        {
        }

        internal UserRefreshToken(string refreshToken, DateTime expireOn, Guid userId)
        {
            if (expireOn < DateTime.UtcNow)
                throw new DomainException("Expired on must be greater than now");

            RefreshToken = refreshToken;
            ExpireOn = expireOn;
            UserId = userId;
        }

        public string RefreshToken { get; private set; }

        public DateTime ExpireOn { get; private set; }

        public Guid UserId { get; private set; }

        public bool IsExpired => DateTime.UtcNow > ExpireOn;
    }
}
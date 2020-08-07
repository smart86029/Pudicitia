using System;
using Pudicitia.Common.Domain;

namespace Pudicitia.Identity.Domain.Users
{
    public class UserDisabled : DomainEvent
    {
        public UserDisabled(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}
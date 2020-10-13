using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Pudicitia.Common.Domain;
using Pudicitia.Common.Exceptions;
using Pudicitia.Common.Utilities;
using Pudicitia.Identity.Domain.Roles;

namespace Pudicitia.Identity.Domain.Users
{
    public class User : AggregateRoot
    {
        private readonly List<UserRole> userRoles = new List<UserRole>();
        private readonly List<UserRefreshToken> userRefreshTokens = new List<UserRefreshToken>();

        private User()
        {
        }

        public User(string userName, string password, string name, string displayName, bool isEnabled)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new DomainException("User name can not be null");
            if (string.IsNullOrWhiteSpace(password))
                throw new DomainException("Password can not be null");
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name can not be null");
            if (string.IsNullOrWhiteSpace(displayName))
                throw new DomainException("Display can not be null");

            UserName = userName.Trim();
            UpdateSalt();
            PasswordHash = CryptographyUtility.Hash(password.Trim(), Salt);
            Name = name.Trim();
            DisplayName = displayName.Trim();
            IsEnabled = isEnabled;
            RaiseDomainEvent(new UserCreated(Id, Name, DisplayName));
        }

        public string UserName { get; private set; }

        public string Salt { get; private set; }

        public string PasswordHash { get; private set; }

        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public bool IsEnabled { get; private set; }

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

        public IReadOnlyCollection<UserRole> UserRoles => userRoles.AsReadOnly();

        public IReadOnlyCollection<UserRefreshToken> UserRefreshTokens => userRefreshTokens.AsReadOnly();

        public void UpdateUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new DomainException("User name can not be null");

            UserName = userName.Trim();
        }

        public void UpdatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return;

            UpdateSalt();
            PasswordHash = CryptographyUtility.Hash(password.Trim(), Salt);
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name can not be null");

            Name = name.Trim();
        }

        public void UpdateDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new DomainException("Display can not be null");

            DisplayName = displayName.Trim();
        }

        public void Enable()
        {
            IsEnabled = true;
            RaiseDomainEvent(new UserDisabled(Id));
        }

        public void Disable()
        {
            IsEnabled = false;
            RaiseDomainEvent(new UserDisabled(Id));
        }

        public void AssignRole(Role role)
        {
            if (!userRoles.Any(x => x.RoleId == role.Id))
                userRoles.Add(new UserRole(Id, role.Id));
        }

        public void UnassignRole(Role role)
        {
            var userRole = userRoles.FirstOrDefault(x => x.RoleId == role.Id);
            if (userRole != default(UserRole))
                userRoles.Remove(userRole);
        }

        public bool IsValidRefreshToken(string refreshToken)
        {
            return userRefreshTokens.Any(x => x.RefreshToken == refreshToken && !x.IsExpired);
        }

        public void AddRefreshToken(string refreshToken, TimeSpan expiry)
        {
            var token = new UserRefreshToken(refreshToken, DateTime.UtcNow.Add(expiry), Id);
            userRefreshTokens.Add(token);
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            var token = userRefreshTokens.First(t => t.RefreshToken == refreshToken);
            userRefreshTokens.Remove(token);
        }

        private void UpdateSalt()
        {
            var saltBytes = new byte[32];
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            rngCryptoServiceProvider.GetNonZeroBytes(saltBytes);
            Salt = Convert.ToBase64String(saltBytes);
        }
    }
}
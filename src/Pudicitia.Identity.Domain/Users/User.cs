using System.Security.Cryptography;
using Pudicitia.Common.Utilities;
using Pudicitia.Identity.Domain.Roles;

namespace Pudicitia.Identity.Domain.Users;

public class User : AggregateRoot
{
    private readonly List<UserRole> _userRoles = new();
    private readonly List<UserRefreshToken> _userRefreshTokens = new();

    private User()
    {
    }

    public User(string userName, string password, string name, string displayName, bool isEnabled)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new DomainException("User name can not be null");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new DomainException("Password can not be null");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name can not be null");
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new DomainException("Display name can not be null");
        }

        UserName = userName.Trim();
        UpdateSalt();
        PasswordHash = CryptographyUtility.Hash(password.Trim(), Salt);
        Name = name.Trim();
        DisplayName = displayName.Trim();
        IsEnabled = isEnabled;
        RaiseDomainEvent(new UserCreated(Id, Name, DisplayName));
    }

    public string UserName { get; private set; } = string.Empty;

    public string Salt { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public bool IsEnabled { get; private set; }

    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    public IReadOnlyCollection<UserRefreshToken> UserRefreshTokens => _userRefreshTokens.AsReadOnly();

    public void UpdatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        UpdateSalt();
        PasswordHash = CryptographyUtility.Hash(password.Trim(), Salt);
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name can not be null");
        }

        Name = name.Trim();
    }

    public void UpdateDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new DomainException("Display name can not be null");
        }

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
        if (!_userRoles.Any(x => x.RoleId == role.Id))
        {
            _userRoles.Add(new UserRole(Id, role.Id));
        }
    }

    public void UnassignRole(Role role)
    {
        var userRole = _userRoles.FirstOrDefault(x => x.RoleId == role.Id);
        if (userRole is not null)
        {
            _userRoles.Remove(userRole);
        }
    }

    public bool IsValidRefreshToken(string refreshToken)
    {
        return _userRefreshTokens.Any(x => x.RefreshToken == refreshToken && !x.IsExpired);
    }

    public void AddRefreshToken(string refreshToken, TimeSpan expiry)
    {
        var token = new UserRefreshToken(refreshToken, DateTime.UtcNow.Add(expiry), Id);
        _userRefreshTokens.Add(token);
    }

    public void RemoveRefreshToken(string refreshToken)
    {
        var token = _userRefreshTokens.First(t => t.RefreshToken == refreshToken);
        _userRefreshTokens.Remove(token);
    }

    private void UpdateSalt()
    {
        var saltBytes = RandomNumberGenerator.GetBytes(32);
        Salt = Convert.ToBase64String(saltBytes);
    }
}

namespace Pudicitia.HR.Domain;

public abstract class Person : AggregateRoot
{
    protected Person()
    {
    }

    protected Person(string name, string displayName, DateOnly birthDate, Gender gender, MaritalStatus maritalStatus)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name can not bet null");
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new DomainException("Display name can not bet null");
        }

        if (birthDate > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new DomainException("Birth date must less than now");
        }

        Name = name.Trim();
        DisplayName = displayName.Trim();
        BirthDate = birthDate;
        Gender = gender;
        MaritalStatus = maritalStatus;
    }

    public string Name { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public DateOnly BirthDate { get; private set; }

    public Gender Gender { get; private set; }

    public MaritalStatus MaritalStatus { get; private set; }

    public Guid? UserId { get; private set; }

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

    public void UpdateBirthDate(DateOnly birthDate)
    {
        if (birthDate > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new DomainException("Birth date must less than now");
        }

        BirthDate = birthDate;
    }

    public void UpdateGender(Gender gender)
    {
        Gender = gender;
    }

    public void UpdateMaritalStatus(MaritalStatus maritalStatus)
    {
        MaritalStatus = maritalStatus;
    }

    public void BindUser(Guid userId)
    {
        if (UserId.HasValue)
        {
            throw new DomainException("User has already bound");
        }

        UserId = userId;
    }
}

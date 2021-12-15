namespace Pudicitia.HR.Domain.Departments;

public class Department : AggregateRoot
{
    private Department()
    {
    }

    public Department(string name, bool isEnabled, Guid? parentId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name can not be null");
        }

        Name = name.Trim();
        IsEnabled = isEnabled;
        ParentId = parentId == Guid.Empty ? null : parentId;
    }

    public string Name { get; private set; } = string.Empty;

    public bool IsEnabled { get; private set; }

    public Guid? ParentId { get; private set; }

    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name can not be null");
        }

        Name = name.Trim();
    }

    public void Enable()
    {
        IsEnabled = true;
    }

    public void Disable()
    {
        IsEnabled = false;
    }
}

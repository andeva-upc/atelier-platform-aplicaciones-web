using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;

public class CustomerRegistration
{
    protected CustomerRegistration()
    {
        CustomerId = null!;
        BranchId = null!;
        Status = "ACTIVE";
    }

    public CustomerRegistration(CustomerId customerId, BranchId branchId) : this()
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        BranchId = branchId;
        Status = "ACTIVE";
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public BranchId BranchId { get; private set; }
    public string Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public void UpdateStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("iot.error.customerRegistration.status.required", nameof(status));

        var normalizedStatus = status.Trim().ToUpperInvariant();

        if (normalizedStatus is not "ACTIVE" and not "INACTIVE")
            throw new ArgumentException("iot.error.customerRegistration.status.invalid", nameof(status));

        Status = normalizedStatus;

        if (normalizedStatus == "ACTIVE")
            DeletedAt = null;

        if (normalizedStatus == "INACTIVE")
            DeletedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = "ACTIVE";
        DeletedAt = null;
    }

    public void Deactivate()
    {
        Status = "INACTIVE";
        DeletedAt = DateTime.UtcNow;
    }
}
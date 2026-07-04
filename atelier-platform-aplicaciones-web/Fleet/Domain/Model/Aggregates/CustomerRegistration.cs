using System;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;

public partial class CustomerRegistration
{
    public CustomerRegistrationId Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid BranchId { get; private set; }
    public CustomerRegistrationStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    protected CustomerRegistration()
    {
        Id = new CustomerRegistrationId();
    }

    public CustomerRegistration(Guid customerId, Guid branchId)
    {
        Id = new CustomerRegistrationId();
        CustomerId = customerId;
        BranchId = branchId;
        Status = CustomerRegistrationStatus.ACTIVE;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        Status = CustomerRegistrationStatus.INACTIVE;
        UpdatedAt = DateTimeOffset.UtcNow;
        DeletedAt = DateTimeOffset.UtcNow;
    }
}

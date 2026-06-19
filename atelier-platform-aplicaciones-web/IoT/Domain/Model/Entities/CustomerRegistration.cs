using System;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;

public class CustomerRegistration
{
    public CustomerRegistration()
    {
        Id = Guid.Empty;
        CustomerId = null!;
        BranchId = null!;
        Status = "ACTIVE";
    }

    public Guid Id { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public BranchId BranchId { get; private set; }
    public string Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }
}

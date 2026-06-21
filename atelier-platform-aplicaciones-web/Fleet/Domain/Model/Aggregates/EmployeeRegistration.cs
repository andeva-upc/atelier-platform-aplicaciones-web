using System;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;

public partial class EmployeeRegistration
{
    public EmployeeRegistrationId Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Guid BranchId { get; private set; }
    public string Speciality { get; private set; }
    public string SpecialityName { get; private set; }
    public decimal Salary { get; private set; }
    public EmployeeRegistrationStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    protected EmployeeRegistration()
    {
        Id = new EmployeeRegistrationId();
    }

    public EmployeeRegistration(Guid employeeId, Guid branchId, string speciality, string specialityName, decimal salary)
    {
        Id = new EmployeeRegistrationId();
        EmployeeId = employeeId;
        BranchId = branchId;
        Speciality = speciality;
        SpecialityName = specialityName;
        Salary = salary;
        Status = EmployeeRegistrationStatus.ACTIVE;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Update(string speciality, string specialityName, decimal salary)
    {
        Speciality = speciality;
        SpecialityName = specialityName;
        Salary = salary;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        Status = EmployeeRegistrationStatus.INACTIVE;
        UpdatedAt = DateTimeOffset.UtcNow;
        DeletedAt = DateTimeOffset.UtcNow;
    }
}

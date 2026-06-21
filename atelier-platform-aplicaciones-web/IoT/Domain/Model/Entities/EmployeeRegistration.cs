using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;

public class EmployeeRegistration
{
    protected EmployeeRegistration()
    {
        EmployeeId = null!;
        BranchId = null!;
        Speciality = string.Empty;
        SpecialityName = string.Empty;
        Status = "ACTIVE";
    }

    public EmployeeRegistration(
        EmployeeId employeeId,
        BranchId branchId,
        string speciality,
        string specialityName,
        decimal salary) : this()
    {
        if (string.IsNullOrWhiteSpace(speciality))
            throw new ArgumentException("iot.error.employeeRegistration.speciality.required", nameof(speciality));

        if (string.IsNullOrWhiteSpace(specialityName))
            throw new ArgumentException("iot.error.employeeRegistration.specialityName.required", nameof(specialityName));

        if (salary < 0)
            throw new ArgumentException("iot.error.employeeRegistration.salary.cannotBeNegative", nameof(salary));

        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        BranchId = branchId;
        Speciality = speciality.Trim();
        SpecialityName = specialityName.Trim();
        Salary = Math.Round(salary, 2, MidpointRounding.AwayFromZero);
        Status = "ACTIVE";
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public EmployeeId EmployeeId { get; private set; }
    public BranchId BranchId { get; private set; }
    public string Speciality { get; private set; }
    public string SpecialityName { get; private set; }
    public decimal Salary { get; private set; }
    public string Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public void Update(
        string speciality,
        string specialityName,
        decimal salary)
    {
        if (string.IsNullOrWhiteSpace(speciality))
            throw new ArgumentException("iot.error.employeeRegistration.speciality.required", nameof(speciality));

        if (string.IsNullOrWhiteSpace(specialityName))
            throw new ArgumentException("iot.error.employeeRegistration.specialityName.required", nameof(specialityName));

        if (salary < 0)
            throw new ArgumentException("iot.error.employeeRegistration.salary.cannotBeNegative", nameof(salary));

        Speciality = speciality.Trim();
        SpecialityName = specialityName.Trim();
        Salary = Math.Round(salary, 2, MidpointRounding.AwayFromZero);
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
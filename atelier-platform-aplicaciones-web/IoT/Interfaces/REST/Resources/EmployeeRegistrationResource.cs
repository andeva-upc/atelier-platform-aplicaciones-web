namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record EmployeeRegistrationResource(
    Guid Id,
    Guid EmployeeId,
    Guid BranchId,
    string Speciality,
    string SpecialityName,
    decimal Salary,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt
);
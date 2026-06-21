using System;

namespace atelier_platform_aplicaciones_web.Fleet.Interfaces.REST.Resources;

public record CreateEmployeeRegistrationResource(
    Guid EmployeeId,
    Guid BranchId,
    string Speciality,
    string SpecialityName,
    decimal Salary
);

public record UpdateEmployeeRegistrationResource(
    string Speciality,
    string SpecialityName,
    decimal Salary
);

public record EmployeeRegistrationResource(
    Guid Id,
    Guid EmployeeId,
    Guid BranchId,
    string Speciality,
    string SpecialityName,
    decimal Salary,
    string Status
);

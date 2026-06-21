using System;

namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;

public record CreateEmployeeRegistrationCommand(
    Guid EmployeeId,
    Guid BranchId,
    string Speciality,
    string SpecialityName,
    decimal Salary
);

public record UpdateEmployeeRegistrationCommand(
    Guid Id,
    string Speciality,
    string SpecialityName,
    decimal Salary
);

public record DeactivateEmployeeRegistrationCommand(Guid Id);

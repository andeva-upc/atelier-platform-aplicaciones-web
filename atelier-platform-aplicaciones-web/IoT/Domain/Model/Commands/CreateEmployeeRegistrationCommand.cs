namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record CreateEmployeeRegistrationCommand(
    Guid EmployeeId,
    Guid BranchId,
    string Speciality,
    string SpecialityName,
    decimal Salary
);
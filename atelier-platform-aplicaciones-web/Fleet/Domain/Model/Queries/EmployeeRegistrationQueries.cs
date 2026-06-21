using System;

namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.Queries;

public record GetEmployeeRegistrationByIdQuery(Guid Id);

public record GetEmployeeRegistrationsByBranchIdQuery(Guid BranchId, Guid? EmployeeId);

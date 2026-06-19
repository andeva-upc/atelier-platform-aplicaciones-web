using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;

public record GetObd2DeviceRegistrationsQuery(BranchId BranchId, Obd2RegistrationStatus Status);

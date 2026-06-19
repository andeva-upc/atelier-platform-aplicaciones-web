using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.IoT.Application.CommandServices;

public interface IVehicleCommandService
{
    Task<Result<Vehicle>> Handle(RegisterVehicleCommand command, CancellationToken cancellationToken = default);
    Task<Result<Vehicle>> Handle(UpdateVehicleCommand command, CancellationToken cancellationToken = default);
    Task<Result<Vehicle>> Handle(DeleteVehicleCommand command, CancellationToken cancellationToken = default);
}

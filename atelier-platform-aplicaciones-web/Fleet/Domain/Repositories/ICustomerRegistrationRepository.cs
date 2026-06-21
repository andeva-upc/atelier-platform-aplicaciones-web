using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Fleet.Domain.Repositories;

public interface ICustomerRegistrationRepository : IBaseRepository<CustomerRegistration>
{
}

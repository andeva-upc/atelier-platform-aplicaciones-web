using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace atelier_platform_aplicaciones_web.Fleet.Infrastructure.Persistence.EFC.Repositories;

public class EmployeeRegistrationRepository(AppDbContext context) 
    : BaseRepository<EmployeeRegistration>(context), IEmployeeRegistrationRepository
{
}

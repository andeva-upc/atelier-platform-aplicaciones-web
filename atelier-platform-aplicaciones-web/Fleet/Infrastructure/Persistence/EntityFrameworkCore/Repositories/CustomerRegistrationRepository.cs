using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace atelier_platform_aplicaciones_web.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class CustomerRegistrationRepository(AppDbContext context) 
    : BaseRepository<CustomerRegistration>(context), ICustomerRegistrationRepository
{
}

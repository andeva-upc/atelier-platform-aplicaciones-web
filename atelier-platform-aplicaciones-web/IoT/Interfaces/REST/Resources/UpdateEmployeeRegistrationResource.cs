using System.ComponentModel.DataAnnotations;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record UpdateEmployeeRegistrationResource(
    [Required] [MaxLength(50)] string Speciality,
    [Required] [MaxLength(50)] string SpecialityName,
    [Required] decimal Salary
);
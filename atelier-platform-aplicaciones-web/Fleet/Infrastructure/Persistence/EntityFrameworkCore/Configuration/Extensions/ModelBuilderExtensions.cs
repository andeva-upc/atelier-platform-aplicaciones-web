using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyFleetConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Appointment>().HasQueryFilter(a => a.DeletedAt == null);

        builder.Entity<Appointment>(entity =>
        {
            entity.ToTable("appointments");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.Id)
                .HasConversion(v => v.Value, v => new AppointmentId(v))
                .IsRequired();

            entity.Property(a => a.BranchId)
                .HasConversion(v => v.Value, v => new BranchId(v))
                .IsRequired();

            entity.Property(a => a.CustomerId)
                .HasConversion(v => v.Value, v => new CustomerId(v))
                .IsRequired();

            entity.Property(a => a.VehicleId)
                .HasConversion(v => v.Value, v => new VehicleId(v))
                .IsRequired();

            entity.Property(a => a.Status)
                .HasConversion(
                    status => status == AppointmentStatus.Pending ? "PENDING" :
                        status == AppointmentStatus.Completed ? "COMPLETED" :
                        status == AppointmentStatus.Canceled ? "CANCELED" : "PENDING",
                    value => value.ToUpper() == "PENDING" ? AppointmentStatus.Pending :
                        value.ToUpper() == "COMPLETED" ? AppointmentStatus.Completed :
                        value.ToUpper() == "CANCELED" ? AppointmentStatus.Canceled :
                        AppointmentStatus.Pending)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(a => a.ScheduledStart).IsRequired();
            entity.Property(a => a.ScheduledEnd).IsRequired();

            entity.Property(a => a.Notes)
                .HasConversion(v => v.Value, v => new AppointmentNotes(v))
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(a => a.CreatedAt).IsRequired();
            entity.Property(a => a.UpdatedAt);
            entity.Property(a => a.DeletedAt);
            entity.Property(a => a.CreatedBy);
            entity.Property(a => a.UpdatedBy);

            entity.Property(a => a.Version).IsConcurrencyToken();

            entity.HasIndex(a => a.BranchId);
            entity.HasIndex(a => a.CustomerId);
            entity.HasIndex(a => a.VehicleId);
        });

        builder.Entity<CustomerRegistration>().HasQueryFilter(c => c.DeletedAt == null);
        builder.Entity<CustomerRegistration>(entity =>
        {
            entity.ToTable("customer_registrations");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id)
                .HasConversion(v => v.Value, v => new CustomerRegistrationId(v))
                .IsRequired();
            
            entity.Property(c => c.Status)
                .HasConversion(
                    status => status.ToString(),
                    value => Enum.Parse<CustomerRegistrationStatus>(value))
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(c => c.BranchId);
            entity.HasIndex(c => c.CustomerId);
        });

        builder.Entity<EmployeeRegistration>().HasQueryFilter(e => e.DeletedAt == null);
        builder.Entity<EmployeeRegistration>(entity =>
        {
            entity.ToTable("employee_registrations");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new EmployeeRegistrationId(v))
                .IsRequired();

            entity.Property(e => e.Status)
                .HasConversion(
                    status => status.ToString(),
                    value => Enum.Parse<EmployeeRegistrationStatus>(value))
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(e => e.BranchId);
            entity.HasIndex(e => e.EmployeeId);
        });
    }
}
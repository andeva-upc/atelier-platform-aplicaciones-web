using atelier_platform_aplicaciones_web.IAM.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using CoreEmployeeId = atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects.EmployeeId;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIotConfiguration(this ModelBuilder builder)
    {
        // Query filters for soft delete
        builder.Entity<Obd2Device>().HasQueryFilter(d => d.DeletedAt == null);
        builder.Entity<Obd2DeviceRegistration>().HasQueryFilter(r => r.DeletedAt == null);
        builder.Entity<Vehicle>().HasQueryFilter(v => v.DeletedAt == null);
        builder.Entity<VehicleRegistration>().HasQueryFilter(vr => vr.DeletedAt == null);
        builder.Entity<CustomerRegistration>().HasQueryFilter(cr => cr.DeletedAt == null);
        builder.Entity<EmployeeRegistration>().HasQueryFilter(er => er.DeletedAt == null);

        // Obd2Device mapping
        builder.Entity<Obd2Device>(entity =>
        {
            entity.ToTable("obd2_devices");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new Obd2DeviceId(v))
                .IsRequired();

            entity.Property(e => e.BranchId)
                .HasConversion(v => v.Value, v => new BranchId(v))
                .IsRequired();

            entity.Property(e => e.MacAddress)
                .IsRequired()
                .HasMaxLength(17);

            entity.HasIndex(e => e.MacAddress)
                .IsUnique();

            entity.Property(e => e.Status)
                .HasConversion(v => v.Value, v => new Obd2DeviceStatus(v))
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.LastPing);

            entity.Property(e => e.Version)
                .IsConcurrencyToken();
        });

        // Obd2DeviceRegistration mapping
        builder.Entity<Obd2DeviceRegistration>(entity =>
        {
            entity.ToTable("obd2_device_registrations");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new Obd2DeviceRegistrationId(v))
                .IsRequired();

            entity.Property(e => e.Obd2DeviceId)
                .HasConversion(v => v.Value, v => new Obd2DeviceId(v))
                .IsRequired();

            entity.Property(e => e.BranchId)
                .HasConversion(v => v.Value, v => new BranchId(v))
                .IsRequired();

            entity.Property(e => e.VehicleId)
                .HasConversion(v => v.Value, v => new VehicleId(v))
                .IsRequired();

            entity.Property(e => e.Status)
                .HasConversion(v => v.Value, v => new Obd2RegistrationStatus(v))
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.DeletedAt);
        });

        // Vehicle mapping
        builder.Entity<Vehicle>(entity =>
        {
            entity.ToTable("vehicles");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new VehicleId(v))
                .IsRequired();

            entity.Property(e => e.PlateNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Vin)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.Vin)
                .IsUnique();

            entity.Property(e => e.Year)
                .IsRequired();

            entity.Property(e => e.Brand)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Model)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .IsRequired();

            entity.Property(e => e.DeletedAt);

            entity.Property(e => e.Version)
                .IsConcurrencyToken();
        });

        // VehicleRegistration mapping
        builder.Entity<VehicleRegistration>(entity =>
        {
            entity.ToTable("vehicle_registrations");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new VehicleRegistrationId(v))
                .IsRequired();

            entity.Property(e => e.UserId)
                .HasConversion(v => v.Value, v => new UserId(v))
                .IsRequired();

            entity.Property(e => e.VehicleId)
                .HasConversion(v => v.Value, v => new VehicleId(v))
                .IsRequired();

            entity.Property(e => e.Status)
                .HasConversion(v => v.Value, v => new VehicleRegistrationStatus(v))
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.DeletedAt);
        });

        // CustomerRegistration mapping
        builder.Entity<CustomerRegistration>(entity =>
        {
            entity.ToTable("customer_registrations");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .IsRequired();

            entity.Property(e => e.CustomerId)
                .HasConversion(v => v.Value, v => new CustomerId(v))
                .IsRequired();

            entity.Property(e => e.BranchId)
                .HasConversion(v => v.Value, v => new BranchId(v))
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.DeletedAt);

            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.BranchId);
        });

        // EmployeeRegistration mapping
        builder.Entity<EmployeeRegistration>(entity =>
        {
            entity.ToTable("employee_registrations");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .IsRequired();

            entity.Property(e => e.EmployeeId)
                .HasConversion(v => v.Value, v => new CoreEmployeeId(v))
                .IsRequired();

            entity.Property(e => e.BranchId)
                .HasConversion(v => v.Value, v => new BranchId(v))
                .IsRequired();

            entity.Property(e => e.Speciality)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.SpecialityName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Salary)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .IsRequired();

            entity.Property(e => e.DeletedAt);

            entity.HasIndex(e => e.EmployeeId);
            entity.HasIndex(e => e.BranchId);
        });

        // TelemetrySnapshot mapping
        builder.Entity<TelemetrySnapshot>(entity =>
        {
            entity.ToTable("telemetry_snapshots");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new TelemetrySnapshotId(v))
                .IsRequired();

            entity.Property(e => e.Obd2DeviceRegistrationId)
                .HasConversion(v => v.Value, v => new Obd2DeviceRegistrationId(v))
                .IsRequired();

            entity.Property(e => e.BranchId)
                .HasConversion(v => v.Value, v => new BranchId(v))
                .IsRequired();

            entity.Property(e => e.Rpm)
                .IsRequired();

            entity.Property(e => e.Temperature)
                .IsRequired();

            entity.Property(e => e.SpeedKmh);

            entity.Property(e => e.OdometerKm);

            entity.Property(e => e.FuelLevelPercent)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired();
        });

        // DtcAlert mapping
        builder.Entity<DtcAlert>(entity =>
        {
            entity.ToTable("dtc_alerts");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasConversion(v => v.Value, v => new DtcAlertId(v))
                .IsRequired();

            entity.Property(e => e.TelemetrySnapshotId)
                .HasConversion(v => v.Value, v => new TelemetrySnapshotId(v))
                .IsRequired();

            entity.Property(e => e.BranchId)
                .HasConversion(v => v.Value, v => new BranchId(v))
                .IsRequired();

            entity.Property(e => e.DtcCode)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(e => e.Description)
                .IsRequired();

            entity.Property(e => e.Severity)
                .HasConversion(v => v.Value, v => new DtcSeverity(v))
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.CreatedAt)
                .IsRequired();
        });
    }
}
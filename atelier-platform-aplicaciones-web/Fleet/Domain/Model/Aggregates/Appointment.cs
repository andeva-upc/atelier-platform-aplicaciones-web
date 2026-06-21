using atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;

public class Appointment : IUserAuditableEntity
{
    public AppointmentId Id { get; private set; }
    public BranchId BranchId { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public VehicleId VehicleId { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public DateTime ScheduledStart { get; private set; }
    public DateTime ScheduledEnd { get; private set; }
    public AppointmentNotes Notes { get; private set; }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public long Version { get; set; }

    protected Appointment()
    {
        Id = null!;
        BranchId = null!;
        CustomerId = null!;
        VehicleId = null!;
        Notes = null!;
    }

    public Appointment(
        BranchId branchId,
        CustomerId customerId,
        VehicleId vehicleId,
        DateTime scheduledStart,
        AppointmentNotes notes) : this()
    {
        Id = new AppointmentId(Guid.NewGuid());
        BranchId = branchId;
        CustomerId = customerId;
        VehicleId = vehicleId;
        Status = AppointmentStatus.Pending;
        ScheduledStart = scheduledStart;
        ScheduledEnd = scheduledStart.AddHours(1);
        Notes = notes;
    }

    public void Update(
        BranchId branchId,
        CustomerId customerId,
        VehicleId vehicleId,
        DateTime scheduledStart,
        AppointmentNotes notes)
    {
        if (Status == AppointmentStatus.Completed || Status == AppointmentStatus.Canceled)
        {
            throw new InvalidOperationException("fleet.error.appointment.cannotUpdateFinalStatus");
        }

        BranchId = branchId;
        CustomerId = customerId;
        VehicleId = vehicleId;
        ScheduledStart = scheduledStart;
        ScheduledEnd = scheduledStart.AddHours(1);
        Notes = notes;
    }

    public void Cancel()
    {
        if (Status == AppointmentStatus.Completed)
        {
            throw new InvalidOperationException("fleet.error.appointment.cannotCancelCompleted");
        }

        Status = AppointmentStatus.Canceled;
        DeletedAt = DateTimeOffset.UtcNow;
    }

    public void Complete()
    {
        if (Status == AppointmentStatus.Canceled)
        {
            throw new InvalidOperationException("fleet.error.appointment.cannotCompleteCanceled");
        }

        Status = AppointmentStatus.Completed;
    }

    public bool IsDeleted()
    {
        return DeletedAt.HasValue;
    }
}
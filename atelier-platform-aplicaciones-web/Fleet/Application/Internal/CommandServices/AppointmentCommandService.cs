using atelier_platform_aplicaciones_web.Fleet.Application.CommandServices;
using atelier_platform_aplicaciones_web.Fleet.Application.Errors;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Fleet.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Fleet.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Fleet.Application.Internal.CommandServices;

public class AppointmentCommandService(
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork) : IAppointmentCommandService
{
    public async Task<Result<Appointment>> Handle(CreateAppointmentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var scheduledEnd = command.ScheduledStart.AddHours(1);

            var overlaps = await appointmentRepository.ExistsOverlappingAppointmentAsync(
                command.BranchId,
                command.ScheduledStart,
                scheduledEnd);

            if (overlaps)
            {
                return Result<Appointment>.Failure(
                    AppointmentError.Overlap,
                    "fleet.error.appointment.overlap");
            }

            var appointment = new Appointment(
                new BranchId(command.BranchId),
                new CustomerId(command.CustomerId),
                new VehicleId(command.VehicleId),
                command.ScheduledStart,
                new AppointmentNotes(command.Notes));

            await appointmentRepository.AddAsync(appointment, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Appointment>.Success(appointment);
        }
        catch (ArgumentException e)
        {
            return Result<Appointment>.Failure(
                AppointmentError.InvalidNotes,
                e.Message);
        }
        catch (Exception)
        {
            return Result<Appointment>.Failure(
                AppointmentError.Unexpected,
                "fleet.error.appointment.unexpected");
        }
    }

    public async Task<Result<Appointment>> Handle(UpdateAppointmentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var appointmentId = new AppointmentId(command.AppointmentId);
            var appointment = await appointmentRepository.FindAppointmentByIdAsync(appointmentId, cancellationToken);

            if (appointment == null)
            {
                return Result<Appointment>.Failure(
                    AppointmentError.NotFound,
                    "fleet.error.appointment.notFound");
            }

            var scheduledEnd = command.ScheduledStart.AddHours(1);

            var overlaps = await appointmentRepository.ExistsOverlappingAppointmentAsync(
                command.BranchId,
                command.ScheduledStart,
                scheduledEnd,
                command.AppointmentId);

            if (overlaps)
            {
                return Result<Appointment>.Failure(
                    AppointmentError.Overlap,
                    "fleet.error.appointment.overlap");
            }

            appointment.Update(
                new BranchId(command.BranchId),
                new CustomerId(command.CustomerId),
                new VehicleId(command.VehicleId),
                command.ScheduledStart,
                new AppointmentNotes(command.Notes));

            appointmentRepository.Update(appointment);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Appointment>.Success(appointment);
        }
        catch (ArgumentException e)
        {
            return Result<Appointment>.Failure(
                AppointmentError.InvalidNotes,
                e.Message);
        }
        catch (InvalidOperationException e)
        {
            return Result<Appointment>.Failure(
                AppointmentError.CannotUpdateFinalStatus,
                e.Message);
        }
        catch (Exception)
        {
            return Result<Appointment>.Failure(
                AppointmentError.Unexpected,
                "fleet.error.appointment.unexpected");
        }
    }

    public async Task<Result<Appointment>> Handle(CancelAppointmentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var appointment = await appointmentRepository.FindAppointmentByIdAsync(
                new AppointmentId(command.AppointmentId),
                cancellationToken);

            if (appointment == null)
            {
                return Result<Appointment>.Failure(
                    AppointmentError.NotFound,
                    "fleet.error.appointment.notFound");
            }

            appointment.Cancel();

            appointmentRepository.Update(appointment);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Appointment>.Success(appointment);
        }
        catch (InvalidOperationException e)
        {
            return Result<Appointment>.Failure(
                AppointmentError.CannotCancelCompleted,
                e.Message);
        }
        catch (Exception)
        {
            return Result<Appointment>.Failure(
                AppointmentError.Unexpected,
                "fleet.error.appointment.unexpected");
        }
    }

    public async Task<Result<Appointment>> Handle(CompleteAppointmentCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var appointment = await appointmentRepository.FindAppointmentByIdAsync(
                new AppointmentId(command.AppointmentId),
                cancellationToken);

            if (appointment == null)
            {
                return Result<Appointment>.Failure(
                    AppointmentError.NotFound,
                    "fleet.error.appointment.notFound");
            }

            appointment.Complete();

            appointmentRepository.Update(appointment);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Appointment>.Success(appointment);
        }
        catch (InvalidOperationException e)
        {
            return Result<Appointment>.Failure(
                AppointmentError.CannotCompleteCanceled,
                e.Message);
        }
        catch (Exception)
        {
            return Result<Appointment>.Failure(
                AppointmentError.Unexpected,
                "fleet.error.appointment.unexpected");
        }
    }
}
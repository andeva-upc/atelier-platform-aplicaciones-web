namespace atelier_platform_aplicaciones_web.Fleet.Application.Errors;

public enum AppointmentError
{
    NotFound,
    Overlap,
    InvalidNotes,
    CannotUpdateFinalStatus,
    CannotCancelCompleted,
    CannotCompleteCanceled,
    Unexpected
}
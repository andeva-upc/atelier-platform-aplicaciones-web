using System;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Entities;

public class DtcAlert
{
    public DtcAlert()
    {
        Id = null!;
        TelemetrySnapshotId = null!;
        BranchId = null!;
        DtcCode = string.Empty;
        Description = string.Empty;
        Severity = DtcSeverity.Low;
    }

    public DtcAlert(TelemetrySnapshotId telemetrySnapshotId, BranchId branchId, string dtcCode, string description, DtcSeverity severity) : this()
    {
        if (string.IsNullOrWhiteSpace(dtcCode))
            throw new ArgumentException("iot.error.dtcCode.required");
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("iot.error.description.required");

        Id = new DtcAlertId(Guid.NewGuid());
        TelemetrySnapshotId = telemetrySnapshotId;
        BranchId = branchId;
        DtcCode = dtcCode;
        Description = description;
        Severity = severity;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public DtcAlertId Id { get; private set; }
    public TelemetrySnapshotId TelemetrySnapshotId { get; private set; }
    public BranchId BranchId { get; private set; }
    public string DtcCode { get; private set; }
    public string Description { get; private set; }
    public DtcSeverity Severity { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}

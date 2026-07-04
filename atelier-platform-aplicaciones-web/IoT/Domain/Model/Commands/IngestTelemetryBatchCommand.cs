using System;
using System.Collections.Generic;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record TelemetryMeasurement(
    int Rpm,
    int Temperature,
    double SpeedKmh,
    int? OdometerKm,
    double FuelLevelPercent
);

public record IngestTelemetryBatchCommand(
    Guid Obd2DeviceId,
    List<TelemetryMeasurement> Measurements
);

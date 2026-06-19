using System;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

public class Vehicle : IAuditableEntity
{
    public Vehicle()
    {
        Id = null!;
        PlateNumber = string.Empty;
        Vin = string.Empty;
        Brand = string.Empty;
        Model = string.Empty;
    }

    public Vehicle(string plateNumber, string vin, int year, string brand, string model) : this()
    {
        if (string.IsNullOrWhiteSpace(plateNumber))
            throw new ArgumentException("iot.error.plateNumber.required");
        if (string.IsNullOrWhiteSpace(vin))
            throw new ArgumentException("iot.error.vin.required");
        if (year <= 1900)
            throw new ArgumentException("iot.error.year.invalid");
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("iot.error.brand.required");
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("iot.error.model.required");

        Id = new VehicleId(Guid.NewGuid());
        PlateNumber = plateNumber;
        Vin = vin;
        Year = year;
        Brand = brand;
        Model = model;
    }

    public VehicleId Id { get; private set; }
    public string PlateNumber { get; private set; }
    public string Vin { get; private set; }
    public int Year { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public long Version { get; set; }

    public void Update(string plateNumber, string vin, int year, string brand, string model)
    {
        if (string.IsNullOrWhiteSpace(plateNumber))
            throw new ArgumentException("iot.error.plateNumber.required");
        if (string.IsNullOrWhiteSpace(vin))
            throw new ArgumentException("iot.error.vin.required");
        if (year <= 1900)
            throw new ArgumentException("iot.error.year.invalid");
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("iot.error.brand.required");
        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("iot.error.model.required");

        PlateNumber = plateNumber;
        Vin = vin;
        Year = year;
        Brand = brand;
        Model = model;
    }
}

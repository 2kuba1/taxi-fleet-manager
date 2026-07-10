using Domain.Enums;

namespace Application.Models.DTOs;

public record TeamReportDto(ReportStatus ReportStatus, int KilometersDriven, float CardTransactionSum, DateTime ShiftDate, Guid? CarId = null, string? CarBrand = null, string? CarModel = null, string? LicensePlate = null);
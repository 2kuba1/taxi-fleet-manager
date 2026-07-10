using Application.Models.DTOs;

namespace Application.Models.Responses;

public record TeamReportResponse(Guid TeamId, List<UserReportsDto> UserReports);
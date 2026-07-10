namespace Application.Models.DTOs;

public record UserReportsDto(Guid UserId,
    string UserFirstName,
    string UserLastName,
    List<TeamReportDto> Reports);
using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.DTO;

public class PaginationDTO
{
    [Range(1, 100)]
    public int PageSize { get; init; } = 10;

    [Range(1, int.MaxValue)]
    public int PageNr { get; init; } = 1;

}

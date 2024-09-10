namespace MovieCardAPI.Model.DTO;

public class PaginationMetaDTO : PaginationDTO
{
    public required int TotalItemCount { get; init; }

    public int TotalPageCount => Convert.ToInt32(Math.Ceiling(TotalItemCount / Convert.ToDouble(PageSize)));
}

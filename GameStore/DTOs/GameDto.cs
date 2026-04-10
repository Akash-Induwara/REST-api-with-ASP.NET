namespace GameStore.api.DTOs
{
    public record GameDto(
        int Id,
        string Title,
        string Genre,
        decimal Price,
        DateTime ReleaseDate
    );
}

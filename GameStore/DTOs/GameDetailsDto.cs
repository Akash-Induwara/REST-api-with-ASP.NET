namespace GameStore.api.DTOs
{
    public record GameDetailsDto(
        int Id,
        string Title,
        int GenreId,
        decimal Price,
        DateOnly ReleaseDate
    );
}

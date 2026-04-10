using System.ComponentModel.DataAnnotations;

namespace GameStore.api.DTOs
{
    public record CreateGameDto(
        [Required][StringLength(50)]string Title,
        [Required][StringLength(20)] string Genre,
        [Range(1,100)]decimal Price,
        DateTime ReleaseDate
    );
}

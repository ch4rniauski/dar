namespace laba2.DTO;

public sealed record ProductToUpdateDto(
    string Name,
    string Description,
    decimal Price);
    
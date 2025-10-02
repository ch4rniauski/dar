namespace laba2.DTO;

public sealed record ProductToCreateDto(
    string Name,
    string Description,
    decimal Price);

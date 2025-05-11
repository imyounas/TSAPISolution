namespace TSWebAPI.Dtos
{
    public record ProductFilterRequestDto
    {
        public string? Name { get; init; }
        public int PageNo { get; init; }
        public int PageSize { get; init; }
    }
}

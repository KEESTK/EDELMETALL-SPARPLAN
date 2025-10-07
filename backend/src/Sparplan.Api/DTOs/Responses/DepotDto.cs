namespace Sparplan.Api.DTOs.Responses
{
    public class DepotDto
    {
        public Guid Id { get; set; }
        public List<SparplanDto> Sparplaene { get; set; } = new();
    }
}

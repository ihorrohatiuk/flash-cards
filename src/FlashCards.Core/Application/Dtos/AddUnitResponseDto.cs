using System.Text.Json.Serialization;

namespace FlashCards.Core.Application.Dtos;

public class AddUnitResponseDto
{
    public Guid UnitId { get; set; }
    public string Message { get; set; }
}
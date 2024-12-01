namespace FlashCardsWebAssembly.Models;

public class Unit
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Theme { get; set; }
    public string? OwnerName { get; set; }

    public Unit(int id, string? name, string? theme, string? ownerName)
    {
        Id = id;
        Name = name;
        Theme = theme;
        OwnerName = ownerName;
    }
}
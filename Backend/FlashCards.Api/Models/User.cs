namespace FlashCards.Api.Models;

public class User
{
    public int Id { get; set; }
    public string? Nickname { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }

    public User(int id, string? nickname, string? firstName, string? lastName, string? email, string? role)
    {
        Id = id;
        Nickname = nickname;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Role = role;
    }

    public override string ToString()
    {
        return $"{{\n\tId: {Id}," +
               $"\n\tNickname: {Nickname}," +
               $"\n\tFirstName: {FirstName}," +
               $"\n\tLastName: {LastName}," +
               $"\n\tEmail: {Email}," +
               $"\n\tRole: {Role}\n}}";
    }

}
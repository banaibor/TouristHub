namespace TouristHub.ApiService.Models;

public class AccountModel
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}
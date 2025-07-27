namespace TouristHub.ApiService;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TouristHub.ApiService.Models;

public class TouristHubDbContext : IdentityDbContext<IdentityUser>
{
    public TouristHubDbContext(DbContextOptions<TouristHubDbContext> options) : base(options)
    {
    }

    public DbSet<AccountModel> Accounts { get; set; }
}
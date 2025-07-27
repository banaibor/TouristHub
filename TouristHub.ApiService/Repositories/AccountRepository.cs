using Microsoft.EntityFrameworkCore;
using TouristHub.ApiService.Models;
using TouristHub.ApiService.Interfaces;

namespace TouristHub.ApiService.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly TouristHubDbContext _dbContext;

    public AccountRepository(TouristHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AccountModel?> GetAccountByIdAsync(string id)
    {
        return await _dbContext.Accounts.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<AccountModel>> GetAllAccountsAsync()
    {
        return await _dbContext.Accounts.ToListAsync();
    }

    public async Task AddAccountAsync(AccountModel account)
    {
        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAccountAsync(AccountModel account)
    {
        _dbContext.Accounts.Update(account);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAccountAsync(string id)
    {
        var account = await GetAccountByIdAsync(id);
        if (account != null)
        {
            _dbContext.Accounts.Remove(account);
            await _dbContext.SaveChangesAsync();
        }
    }
}
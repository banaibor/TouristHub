namespace TouristHub.ApiService.Interfaces;

using TouristHub.ApiService.Models;

public interface IAccountRepository
{
    Task<AccountModel?> GetAccountByIdAsync(string id);
    Task<IEnumerable<AccountModel>> GetAllAccountsAsync();
    Task AddAccountAsync(AccountModel account);
    Task UpdateAccountAsync(AccountModel account);
    Task DeleteAccountAsync(string id);
}
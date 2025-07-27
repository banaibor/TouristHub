namespace TouristHub.ApiService.Services;

using TouristHub.ApiService.Interfaces;
using TouristHub.ApiService.Models;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountModel?> GetAccountByIdAsync(string id)
    {
        return await _accountRepository.GetAccountByIdAsync(id);
    }

    public async Task<IEnumerable<AccountModel>> GetAllAccountsAsync()
    {
        return await _accountRepository.GetAllAccountsAsync();
    }

    public async Task AddAccountAsync(AccountModel account)
    {
        await _accountRepository.AddAccountAsync(account);
    }

    public async Task UpdateAccountAsync(AccountModel account)
    {
        await _accountRepository.UpdateAccountAsync(account);
    }

    public async Task DeleteAccountAsync(string id)
    {
        await _accountRepository.DeleteAccountAsync(id);
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TouristHub.ApiService.Interfaces;
using TouristHub.ApiService.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(IAccountService accountService, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
    {
        _accountService = accountService;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(new { message = "API is reachable", timestamp = DateTime.UtcNow });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        Console.WriteLine($"[API] Register endpoint called. Username: {model.Username}, Password: {model.Password}"); // Log received password (remove after debugging!)
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new IdentityUser { UserName = model.Username, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        if (!await _roleManager.RoleExistsAsync(model.Role))
        {
            await _roleManager.CreateAsync(new IdentityRole(model.Role));
        }

        await _userManager.AddToRoleAsync(user, model.Role);

        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized("Invalid login attempt.");
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

        if (!result.Succeeded)
        {
            return Unauthorized("Invalid login attempt.");
        }

        // Get user roles
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "USER";

        return Ok(new { message = "Login successful.", role });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(string id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        if (account == null)
        {
            return NotFound("Account not found.");
        }

        return Ok(account);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAccounts()
    {
        var accounts = await _accountService.GetAllAccountsAsync();
        return Ok(accounts);
    }
}
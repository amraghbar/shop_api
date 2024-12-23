using Microsoft.AspNetCore.Mvc;
using Shop_Core.Models;
using Shop_Core.Interfaces;
using System.Threading.Tasks;
using Shop_Core.DTOS.Account;

namespace Shop_Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;

        public AccountsController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        // POST api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new Users
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                Gov_Id = registerDTO.Gov_Code,
                City_Id = registerDTO.City_Code,
                Class_Id = registerDTO.Cus_classId,
            };

            var result = await accountRepository.RegisterAsync(user, registerDTO.Password);
            return new JsonResult(result);
        }

        // POST api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await accountRepository.LoginAsync(model.Username, model.Password);
            if (result == "Invalid Username or Password")
            {
                return Unauthorized(result); // Invalid login credentials
            }

            return Ok(new { Token = result }); // Login successful, return the token
        }

        // POST api/account/change-password
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await accountRepository.ChangePasswordAsync(model.Email, model.OldPassword, model.NewPassword);
            if (result == "Password changed successfully.")
            {
                return Ok(result); // Password changed successfully
            }

            return BadRequest(result); // Error during password change
        }

        // POST api/account/forgot-password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest("Invalid data.");
            }

            var result = await accountRepository.ForgotPasswordAsync(model.Email);
            if (result == "Password reset link sent successfully.")
            {
                return Ok(result); // Sending reset link successfully
            }

            return BadRequest(result); // Failed to send reset link
        }

        // POST api/account/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                return BadRequest("Invalid data.");
            }

            var result = await accountRepository.ResetPasswordAsync(model);
            if (result == "Password reset successfully.")
            {
                return Ok(result); // Password reset successfully
            }

            return BadRequest(result); // Error during password reset
        }

    }
}

using Shop_Core.DTOS.Account;
using Shop_Core.Models;

namespace Shop_Core.Interfaces
{
    public interface IAccountRepository
    {
        Task<string> RegisterAsync(Users user, string password);
        Task<string> LoginAsync(string username, string password);
        Task<string> ChangePasswordAsync(string email, string oldPasswrod, string newPassword);
        Task<string> ForgotPasswordAsync(string email); 
        Task<string> ResetPasswordAsync(ResetPasswordModel resetPasswordModel);
        Task<string> ConfirmAccountAsync(string email, string confirmationToken);

    }
}

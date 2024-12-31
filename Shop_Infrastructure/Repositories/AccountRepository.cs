using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Shop_Core.DTOS.Account;

namespace Shop_Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;
        private readonly IConfiguration configuration;
        private readonly ILogger<AccountRepository> logger;
        private IEmailService emailService;

        public AccountRepository(UserManager<Users> userManager, SignInManager<Users> signInManager, IConfiguration configuration, ILogger<AccountRepository> logger, IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.logger = logger;
            this.emailService = emailService;
        }

        // تسجيل المستخدم
        public async Task<string> RegisterAsync(Users user, string password)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                // إنشاء رمز تأكيد البريد الإلكتروني
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = $"https://yourdomain.com/confirm-account?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

                var emailModel = new Email
                {
                    Subject = "Please Confirm Your Email Address",
                    Body = $@"
                        <p>Click the link below to confirm your account:</p>

                        <p><a href='{confirmationLink}'>Confirm Account</a></p>
                        <p><b>Confirmation Token:</b> {token}</p>

                    ",
                    Recivers = user.Email
                };

                try
                {
                    emailService.SendEmail(emailModel);
                    return "User Registered Successfully. Please check your email for the confirmation link.";
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error sending confirmation email: {ex.Message}");
                    return "User registered successfully, but failed to send confirmation email.";
                }
            }

            var errorMessages = result.Errors.Select(error => error.Description).ToList();
            logger.LogWarning($"User registration failed: {string.Join(", ", errorMessages)}");
            return string.Join(", ", errorMessages);
        }

        // تسجيل الدخول
        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                logger.LogWarning($"Login attempt failed: Invalid username {username}");
                return "Invalid Username or Password";
            }

            // التحقق من حالة تأكيد البريد الإلكتروني
            if (!user.EmailConfirmed)
            {
                return "Your account is not confirmed. Please confirm your account via the email link.";
            }

            var result = await signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
            {
                logger.LogWarning($"Login attempt failed: Incorrect password for {username}");
                return "Invalid Username or Password";
            }

            logger.LogInformation($"User {username} logged in successfully.");

            var tokenName = "JWTToken";
            var loginProvider = "YourApp";

            var existingToken = await userManager.GetAuthenticationTokenAsync(user, loginProvider, tokenName);

            if (!string.IsNullOrEmpty(existingToken))
            {
                logger.LogInformation($"Reusing existing token for user {username}.");
                return existingToken;
            }

            var newToken = GenerateToken(user);
            await userManager.SetAuthenticationTokenAsync(user, loginProvider, tokenName, newToken);

            logger.LogInformation($"Generated new token for user {username}.");
            return newToken;
        }

        private string GenerateToken(Users user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["JWT:Issuer"],
                configuration["JWT:Audience"],
                claims,
                signingCredentials: cred,
                expires: DateTime.Now.AddMinutes(6000)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> ConfirmAccountAsync(string email, string confirmationToken)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return $"User with email {email} not found.";
            }

            var result = await userManager.ConfirmEmailAsync(user, confirmationToken);
            if (!result.Succeeded)
            {
                return $"Invalid confirmation token for user with email {email}. Please check the token or request a new one."; 
            }

            user.EmailConfirmed = true;
            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return $"Failed to update user with email {email}. Please try again.";
            }

            return $"Account for {email} confirmed successfully. Token used: {confirmationToken}"; 
        }

        public async Task<string> ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                logger.LogWarning($"User with email {email} not found.");
                return "User not found.";
            }

            var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (result.Succeeded)
            {
                logger.LogInformation($"Password changed successfully for {email}.");
                return "Password changed successfully.";
            }

            var errorMessages = result.Errors.Select(error => error.Description).ToList();
            logger.LogWarning($"Password change failed for {email}: {string.Join(", ", errorMessages)}");
            return string.Join(", ", errorMessages);
        }

        // استعادة كلمة المرور
        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return "If this email is associated with an account, a password reset link has been sent.";
            }

            // إنشاء رابط لإعادة تعيين كلمة المرور
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://yourdomain.com/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email)}";

            // إرسال رسالة بريد إلكتروني تحتوي على التوكين
            var emailModel = new Email
            {
                Subject = "Password Reset Request",
                Body = $@"
        <p>Click the link below to reset your password:</p>
        <p><a href='{resetLink}'>Reset Password</a></p>
        <p><strong>Your reset token: {token}</strong></p>  <!-- إضافة التوكين هنا -->
    ",
                Recivers = email
            };


            try
            {
                emailService.SendEmail(emailModel);
                return "If this email is associated with an account, a password reset link has been sent.";
            }
            catch (Exception ex)
            {
                logger.LogError($"Error sending email: {ex.Message}");
                return "Failed to send password reset email.";
            }
        }

        // إعادة تعيين كلمة المرور
        public async Task<string> ResetPasswordAsync(ResetPasswordModel resetPasswordModel)
        {
            if (resetPasswordModel.NewPassword != resetPasswordModel.ConfirmPassword)
            {
                return "Passwords do not match.";
            }

            var user = await userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
            {
                return "Invalid password reset request.";
            }

            var result = await userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.NewPassword);

            if (result.Succeeded)
            {
                return "Password reset successfully.";
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return $"Password reset failed: {errors}";
        }
    }
}

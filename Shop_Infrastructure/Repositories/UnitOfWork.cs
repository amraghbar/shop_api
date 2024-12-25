using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using Shop_Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Shop_Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext appContext;
        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;
        private readonly IConfiguration configuration;
        private readonly ILogger<AccountRepository> logger;

        // تعريف الخاصية IAccountRepository

        public IItemsRepository ItemsRepository { get; }
        public ICartRepository CartRepository { get; }


        public UnitOfWork(AppDbContext appContext, UserManager<Users> userManager, SignInManager<Users> signInManager, IConfiguration configuration, ILogger<AccountRepository> logger)
        {
            this.appContext = appContext;
            this.logger = logger;
            ItemsRepository = new ItemsRepository(appContext);
            CartRepository = new CartRepository(appContext);

        }

        public void Dispose()
        {
            appContext.Dispose();
        }

        public async Task<int> saveAsync()
        {
            return await appContext.SaveChangesAsync();
        }
    }
}

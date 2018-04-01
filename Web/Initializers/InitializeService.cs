using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database.EntityFrameworkCore;
using Domain.DataBaseModels.Goods;
using Domain.DataBaseModels.Identity;
using Domain.Identity;
using Domain.ServiceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Servises.Interfaces.AuthenticationServices;
using Servises.Interfaces.Products;

namespace Web.Initializers
{
    public static class InitializeService
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleService = services.GetRequiredService<IRoleService>();
            var userService = services.GetRequiredService<IUserService>();
            var shopModesService = services.GetRequiredService<IShopModesService>();
            var json = File.ReadAllText("admin.json");
            var adminModel = JsonConvert.DeserializeObject<AdminModel>(json);
            await RoleInitializeAsync(roleService, Roles.Admin);
            await AdminInitializeAsync(userService, adminModel);
            await ShopModesInitializeAsync(shopModesService);
        }

        private static async Task ShopModesInitializeAsync(IShopModesService shopModesService)
        {
            var modes = await shopModesService.GetListAsync();
            if (modes.All(e => e.Name != ShopMods.Сontinually))
            {
                await shopModesService.CreateAsync(new ShopMode() {Name = ShopMods.Сontinually});
            }
            if (modes.All(e => e.Name != ShopMods.OnWeekdays))
            {
                await shopModesService.CreateAsync(new ShopMode() { Name = ShopMods.OnWeekdays });
            }
            if (modes.All(e => e.Name != ShopMods.DuringTheDay7Days))
            {
                await shopModesService.CreateAsync(new ShopMode() { Name = ShopMods.DuringTheDay7Days });
            }
            if (modes.All(e => e.Name != ShopMods.DuringTheDayNotWorkOnWeekends))
            {
                await shopModesService.CreateAsync(new ShopMode() { Name = ShopMods.DuringTheDayNotWorkOnWeekends });
            }
        }

        private static async Task RoleInitializeAsync(IRoleService roleService, string roleName)
        {
            var role = await roleService.GetByNameOrDafault(roleName);
            if (role == null)
            {
                var result = await roleService.CreateAsync(roleName);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First());
                }
            }
        }

        private static async Task AdminInitializeAsync(IUserService userService, AdminModel adminModel)
        {
            var defAdmin = await userService.GetByUserNameOrDefaultAsync(adminModel.UserName);
            if (defAdmin == null)
            {
                defAdmin = new ApplicationUser
                {
                    UserName = adminModel.UserName,
                    Email = adminModel.Email
                };

                var result = await userService.CreateAsync(defAdmin, adminModel.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First());
                }

                result = await userService.AddToRoleAsync(defAdmin, "ADMIN");
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First());
                }
            }
        }
    }
}

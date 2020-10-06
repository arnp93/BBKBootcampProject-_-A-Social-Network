using BBKBootcampSocial.Core.IServices;
using BBKBootcampSocial.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BBKBootcampSocial.IoC
{
    public class DependencyContainer
    {

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}

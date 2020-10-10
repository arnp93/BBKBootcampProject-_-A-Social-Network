using BBKBootcampSocial.Core.IServices;
using BBKBootcampSocial.Core.Services;
using BBKBootcampSocial.Core.Utilities.Convertors;
using Microsoft.Extensions.DependencyInjection;

namespace BBKBootcampSocial.IoC
{
    public class DependencyContainer
    {

        public static void RegisterServices(IServiceCollection services)
        {
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMailSender, SendEmail>();
            services.AddScoped<IViewRenderService, RenderViewToString>();
            services.AddScoped<IPostService, PostService>();
        }
    }
}

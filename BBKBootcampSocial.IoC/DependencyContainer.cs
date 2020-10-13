using BBKBootcampSocial.Core.AllServices.IServices;
using BBKBootcampSocial.Core.AllServices.Services;
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
            services.AddScoped<ICommentService, CommentService>();
        }
    }
}

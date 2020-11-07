using System.Text;
using AutoMapper;
using BBKBootcampSocial.Core.DTOs.Account;
using BBKBootcampSocial.Core.DTOs.Comment;
using BBKBootcampSocial.Core.DTOs.Post;
using BBKBootcampSocial.DataLayer;
using BBKBootcampSocial.DataLayer.Implementations;
using BBKBootcampSocial.DataLayer.Interfaces;
using BBKBootcampSocial.Domains.Comment;
using BBKBootcampSocial.Domains.Post;
using BBKBootcampSocial.Domains.User;
using BBKBootcampSocial.IoC;
using BBKBootcampSocial.Web.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace BBKBootcampSocial.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            #region AutoMapper Configurations

            services.AddAutoMapper(configuration =>
            {
                configuration.CreateMap<RegisterUserDTO,User>();
                configuration.CreateMap<PostDTO, Post>();
                configuration.CreateMap<Post, ShowPostDTO>();
                configuration.CreateMap<Comment, CommentDTO>();
                configuration.CreateMap<NewCommentDTO, Comment>();
                configuration.CreateMap<User,LoginUserInfoDTO>();
                //configuration.CreateMap<LikeDTO, Like>();
                configuration.CreateMap<Like, LikeDTO>();

            }, typeof(Startup));

           
            #endregion

            #region Database Configurations

            services.AddDbContext<BBKDatabaseContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("BBKSocialConnection"));
                }
            );
           
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion

            #region Pass service to IoC Layer

            RegisterServices(services);

            #endregion

            #region Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:44317",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BBKBootCampIssuerKeyJWTByArashNP"))
                };
            });
            #endregion

            #region Enable CORS
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCors", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .Build();
                });
            });
            #endregion

            #region SignalR Config

            services.AddSignalR();

            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("EnableCors");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<NotificationHub>("/notificationHub");
            });

            #region SignalR Configuration

            //app.UseSignalR(route =>
            //{
            //    route.MapHub<NotificationHub>("/notificationHub");
            //});

            #endregion

        }
        public static void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }
    }
}

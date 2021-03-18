using API.Data;
using API.Helpers;
using API.Interfaces;
using API.MediatR.Comments;
using API.Services;
using API.SignalR;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, 
        IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            services.AddDbContext<DataContext>(x => {
                x.UseLazyLoadingProxies().UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<PresenceTracker>();

            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IMessageRepo, MessageRepo>();
            services.AddScoped<IUserService>();
            services.AddScoped<LogUserActivity>();
            
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddMediatR(typeof(List.Handler).Assembly);
            services.AddSignalR();
            
            return services;
        }
    }
}
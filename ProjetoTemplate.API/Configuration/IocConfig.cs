using ProjetoTemplate.BL.Authentication;
using ProjetoTemplate.BL.AzureStorage;
using ProjetoTemplate.BL.FindCep;
using ProjetoTemplate.BL.LayoutLot;
using ProjetoTemplate.BL.Profile;
using ProjetoTemplate.BL.Security;
using ProjetoTemplate.BL.SendEmail;
using ProjetoTemplate.BL;
using ProjetoTemplate.Data.Repository;
using ProjetoTemplate.Repository;
using Microsoft.EntityFrameworkCore;
using ProjetoTemplate.Domain.Helpers;
using Azure.Storage.Blobs;
using ProjetoTemplate.BL.Jwt;

namespace ProjetoTemplate.API.Configuration
{
    public static class IocConfig
    {
        public static IServiceCollection IocResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region INFRA
            services.AddDbContext<ProjetoTemplateDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString")));

            var appSettingsConfig = configuration.GetSection("LinkApplication").Get<AppSettingsConfig>();
            services.AddSingleton(appSettingsConfig);

            var azureConfig = configuration.GetSection("AzureConfig").Get<AzureConfig>();
            services.AddSingleton(azureConfig);

            services.AddSingleton(x => new BlobServiceClient(configuration["AzureConfig:StorageAccount"]));

            services.AddSingleton<IJwtFactory, JwtFactory>();

            #endregion

            #region SERVICES

            // Registro de BOs (Business Objects)
            services.AddScoped<ISecurityBO, SecurityBO>();
            services.AddScoped<IAzureStorageBO, AzureStorageBO>();
            services.AddScoped<ISendEmailBO, SendEmailBO>();
            services.AddScoped<IFindCepBO, FindCepBO>();
            services.AddScoped<ILayoutColumnsBO, LayoutColumnsBO>();
            services.AddScoped<IAuthenticationBO, AuthenticationBO>();
            services.AddScoped<IUserBO, UserBO>();
            services.AddScoped<IProfileBO, ProfileBO>();

            #endregion

            return services;
        }
    }
}

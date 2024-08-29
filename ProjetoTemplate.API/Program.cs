using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Globalization;
using ProjetoTemplate.API.Configuration;
using ProjetoTemplate.Repository;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do CORS
var origins = builder.Configuration["Cors:CorsOrigins"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder
        .WithOrigins(origins)
        .SetIsOriginAllowed((host) => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        ;
    });
});


// Registro de outros servi�os e depend�ncias
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.IocResolveDependencies(builder.Configuration);

// Adiciona outras configura��es necess�rias
builder.Services.AddOptions();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddJwtConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddDirectoryBrowser();
builder.Services.AddQuartzConfig(builder.Configuration);
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR");
    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-BR"), new CultureInfo("pt-BR") };
});

var app = builder.Build();

var enviroment = app.Environment.EnvironmentName;

app.UseDeveloperExceptionPage();
app.UseExceptionHandler("/error");
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = $"API {enviroment}";
});

UpdateDatabase(app);

app.UseCors("EnableCORS");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
    RequestPath = new PathString("/wwwroot")
});

app.MapControllers();

app.Run();

static void UpdateDatabase(IApplicationBuilder app)
{
    using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
    {
        using (var context = serviceScope.ServiceProvider.GetService<ProjetoTemplateDbContext>())
        {
            context.Database.Migrate();
        }
    }
}

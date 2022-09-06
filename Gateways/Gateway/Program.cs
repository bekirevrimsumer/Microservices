using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationSchema", options =>
{
    options.Authority = builder.Configuration["IdentityServerUrl"];
    options.Audience = "resources_gateway";
    options.RequireHttpsMetadata = false;
});

builder.Services.AddOcelot();

await app.UseOcelot();

Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"configuration.{hostingContext.HostingEnvironment.EnvironmentName.ToLower()}.json")
        .AddEnvironmentVariables();
});

app.Run();
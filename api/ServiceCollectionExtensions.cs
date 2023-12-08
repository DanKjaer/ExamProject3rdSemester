using System.Text;
using Azure.Storage.Blobs;
using infrastructure.datamodels;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.OpenApi.Models;
using service.Services;

namespace api;

public static class ServiceCollectionExtensions
{
    public static void AddJwtService(this IServiceCollection services)
    {
        // Add a singleton instance of JwtOptions to the service collection
        services.AddSingleton<JwtOptions>(services =>
        {
            // Obtain the IConfiguration service from the service collection
            var configuration = services.GetRequiredService<IConfiguration>();
        
            // Setting variables for the JwtOption
            byte[] secret = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JwtSecret")!);
            TimeSpan lifetime = TimeSpan.Parse(configuration["JWT:Lifetime"]!);
            
            // Creating JwtOptions with the variables we want, except address
            var options = JwtOptions.create(secret, lifetime);

            // If address isn't set in the config, use the server address as the issuer for JWT
            if (string.IsNullOrEmpty(options?.Address))
            {
                var server = services.GetRequiredService<IServer>();
                var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;
            
                // Set the JWT address to the first server address if available
                options.Address = addresses?.FirstOrDefault();
            }
            
            return options; // Return the configured JwtOptions
        });

        services.AddSingleton<JwtService>();
    }

    
    public static void AddSwaggerGenWithBearerJWT(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new string[] { }
                    }
                });
            }
        );
    }
    
    public static void AddAvatarBlobService(this IServiceCollection services)
    {
        services.AddSingleton<BlobService>(provider =>
        {
            var connectionString = provider.GetService<IConfiguration>()!
                .GetConnectionString("AvatarStorage");
            var client = new BlobServiceClient(connectionString);
            return new BlobService(client);
        });
    }
}
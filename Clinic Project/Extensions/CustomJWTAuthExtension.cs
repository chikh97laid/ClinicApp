using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Clinic_Project.Extensions
{
    public static class CustomJWTAuthExtension
    {
        public static void AddCustomJWTAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(o => { 
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o => { // to handl the token by this method
                o.RequireHttpsMetadata = false; // using https or http
                o.SaveToken = true; // to save tokens and invoking it later throw context.user
                o.TokenValidationParameters = new TokenValidationParameters() // this is the core object to validate token components
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // this is maybe should be <=5 to combination between systems delay
                };
            });
        }


        public static void AddSwaggerGenJWTAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(o => // o is a SwaggerGeneration object (composition object)
            {// open api responsible for providing json which is contained metadata(description) about our api
                o.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Test API",
                    Description = "Open API",
                    Contact = new OpenApiContact()
                    {
                        Name = "Api",
                        Email = "Api@gmail.com",
                        Url = new Uri("https://mydomain.com")
                    }
                });
                
                // this define an authorize way in openapi to swagger that names bearer and how to pass the token
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {// definition
                    Name = "Authorization", // name of the header
                    Type = SecuritySchemeType.Http, // type of sending , here http providing you to add bearer + ... token , so you can pass the token only
                    Scheme = "Bearer",
                    BearerFormat = "JWT", // header.payload.signature
                    In = ParameterLocation.Header,// send token through header
                    Description = "Enter the JWT key as: Bearer {Token}"
                });
                // this tells " system (bearer) is required when we call an authorized endpoint like (category in our case)
                o.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference() // this relates requirement with definition (we named Bearer)
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>() // an empty value because we don't use scopes like Auth2
                    }
                });
            });
        }
    }
}

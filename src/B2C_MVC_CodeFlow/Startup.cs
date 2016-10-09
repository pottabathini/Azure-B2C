using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;

namespace MvcHybrid
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "cookies",
                AutomaticAuthenticate = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(60)
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var oidcOptions = new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "cookies",

                Authority = Constants.BaseAddress,
                RequireHttpsMetadata = false,
                ClientId = "2048095b-a953-4150-9ab4-5a2f6334d99f",
                ClientSecret = "g\"2Mj/8m379Sdff4",
                //ResponseType = "token id_token",
                ResponseType = "code",
                SaveTokens = true,                                
                TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                }
            };
            oidcOptions.ProtocolValidator.RequireNonce = false;              
            oidcOptions.MetadataAddress = "https://login.microsoftonline.com/tfp/tenantazureb2c.onmicrosoft.com/B2C_1_sign_in/v2.0/.well-known/openid-configuration";           
            oidcOptions.Scope.Clear();
            oidcOptions.Scope.Add("openid 2048095b-a953-4150-9ab4-5a2f6334d99f");
            oidcOptions.Scope.Add("offline_access");            

            app.UseOpenIdConnectAuthentication(oidcOptions);
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

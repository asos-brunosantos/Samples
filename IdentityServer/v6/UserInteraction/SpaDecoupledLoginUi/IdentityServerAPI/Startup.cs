// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using IdentityServerAPI.Resources;

namespace IdentityServerAPI
{
    public class Startup
    {
        private const string _identityAPIAllowSpecificOrigins = "IdentityAPIAllowSpecificOrigins";
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var builder = services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/login.html";
                options.UserInteraction.ConsentUrl = "/consent.html";
                options.UserInteraction.LogoutUrl = "/logout.html";
                options.UserInteraction.ErrorUrl = "/error.html";

                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
                .AddTestUsers(TestUsers.Users);

            // in-memory, code config
            builder.AddInMemoryIdentityResources(Config.IdentityResources);
            builder.AddInMemoryClients(Config.Clients);

            //setup CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: _identityAPIAllowSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:5004")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(_identityAPIAllowSpecificOrigins);

            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
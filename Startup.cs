using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ShoeStore_Group9.Data;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Register the ApplicationDbContext with the DI container and configure it to use SQL Server.
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // Add controllers with views support (MVC pattern).
        services.AddControllersWithViews();

        // If you need to add more services like Identity or other middlewares, you can do so here.
        // For example:
        // services.AddIdentity<ApplicationUser, IdentityRole>()
        //         .AddEntityFrameworkStores<ApplicationDbContext>()
        //         .AddDefaultTokenProviders();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Development exception page for detailed errors
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            // Disable Browser Link to prevent interference with CSS
            // Browser Link is commented out to avoid injecting unwanted classes
            // app.UseBrowserLink();
        }
        else
        {
            // Global exception handler and HSTS in production
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // Use HTTPS redirection for security
        app.UseHttpsRedirection();

        // Serve static files (e.g., CSS, JavaScript, images)
        app.UseStaticFiles();

        // Enable routing
        app.UseRouting();

        // Authorization middleware (can be extended with Authentication if needed)
        app.UseAuthorization();

        // Endpoint routing configuration
        app.UseEndpoints(endpoints =>
        {
            // Default route configuration
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        // If you have Razor Pages, you can also include:
        // endpoints.MapRazorPages();
    }
}

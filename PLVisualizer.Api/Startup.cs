using ApiUtils.ContainerConfiguration;
using ApiUtils.Middlewares;

namespace PlVisualizer;

public class Startup
{
    private IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureLogicServices()
            .ConfigureLogger();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(options =>
        {
            options.AddPolicy(CorsConfigurationName, policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseCors(CorsConfigurationName);
        app.UseHttpsRedirection();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<ExceptionsMiddleware>();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    private const string CorsConfigurationName = "AllowOrigins";
}
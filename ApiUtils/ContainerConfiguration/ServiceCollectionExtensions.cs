using Loggers;
using Loggers.NLog;
using Microsoft.Extensions.DependencyInjection;
using PLVisualizer.BusinessLogic.Clients.DocxClient;
using PLVisualizer.BusinessLogic.Clients.ExcelClient;
using PLVisualizer.BusinessLogic.Clients.GoogleClient;
using PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;
using PLVisualizer.BusinessLogic.Services;

namespace ApiUtils.ContainerConfiguration;
    
public static class ContainerConfiguration
{
    /// <summary>
    /// Configures logger
    /// </summary>
    public static IServiceCollection ConfigureLogger(this IServiceCollection services)
    {
        services.AddSingleton<ILogger>(new NLogLogger("Default"));
        return services;
    }
    
    /// <summary>
    /// Configures custom services of application
    /// </summary>
    public static IServiceCollection ConfigureLogicServices(this IServiceCollection services)
    {
        services.AddTransient<ISpreadsheetProvider, SpreadsheetProvider>();

        services.AddTransient<IExcelClient, ExcelClient>();
        services.AddTransient<IDocxClient, DocxClient>();
        services.AddTransient<IGoogleClient, GoogleClient>();

        services.AddTransient<ITablesService, TablesService>();

        return services;
    }
}
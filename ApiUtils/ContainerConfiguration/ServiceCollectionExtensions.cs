using Microsoft.Extensions.DependencyInjection;
using PLVisualizer.BusinessLogic.Clients.DocxClient;
using PLVisualizer.BusinessLogic.Clients.SpreadsheetsClient;
using PLVisualizer.BusinessLogic.Clients.XlsxClient;
using PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;
using PLVisualizer.BusinessLogic.Services;

namespace ApiUtils.ContainerConfiguration;

public static class ContainerConfiguration
{
    public static IServiceCollection ConfigureLogicServices(this IServiceCollection services)
    {
        services.AddTransient<ISpreadsheetProvider, SpreadsheetProvider>();

        services.AddTransient<IXlsxClient, XlsxClient>();
        services.AddTransient<IDocxClient, DocxClient>();
        services.AddTransient<ISpreadsheetsClient, SpreadsheetsClient>();

        services.AddTransient<ITablesService, TablesService>();

        return services;
    }
}
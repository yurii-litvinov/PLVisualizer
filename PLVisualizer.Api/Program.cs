using DocumentFormat.OpenXml.Packaging;
using PLVisualizer.BusinessLogic.Clients.DocxClient;
using PLVisualizer.BusinessLogic.Clients.SpreadsheetsClient;
using PLVisualizer.BusinessLogic.Clients.XlsxClient;
using PLVisualizer.BusinessLogic.Providers.SpreadsheetProvider;
using PLVisualizer.BusinessLogic.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddTransient<IXlsxClient, XlsxClient>();
builder.Services.AddTransient<IDocxClient, DocxClient>();
builder.Services.AddTransient<ISpreadsheetsClient, SpreadsheetsClient>();
builder.Services.AddTransient<ISpreadsheetProvider, SpreadsheetProvider>();

builder.Services.AddTransient<ITablesService, TablesService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();
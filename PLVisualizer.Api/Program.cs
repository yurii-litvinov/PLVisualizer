var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();



app.MapGet("/", () => "bye world");

app.Run();
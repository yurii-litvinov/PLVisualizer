var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();



app.MapGet("/", () => "Hello phaser -->>>>>>>>>");

app.Run();
FROM mcr.microsoft.com/dotnet/sdk:6.0@sha256:dd19f6aa2774de9fde18c78970bc4fdebc695bd824c73371b6faec306a18b230 AS build

WORKDIR /app
COPY ["PLVisualizer.Api/PLVisualizer.Api.csproj", "PLVisualizer.Api/"]
COPY ["PLVisualizer.Api.Dto/PLVisualizer.Api.Dto.csproj", "PLVisualizer.Api.Dto/"]
COPY ["PLVisualizer.BusinessLogic/PLVisualizer.BusinessLogic.csproj", "PLVisualizer.BusinessLogic/"]
COPY ["ApiUtils/ApiUtils.csproj", "ApiUtils/"]
COPY ["curriculum-parser/CurriculumParser/CurriculumParser.csproj", "CurriculumParser/"]
COPY ["Loggers/Loggers.csproj", "Loggers/"]
RUN dotnet restore "/app/PLVisualizer.Api/PLVisualizer.Api.csproj"
RUN dotnet publish "/app/PLVisualizer.Api/PLVisualizer.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0@sha256:6ca5c440d36869d4b83059cf16f111bb4dec371c08b6e935186cc696e89cc0ba AS final 
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "publish/PLVisualizer.Api.dll"]




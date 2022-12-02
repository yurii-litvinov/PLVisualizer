FROM mcr.microsoft.com/dotnet/sdk:6.0@sha256:dd19f6aa2774de9fde18c78970bc4fdebc695bd824c73371b6faec306a18b230 AS build
RUN dotnet dev-certs https
WORKDIR /app

WORKDIR /source
COPY ["PLVisualizer.Api/PLVisualizer.Api.csproj", "PLVisualizer.Api/"]
COPY ["PLVisualizer.Api.Dto/PLVisualizer.Api.Dto.csproj", "PLVisualizer.Api.Dto/"]
COPY ["PLVisualizer.BusinessLogic/PLVisualizer.BusinessLogic.csproj", "PLVisualizer.BusinessLogic/"]
#copy working plans preserving file structure
COPY ["PLVisualizer.BusinessLogic/Clients/DocxClient/WorkingPlans", "/PLVisualizer.BusinessLogic/Clients/DocxClient/WorkingPlans"]
COPY ["ApiUtils/ApiUtils.csproj", "ApiUtils/"]
COPY ["curriculum-parser/CurriculumParser/CurriculumParser.csproj", "CurriculumParser/"]
COPY ["Loggers/Loggers.csproj", "Loggers/"]
RUN dotnet restore "PLVisualizer.Api/PLVisualizer.Api.csproj"
COPY . .

FROM build AS publish
WORKDIR /source/PLVisualizer.Api
RUN dotnet publish "PLVisualizer.Api.csproj" -c Release -o /app/PLVisualizer.Api

FROM mcr.microsoft.com/dotnet/aspnet:6.0@sha256:6ca5c440d36869d4b83059cf16f111bb4dec371c08b6e935186cc696e89cc0ba
#restore working plans preserving file structure
COPY --from=build /PLVisualizer.BusinessLogic /PLVisualizer.BusinessLogic
WORKDIR /app
COPY --from=publish /app/PLVisualizer.Api PLVisualizer.Api

WORKDIR /app/PLVisualizer.Api
ENTRYPOINT ["dotnet", "PLVisualizer.Api.dll"]




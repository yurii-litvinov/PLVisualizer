FROM mcr.microsoft.com/dotnet/sdk:6.0@sha256:dd19f6aa2774de9fde18c78970bc4fdebc695bd824c73371b6faec306a18b230 AS build

WORKDIR /api
COPY PLVisualizer.Api.csproj .
RUN dotnet restore

FROM build AS publish
RUN dotnet publish -c Release -o /api/publish

FROM publish AS final 
WORKDIR /api
COPY --from=publish /api/publish .

ENTRYPOINT ["dotnet", "PLVisualizer.Api.dll"]




FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY . ./

WORKDIR "/app/Web/YPermitin.FIASToolSet.API"
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/publish .

WORKDIR "/app"
ENTRYPOINT ["dotnet", "YPermitin.FIASToolSet.API.dll"]
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build

WORKDIR /app

COPY . .
RUN dotnet publish -c Release -o out SmartProxyWebApplication/SmartProxyWebApplication.csproj


FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime

WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "SmartProxyWebApplication.dll"]
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /app
COPY atelier-platform-aplicaciones-web/*.csproj atelier-platform-aplicaciones-web/
RUN dotnet restore ./atelier-platform-aplicaciones-web
COPY . .
RUN dotnet publish ./atelier-platform-aplicaciones-web -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=builder /app/out .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "atelier-platform-aplicaciones-web.dll"]
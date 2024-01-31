# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY *.sln .
COPY BakaMangaAPI/. ./BakaMangaAPI/
COPY /etc/secrets/appsettings.Production.json ./BakaMangaAPI/
WORKDIR /source/BakaMangaAPI
RUN dotnet publish -c release -o /app

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "BakaMangaAPI.dll"]

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env
WORKDIR /src
COPY ["/Server/Server.csproj", "Server/"]
COPY ["/Core/Core.csproj", "Core/"]
RUN dotnet restore "Server/Server.csproj"

COPY Server Server
COPY Core Core
WORKDIR "/src/Server"
RUN dotnet build "Server.csproj" -c Release -o /app

RUN dotnet publish "Server.csproj" -c Release -o /app


# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /app .
# ENV DOTNET_URLS=http://+:5000

ENTRYPOINT ["dotnet", "Server.dll"]
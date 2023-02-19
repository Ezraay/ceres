FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /src
COPY ["/Server/Server.csproj", "Server/"]
COPY ["/Core/Core.csproj", "Core/"]
RUN dotnet restore "Server/Server.csproj"

COPY . .
WORKDIR "/src/Server"
RUN dotnet build "Server.csproj" -c Release -o /app

RUN dotnet publish "Server.csproj" -c Release -o /app

ENV DOTNET_URLS=http://+:5000
WORKDIR /app
ENTRYPOINT ["dotnet", "Server.dll"]
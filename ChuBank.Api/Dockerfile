FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Note que agora os caminhos são ChuBank...
COPY ["ChuBank.Api/ChuBank.Api.csproj", "ChuBank.Api/"]
COPY ["ChuBank.Application/ChuBank.Application.csproj", "ChuBank.Application/"]
COPY ["ChuBank.Domain/ChuBank.Domain.csproj", "ChuBank.Domain/"]
COPY ["ChuBank.Infrastructure/ChuBank.Infrastructure.csproj", "ChuBank.Infrastructure/"]

RUN dotnet restore "ChuBank.Api/ChuBank.Api.csproj"

COPY . .
WORKDIR "/src/ChuBank.Api"
RUN dotnet publish "ChuBank.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ChuBank.Api.dll"]
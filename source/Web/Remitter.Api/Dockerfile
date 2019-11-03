FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Web/Remitter.Api/Remitter.Api.csproj", "Web/Remitter.Api/"]
RUN dotnet restore "Web/Remitter.Api/Remitter.Api.csproj"
COPY . .
WORKDIR "/src/Web/Remitter.Api"
RUN dotnet build "Remitter.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Remitter.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Remitter.Api.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG VERSION=0.0.0
ENV MINVERVERSIONOVERRIDE=$VERSION
WORKDIR /src
COPY . .
RUN dotnet restore "FacturXDotNet.API/FacturXDotNet.API.csproj"
RUN dotnet build "FacturXDotNet.API/FacturXDotNet.API.csproj" -c $BUILD_CONFIGURATION -o /app/build /p:Version=$VERSION

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FacturXDotNet.API/FacturXDotNet.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
ARG PORT=5295
ENV ASPNETCORE_HTTP_PORTS=$PORT
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FacturXDotNet.API.dll"]
HEALTHCHECK CMD curl --fail http://localhost:$PORT/health || exit

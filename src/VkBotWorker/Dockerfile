FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/VkBotWorker/VkBotWorker.csproj", "src/VkBotWorker/"]
RUN dotnet restore "src/VkBotWorker/VkBotWorker.csproj"
COPY . .
WORKDIR "/src/src/VkBotWorker"
RUN dotnet build "VkBotWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "VkBotWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VkBotWorker.dll"]

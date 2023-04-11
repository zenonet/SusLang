FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SusLang.DiscordBot/SusLang.DiscordBot.csproj", "SusLang.DiscordBot/"]
RUN dotnet restore "SusLang.DiscordBot/SusLang.DiscordBot.csproj"
COPY . .
WORKDIR "/src/SusLang.DiscordBot"
RUN dotnet build "SusLang.DiscordBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SusLang.DiscordBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SusLang.DiscordBot.dll"]

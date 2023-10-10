FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SusLang.Discord/SusLang.Discord.csproj", "SusLang.Discord/"]
RUN dotnet restore "SusLang.Discord/SusLang.Discord.csproj"
COPY . .
WORKDIR "/src/SusLang.Discord"
RUN dotnet build "SusLang.Discord.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SusLang.Discord.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SusLang.Discord.dll"]

#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY ./DiscordBot/*.csproj ./
COPY ./DiscordBot/NuGet.Config ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "DiscordBot.dll"]

#FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
#WORKDIR ./
#COPY ["DiscordBot.csproj", "DiscordBot/"]
#COPY ["NuGet.Config", "DiscordBot/"]
#RUN dotnet restore "DiscordBot/DiscordBot.csproj"
#COPY . .
#WORKDIR ./
#RUN dotnet build "DiscordBot/DiscordBot.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "DiscordBot/DiscordBot.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "DiscordBot.dll"]
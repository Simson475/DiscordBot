FROM mcr.microsoft.com/dotnet/sdk:latest AS build-env
WORKDIR /app

COPY ./DiscordBot/*.csproj ./
COPY ./DiscordBot/NuGet.Config ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out -r linux-arm

FROM --platform linux/arm/v7 mcr.microsoft.com/dotnet/aspnet:latest
WORKDIR /app
COPY --from=build-env /app/out .

#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["TwitterPoc/TwitterPoc.csproj", "TwitterPoc/"]
RUN dotnet restore "TwitterPoc/TwitterPoc.csproj"
COPY . .
WORKDIR "/src/TwitterPoc"
RUN dotnet build "TwitterPoc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TwitterPoc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TwitterPoc.dll"]
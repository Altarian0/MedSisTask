FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FileApi/FileApi.csproj", "FileApi/"]
RUN dotnet restore "FileApi/FileApi.csproj"
COPY . .
WORKDIR "/src/FileApi"
RUN dotnet build "FileApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileApi.dll"]

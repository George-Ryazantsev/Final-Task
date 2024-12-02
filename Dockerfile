FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ProjectName/ProjectName.csproj", "ProjectName/"]
RUN dotnet restore "ProjectName/ProjectName.csproj"
COPY . .
WORKDIR "/src/ProjectName"
RUN dotnet build "ProjectName.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectName.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectName.dll"]
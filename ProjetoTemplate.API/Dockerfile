#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ProjetoTemplate.API/ProjetoTemplate.API.csproj", "ProjetoTemplate.API/"]
RUN dotnet restore "ProjetoTemplate.API/ProjetoTemplate.API.csproj"
COPY . .
WORKDIR "/src/ProjetoTemplate.API"
RUN dotnet build "ProjetoTemplate.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjetoTemplate.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjetoTemplate.API.dll"]

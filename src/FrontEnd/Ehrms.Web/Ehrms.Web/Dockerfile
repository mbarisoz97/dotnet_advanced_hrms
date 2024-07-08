#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FrontEnd/Ehrms.Web/Ehrms.Web/Ehrms.Web.csproj", "src/FrontEnd/Ehrms.Web/Ehrms.Web/"]
COPY ["src/FrontEnd/Ehrms.Web/Ehrms.Web.Client/Ehrms.Web.Client.csproj", "src/FrontEnd/Ehrms.Web/Ehrms.Web.Client/"]
RUN dotnet restore "./src/FrontEnd/Ehrms.Web/Ehrms.Web/Ehrms.Web.csproj"
COPY . .
WORKDIR "/src/src/FrontEnd/Ehrms.Web/Ehrms.Web"
RUN dotnet build "./Ehrms.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ehrms.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ehrms.Web.dll"]
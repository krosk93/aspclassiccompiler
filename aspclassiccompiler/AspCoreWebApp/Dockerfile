#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine3.12 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine3.12 AS build
WORKDIR /src
COPY ["AspCoreWebApp/AspCoreWebApp.csproj", "AspCoreWebApp/"]
COPY ["AspCore/AspCore.csproj", "AspCore/"]
COPY ["VBScript/VBScript.VS2013.csproj", "VBScript/"]
COPY ["Parser/Dlrsoft.VBScript.Parser.vbproj", "Parser/"]
RUN dotnet restore "AspCoreWebApp/AspCoreWebApp.csproj"
COPY . .
WORKDIR "/src/AspCoreWebApp"
RUN dotnet build "AspCoreWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspCoreWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspCoreWebApp.dll"]
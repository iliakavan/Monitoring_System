#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM harbor.saminray.com/base-images/aspnet:8.0 AS base
#USER app
WORKDIR /app
EXPOSE 80
#EXPOSE 8081

FROM harbor.saminray.com/base-images/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/notifier/notifier.csproj", "Src/notifier/"]
COPY ["Src/notifier.Application/notifier.Application.csproj", "Src/notifier.Application/"]
COPY ["Src/notifier.Domain/notifier.Domain.csproj", "Src/notifier.Domain/"]
COPY ["Src/notifier.Infrastructure/notifier.Infrastructure.csproj", "Src/notifier.Infrastructure/"]
RUN dotnet restore "./Src/notifier/./notifier.csproj"
COPY . .
WORKDIR "/src/Src/notifier"
RUN dotnet build "./notifier.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./notifier.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "notifier.dll"]

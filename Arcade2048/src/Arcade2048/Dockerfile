FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["Arcade2048/src/Arcade2048/Arcade2048.csproj", "./Arcade2048/src/Arcade2048/"]
COPY ["SharedKernel/SharedKernel.csproj", "./SharedKernel/"]
RUN dotnet restore "./Arcade2048/src/Arcade2048/Arcade2048.csproj"

# Copy everything else and build
COPY . ./
RUN dotnet build "Arcade2048/src/Arcade2048/Arcade2048.csproj" -c Release -o /app/build

FROM build-env AS publish
RUN dotnet publish "Arcade2048/src/Arcade2048/Arcade2048.csproj" -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "/app/Arcade2048.dll"]

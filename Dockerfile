FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY BookingService/*.csproj BookingService/
COPY BookingService.Tests/*.csproj BookingService.Tests/
RUN dotnet restore BookingService/BookingService.csproj 
RUN dotnet restore BookingService.Tests/BookingService.Tests.csproj


# copy and build app
COPY BookingService/ BookingService/
WORKDIR /source/BookingService
RUN dotnet build -c release --no-restore

# test stage -- exposes optional entrypoint
# target entrypoint with: docker build --target test
FROM build AS test
WORKDIR /source/BookingService.Tests
COPY BookingService.Tests/ .
ENTRYPOINT ["dotnet", "test", "--logger:console;verbosity=detailed"]

FROM build AS publish
WORKDIR /source/BookingService
RUN dotnet publish -c release --no-build -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BookingService.dll"]
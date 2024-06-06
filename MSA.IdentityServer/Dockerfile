# Use the .NET 8.0 runtime image
FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001
EXPOSE 15001

RUN mkdir -p startup
COPY ./aspnet/https/update-ca-certificate-identity.sh /startup/
RUN chmod +x /startup/update-ca-certificate-identity.sh

# Use the .NET 8.0 SDK image for the build
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src
COPY ["./MSA.IdentityServer/MSA.IdentityServer.csproj", "src/MSA.IdentityServer/"]
RUN dotnet restore "src/MSA.IdentityServer/MSA.IdentityServer.csproj"
COPY . .
WORKDIR "/src/MSA.IdentityServer"
RUN dotnet build "MSA.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MSA.IdentityServer.csproj" -c Release -o /app/publish --self-contained false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/startup/update-ca-certificate-identity.sh"]

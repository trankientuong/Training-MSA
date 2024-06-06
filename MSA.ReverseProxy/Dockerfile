# Use the .NET 8.0 runtime image
FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 18080

RUN mkdir -p startup
COPY ./aspnet/https/update-ca-certificate-reverseproxy.sh /startup/
RUN chmod +x /startup/update-ca-certificate-reverseproxy.sh

# Use the .NET 8.0 SDK image for the build
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src
COPY ["./MSA.ReverseProxy/MSA.ReverseProxy.csproj","src/MSA.ReverseProxy/"]
RUN dotnet restore "src/MSA.ReverseProxy/MSA.ReverseProxy.csproj"
COPY . .
WORKDIR "/src/MSA.ReverseProxy"
RUN dotnet build "MSA.ReverseProxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MSA.ReverseProxy.csproj" -c Release -o /app/publish --self-contained false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/startup/update-ca-certificate-reverseproxy.sh"]

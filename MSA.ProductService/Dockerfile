# Use the .NET 8.0 runtime image
FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5002
EXPOSE 15002

RUN mkdir -p startup
COPY ./aspnet/https/update-ca-certificate-product.sh /startup/
RUN chmod +x /startup/update-ca-certificate-product.sh

# Use the .NET 8.0 SDK image for the build
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src
COPY ["./MSA.ProductService/MSA.ProductService.csproj","src/MSA.ProductService/"]
COPY ["./MSA.Common.Contracts/MSA.Common.Contracts.csproj","src/MSA.Common.Contracts/"]
COPY ["./MSA.Common.Mongo/MSA.Common.Mongo.csproj","src/MSA.Common.Mongo/"]
COPY ["./MSA.Common.PostgresMassTransit/MSA.Common.PostgresMassTransit.csproj","src/MSA.Common.PostgresMassTransit/"]
COPY ["./MSA.Common.Security/MSA.Common.Security.csproj","src/MSA.Common.Security/"]
RUN dotnet restore "src/MSA.ProductService/MSA.ProductService.csproj"
COPY . .
WORKDIR "/src/MSA.ProductService"
RUN dotnet build "MSA.ProductService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MSA.ProductService.csproj" -c Release -o /app/publish --self-contained false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/startup/update-ca-certificate-product.sh"]

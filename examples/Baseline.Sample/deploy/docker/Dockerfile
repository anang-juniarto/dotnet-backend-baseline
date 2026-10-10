FROM mcr.microsoft.com/dotnet/sdk:10.0.201 AS build
WORKDIR /source
COPY . .
RUN dotnet restore Baseline.Sample.slnx
RUN dotnet build Baseline.Sample.slnx --configuration Release --no-restore
RUN dotnet test Baseline.Sample.slnx --configuration Release --no-build --no-restore
RUN dotnet publish src/Presentation/Baseline.Sample.WebApi/Baseline.Sample.WebApi.csproj --configuration Release --no-build --no-restore --self-contained false --output /artifacts/webapi

FROM scratch AS artifacts
COPY --from=build /artifacts/webapi /webapi

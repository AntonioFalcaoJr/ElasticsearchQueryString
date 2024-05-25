ARG ASPNET_VERSION="9.0-preview"
ARG SDK_VERSION="9.0-preview"
ARG BASE_ADRESS="mcr.microsoft.com/dotnet"

FROM $BASE_ADRESS/aspnet:$ASPNET_VERSION AS base
WORKDIR /app

EXPOSE 80

FROM $BASE_ADRESS/sdk:$SDK_VERSION AS build

COPY ./global.json ./
COPY ./nuget.config ./
COPY ./Directory.Packages.props ./

WORKDIR /src

COPY ./src/WebAPI/*.csproj ./WebAPI/
COPY ./src/Infrastructure.Search/*.csproj ./Infrastructure.Search/

RUN dotnet restore -v m ./WebAPI

COPY ./src/WebAPI/. ./WebAPI/
COPY ./src/Infrastructure.Search/. ./Infrastructure.Search/

WORKDIR /src/WebAPI
RUN dotnet build -c Release --no-restore -v m -o /app/build

FROM build AS publish
RUN dotnet publish -c Release --no-restore -v m -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]
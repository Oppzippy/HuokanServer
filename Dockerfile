FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build-env
COPY HuokanServer /opt/HuokanServer
WORKDIR /opt/HuokanServer
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /opt/HuokanServer
COPY --from=build-env /opt/HuokanServer/out .
ENTRYPOINT [ "dotnet", "HuokanServer.dll" ]

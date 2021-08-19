FROM mcr.microsoft.com/dotnet/sdk

COPY . /opt/huokan

WORKDIR /opt/huokan/HuokanServer.IntegrationTests

CMD bash docker/wait-for-it.sh "$DB_HOST:$DB_PORT" -- dotnet test --logger "console;verbosity=detailed"

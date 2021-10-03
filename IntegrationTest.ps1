dotnet test HuokanServer.IntegrationTests /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=".\TestResults\" /p:SkipAutoProps=true /p:Exclude="[*]HuokanServer.*Tests.*"
.\HuokanServer.IntegrationTests\TestResults\html\index.html

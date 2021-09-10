cd HuokanServer.IntegrationTests
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=".\TestResults\" /p:SkipAutoProps=true
.\TestResults\html\index.html
cd ..

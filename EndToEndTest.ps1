dotnet test HuokanServer.EndToEndTests /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=".\TestResults\" /p:SkipAutoProps=true /p:Exclude="[*]HuokanServer.*Tests.*"
.\HuokanServer.EndToEndTests\TestResults\html\index.html

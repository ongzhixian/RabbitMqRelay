# dotnet CLI

## Solution

dotnet new sln -n RabbitMqRelay

## Project

dotnet new console -n RabbitMqRelay.ServiceApp

dotnet sln .\RabbitMqRelay.sln add .\RabbitMqRelay.ServiceApp\RabbitMqRelay.ServiceApp.csproj 

dotnet add .\RabbitMqRelay.ServiceApp\RabbitMqRelay.ServiceApp.csproj 
dotnet add .\RabbitMqRelay.ServiceApp\RabbitMqRelay.ServiceApp.csproj package Microsoft.Extensions.Hosting.WindowsServices --version 3.1.18
dotnet add .\RabbitMqRelay.ServiceApp\RabbitMqRelay.ServiceApp.csproj package Microsoft.Extensions.Configuration --version 3.1.18
dotnet add .\RabbitMqRelay.ServiceApp\RabbitMqRelay.ServiceApp.csproj package Microsoft.Extensions.Configuration.Json --version 3.1.18
dotnet add .\RabbitMqRelay.ServiceApp\RabbitMqRelay.ServiceApp.csproj package Microsoft.Extensions.DependencyInjection --version 3.1.18

## Run
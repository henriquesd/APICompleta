# APICompleta
Aplicação em .NET CORE 2.2

## No projeto DevIO.Api:

### Install
AutoMapper e extensão para fazer injeção de dependência através do ASP.NET Core
```
install-package AutoMapper.Extensions.Microsoft.DependencyInjection
```

## No projeto DevIO.Api:

### Comando para criar migrations:
PowerShell:
```
Add-Migration Identity -Verbose -Context ApplicationDbContext
```
Console:
```
dotnet ef migrations add Identity --startup-project ..\DevIO.Api\ --context ApplicationDbContext
```

### Comando para atualizar o banco de dados
PowerShell:
```
Update-Database -Context ApplicationDbContext
Update-Database -Context MeuDbContext
```
Console:
```
dotnet ef database update --startup-project ..\DevIO.Api\ --context ApplicationDbContext
dotnet ef database update --startup-project ..\DevIO.Api\ --context MeuDbContext
```

### Pacotes para versionamento da API
```
install-package Microsoft.AspNetCore.Mvc.Versioning
```

Este pacote é sobre versionamento também, o qual irá ajudar a expor estes versionamentos, para que por exemplo a documentação entenda as versões da API:
```
install-package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
```

### Instalação do Swagger (para documentação da API)
```
Install-Package Swashbuckle.AspNetCore
```
Para visualizar a documentação no Swagger, execute a aplicação e acesse: https://localhost:xxxxx/swagger

------------------------------

## Projeto Angular

## Acesse pela pasta wwwroot\app\demo-webapi e execute:
```
npm i
```

## Executar a aplicação
```
ng s
```

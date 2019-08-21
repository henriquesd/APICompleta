# APICompleta
Aplicação em .NET CORE 2.2

## No projeto DevIO.Api:

### Install
AutoMapper e extensão para fazer injeção de dependência através do ASP.NET Core
```
install-package AutoMapper.Extensions.Microsoft.DependencyInjection
```


## No projeto DevIO.Data:

### Comando para atualizar o banco de dados
PowerShell:
```
Update-Database
```
Console:
```
dotnet ef database update --startup-project ..\DevIO.Api\
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
```
Console:
```
dotnet ef database update --startup-project ..\DevIO.Api\ --context ApplicationDbContext
```

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
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
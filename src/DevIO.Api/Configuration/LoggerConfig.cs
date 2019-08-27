using DevIO.Api.Extensions;
using Elmah.Io.AspNetCore;
using Elmah.Io.AspNetCore.HealthChecks;
using Elmah.Io.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DevIO.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // loga tudo, problemas, erros, etc;
            services.AddElmahIo(o =>
            {
                o.ApiKey = "API_KEY";
                o.LogId = new Guid("LOG_ID");
            });

            // Com a configuração abaixo, o Elmah vai logar agora os logs do ASP.NET Core, e não apenas as situações de problema que ele já faz na configuração acima;

            // O Elmah captura os erros do pipeline, do middleware, e não dos erros ou das informações do log do ASP.NET, porque ele não é um provider,
            // se quiser isto, precisa adicionar a configuração para conectar o Elmah com o log:
            //services.AddLogging(builder =>
            //{
            //    // configurar o Elmah aqui dentro como um provider dos logs do ASP.NET;
            //    // para isto precisa instalar o provider tool Elmah para os logs do ASP.NET;
            //    builder.AddElmahIo(o =>
            //    {
            //        o.ApiKey = "API_KEY";
            //        o.LogId = new Guid("LOG_ID");
            //    });
            //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning); // Se colocar Information, virá muita coisa;
            //});

            // Verifica se o banco está conectado, se não estiver responsendo a aplicação estará unhealthy;
            services.AddHealthChecks()
                .AddElmahIoPublisher("API_KEY", new Guid("LOG_ID"), "API Fornecedores") // configurando o Elmah, como o publisher dos health cheks; se alguma coisa acontece a aplicação por algum motivo parar, da para verificar por ele;
                .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            services.AddHealthChecksUI();

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            // app.UseHealthChecks("/api/hc");

            // Para acessar, execute a aplicação e adicione o "/api/hc" na url, ex: localhost:xxxx/api/hc
            app.UseHealthChecks("/api/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            // Para acessar, execute a aplicação e adicione o "api/hc-ui" na url, ex: localhost:xxxx/api/hc-ui
            app.UseHealthChecksUI(options => { options.UIPath = "/api/hc-ui"; });

            return app;
        }
    }
}

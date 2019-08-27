using Elmah.Io.AspNetCore;
using Elmah.Io.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DevIO.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services)
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

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}

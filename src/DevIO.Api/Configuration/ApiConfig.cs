using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace DevIO.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection WebApiConfig(this IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true; // Quando não tiver uma versão especificada (v1, v2, v3, etc), ele vai assumir a default;
                options.DefaultApiVersion = new ApiVersion(1, 0); // Seria a versão 1.0; poderia usar três números também;
                options.ReportApiVersions = true; // Quando for consumir essa API, ele vai passar no header do response informando se a API está ok ou se ela está obsoleta e já pode utilizar a versão superior;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // o padrão que vai ser utilizado para agrupar a versão das APIs;
                options.SubstituteApiVersionInUrl = true; // por exemplo, se tiver uma rota padrão, ele substitui o nro da versão, pela versão default por exemplo, e substitui na url;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true; // suprimindo a forma da validação da ViewModel automática (para validar manualmente);
            });

            // O CORS é aplicado pelo browser, e ele já vem no ASP.NET configurado para negar qualquer requisição de outra origem.
            // Então quando está adicionando CORS(addCors) na verdade está adicionando uma política para relaxar/facilitar este
            // acesso de fora. Se não quiser trabalhar com acesso externo de outras origens, simplesmente não precisa habilitar o CORS na sua aplicação;
            services.AddCors(options =>
            {
                // Em desenvolvimento está permitido tudo;
                options.AddPolicy("Development",
                        builder =>
                    builder
                    .AllowAnyOrigin() // permite qualquer origem (ou seja, domínios diferentes)
                    .AllowAnyMethod() // permite chamar qualquer método
                    .AllowAnyHeader() // permite utilizar qualquer tipo de header
                    .AllowCredentials()); // permite credenciais apresentadas por um usuário; isto não costuma funcionar muito bem porque é
                                          // muito fácil simular um outro usuário para fazer uma requisição; então quando permite credenciais, na verdade não está filtrando, está colocando uma barreira de impedimento que pode ser facilmente quberada;

                // Política padrão;
                //options.AddDefaultPolicy(
                //    builder =>
                //        builder
                //            .AllowAnyOrigin()
                //            .AllowAnyMethod()
                //            .AllowAnyHeader()
                //            .AllowCredentials());

                // Política de produção
                options.AddPolicy("Production",
                        builder =>
                    builder
                        .WithMethods("GET") // só vai permitir métodos com o verbo GET, então não pode fazer POST, PUT, DELETE;
                        // .WithMethods("GET", "PUT")
                        .WithOrigins("http://desenvolvedor.io") // apenas para a origem do site http://desenvolvedor.io;
                        // .WithOrigins("http://desenvolvedor.io", "")
                        .SetIsOriginAllowedToAllowWildcardSubdomains() // está setando permissão para subdomínios, então se uma outra aplicação estiver rodando no mesmo subdomínio da API, então também permite a comunicação;
                        // .WithHeaders(HeaderNames.ContentType, "x-custom-header") // definindo o tipo de header que vai aceitar;
                        .AllowAnyHeader());
            });
            // O CORS não é um recurso de segurança, até porque ele é para relaxar a segurança, ou seja, a sua aplicação está fechada e
            // você está abrindo uma possibilidade de acessarem ela de uma outra origem, através de uma política que você criou.
            // Então sua aplicação não vai ser mais segura utilizando CORS. É preciso tomar cuidado com a facilidade de acesso da sua aplicação.

            return services;
        }

        public static IApplicationBuilder UseMvcConfiguration(this IApplicationBuilder app)
        {
            // app.UseCors("Development");
            app.UseHttpsRedirection(); // caso a aplicação seja chamada via HTTP, ele irá redirecionar para HTTPS;
            app.UseMvc();

            return app;
        }
    }
}

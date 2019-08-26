using AutoMapper;
using DevIO.Api.Configuration;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MeuDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentityConfiguration(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.WebApiConfig();

            services.AddSwaggerConfig();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "My Api", Version = "v1" });
            //});

            services.ResolveDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCors("Production");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // O Hsts é um recurso de segurança, ele faz uso do Strict-Transport-Security-header, que nada mais é do que um header, é uma declaração, a chave e o valor
                // que você passa da sua aplicação pro client, e o client uma vez que recebe esse header ele vai entender que você só conversa HTTPS;
                // A intenção do Hsts é que uma vez que a sua aplicação foi acessada através de um browser, ela vai responder a primeira vez informando que só fala HTTPS,
                // então ela vai passar no header o Hsts; uma vez informado isso, o browser vai fazer um cache dessa informação, e não vai aceitar mais nada que não seja Https e entende que a aplicação também não vai;
                // a partir do momento que eles trocam essa informação, o browser ou outro meio que esteja acessando essa aplicação vai entender e não vai falar fora do HTTPS com essa aplicação;
                // Porém o Hsts tem uma "pequena brecha", ele só é estabelecido quando a conexão é feita via HTTPS, fora do HTTPs ele também não é feito, então se a sua aplicação ela for chamada
                // por um browser fora do HTTPS, ela não vai informar aqui que ela fala somente HTTPS, porque o Hsts não vai funcionar, e ai irá estabelecer uma conexão fora do meio seguro, para isso então
                // existe também um outro reforço de segurança, que é o UseHttpsRedirection (que está na classe ApiConfig) - então se alguém chamar sua aplicação via HTTP, automaticamente o ASP.NET Core
                // já faz o redirecionamento interno para HTTPS, então você terá um código 307 que é local redirection, ele mesmo vai redirecionar internamente, e a partir desse momento ele
                // estabele o HTTPS e ai sim ele faz a troca de informação de Hsts com o servidor;
                app.UseHsts();

                // A combinação de UseHttpsRedirection mais a utilização do UseHsts, vai dar a garantia de segurança que você precisa, porque ele sempre vai
                // redirecionar para um canal seguro HTTPS, e uma vez redirecionado para o HTTPS, ele não conversa mais via HTTP, só que pra isso a
                // primeira conexão via HTTPS precisa acontecer, por isso que o redirecionamento é muito importante.
                // Inclusive pode fazer também essa configuração de redirecionamento no próprio servidor (a maioria dos servidores permitem essa configuração);
            }

            app.UseAuthentication(); // este precisa sempre vir antes da configuração do MVC;

            // O CORS nunca pode ser chamado após o UseMvc, tem que ser sempre antes.

            app.UseMvcConfiguration();

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});

            app.UseSwaggerConfig(provider);
        }
    }
}

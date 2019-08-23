using DevIO.Api.Data;
using DevIO.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DevIO.Api.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
          IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddDefaultTokenProviders(); // este é para adicionar o recurso para poder gerar tokens, por exemplo, para resetar de senhas, para fazer autenticação de e-mail...;

            // JWT

            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                // estas duas configurações, está dizendo que é para que toda vez que for autenticar alguém, o padrão de autenticação é para gerar um Token,
                // e toda vez que for validar este token, que é o challenge (que é um desafio - irá usar para verificar se a pessoa está autenticada),
                // é com base também no token; está dizendo que basicamente a aplicação funciona via token;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                // Por questão de segurança, se você só estiver trabalhando com https, pode por true; se tem garantia que só vai trabalhar com https, pode usar true;
                // ele irá requerer que a pessoa que está chamando ele, está vindo já dentro de HTTPS para evitar também um ataque do tipo Man-in-the-Middle;
                x.RequireHttpsMetadata = false;

                // Se o token deve ser guardado no http authentication property após uma autenticação feita com sucesso;
                // é bom que ele guarde porque fica mais fácil para aplicação validar o usuário logado após a apresentação do token;
                x.SaveToken = true;

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // Ele vai validar se o Issuer (quem está emitindo), tem que ser o mesmo quando receber o token;
                    // no token vai ter de quem foi emitido aquele token; E essa validação vai ser feita com base na chave que você passou,
                    // não apenas no nome do issuer, e sim também na chave;
                    ValidateIssuerSigningKey = true,

                    // a chave está sendo configurada logo abaixo;
                    // SymmetricSecurityKey: é onde ele vai fazer a configuração da sua chave, ele vai transformar essa chave que está apenas
                    // codificada em ASCII, para uma chave criptografada;
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    // Ele vai validar apenas o Issuer, conforme o nome;
                    ValidateIssuer = true,

                    // Aonde que o seu token é válido, em qual audiência;
                    ValidateAudience = true,

                    // Além de setar os dois parâmetros anteriores como true, precisa também dizer qual é a sua Audience e qual é o seu Issuer;
                    // Estas duas informações abaixo vão vir no Token, e se o Token não tiver estas informações, ele não vai estar válido;
                    ValidAudience = appSettings.ValidoEm, // no caso aqui é o localhost (veja no appsettings.json) (ou o site da aplicação, a url);
                    ValidIssuer = appSettings.Emissor // o nome da sua aplicação;
                };
            });

            return services;
        }
    }
}

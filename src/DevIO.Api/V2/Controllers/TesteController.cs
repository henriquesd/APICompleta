using DevIO.Api.Controllers;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevIO.Api.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/teste")]
    public class TesteController : MainController
    {
        private readonly ILogger _logger;

        // ILogger<TesteController> logger - TesteController = objeto de quem está gerando o Log;
        public TesteController(INotificador notificador, IUser appUser, ILogger<TesteController> logger) : base(notificador, appUser)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Valor()
        {
            // estes dois primeiros devem ser utilizados apenas durante o desenvolvimento;

            // O log de Trace ele não gera porque a Microsoft desabilitou por padrão no ASP.NET Core, justamente para evitar que suba em produção um Trace que gera muito volume de dados,
            // mas é possível habilitar ele manualmente; porém se precisar fazer um Trace, o Information serve para isto também; Pode utilizar o information porque ele aceita qualquer coisa;
            _logger.LogTrace("Log de Trace"); // é para desenvolvimento; por exemplo, para marcar a hora que o processo começou e a hora que terminou;
            _logger.LogDebug("Log de Debug"); // informações de debug;

            // estes abaixo podem ser utilizados na aplicação;
            _logger.LogInformation("Log de Informação"); // vai gravar qualquer coisa que queira informar, que não seja nada de importante mas que queira registrar;
            _logger.LogWarning("Log de Aviso"); // quando existe alguma situação, por exemplo um erro 404, ou alguma coisa que não é um erro mas também não deveria acontecer;
            _logger.LogError("Log de Erro"); // propriamente o erro;
            _logger.LogCritical("Log de Problema Critico"); // o critical é alguma coisa mais séria, alguma coisa que ameaça a performance, a saúde da aplicação; o critical é um nível acima do error;

            return "Sou a V2";
        }
    }
}
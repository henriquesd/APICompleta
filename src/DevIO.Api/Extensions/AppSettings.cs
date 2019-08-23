namespace DevIO.Api.Extensions
{
    public class AppSettings
    {
        // a chave de criptografia do token;
        public string Secret { get; set; }
        
        // quantas horas o token vai levar até perder a validade;
        public int ExpiracaoHoras { get; set; }

        // quem emite, no caso é a aplicação;
        public string Emissor { get; set; }

        // em quais url's este token é valido;
        public string ValidoEm { get; set; }
    }
}

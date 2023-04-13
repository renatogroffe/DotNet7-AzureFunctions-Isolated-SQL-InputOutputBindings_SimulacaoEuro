using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using FunctionAppMoedas.Models;

namespace FunctionAppMoedas;

public class UltimosValoresCotacaoEuro
{
    private readonly ILogger _logger;

    public UltimosValoresCotacaoEuro(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<UltimosValoresCotacaoEuro>();
    }

    [Function(nameof(UltimosValoresCotacaoEuro))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
        [SqlInput("SELECT TOP 3 * FROM dbo.Cotacoes WHERE Sigla = 'EUR' ORDER BY Id DESC", "BaseMoedas")]
        IEnumerable<HistoricoCotacao> historicos)
    {
        _logger.LogInformation("Consultando ultimas cotacoes cadastradas...");
        _logger.LogInformation($"Numero de cotacoes mais recentes = {historicos.Count()}");
        
        var response = req.CreateResponse();
        response.StatusCode = HttpStatusCode.OK;
        await response.WriteAsJsonAsync(historicos);
        return response;
    }
}
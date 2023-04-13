using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using FunctionAppMoedas.Models;

namespace FunctionAppMoedas;

public static class SimularCotacaoEuro
{
    private const decimal VALOR_BASE = 5.43m;

    [Function(nameof(SimularCotacaoEuro))]
    [SqlOutput("dbo.Cotacoes", "BaseMoedas")]
    public static DadosCotacao Run([TimerTrigger("*/10 * * * * *")] FunctionContext context)
    {
        var logger = context.GetLogger(nameof(SimularCotacaoEuro));

        var cotacao = new DadosCotacao()
        {
            Sigla = "EUR",
            Horario = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
            Valor = Math.Round(VALOR_BASE + new Random().Next(0, 21) / 1000m, 3)
        };
        logger.LogInformation($"Dados gerados: {JsonSerializer.Serialize(cotacao)}");

        return cotacao;
    }
}
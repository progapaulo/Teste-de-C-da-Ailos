using System.Data;
using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;

namespace Questao5.Infrastructure.Database.CommandStore;

public class MovimentoRepository : IMovimentoRepository
{
    private readonly IDbConnection _dbConnection;

    public MovimentoRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<string> CriarMovimentoAsync(Movimento movimento)
    {
        var query = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                      VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";
        await _dbConnection.ExecuteAsync(query, movimento);
        return movimento.IdMovimento;
    }
}
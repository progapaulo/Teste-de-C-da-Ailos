using System.Data;
using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;

namespace Questao5.Infrastructure.Database.CommandStore;

public class IdempotenciaRepository : IIdempotenciaRepository
{
    private readonly IDbConnection _dbConnection;

    public IdempotenciaRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    
    public async Task<Idempotencia> GetIdempotencyResultAsync(string idempotencyKey)
    {
        var query = "select * from idempotencia WHERE chave_idempotencia = @Numero";
        
        return await _dbConnection.QueryFirstOrDefaultAsync<Idempotencia>(query, new { ChaveIdempotencia = idempotencyKey });
    }

    public async Task SaveIdempotencyAsync(string idempotencyKey, string request, string response)
    {
        var query = @"INSERT INTO idempotencia (chave_idempotencia, idempotencia, idempotencia) 
                      VALUES ("+ idempotencyKey +", "+ request +", "+ response +")";
        await _dbConnection.ExecuteAsync(query);
    }
}
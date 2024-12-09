using System.Data;
using System.Threading.Tasks;
using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;

namespace Questao5.Infrastructure.Database.CommandStore;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly IDbConnection _dbConnection;

    public ContaCorrenteRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ContaCorrente> ObterPorNumeroAsync(int numero)
    {
        var query = "SELECT * FROM contacorrente WHERE numero = @Numero";
        
        return await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(query, new { Numero = numero });
        
    }
    
    public async Task<decimal> ObterSaldoAsync(string idContaCorrente)
    {
        var query = @"SELECT 
                        SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE 0 END) - 
                        SUM(CASE WHEN tipomovimento = 'D' THEN valor ELSE 0 END) 
                      FROM movimento WHERE idcontacorrente = @IdContaCorrente";
        return await _dbConnection.ExecuteScalarAsync<decimal>(query, new { IdContaCorrente = idContaCorrente });
    }
}
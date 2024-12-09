using System.Threading.Tasks;
using Questao5.Domain.Entities;

namespace Questao5.Domain.Repositories;

public interface IIdempotenciaRepository
{
    Task<Idempotencia> GetIdempotencyResultAsync(string idempotencyKey);
    
    Task SaveIdempotencyAsync(string idempotencyKey, string request, string response);
}
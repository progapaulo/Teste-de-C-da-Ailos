using Questao5.Domain.Entities;

namespace Questao5.Domain.Repositories;

public interface IMovimentoRepository
{
    Task<string> CriarMovimentoAsync(Movimento movimento);
}
using System.Data;
using Dapper;
using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Repositories;

namespace Questao5.Application.Handlers;

public class ConsultaSaldoQueryHandler : IRequestHandler<ConsultarSaldoQuery, SaldoResponse>
{
    private readonly IContaCorrenteRepository _contaCorrenteRepository;

    public ConsultaSaldoQueryHandler(IContaCorrenteRepository contaCorrenteRepository)
    {
        _contaCorrenteRepository = contaCorrenteRepository;
    }

    public async Task<SaldoResponse> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
    {
        // Validações
        var conta = await _contaCorrenteRepository.ObterPorNumeroAsync(request.NumeroContaCorrente);
        if (conta == null)
            throw new ArgumentException("Conta não encontrada.", "INVALID_ACCOUNT");
        if (!conta.Ativo)
            throw new ArgumentException("Conta inativa.", "INACTIVE_ACCOUNT");

        var saldo = await _contaCorrenteRepository.ObterSaldoAsync(conta.IdContaCorrente);

        return new SaldoResponse
        {
            Numero = conta.Numero,
            Nome = conta.Nome,
            DataConsulta = DateTime.Now,
            Saldo = saldo
        };
    }
}
using System.Data;
using Dapper;
using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;

namespace Questao5.Application.Handlers;

public class MovimentacaoCommandHandler : IRequestHandler<MovimentacaoCommand, string>
{
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IMovimentoRepository _movimentoRepository;

    public MovimentacaoCommandHandler(IContaCorrenteRepository contaCorrenteRepository, IMovimentoRepository movimentoRepository)
    {
        _contaCorrenteRepository = contaCorrenteRepository;
        _movimentoRepository = movimentoRepository;
    }

    public async Task<string> Handle(MovimentacaoCommand request, CancellationToken cancellationToken)
    {
        // Validações
        var conta = await _contaCorrenteRepository.ObterPorNumeroAsync(request.NumeroContaCorrente);
        if (conta == null)
            throw new ArgumentException("Conta não encontrada.", "INVALID_ACCOUNT");
        if (!conta.Ativo)
            throw new ArgumentException("Conta inativa.", "INACTIVE_ACCOUNT");
        if (request.Valor <= 0)
            throw new ArgumentException("Valor inválido.", "INVALID_VALUE");
        if (request.TipoMovimento != 'C' && request.TipoMovimento != 'D')
            throw new ArgumentException("Tipo de movimento inválido.", "INVALID_TYPE");

        var movimento = new Movimento
        {
            IdMovimento = Guid.NewGuid().ToString(),
            IdContaCorrente = conta.IdContaCorrente,
            DataMovimento = DateTime.Now,
            TipoMovimento = request.TipoMovimento,
            Valor = request.Valor
        };

        return await _movimentoRepository.CriarMovimentoAsync(movimento);
    }
}
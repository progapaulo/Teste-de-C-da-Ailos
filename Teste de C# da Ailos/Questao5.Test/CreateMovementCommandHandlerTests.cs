using System.Data;
using Dapper;
using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Repositories;

namespace Questao5.Test;

public class CreateMovementCommandHandlerTests
{
    private readonly IContaCorrenteRepository _contaCorrenteRepository;
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly MovimentacaoCommandHandler _handler;

    public CreateMovementCommandHandlerTests()
    {
        _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
        _movimentoRepository = Substitute.For<IMovimentoRepository>();
        _handler = new MovimentacaoCommandHandler(_contaCorrenteRepository, _movimentoRepository);
    }
    
    [Fact]
    public async Task DeveRetornarErro_SeContaNaoExistir()
    {
        // Arrange
        var command = new MovimentacaoCommand { NumeroContaCorrente = 999, TipoMovimento = 'C', Valor = 100 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>("INVALID_ACCOUNT", () => _handler.Handle(command, CancellationToken.None));
    }
}
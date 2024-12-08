using MediatR;

namespace Questao5.Application.Commands.Requests;

public class MovimentacaoCommand : IRequest<string>
{
    public string ChaveIdempotencia { get; set; }
    public int NumeroContaCorrente { get; set; }
    public char TipoMovimento { get; set; }
    public decimal Valor { get; set; }
}
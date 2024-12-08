using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;

namespace Questao5.Infrastructure.Services.Controllers;

[ApiController]
[Route("[controller]")]
public class ContaCorrenteController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContaCorrenteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("movimentacao")]
    public async Task<IActionResult> MovimentarConta([FromBody] MovimentacaoCommand command)
    {
        try
        {
            var idMovimento = await _mediator.Send(command);
            return Ok(new { IdMovimento = idMovimento });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Mensagem = ex.Message, Tipo = ex.ParamName });
        }
    }

    
    [HttpGet("saldo/{numeroContaCorrente}")]
    public async Task<IActionResult> ConsultarSaldo(int numeroContaCorrente)
    {
        try
        {
            var response = await _mediator.Send(new ConsultarSaldoQuery { NumeroContaCorrente = numeroContaCorrente });
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Mensagem = ex.Message, Tipo = ex.ParamName });
        }
    }
}
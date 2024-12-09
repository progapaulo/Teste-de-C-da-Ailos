using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Repositories;

namespace Questao5.Infrastructure.Services.Controllers;

[ApiController]
[Route("[controller]")]
public class ContaCorrenteController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IIdempotenciaRepository _idempotenciaRepository;
    
    public ContaCorrenteController(IMediator mediator, IIdempotenciaRepository idempotenciaRepository)
    {
        _mediator = mediator;
        _idempotenciaRepository = idempotenciaRepository;
    }

    [HttpPost("movimentacao")]
    public async Task<IActionResult> MovimentarConta([FromBody] MovimentacaoCommand command)
    {
        try
        {
            var idempotencia = await _idempotenciaRepository.GetIdempotencyResultAsync(command.ChaveIdempotencia);
            if (idempotencia != null)
                return Ok(idempotencia.Resultado);

            var idMovimento = await _mediator.Send(command);
            
            await _idempotenciaRepository.SaveIdempotencyAsync(command.ChaveIdempotencia, 
                JsonConvert.SerializeObject(command), idMovimento);

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
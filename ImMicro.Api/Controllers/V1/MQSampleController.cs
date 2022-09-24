using System;
using System.Threading;
using System.Threading.Tasks;
using ImMicro.Common.Options;
using ImMicro.Contract.Consumer;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ImMicro.Api.Controllers.V1;

/// <summary>
/// MQTest Controller
/// </summary>
[ApiVersion("1.0")]
public class MQSampleController : BaseController
{
    //INFO: This is just sample usage, the correct way is to use this code in business flow; for the right test also consumer project need to work
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly RabbitMqOption _rabbitMqOptions;

    /// <summary>
    /// MQSample Controller
    /// </summary>
    /// <param name="sendEndpointProvider"></param>
    /// <param name="rabbitMqOptions"></param>
    public MQSampleController(ISendEndpointProvider sendEndpointProvider, IOptions<RabbitMqOption> rabbitMqOptions)
    {
        _rabbitMqOptions = rabbitMqOptions.Value;
        _sendEndpointProvider = sendEndpointProvider;
    }

    /// <summary>
    /// Get Product
    /// </summary>
    [HttpGet("test")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Test(CancellationToken cancellationToken)
    {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"{_rabbitMqOptions.RabbitMqUri}/{_rabbitMqOptions.SampleQueue}"));
        await endpoint.Send(new SampleConsumerCommand {RequestTime = DateTime.UtcNow}, cancellationToken);

        return Ok();
    }
}
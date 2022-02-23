using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Client;
using FrontEnd.TimerTest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecretsController : BaseController<SecretsController>
    {
        private readonly IConfiguration _configuration;
        public SecretsController(ILogger<SecretsController> logger, DaprClient daprClient, IConfiguration configuration) : base(logger,daprClient)
        {
            _configuration = configuration;
        }

        
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            Dictionary<string, string> secrets = await _daprClient.GetSecretAsync("secrets01", "RabbitMQConnectStr");
            return Ok(secrets["RabbitMQConnectStr"]);
        }

        [HttpGet("get01")]
        public async Task<ActionResult> Get01Async()
        {
            return Ok(_configuration["RabbitMQConnectStr"]);
        }

    }
}

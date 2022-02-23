using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PubSubController : Controller
    {
        private readonly ILogger<DaprController> _logger;
        private readonly DaprClient _daprClient;
        public PubSubController(ILogger<DaprController> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync()
        {
            Stream stream = Request.Body;
            byte[] buffer = new byte[Request.ContentLength.Value];
            stream.Position = 0L;
            await stream.ReadAsync(buffer, 0, buffer.Length);
            string content = Encoding.UTF8.GetString(buffer);
            _logger.LogInformation("sub" + content);
            return Ok(content);
        }

        [HttpGet("pub")]
        public async Task<ActionResult> PubAsync()
        {
            var data = new WeatherForecast();
            await _daprClient.PublishEventAsync<WeatherForecast>("pubsub6379", "hxs_topic", data);
            return Ok();
        }

        [HttpGet("pub_code")]
        public async Task<ActionResult> PubCodeAsync()
        {
            var data = new WeatherForecast();
            await _daprClient.PublishEventAsync<WeatherForecast>("rabbitmq-pubsub", "hxs_topic_bind", data);
            return Ok();
        }

        [Topic("rabbitmq-pubsub", "hxs_topic_bind")]
        [HttpPost("subcode")]
        public async Task<ActionResult> Sub()
        {
            Stream stream = Request.Body;
            byte[] buffer = new byte[Request.ContentLength.Value];
            stream.Position = 0L;
            await stream.ReadAsync(buffer, 0, buffer.Length);
            string content = Encoding.UTF8.GetString(buffer);
            _logger.LogInformation("hxs_topic_bind" + content);
            return Ok(content);
        }

    }
}

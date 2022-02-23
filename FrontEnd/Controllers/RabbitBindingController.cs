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
    public class RabbitBindingController : Controller
    {
        private readonly ILogger<DaprController> _logger;
        private readonly DaprClient _daprClient;
        public RabbitBindingController(ILogger<DaprController> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        [HttpPost]
        public ActionResult Post()
        {
            Stream stream = Request.Body;
            byte[] buffer = new byte[Request.ContentLength.Value];
            stream.Position = 0L;
            stream.ReadAsync(buffer, 0, buffer.Length);
            string content = Encoding.UTF8.GetString(buffer);
            _logger.LogInformation("-----binding:" + content);
            return Ok();
        }

        /*
         *  create           
         *  list
         *  get
         *  delete
         */
        [HttpGet("output")]
        public async Task<ActionResult> OutputAsync([FromServices] DaprClient daprClient)
        {
            await daprClient.InvokeBindingAsync("api/RabbitBinding/output", "create", "hxsceshi-001");
            //await daprClient.InvokeBindingAsync("api/RabbitBinding/output", "get", "hxsceshi-001");
            //_logger.LogInformation("-----binding:" + content);
            return Ok();
        }

        //[HttpPost]
        //public  ActionResult OutputValue()
        //{
        //    Stream stream = Request.Body;
        //    byte[] buffer = new byte[Request.ContentLength.Value];
        //    stream.Position = 0L;
        //    stream.ReadAsync(buffer, 0, buffer.Length);
        //    string content = Encoding.UTF8.GetString(buffer);
        //    _logger.LogInformation("-----binding:" + content);
        //    return Ok();
        //    //await daprClient.InvokeBindingAsync("api/RabbitBinding/output", "get", "hxsceshi-001");
        //    //return Ok();
        //}

        //[HttpPost("cron")]
        //public ActionResult Cron()
        //{
        //    _logger.LogInformation(".............corn binding.............");
        //    return Ok();
        //}

    }
}

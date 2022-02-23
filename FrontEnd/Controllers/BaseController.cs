using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Client;
using FrontEnd.TimerTest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEnd.Controllers
{
    [ApiController]
    public class BaseController<T> : Controller
    {
        public readonly ILogger<T> _logger;
        public readonly DaprClient _daprClient;
        public BaseController(ILogger<T> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }       
    }
}

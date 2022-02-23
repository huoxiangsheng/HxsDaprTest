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
    [Route("api/[controller]")]
    public class ActorsController : Controller
    {
        private readonly ILogger<DaprController> _logger;
        private readonly DaprClient _daprClient;
        public ActorsController(ILogger<DaprController> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult> ApproveAsync(string orderId)
        {
            var actorId = new ActorId("actorprifix-" + orderId);
            var proxy = ActorProxy.Create<IWorkTimerActor>(actorId, "WorkTimerActor");

            return Ok(await proxy.Approve());
        }

        #region Timer
        [HttpGet("timer/{orderId}")]
        public async Task<ActionResult> TimerAsync(string orderId)
        {
            var actorId = new ActorId("actortimer-" + orderId);
            var proxy = ActorProxy.Create<IWorkTimerActor>(actorId, "WorkTimerActor");
            await proxy.RegisterTimer();
            return Ok("done");
        }

        [HttpGet("unregist/timer/{orderId}")]
        public async Task<ActionResult> UnregistTimerAsync(string orderId)
        {
            var actorId = new ActorId("actortimer-" + orderId);
            var proxy = ActorProxy.Create<IWorkTimerActor>(actorId, "WorkTimerActor");
            await proxy.UnregisterTimer();
            return Ok("done");
        }
        #endregion

        #region Reminder
        [HttpGet("reminder/{orderId}")]
        public async Task<ActionResult> ReminderAsync(string orderId)
        {
            var actorId = new ActorId("actorreminder-" + orderId);
            var proxy = ActorProxy.Create<IWorkTimerActor>(actorId, "WorkTimerActor");
            await proxy.RegisterReminder();
            return Ok("done");
        }

        [HttpGet("unregist/reminder/{orderId}")]
        public async Task<ActionResult> UnregistReminderAsync(string orderId)
        {
            var actorId = new ActorId("actorreminder-" + orderId);
            var proxy = ActorProxy.Create<IWorkTimerActor>(actorId, "WorkTimerActor");
            await proxy.UnregisterReminder();
            return Ok("done");
        }
        #endregion

    }
}

using Dapr.Actors.Runtime;
using Dapr.Client;
using FrontEnd.Controllers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FrontEnd.TimerTest
{
    public class WorkTimerActor : Actor, IWorkTimerActor
    {
        private readonly ILogger<DaprController> _logger;
        private readonly DaprClient _daprClient;
        public WorkTimerActor(ActorHost host, ILogger<DaprController> logger, DaprClient daprClient) : base(host)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        public async Task<bool> Approve()
        {
            await StateManager.AddOrUpdateStateAsync(Id.ToString(), "approve", (key, currentStatus) => "approve");
            return true;
        }

        #region Timer 重启服务的话，Timer不会自动重启
        public Task RegisterTimer()
        {
            var serializedTimerParams = JsonSerializer.SerializeToUtf8Bytes("now is " + DateTime.Now.ToString());
            return this.RegisterTimerAsync("TestTimer", nameof(this.TimerCallback), serializedTimerParams, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
        }

        public async Task TimerCallback(byte[] data)
        {
            var stateKey = "nowtime";
            var content = JsonSerializer.Deserialize<string>(data);
            _logger.LogInformation(" ---------" + content);
            await this.StateManager.SetStateAsync<string>(stateKey, content);
        }

        public Task UnregisterTimer()
        {
            return this.UnregisterTimerAsync("TestTimer");
        }
        #endregion

        #region Reminder 重启服务会自动重启
        public async Task RegisterReminder()
        {
            await this.RegisterReminderAsync("TestReminder", null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
        }

        public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            var stateKey = "nowtime";
            var content = "now is " + DateTime.Now.ToString();
            _logger.LogInformation(" reminder---------" + content);
            await this.StateManager.SetStateAsync<string>(stateKey, content);
        }

        public Task UnregisterReminder()
        {
            return this.UnregisterReminderAsync("TestReminder");
        }

        #endregion

    }
}

using Dapr.Client;
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
    public class StateController : Controller
    {
        private readonly ILogger<DaprController> _logger;
        private readonly DaprClient _daprClient;
        public StateController(ILogger<DaprController> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        const string STATE_STORE = "statestore";
        const string KEY_NAME = "guid";

        // 获取一个值
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var result = await _daprClient.GetStateAsync<string>(STATE_STORE, KEY_NAME);
            return Ok(result);
        }

        // 获取一个值和etag
        [HttpGet("withetag")]
        public async Task<ActionResult> GetWithEtagAsync()
        {
            var (value, etag) = await _daprClient.GetStateAndETagAsync<string>(STATE_STORE, KEY_NAME);
            return Ok($"value is {value}, etag is {etag}");
        }

        //保存一个值
        [HttpPost]
        public async Task<ActionResult> PostAsync()
        {
            await _daprClient.SaveStateAsync<string>(STATE_STORE, KEY_NAME, Guid.NewGuid().ToString(), new StateOptions() { Consistency = ConsistencyMode.Strong });
            return Ok("done");
        }

        //删除一个值
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync()
        {
            await _daprClient.DeleteStateAsync(STATE_STORE, KEY_NAME);
            return Ok("done");
        }

        //通过tag防止并发冲突，保存一个值
        [HttpPost("withtag")]
        public async Task<ActionResult> PostWithTagAsync()
        {
            var (value, etag) = await _daprClient.GetStateAndETagAsync<string>(STATE_STORE, KEY_NAME);
            await _daprClient.TrySaveStateAsync<string>(STATE_STORE, KEY_NAME, Guid.NewGuid().ToString(), etag);
            return Ok("done");
        }

        //通过tag防止并发冲突，删除一个值
        [HttpDelete("withtag")]
        public async Task<ActionResult> DeleteWithTagAsync()
        {
            var (value, etag) = await _daprClient.GetStateAndETagAsync<string>(STATE_STORE, KEY_NAME);
            return Ok(await _daprClient.TryDeleteStateAsync(STATE_STORE, KEY_NAME, etag));
        }
    }
}

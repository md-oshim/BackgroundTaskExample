using BackgroundTaskExample.Models;
using BackgroundTaskExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundTaskExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackgroundTaskController : ControllerBase
    {
        private readonly BackgroundTaskParallelService _backgroundTaskParallelService;
        private readonly ITaskQueue _taskQueue;

        public BackgroundTaskController(
            BackgroundTaskParallelService backgroundTaskParallelService,
            ITaskQueue taskQueue
            )
        {
            _backgroundTaskParallelService = backgroundTaskParallelService;
            _taskQueue = taskQueue;
        }

        [HttpGet("parallel")]
        public ActionResult Parallel([FromQuery] RequestModel requestModel)
        {
            _ = _backgroundTaskParallelService.StartParallelBackgroundTask(requestModel);

            return Ok(requestModel.TaskId);
        }

        [HttpGet("queue")]
        public async Task<ActionResult> Queue([FromQuery] RequestModel requestModel)
        {
            await _taskQueue.QueueBackgroundWorkItemAsync(requestModel);

            return Ok(requestModel.TaskId);
        }

    }

}


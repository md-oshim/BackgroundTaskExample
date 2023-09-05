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
        private readonly TaskQueue _taskQueue;

        public BackgroundTaskController(
            BackgroundTaskParallelService backgroundTaskParallelService,
            TaskQueue taskQueue)
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
        public ActionResult Queue([FromQuery] RequestModel requestModel)
        {
            _taskQueue.Enqueue(requestModel);

            return Ok(requestModel.TaskId);
        }

    }

}


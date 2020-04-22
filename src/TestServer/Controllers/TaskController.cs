//using Microsoft.AspNetCore.Mvc;
//using MyLab.StatusProvider;

//namespace TestServer.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class TaskController : ControllerBase
//    {
//        private readonly IAppStatusService _statusService;

//        /// <summary>
//        /// Initializes a new instance of <see cref="TaskController"/>
//        /// </summary>
//        public TaskController(IAppStatusService statusService)
//        {
//            _statusService = statusService;
//        }

//        [HttpPost("start")]
//        public IActionResult StartTask()
//        {
//            _statusService.TaskLogicStarted();

//            return Ok();
//        }

//        [HttpPost("complete")]
//        public IActionResult EndTask()
//        {
//            _statusService.TaskLogicCompleted();

//            return Ok();
//        }

//        [HttpPost("error")]
//        public IActionResult EndTask([FromQuery]string msg)
//        {
//            _statusService.TaskLogicError(new StatusError{ Message = msg});

//            return Ok();
//        }
//    }
//}

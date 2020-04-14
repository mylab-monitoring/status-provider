using Microsoft.AspNetCore.Mvc;
using MyLab.StatusProvider;

namespace TestServer.Controllers
{
    [ApiController]
    [Route("mq/incoming")]
    public class IncomingMqController : ControllerBase
    {
        private readonly IAppStatusService _statusService;

        /// <summary>
        /// Initializes a new instance of <see cref="IncomingMqController"/>
        /// </summary>
        public IncomingMqController(IAppStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpPost("connect")]
        public IActionResult StartTask([FromQuery]string queueName)
        {
            _statusService.QueueConnected(queueName);

            return Ok();
        }

        [HttpPost("send-msg")]
        public IActionResult SendMessage([FromQuery]string queueName)
        {
            _statusService.IncomingMqMessageReceived(queueName);

            return Ok();
        }

        [HttpPost("complete-msg")]
        public IActionResult CompleteMsg()
        {
            _statusService.IncomingMqMessageProcessed();

            return Ok();
        }

        [HttpPost("fail-msg")]
        public IActionResult FailMsg([FromQuery]string msg)
        {
            _statusService.IncomingMqMessageError(new StatusError{Message = msg});

            return Ok();
        }
    }
}

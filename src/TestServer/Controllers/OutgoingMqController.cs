//using Microsoft.AspNetCore.Mvc;
//using MyLab.StatusProvider;

//namespace TestServer.Controllers
//{
//    [ApiController]
//    [Route("mq/outgoing")]
//    public class OutgoingMqController : ControllerBase
//    {
//        private readonly IAppStatusService _statusService;

//        /// <summary>
//        /// Initializes a new instance of <see cref="OutgoingMqController"/>
//        /// </summary>
//        public OutgoingMqController(IAppStatusService statusService)
//        {
//            _statusService = statusService;
//        }

//        [HttpPost("start-send")]
//        public IActionResult StartSendMessage([FromQuery]string queueName)
//        {
//            _statusService.OutgoingMessageStartSending(queueName);

//            return Ok();
//        }

//        [HttpPost("complete-sending")]
//        public IActionResult CompleteSending()
//        {
//            _statusService.OutgoingMessageSent();

//            return Ok();
//        }

//        [HttpPost("fail-sending")]
//        public IActionResult FailSending([FromQuery]string msg)
//        {
//            _statusService.OutgoingMessageSendingError(new StatusError{Message = msg});

//            return Ok();
//        }
//    }
//}

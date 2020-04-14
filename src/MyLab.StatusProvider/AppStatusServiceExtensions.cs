using System;

namespace MyLab.StatusProvider
{
    public static class AppStatusServiceExtensions
    {
        /// <summary>
        /// Report about message sending error
        /// </summary>
        public static void OutgoingMessageSendingError(this IAppStatusService srv, Exception e)
        {
            srv.OutgoingMessageSendingError(new StatusError(e));
        }

        /// <summary>
        /// Report about error when incoming message processing
        /// </summary>
        public static void IncomingMqMessageError(this IAppStatusService srv, Exception e)
        {
            srv.IncomingMqMessageError(new StatusError(e));
        }

        /// <summary>
        /// Report about task logic error
        /// </summary>
        public static void TaskLogicError(this IAppStatusService srv, Exception e)
        {
            srv.TaskLogicError(new StatusError(e));
        }
    }
}
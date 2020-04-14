namespace MyLab.StatusProvider
{
    /// <summary>
    /// Allow to work with application status
    /// </summary>
    public interface IAppStatusService
    {
        /// <summary>
        /// Provides an application status
        /// </summary>
        ApplicationStatus GetStatus();
        /// <summary>
        /// Sets application version
        /// </summary>
        void SetAppVersion(string version);
        /// <summary>
        /// Sets host name
        /// </summary>
        void SetHost(string host);
        /// <summary>
        /// Report about task logic starting
        /// </summary>
        void TaskLogicStarted();
        /// <summary>
        /// Report about task logic error
        /// </summary>
        void TaskLogicError(StatusError err);
        /// <summary>
        /// Report about task logic success completion
        /// </summary>
        void TaskLogicCompleted();
        /// <summary>
        /// Report about connection to queue for listening
        /// </summary>
        void QueueConnected(string queueName);
        /// <summary>
        /// Report about incoming message has received 
        /// </summary>
        void IncomingMqMessageReceived(string srcQueue);
        /// <summary>
        /// Report about incoming message succeed processing
        /// </summary>
        void IncomingMqMessageProcessed();
        /// <summary>
        /// Report about error when incoming message processing
        /// </summary>
        void IncomingMqMessageError(StatusError e);
        /// <summary>
        /// Report about outgoing message sending started
        /// </summary>
        void OutgoingMessageStartSending(string queueName);
        /// <summary>
        /// Report about outgoing message has sent
        /// </summary>
        void OutgoingMessageSent();
        /// <summary>
        /// Report about message sending error
        /// </summary>
        void OutgoingMessageSendingError(StatusError e);
        
    }
}
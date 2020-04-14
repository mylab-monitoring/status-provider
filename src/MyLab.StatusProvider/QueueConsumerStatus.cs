using System;
using System.Linq;

namespace MyLab.StatusProvider
{
    /// <summary>
    /// Queue consumer application specific status
    /// </summary>
    public class QueueConsumerStatus
    {
        /// <summary>
        /// Listened queues
        /// </summary>
        public string[] Queues { get; set; }
        /// <summary>
        /// Gets date-time when last incoming message has received
        /// </summary>
        public DateTime? LastIncomingMessageTime { get; set; }
        /// <summary>
        /// Gets queue name from incoming message has received
        /// </summary>
        public string LastIncomingMessageQueue{ get; set; }
        /// <summary>
        /// An error which occured when last incoming message processing
        /// </summary>
        public StatusError LastIncomingMessageError { get; set; }
        /// <summary>
        /// Gets date-time when last outgoing message has sent
        /// </summary>
        public DateTime? LastOutgoingMessageTime { get; set; }
        /// <summary>
        /// Gets queue name where outgoing message has sent
        /// </summary>
        public string LastOutgoingMessageQueue { get; set; }
        /// <summary>
        /// An error which occured when last outgoing message sending
        /// </summary>
        public StatusError LastOutgoingMessageError { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="QueueConsumerStatus"/>
        /// </summary>
        public QueueConsumerStatus()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="QueueConsumerStatus"/>
        /// </summary>
        public QueueConsumerStatus(QueueConsumerStatus origin)
        {
            if(origin.Queues != null)
                Queues = origin.Queues.ToArray();
            LastIncomingMessageTime = origin.LastIncomingMessageTime;
            LastIncomingMessageError = origin.LastIncomingMessageError;
            LastOutgoingMessageTime = origin.LastOutgoingMessageTime;
            LastOutgoingMessageError = origin.LastOutgoingMessageError;
            LastIncomingMessageQueue = origin.LastIncomingMessageQueue;
            LastOutgoingMessageQueue = origin.LastOutgoingMessageQueue;
        }
    }
}
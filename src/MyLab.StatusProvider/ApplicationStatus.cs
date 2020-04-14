using System;

namespace MyLab.StatusProvider
{
    /// <summary>
    /// Contains application status information
    /// </summary>
    public class ApplicationStatus
    {
        /// <summary>
        /// Application name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Application version
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Server time
        /// </summary>
        public DateTime ServerTime => DateTime.Now;

        /// <summary>
        /// Application start time
        /// </summary>
        public DateTime StartAt { get; set; }

        /// <summary>
        /// Duration since start time 
        /// </summary>
        public TimeSpan UpTime => DateTime.Now - StartAt;
        /// <summary>
        /// Server hostname
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// Task application specific status
        /// </summary>
        public TaskStatus Task { get; set; }
        /// <summary>
        ///  Queue consumer application specific status
        /// </summary>
        public QueueConsumerStatus Mq { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationStatus"/>
        /// </summary>
        public ApplicationStatus()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationStatus"/>
        /// </summary>
        public ApplicationStatus(ApplicationStatus origin)
        {
            Name = origin.Name;
            Version = origin.Version;
            StartAt = origin.StartAt;
            Host = origin.Host;

            if(origin.Task != null)
                Task = new TaskStatus(origin.Task);

            if (origin.Mq != null)
                Mq = new QueueConsumerStatus(origin.Mq);
        }
    }
}

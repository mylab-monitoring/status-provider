using System;

namespace MyLab.StatusProvider
{
    /// <summary>
    /// Task application specific status
    /// </summary>
    public class TaskStatus
    {
        /// <summary>
        /// Last time when an application logic was started
        /// </summary>
        public DateTime? LastTimeStart { get; set; }
        /// <summary>
        /// Duration of application task logic performing
        /// </summary>
        public TimeSpan? LastTimeDuration { get; set; }
        /// <summary>
        /// Error description which occured at last logic performing
        /// </summary>
        public StatusError LastTimeError { get; set; }

        /// <summary>
        /// Determines that task perform itself logic at this time
        /// </summary>
        public bool Processing { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="TaskStatus"/>
        /// </summary>
        public TaskStatus()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TaskStatus"/>
        /// </summary>
        public TaskStatus(TaskStatus origin)
        {
            LastTimeDuration = origin.LastTimeDuration;
            LastTimeStart = origin.LastTimeStart;
            LastTimeError = origin.LastTimeError;
            Processing = origin.Processing;
        }
    }
}
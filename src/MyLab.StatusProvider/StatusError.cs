using System;

namespace MyLab.StatusProvider
{
    /// <summary>
    /// Contains error data
    /// </summary>
    public class StatusError
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Error description, i.e. stack trace
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="StatusError"/>
        /// </summary>
        public StatusError()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="StatusError"/>
        /// </summary>
        public StatusError(Exception e)
        {
            Message = e.Message;
            Description = e.ToString();
        }
    }
}
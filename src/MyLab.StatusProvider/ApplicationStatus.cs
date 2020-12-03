using System;
using System.Collections.Generic;
using System.Linq;

namespace MyLab.StatusProvider
{
    /// <summary>
    /// Contains application status information
    /// </summary>
    public class ApplicationStatus
    {
        /// <summary>
        /// Status provider lib version
        /// </summary>
        public string StatusProviderVersion { get; set; }
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

        public IDictionary<string, ICloneable> SubStatuses { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationStatus"/>
        /// </summary>
        public ApplicationStatus()
        {
            SubStatuses = new Dictionary<string, ICloneable>();
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
            StatusProviderVersion = origin.StatusProviderVersion;

            SubStatuses = origin.SubStatuses.ToDictionary(
                ss => ss.Key,
                ss => (ICloneable) ss.Value.Clone());
        }

        /// <summary>
        /// Gets sub status of specified type
        /// </summary>
        public T GetSubStatus<T>()
            where T : class
        {
            SubStatuses.TryGetValue(typeof(T).Name, out var ss);
            return (T)ss;
        }
    }
}

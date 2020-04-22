using System;

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
        /// Adds sub status into application status
        /// </summary>
        T RegSubStatus<T>() where T : class, ICloneable, new();

    }
}
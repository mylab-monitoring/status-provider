using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MyLab.StatusProvider
{
    class DefaultAppStatusService : IAppStatusService
    {
        private readonly ApplicationStatus _status = new ApplicationStatus();

        DefaultAppStatusService()
        {
            
        }

        public static DefaultAppStatusService Create()
        {
            var srv = new DefaultAppStatusService();

            SetName(srv._status);
            SetHost(srv._status);
            SetVersion(srv._status);

            srv._status.StartAt = DateTime.Now;
            srv._status.StartAt = DateTime.Now;

            return srv;
        }

        private static void SetVersion(ApplicationStatus status)
        {
            var envVer = Environment.GetEnvironmentVariable("APP_VERSION");
            if (string.IsNullOrWhiteSpace(envVer))
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly != null)
                    status.Version = entryAssembly.GetName().Version.ToString();
            }
            else
            {
                status.Version = envVer;
            }
        }

        private static void SetHost(ApplicationStatus status)
        {
            if (File.Exists("/etc/hostname"))
                status.Host = File.ReadAllText("/etc/hostname");
        }

        private static void SetName(ApplicationStatus status)
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
                status.Name = entryAssembly.GetName().Name;
        }

        public ApplicationStatus GetStatus()
        {
            return new ApplicationStatus(_status);
        }

        public void SetAppVersion(string version)
        {
            _status.Version = version;
        }

        public void SetHost(string host)
        {
            _status.Host = host;
        }

        public T RegSubStatus<T>() 
            where T : class, ICloneable, new()
        {
            var key = typeof(T).Name;
            T resSubStatus;

            if(!_status.SubStatuses.TryGetValue(key, out var subStatus))
            {
                resSubStatus = new T();
                _status.SubStatuses.Add(key, resSubStatus);
            }
            else
            {
                resSubStatus = (T) subStatus;
            }

            return resSubStatus;
        }
    }
}

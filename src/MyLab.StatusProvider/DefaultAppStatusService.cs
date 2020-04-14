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

        public void TaskLogicStarted()
        {
            if(_status.Task == null)
                _status.Task = new TaskStatus();
            _status.Task.LastTimeStart = DateTime.Now;
            _status.Task.LastTimeDuration = null;
            _status.Task.Processing = true;
        }

        public void TaskLogicError(StatusError err)
        {
            if (_status.Task == null)
                _status.Task = new TaskStatus();
            _status.Task.LastTimeError = err;
            _status.Task.LastTimeDuration = DateTime.Now - _status.Task.LastTimeStart;
            _status.Task.Processing = false;
        }

        public void TaskLogicCompleted()
        {
            if (_status.Task == null)
                _status.Task = new TaskStatus();
            _status.Task.LastTimeError = null;
            _status.Task.LastTimeDuration = DateTime.Now - _status.Task.LastTimeStart;
            _status.Task.Processing = false;
        }

        public void QueueConnected(string queueName)
        {
            if (_status.Mq == null)
                _status.Mq = new QueueConsumerStatus();

            var queues = _status.Mq.Queues != null
                ? new List<string>(_status.Mq.Queues) {queueName}
                : new List<string> { queueName };

            _status.Mq.Queues = queues.ToArray();
        }

        public void IncomingMqMessageReceived(string srcQueue)
        {
            if (_status.Mq == null)
                _status.Mq = new QueueConsumerStatus();

            _status.Mq.LastIncomingMessageTime = DateTime.Now;
            _status.Mq.LastIncomingMessageQueue = srcQueue;
        }

        public void IncomingMqMessageProcessed()
        {
            if (_status.Mq == null)
                _status.Mq = new QueueConsumerStatus();

            _status.Mq.LastIncomingMessageError= null;
        }

        public void IncomingMqMessageError(StatusError e)
        {
            if (_status.Mq == null)
                _status.Mq = new QueueConsumerStatus();

            _status.Mq.LastIncomingMessageError = e;
        }

        public void OutgoingMessageStartSending(string queueName)
        {
            if (_status.Mq == null)
                _status.Mq = new QueueConsumerStatus();

            _status.Mq.LastOutgoingMessageTime = DateTime.Now;
            _status.Mq.LastOutgoingMessageQueue = queueName;
        }

        public void OutgoingMessageSent()
        {
            if (_status.Mq == null)
                _status.Mq = new QueueConsumerStatus();

            _status.Mq.LastOutgoingMessageError = null;
        }

        public void OutgoingMessageSendingError(StatusError e)
        {
            if (_status.Mq == null)
                _status.Mq = new QueueConsumerStatus();

            _status.Mq.LastOutgoingMessageError = e;
        }
    }
}

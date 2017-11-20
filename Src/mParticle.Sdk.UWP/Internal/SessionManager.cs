using System;
using System.Linq;
using mParticle.Sdk.Core.Dto.Events;

namespace mParticle.Sdk.UWP
{
    internal sealed class SessionManager
    {
        private readonly MessageManager messageManager;
        private readonly PersistenceManager persistenceManager;

        internal SessionManager(MessageManager messageManager, PersistenceManager persistenceManager)
        {
            this.messageManager = messageManager;
            this.persistenceManager = persistenceManager;
        }

        internal Session CurrentSession { get; private set; }

        internal void Application_Launched()
        {
            StartSession();
            ApplicationStateTransitionMessage astMessage = new ApplicationStateTransitionMessage()
            {
                SessionId = CurrentSession.Id,
                ApplicationTransitionType = ApplicationTransitionType.app_init,
                IsFirstRun = persistenceManager.IsFirstRun,
                IsUpgrade = persistenceManager.IsUpgrade,
                LaunchParameters = MParticle.Instance.Options.LaunchArgs?.Arguments
            };
            this.messageManager.LogMessage(astMessage);
        }

        internal void Application_LeavingBackground()
        {
            long lastEnteredBackgroundTime = CurrentSession.LastEventTimeMillis;
            if (lastEnteredBackgroundTime > 0) //if 0, this session has never been in the background
            {
                long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                long timeInBackground = currentTime - lastEnteredBackgroundTime;
                CurrentSession.BackgroundTime += timeInBackground;
                if (timeInBackground >= (MParticle.Instance.Options.SessionTimeoutSeconds * 1000))
                {
                    EndSession(CurrentSession, currentTime);
                    StartSession();
                }

                var astMessage = new ApplicationStateTransitionMessage()
                {
                    SessionId = CurrentSession.Id,
                    ApplicationTransitionType = ApplicationTransitionType.app_fore,
                    IsFirstRun = persistenceManager.IsFirstRun,
                    IsUpgrade = persistenceManager.IsUpgrade
                };
                this.messageManager.LogMessage(astMessage);
            }
        }

        internal void Application_EnteredBackground()
        {
            CurrentSession.LastEventTimeMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var astMessage = new ApplicationStateTransitionMessage()
            {
                SessionId = CurrentSession.Id,
                ApplicationTransitionType = ApplicationTransitionType.app_back,
                IsFirstRun = persistenceManager.IsFirstRun,
                IsUpgrade = persistenceManager.IsUpgrade
            };
            this.messageManager.LogMessage(astMessage);
        }

        private void EndSession(Session session, long currentTime)
        {
            long totalSessionTime = currentTime - session.StartTimeMillis;
            long foregroundSessionTime = totalSessionTime - session.BackgroundTime;

            var message = new SessionEndSdkMessage()
            {
                SessionId = session.Id,
                SessionStartTimestamp = session.StartTimeMillis,
                SessionLength = foregroundSessionTime,
                SessionLengthWithBackgroundTime = totalSessionTime,
                SpanningMpIds = session.Mpids.ToArray()
            };
            this.messageManager.LogMessage(message);
            this.persistenceManager.LastSession = null;
        }

        private void StartSession()
        {
            Session previousSession = this.persistenceManager.LastSession;
            if (previousSession != null)
            {
                EndSession(previousSession, previousSession.LastEventTimeMillis);
            }
            CurrentSession = new Session();
            CurrentSession.AddMpid(MParticle.Instance.Identity.CurrentUser);
            this.persistenceManager.LastSession = CurrentSession;
            var message = new SessionStartSdkMessage()
            {
                Id = CurrentSession.Id,
                Timestamp = CurrentSession.StartTimeMillis
            };
            this.messageManager.LogMessage(message);
        }
    }
}
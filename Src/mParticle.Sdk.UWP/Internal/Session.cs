using System;
using System.Collections.Generic;

namespace mParticle.Sdk.UWP
{
    internal sealed class Session
    {
        internal Session()
        {
            StartTimeMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            LastEventTimeMillis = StartTimeMillis;
            Id = Guid.NewGuid().ToString();
            Mpids = new List<long>();
        }

        internal Session(long startTimestamp, string sessionId, IList<long> sessionMpids)
        {
            StartTimeMillis = startTimestamp;
            LastEventTimeMillis = StartTimeMillis;
            Id = sessionId;
            Mpids = sessionMpids;
        }

        internal void AddMpid(MParticleUser user)
        {
            if (user != null)
            {
                if (!Mpids.Contains(user.Mpid))
                {
                    Mpids.Add(user.Mpid);
                }
            }
        }

        internal long StartTimeMillis { get; private set; }
        internal string Id { get; private set; }
        internal IList<long> Mpids { get; private set; }
        internal long BackgroundTime { get; set; }
        internal long LastEventTimeMillis { get; set; }
    }
}
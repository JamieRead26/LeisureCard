using System;
using System.Collections.Generic;
using System.Linq;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Service
{
    public interface IUserSessionService
    {
        string GetToken(LeisureCard card);
        ISession GetSession(string token);
    }

    public class DbUserSessionService : IUserSessionService
    {
        private readonly TimeSpan _sessionDuration;

        public DbUserSessionService(int sessionDurationMinutes)
        {
            _sessionDuration =
                TimeSpan.FromMinutes(sessionDurationMinutes);
        }

        public string GetToken(LeisureCard card)
        {
            //lock (card.Code)
            //{
            //    var session = _sessions.SingleOrDefault(c => c.CardCode == card.Code);

            //    if (session != null) return session.Token;

            //    session = new Session(_sessionDuration, card);


            //    _sessions.Add(session);

            //    return session.Token;
            //}

            throw new NotImplementedException();
        }

        public ISession GetSession(string token)
        {
            //lock (token)
            //{
            //    var session = _sessions.FirstOrDefault(t => t.Token == token);

            //    if (session == null)
            //        return null;

            //    if (session.HasExpired)
            //    {
            //        _sessions.Remove(session);
            //        return null;
            //    }

            //    session.Renew();
            //    return session;
            //}

            throw new NotImplementedException();
        }
    }

    public class InMemoryUserSessionService : IUserSessionService
    {
        private readonly IList<Session> _sessions = new List<Session>();

        private readonly TimeSpan _sessionDuration;

        public InMemoryUserSessionService(int sessionDurationMinutes)
        {
            _sessionDuration =
                TimeSpan.FromMinutes(sessionDurationMinutes);
        }

        public string GetToken(LeisureCard card)
        {
            lock (card.Code)
            {
                var session = _sessions.SingleOrDefault(c => c.CardCode == card.Code);

                if (session != null) return session.Token;

                session = new Session(_sessionDuration, card);
                
                _sessions.Add(session);

                return session.Token;
            }
        }

        public ISession GetSession(string token)
        {
            lock (token)
            {
                var session = _sessions.FirstOrDefault(t => t.Token == token);

                if (session == null)
                    return null;

                if (session.HasExpired)
                {
                    _sessions.Remove(session);
                    return null;
                }

                session.Renew();
                return session;
            }
        }

        public class Session : ISession
        {
            private readonly TimeSpan _sessionDuration;

            public Session(TimeSpan sessionDuration, LeisureCard card)
            {
                _sessionDuration = sessionDuration;
                CardCode = card.Code;
                RenewalDate = card.RenewalDate;
                ExpiryUtc = DateTime.UtcNow + _sessionDuration;
                Token = Guid.NewGuid().ToString();
                TenantKey = card.Tenant.Key;
            }

            public void Renew()
            {
                ExpiryUtc = DateTime.UtcNow + _sessionDuration;
            }

            public string Token { get; private set; }
            private DateTime ExpiryUtc { get; set; }
            public string CardCode { get; private set; }
            public string TenantKey { get; private set; }

            public bool HasExpired
            {
                get { return ExpiryUtc < DateTime.UtcNow; }
            }

            public bool IsAdmin
            {
                get { return CardCode == AdminLeisureCard.Instance.Code; }
            }

            public DateTime? RenewalDate { get; set; }
        }
    }
}

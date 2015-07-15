using System;
using System.Collections.Generic;
using System.Linq;
using GRG.LeisureCards.DomainModel;
using GRG.LeisureCards.Persistence;
using GRG.LeisureCards.Persistence.NHibernate;

namespace GRG.LeisureCards.Service
{
    public interface IUserSessionService
    {
        string GetToken(LeisureCard card, string tenantKey);
        ISession GetSession(string token);
    }

    public class DbUserSessionService : IUserSessionService
    {
        private readonly string _adminCode;
        private readonly ISessionRepository _sessionRepository;
        private readonly TimeSpan _sessionDuration;

        public DbUserSessionService(
            string adminCode,
            int sessionDurationMinutes, 
            ISessionRepository sessionRepository)
        {
            _adminCode = adminCode.ToUpper().Trim();
            _sessionRepository = sessionRepository;
            _sessionDuration =
                TimeSpan.FromMinutes(sessionDurationMinutes);
        }

        [UnitOfWork]
        public string GetToken(LeisureCard card, string tenantKey)
        {
            lock (card.Code)
            {
                var session = _sessionRepository.GetLiveByCardCode(card.Code);

                if (session != null) return session.Token;

                session = new DomainModel.Session
                {
                    Token = Guid.NewGuid().ToString(),
                    CardCode = card.Code,
                    ExpiryUtc = DateTime.UtcNow + _sessionDuration,
                    IsAdmin = card.Code.ToUpper().Trim() == _adminCode,
                    TenantKey = tenantKey
                };

                _sessionRepository.Save(session);

                return session.Token;
            }
        }

        [UnitOfWork]
        public ISession GetSession(string token)
        {
            lock (token)
            {
                var session = _sessionRepository.Get(token);

                if (session == null)
                    return null;

                if (session.ExpiryUtc < DateTime.UtcNow)
                {
                    _sessionRepository.Delete(session);
                    return null;
                }

                session.ExpiryUtc = DateTime.UtcNow + _sessionDuration;
                _sessionRepository.Update(session);

                return new Session(
                    _sessionDuration,
                    session.CardCode,
                    session.TenantKey,
                    session.Token);
            }
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

        public string GetToken(LeisureCard card, string tenantKey)
        {
            lock (card.Code)
            {
                var session = _sessions.SingleOrDefault(c => c.CardCode == card.Code);

                if (session != null) return session.Token;

                session = new Session(_sessionDuration, card, tenantKey);
                
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
    }
    public class Session : ISession
    {
        private readonly TimeSpan _sessionDuration;

        public Session(TimeSpan sessionDuration, LeisureCard card, string tenantKey)
        {
            _sessionDuration = sessionDuration;
            CardCode = card.Code;
            ExpiryUtc = DateTime.UtcNow + _sessionDuration;
            Token = Guid.NewGuid().ToString();
            TenantKey = tenantKey;
        }

        public Session(TimeSpan sessionDuration,
            string cardCode,
            string tenantKey,
            string token)
        {
            _sessionDuration = sessionDuration;
            CardCode = cardCode;
            ExpiryUtc = DateTime.UtcNow + _sessionDuration;
            Token = token;
            TenantKey = tenantKey;
        }

        public void Renew()
        {
            ExpiryUtc = DateTime.UtcNow + _sessionDuration;
        }

        public string Token { get; private set; }
        public DateTime ExpiryUtc { get; private set; }
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
    }
}

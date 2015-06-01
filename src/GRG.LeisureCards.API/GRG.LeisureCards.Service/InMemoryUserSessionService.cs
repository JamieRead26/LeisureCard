﻿using System;
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
            throw new NotImplementedException();
        }

        public ISession GetSession(string token)
        {
            throw new NotImplementedException();
        }
    }

    public class InMemoryUserSessionService : IUserSessionService
    {
        private readonly IList<Session> _tokenCards = new List<Session>();

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
                var session = _tokenCards.SingleOrDefault(c => c.LeisureCard.Code == card.Code);

                if (session != null) return session.Token;

                session = new Session(_sessionDuration, card);
                _tokenCards.Add(session);

                return session.Token;
            }
        }

        public ISession GetSession(string token)
        {
            lock (token)
            {
                var session = _tokenCards.FirstOrDefault(t => t.Token == token);

                if (session == null)
                    return null;

                if (session.HasExpired)
                {
                    _tokenCards.Remove(session);
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
                LeisureCard = card;
                ExpiryUtc = DateTime.UtcNow + _sessionDuration;
                Token = Guid.NewGuid().ToString();
            }

            public void Renew()
            {
                ExpiryUtc = DateTime.UtcNow + _sessionDuration;
            }

            public string Token { get; private set; }
            private DateTime ExpiryUtc { get; set; }
            public LeisureCard LeisureCard { get; private set; }

            public bool HasExpired
            {
                get { return ExpiryUtc < DateTime.UtcNow; }
            }

            public bool IsAdmin
            {
                get { return LeisureCard == AdminLeisureCard.Instance; }
            }
        }
    }
}
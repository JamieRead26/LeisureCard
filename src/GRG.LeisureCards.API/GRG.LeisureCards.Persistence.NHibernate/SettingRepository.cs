using System;
using GRG.LeisureCards.DomainModel;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class SettingRepository : Repository<Setting, String>, ISettingRepository
    {
    }
}

using System;
using GRG.LeisureCards.Model;

namespace GRG.LeisureCards.Persistence.NHibernate
{
    public class SettingRepository : Repository<Setting, String>, ISettingRepository
    {
    }
}

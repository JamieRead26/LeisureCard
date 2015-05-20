using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRG.LeisureCards.DomainModel
{
    public interface ILatLong
    {
        double Latitude { get; set; }
        double Longitude { get; set; } 
    }
}

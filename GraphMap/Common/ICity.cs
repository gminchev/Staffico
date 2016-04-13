using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface ICity
    {
        Guid id { get; set; }

        string label { get; set; }

        float x { get; set; }

        float y { get; set; }

        List<INeighbor> Neighbors { get; set; }
    }
}

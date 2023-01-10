using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMiningGame.Tiles
{
    public interface IHardnessModifier
    {
        float Hardness { get; }
    }
}

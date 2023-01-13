using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMiningGame
{
    public interface IKillCallback
    {
        public void OnKill( bool silent );
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMiningGame.Inventories
{
    public abstract class ItemEquippable : Item
    {
        public abstract void OnEquip( Inventory i );
    }
}

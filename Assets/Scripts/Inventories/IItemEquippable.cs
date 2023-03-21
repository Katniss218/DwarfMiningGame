using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfMiningGame.Inventories
{
    public interface IItemEquippable
    {
        /// <summary>
        /// Called when the item is equipped. Equipment is the component that has the item equipped.
        /// </summary>
        public abstract void OnEquip( Equipment eq );
        /// <summary>
        /// Called when the item is unequipped. Equipment is the component that had the item equipped before it was unequipped.
        /// </summary>
        public abstract void OnUnequip( Equipment eq );
    }

}

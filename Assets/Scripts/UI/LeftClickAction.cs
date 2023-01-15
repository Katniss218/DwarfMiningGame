using DwarfMiningGame.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DwarfMiningGame.UI
{
    public class LeftClickAction : MonoBehaviour, IPointerClickHandler
    {
        // the method that does the equipping or selling, or whatever from whatever slot we selected.
        public Action OnClick;

        // the slot in the inventory that we click on.
        //public Inventory.ItemSlot Item;


        // additional class that handles the equipping on click.
        // equipped items are still present in the inventory, they just tell you which item is selected in which active slot (pickaxe/bag/etc).

        public void OnPointerClick( PointerEventData eventData )
        {
            if( eventData.button == PointerEventData.InputButton.Left )
            {
                OnClick?.Invoke();
            }
        }
    }
}
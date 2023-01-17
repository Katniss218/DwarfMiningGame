using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DwarfMiningGame.UI
{
    public class ContextMenuClickBlocker : MonoBehaviour, IPointerClickHandler
    {
        public void Show()
        {
            this.gameObject.SetActive( true );
        }

        public void Hide()
        {
            this.gameObject.SetActive( false );
        }

        public void OnPointerClick( PointerEventData eventData )
        {
            ContextMenu.CloseAll();
        }

        public static ContextMenuClickBlocker Create()
        {
            GameObject go = UIHelper.UIFill( GameManager.ContextMenuCanvas, "_ CLICK BLOCKER _", 0, 0, 0, 0 );
            go.transform.SetAsFirstSibling(); // make all windows appear in front.
            UIHelper.AddRaycastTarget( go );

            ContextMenuClickBlocker cmb = go.AddComponent<ContextMenuClickBlocker>();

            return cmb;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.UI
{
    public abstract class ContextMenu : MonoBehaviour
    {
        // Multiple context menus can be open.
        // clicking on a non-context menu closes all context menus.



        private static ContextMenuClickBlocker _clickBlocker;

        private static List<ContextMenu> _openContextMenus = new List<ContextMenu>();

        public virtual void Close()
        {
            Destroy( this.gameObject );
        }

        public static void CloseAll()
        {
            foreach( var menu in _openContextMenus.ToArray() ) // copy the opened context menus because Close() removes them from the list.
            {
                menu.Close();
            }
        }

        protected virtual void OnEnable()
        {
            if( _clickBlocker == null )
            {
                _clickBlocker = ContextMenuClickBlocker.Create();
            }
            else
            {
                _clickBlocker.Show();
            }
            if( !_openContextMenus.Contains( this ) )
            {
                _openContextMenus.Add( this );
            }
        }

        protected virtual void OnDisable()
        {
            if( _clickBlocker != null )
            {
                _clickBlocker.Hide();
            }
            if( _openContextMenus.Contains( this ) )
            {
                _openContextMenus.Remove( this );
            }
        }
    }
}
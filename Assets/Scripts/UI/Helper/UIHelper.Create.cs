using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame.UI
{
    public static partial class UIHelper
    {
        // Create UI elements with the specific properties with C#.

        // generalized:

        public static GameObject UI( Transform parent, string name, Vector2 anchorPivot, Vector2 anchoredPos, Vector2 sizeDelta )
        {
            return UI( parent, name, anchorPivot, anchorPivot, anchorPivot, anchoredPos, sizeDelta );
        }

        public static GameObject UI( Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPos, Vector2 sizeDelta )
        {
            return UI( parent, name, anchorMin, anchorMax, new Vector2( (anchorMin.x + anchorMax.x) / 2, (anchorMin.y + anchorMax.y) / 2 ), anchoredPos, sizeDelta );
        }

        public static GameObject UI( Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPos, Vector2 sizeDelta )
        {
            GameObject gameObject = new GameObject( name );
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.SetParent( parent.transform );
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.pivot = pivot;
            rectTransform.anchoredPosition = anchoredPos;
            rectTransform.sizeDelta = sizeDelta;

            return gameObject;
        }

        // fill size relative parent:

        public static GameObject UIFill( Transform parent, string name )
        {
            return UIFill( parent, name, 0, 0, 0, 0 );
        }

        [Obsolete("The anchored position and size calculations are wrong. Only works for (0,0,0,0)")]
        public static GameObject UIFill( Transform parent, string name, float left, float right, float top, float bottom )
        {
            Vector2 anchorMin = new Vector2( 0.0f, 0.0f );
            Vector2 anchorMax = new Vector2( 1.0f, 1.0f );
            Vector2 pivot = new Vector2( 0.5f, 0.5f );

            Vector2 anchoredPos = new Vector2( left, -top );
            Vector2 sizeDelta = new Vector2( -left - right, -top - bottom );

            return UI( parent, name, anchorMin, anchorMax, pivot, anchoredPos, sizeDelta );
        }

        /// <summary>
        /// Creates a new UI object that fills its parent, and the edges are at the specific percent marks of the parent.
        /// </summary>
        /// <param name="left">Horizontal percent value of the parent.</param>
        /// <param name="right">Horizontal percent value of the parent.</param>
        /// <param name="top">Vertical percent value of the parent.</param>
        /// <param name="bottom">Vertical percent value of the parent.</param>
        public static GameObject UIFillPercent( Transform parent, string name, float left, float right, float top, float bottom )
        {
            Vector2 anchorMin = new Vector2( left, bottom );
            Vector2 anchorMax = new Vector2( 1.0f - right, 1.0f - top );
            Vector2 pivot = new Vector2( 0.5f, 0.5f );
            Vector2 anchoredPos = new Vector2( left, -top );
            Vector2 sizeDelta = new Vector2( -left - right, -top - bottom );

            return UI( parent, name, anchorMin, anchorMax, pivot, anchoredPos, sizeDelta );
        }
    }
}
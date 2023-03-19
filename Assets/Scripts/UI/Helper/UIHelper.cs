using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityPlus.AssetManagement;

namespace DwarfMiningGame.UI
{
    public static partial class UIHelper
    {
        // #############################

        // Design / Assumptions:
        // - A singular consistent visual style.

        // Naming Conventions:
        // - Make___    - doesn't create any new objects.
        // - Add___     - creates new objects.


        // #############################

        public static readonly Color FOREGROUND_COLOR = new Color( 0.5f, 0.5f, 0.5f, 1.0f );
        public static readonly Color BACKGROUND_COLOR = new Color( 0.25f, 0.25f, 0.25f, 1.0f );

        /// <summary>
        /// Makes the UI element a raycast target for the UI event system. Enables the UI object to listen to UI event system inputs.
        /// </summary>
        public static void MakeRaycastTarget( GameObject go )
        {
            Image raycastImage = go.AddComponent<Image>();
            raycastImage.raycastTarget = true;
            raycastImage.color = new Color( 0, 0, 0, 0 ); // transparent.
        }

        /// <summary>
        /// Makes the specific object a text.
        /// </summary>
        public static TMPro.TextMeshProUGUI MakeText( GameObject obj, string text, TMPro.HorizontalAlignmentOptions horizontalAlign )
        {
            TMPro.TextMeshProUGUI tm = obj.AddComponent<TMPro.TextMeshProUGUI>();
            tm.fontSize = 16.0f;
            tm.raycastTarget = false;
            tm.horizontalAlignment = horizontalAlign;
            tm.verticalAlignment = TMPro.VerticalAlignmentOptions.Middle;
            tm.overflowMode = TMPro.TextOverflowModes.Overflow;
            tm.enableWordWrapping = false;
            tm.text = text;

            return tm;
        }

        /// <summary>
        /// Makes the specific object a progress bar (e.g. healthbar).
        /// </summary>
        public static Image MakeProgressBar( GameObject go, int segments, Color color, float perc )
        {
            Image image = go.AddComponent<Image>();
            image.raycastTarget = false;
            image.sprite = AssetRegistry<Sprite>.GetAsset( $"Sprites/ui_progressbar_{segments}_{segments}" );
            image.color = color;
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Horizontal;
            image.fillOrigin = 0;
            image.fillAmount = perc;
            return image;
        }

        /// <summary>
        /// Makes the specific object the background of a progress bar.
        /// </summary>
        public static void MakeProgressBarBackground( GameObject go )
        {
            Image image = go.AddComponent<Image>();
            image.raycastTarget = false;
            image.sprite = AssetRegistry<Sprite>.GetAsset( "Sprites/ui_progressbar_background" );
            image.color = FOREGROUND_COLOR;
            image.type = Image.Type.Sliced;
        }

        /// <summary>
        /// Makes the specific object a background panel.
        /// </summary>
        public static void MakeForeground( GameObject go )
        {
            Image image = go.AddComponent<Image>();
            image.raycastTarget = false;
            image.sprite = AssetRegistry<Sprite>.GetAsset( "Sprites/ui_beveled" );
            image.color = FOREGROUND_COLOR;
            image.type = Image.Type.Sliced;
        }

        /// <summary>
        /// Makes the specific object a foreground panel.
        /// </summary>
        public static void MakeBackground( GameObject go )
        {
            Image image = go.AddComponent<Image>();
            image.raycastTarget = false;
            image.sprite = AssetRegistry<Sprite>.GetAsset( "Sprites/ui_beveled" );
            image.color = BACKGROUND_COLOR;
            image.type = Image.Type.Sliced;
        }

        /// <summary>
        /// Makes the specific object a vertical layout group.
        /// </summary>
        public static void MakeVerticalLayoutGroup( GameObject go, int padding, int spacing, bool containerFitsContents, bool reversed = false )
        {
            VerticalLayoutGroup vl = go.AddComponent<VerticalLayoutGroup>();
            vl.childAlignment = reversed ? TextAnchor.LowerRight : TextAnchor.UpperLeft;
            vl.padding = new RectOffset( padding, padding, padding, padding );
            vl.spacing = spacing;
            vl.childControlWidth = true;
            vl.childControlHeight = false;
            vl.childScaleWidth = false;
            vl.childScaleHeight = false;
            vl.childForceExpandWidth = true;
            vl.childForceExpandHeight = false;

            if( containerFitsContents )
            {
                ContentSizeFitter cs = go.AddComponent<ContentSizeFitter>();
                cs.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                cs.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }

        /// <summary>
        /// Makes the specific object a horizontal layout group.
        /// </summary>
        public static void MakeHorizontalLayoutGroup( GameObject go, int padding, int spacing, bool containerFitsContents, bool reversed = false )
        {
            HorizontalLayoutGroup vl = go.AddComponent<HorizontalLayoutGroup>();
            vl.childAlignment = reversed ? TextAnchor.LowerRight : TextAnchor.UpperLeft;
            vl.padding = new RectOffset( padding, padding, padding, padding );
            vl.spacing = spacing;
            vl.childControlWidth = false;
            vl.childControlHeight = true;
            vl.childScaleWidth = false;
            vl.childScaleHeight = false;
            vl.childForceExpandWidth = false;
            vl.childForceExpandHeight = true;

            if( containerFitsContents )
            {
                ContentSizeFitter cs = go.AddComponent<ContentSizeFitter>();
                cs.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                cs.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            }
        }

        /// <summary>
        /// Makes the specific object a grid layout group with the specific number of columns.
        /// </summary>
        public static void MakeColumnGridLayoutGroup( GameObject go, int padding, int spacing, Vector2 cellSize, GridLayoutGroup.Corner startCorner, TextAnchor childAlignment, int columnCount, bool containerFitsContents )
        {
            GridLayoutGroup gl = go.AddComponent<GridLayoutGroup>();
            gl.padding = new RectOffset( padding, padding, padding, padding );
            gl.spacing = new Vector2( spacing, spacing );
            gl.cellSize = cellSize;
            gl.startCorner = startCorner;
            gl.startAxis = GridLayoutGroup.Axis.Horizontal;
            gl.childAlignment = childAlignment;
            gl.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gl.constraintCount = columnCount;

            if( containerFitsContents )
            {
                ContentSizeFitter cs = go.AddComponent<ContentSizeFitter>();
                cs.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                cs.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }

        /// <summary>
        /// Makes the specified object into a scroll rect. Adds the required child elements.
        /// </summary>
        /// <remarks>
        /// Doesn't include a scrollbar, doesn't include any layout for the content.
        /// </remarks>
        /// <returns>The gameobject that will contain the contents.</returns>
        public static GameObject AddScrollRect( GameObject obj, bool horizontal, bool vertical )
        {
            GameObject items = UIHelper.UIFill( obj.transform, "items" );

            GameObject viewport = UIHelper.UIFill( items.transform, "viewport" );

            Image maskImage = viewport.AddComponent<Image>();
            Mask mask = viewport.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            GameObject content = UIHelper.UI( viewport.transform, "content", new Vector2( 0.0f, 1.0f ), new Vector2( 1, 1 ), Vector2.zero, new Vector2( 0.0f, 280.0f ) );

            ScrollRect scrollRect = items.AddComponent<ScrollRect>();
            scrollRect.content = (RectTransform)content.transform;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.horizontal = horizontal;
            scrollRect.vertical = vertical;
            scrollRect.inertia = true;
            scrollRect.decelerationRate = 0.5f;
            scrollRect.scrollSensitivity = 30f;
            scrollRect.viewport = (RectTransform)viewport.transform;

            return content;
        }
    }
}

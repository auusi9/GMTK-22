using UnityEngine;
using UnityEngine.UI;

namespace UIGeneric
{
    [CreateAssetMenu(menuName = "Helpers/GridCanvas", fileName = "GridCanvas", order = 0)]
    public class GridCanvas : ScriptableObject
    {
        public void Configure(Canvas canvas, RectTransform parent, GraphicRaycaster graphicRaycaster)
        {
            Canvas = canvas;
            Parent = parent;
            GraphicRaycaster = graphicRaycaster;
        }

        public Canvas Canvas { get; set; }
        public RectTransform Parent { get; set; }
        public GraphicRaycaster GraphicRaycaster { get; set; }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIGeneric
{
    public class GridCanvasAssigner : MonoBehaviour
    {
        [SerializeField] private GridCanvas _gridCanvas;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _parent;
        [SerializeField] private GraphicRaycaster _graphicRaycaster;

        private void Awake()
        {
            _gridCanvas.Configure(_canvas, _parent, _graphicRaycaster);
        }
    }
}
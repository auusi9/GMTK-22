using UnityEngine;
using UnityEngine.EventSystems;

namespace CityBuilder
{
    public class CityTile : MonoBehaviour, IDropHandler
    {
        [SerializeField] private RectTransform _rectTransform;

        public Vector2 Size => _rectTransform.sizeDelta;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                ((RectTransform)eventData.pointerDrag.transform).anchoredPosition = _rectTransform.anchoredPosition;
            }
        }
    }
}

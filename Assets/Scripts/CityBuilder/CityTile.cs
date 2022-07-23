using UnityEngine;
using UnityEngine.EventSystems;

namespace CityBuilder
{
    public class CityTile : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        public Vector2 Size => _rectTransform.sizeDelta;
    }
}

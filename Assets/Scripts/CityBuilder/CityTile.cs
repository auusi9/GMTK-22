using UnityEngine;
using UnityEngine.EventSystems;

namespace CityBuilder
{
    public class CityTile : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        public Building Building { get; set; }

        public bool Available => Building == null;
    }
}

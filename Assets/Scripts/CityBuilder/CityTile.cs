using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CityBuilder
{
    public class CityTile : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        public Building Building { get; set; }

        public bool Available => Building == null;

        public int X { get; private set; }
        public int Y { get; private set; }
        
        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

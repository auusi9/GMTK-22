using System;
using UnityEngine;
using Workers;

namespace CityBuilder.UI
{
    public class ZoomManager : MonoBehaviour
    {
        [SerializeField] private DayNight _dayNight;
        [SerializeField] private int _removedTilesOnstart = 5;
        [SerializeField] private int _tilesShownXDay = 1;
        [SerializeField] private float _tileSize = 100;
        [SerializeField] private float _offset = 110;

        private int _currentTiles;
        
        private void Start()
        {
            _dayNight.StartDay += DayNightOnStartDay;

            RectTransform rectTransform = (RectTransform)transform;

            int maxTiles = Mathf.FloorToInt(rectTransform.rect.width/_tileSize);
            
            _currentTiles = _removedTilesOnstart;

            float scale = rectTransform.rect.width / (_tileSize * (maxTiles - _currentTiles) + _offset);
            
            transform.localScale = new Vector3(scale,
                scale, scale);
        }

        private void OnDestroy()
        {
            _dayNight.StartDay -= DayNightOnStartDay;
        }

        private void DayNightOnStartDay()
        {
            if (transform.localScale.x <= 1f)
            {
                transform.localScale = Vector3.one;
                return;
            }
            RectTransform rectTransform = (RectTransform)transform;

            _currentTiles -= _tilesShownXDay;

            int maxTiles = Mathf.FloorToInt(rectTransform.rect.width/_tileSize);
            float newScale = 1f;
            
            if (_currentTiles > 0)
            {
                newScale = rectTransform.rect.width / (_tileSize * (maxTiles - _currentTiles) + _offset);
            }

            
            if (newScale < 1f)
                newScale = 1f;

            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
}
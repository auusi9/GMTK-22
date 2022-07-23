using System;
using UnityEngine;
using UnityEngine.UI;

namespace CityBuilder
{
    public class CityMap : MonoBehaviour
    {
        [SerializeField] private int _x, _y;
        [SerializeField] private Vector2 _tileSize;
        [SerializeField] private Vector2 _spacing;
        [SerializeField] private Vector2 _padding;
        [SerializeField] private CityTile _tilePrefab;

        private CityTile[,] _grid;
        
        private void Start()
        {
            _grid = new CityTile[_x, _y];

            for (int x = 0; x < _x; x++)
            {
                for (int y = 0; y < _y; y++)
                {
                    _grid[x,y] = Instantiate(_tilePrefab, transform);
                    ((RectTransform)_grid[x,y].transform).anchoredPosition =
                        new Vector2(_padding.x + _tileSize.x/2f + (x * _tileSize.x + _spacing.x * x), -_padding.y + _tileSize.y/-2f + (y * -_tileSize.y + -_spacing.y * y));
                }
            }    
        }
    }
}
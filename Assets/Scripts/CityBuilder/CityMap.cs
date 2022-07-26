using System;
using System.Drawing;
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
        [SerializeField] private CityMapLocator _cityMapLocator;
        
        private CityTile[,] _grid;

        private void Awake()
        {
            _cityMapLocator.SetCityMap(this);
            _grid = new CityTile[_x, _y];
        }

        private void Start()
        {
            /*
             for (int x = 0; x < _x; x++)
            {
                for (int y = 0; y < _y; y++)
                {
                    _grid[x,y] = Instantiate(_tilePrefab, transform);
                    ((RectTransform)_grid[x,y].transform).anchoredPosition =
                        new Vector2(_padding.x + _tileSize.x/2f + (x * _tileSize.x + _spacing.x * x), -_padding.y + _tileSize.y/-2f + (y * -_tileSize.y + -_spacing.y * y));
                    _grid[x,y].SetPosition(x, y);
                }
            }   
            */ 
        }

        public Building[] GetBuildingsNextToPosition(int x, int y)
        {
            Building[] buildings = new Building[4];
            
            buildings[0] = GetBuildingInPosition(x - 1, y);
            buildings[1] = GetBuildingInPosition(x + 1, y);
            buildings[2] = GetBuildingInPosition(x, y + 1);
            buildings[3] = GetBuildingInPosition(x, y - 1);

            return buildings;
        }

        private Building GetBuildingInPosition(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _x || y >= _y || _grid[x, y] == null)
            {
                return null;
            }
            
            return _grid[x, y].Building;
        }
        
        public Point WorldPositionToGrid(Vector2 position)
        {
            float percentX = (position.x - _padding.x) / ((_x-1) * _tileSize.x + (_x-1) * _spacing.x);
            float percentY = (Mathf.Abs(position.y) - _padding.y) / ((_y-1) * _tileSize.x + (_y-1) * _spacing.y);
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((_x - 1) * percentX);
            int y = Mathf.RoundToInt((_y - 1) * percentY);
            return new Point(x, y);
        }

        public void SetTileAtPosition(CityTile tile)
        {
            Point point = WorldPositionToGrid(((RectTransform)tile.transform).anchoredPosition);
            
            tile.SetPosition(point.X, point.Y);
            if (_grid[point.X, point.Y] == null)
            {
                _grid[point.X, point.Y] = tile;
            }
        }
    }
}
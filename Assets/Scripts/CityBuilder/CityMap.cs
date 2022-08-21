using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Dice;
using MainMenu;
using UIGeneric;
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
        [SerializeField] private DiceInventory _diceInventory;
        [SerializeField] private List<Building> _allBuildings;
        [SerializeField] private GridCanvas _gridCanvas;
        
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

        public Building[] Get4BuildingsNextToPosition(int x, int y)
        {
            Building[] buildings = new Building[4];
            
            buildings[0] = GetBuildingInPosition(x - 1, y);
            buildings[1] = GetBuildingInPosition(x + 1, y);
            buildings[2] = GetBuildingInPosition(x, y + 1);
            buildings[3] = GetBuildingInPosition(x, y - 1);

            return buildings;
        }
        
        public Building[] Get8BuildingsNextToPosition(int x, int y)
        {
            Building[] buildings = new Building[8];

            int k = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        buildings[k] = GetBuildingInPosition(x + i, y + j);
                        k++;
                    }
                }
            }

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

        public List<Building> GetBuildingsInRadius(int x, int y, int radius)
        {
            List<Building> buildings = new List<Building>();

            int r = radius / 2;

            for (int i = -r; i < r + 1; i++)
            {
                for (int j = -r; j < r + 1; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        Building building = GetBuildingInPosition(x + i, y + j);
                        if(building != null)
                            buildings.Add(building);
                    }
                }
            }

            return buildings;
        }
        
        public Point WorldPositionToGrid(Vector2 position, Vector2 pivot)
        {
            float percentX = (position.x - _padding.x - _tileSize.x * pivot.x) / ((_x-1) * _tileSize.x + (_x-1) * _spacing.x);
            float percentY = (Mathf.Abs(position.y) - _padding.y - _tileSize.y * pivot.y) / ((_y-1) * _tileSize.y + (_y-1) * _spacing.y);
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((_x - 1) * percentX);
            int y = Mathf.RoundToInt((_y - 1) * percentY);
            return new Point(x, y);
        }

        public void SetTileAtPosition(CityTile tile)
        {
            var rectTransform = (RectTransform)tile.transform;
            Point point = WorldPositionToGrid(rectTransform.anchoredPosition, rectTransform.pivot);
            
            tile.SetPosition(point.X, point.Y);
            if (_grid[point.X, point.Y] == null)
            {
                _grid[point.X, point.Y] = tile;
            }
        }

        public void RemoveBuildingFromPosition(Building building, int x, int y)
        {
            if (x < 0 || y < 0 || x >= _x || y >= _y || _grid[x, y] == null)
            {
                return;
            }
            
            if (_grid[x, y] != null && _grid[x, y].Building == building)
            {
                _grid[x, y].Building = null;
            }
        }

        public SaveBuilding[] GetBuildingsToSave()
        {
            SaveBuilding[] saveBuildings = new SaveBuilding[_grid.GetLength(0) * _grid.GetLength(1)];
            
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    saveBuildings[j * _grid.GetLength(0) + i] = GetSaveBuilding(_grid[i, j]);
                }
            }

            return saveBuildings;
        }

        private SaveBuilding GetSaveBuilding(CityTile cityTile)
        {
            if (cityTile == null || cityTile.Building == null)
            {
                return null;
            }

            return new SaveBuilding()
            {
                BuildingId = cityTile.Building.BuildingName,
                SaveFace = GetSaveFace(cityTile.Building),
                Spots = GetWorkerSpots(cityTile.Building)
            };
        }

        private List<SaveWorkerSpot> GetWorkerSpots(Building cityTileBuilding)
        {
            List<SaveWorkerSpot> workerSpots = new List<SaveWorkerSpot>();

            foreach (var workerSpot in cityTileBuilding.WorkerSpots)
            {
                workerSpots.Add(new SaveWorkerSpot()
                {
                    Worker = workerSpot.Worker ? new SaveWorker()
                    {
                        CurrentEnergy = workerSpot.Worker.Energy
                    }: null
                });
            }

            return workerSpots;
        }

        private SaveFace GetSaveFace(Building cityTileBuilding)
        {
            if (cityTileBuilding.Face == null)
                return new SaveFace()
                {
                    HasFace = false
                };
            
            return new SaveFace()
            {
                DiceId = _diceInventory.GetFaceIndex(cityTileBuilding.Face, out int index),
                Position = index,
                HasFace = true
            };
        }

        public void SetSavedBuildings(SaveBuilding[] saveDataBuildings)
        {
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    if (_grid[i, j] != null && saveDataBuildings[j * _grid.GetLength(0) + i] != null)
                    {
                        SpawnBuildingAtPosition(i, j, saveDataBuildings[j * _grid.GetLength(0) + i]);
                    }
                }
            }
        }

        private void SpawnBuildingAtPosition(int i, int j, SaveBuilding saveDataBuilding)
        {
            Building building = _allBuildings.FirstOrDefault(x => saveDataBuilding.BuildingId == x.BuildingName);
            
            if(building == null || string.IsNullOrEmpty(saveDataBuilding.BuildingId))
                return;

            Building buildingSpawned = Instantiate(building, _gridCanvas.Parent);
            _grid[i, j].Building = buildingSpawned;
            buildingSpawned.SetPosition(_grid[i, j]);
            buildingSpawned.Spawn(saveDataBuilding.SaveFace, saveDataBuilding.Spots);
        }
    }
}
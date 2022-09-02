using System;
using System.Collections.Generic;
using System.Linq;
using Dice;
using Resources;
using UnityEngine;
using Workers;
using DG.Tweening;
using MainMenu;

namespace CityBuilder
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private BuildingCost[] _cost;
        [SerializeField] private MovingBuilding _draggableObject;
        [SerializeField] private List<WorkerSpot> _workerSpots;
        [SerializeField] private GrantFace _grantFace;
        [SerializeField] private CityMapLocator _cityMapLocator;
        [SerializeField] private BuildingLibrary _buildingLibrary;
        [SerializeField] private Scenery _scenery;
        [SerializeField] private House _house;
        [SerializeField] private Factory _factory;
        [SerializeField] private string _buildingName;
        [SerializeField] private string _buildingDescription;
        [SerializeField] private GameObject _hover;

        public MovingBuilding DraggableObject => _draggableObject;
        public BuildingCost[] Cost => _cost;

        public List<WorkerSpot> WorkerSpots => _workerSpots;
        public Scenery Scenery => _scenery;
        public House House => _house;
        public Factory Factory => _factory;

        public string BuildingName => _buildingName;
        public string BuildingDescription => _buildingDescription;

        public event Action<List<SaveWorkerSpot>> Spawned;
        public event Action Destroyed;
        public event Action NewNeighbour;

        private Face _face;
        public Face Face => _face;
        private int _x, _y;

        private void OnDestroy()
        {
            if (_face != null)
            {
                _face.Destroying -= Destroying;
                _face = null;
            }

            _cityMapLocator.RemoveBuildingFromPosition(this, _x, _y);
            _buildingLibrary.DestroyedBuilding(this);

            NotifyNeighbours();
            Destroyed?.Invoke();
        }

        public void NewFace(Face obj)
        {
            obj.SetBuilding(this);
            _face = obj;
            _face.Destroying += Destroying;
        }

        private void Destroying()
        {
            _face = null;
        }

        public Building[] Get4NearBuildings()
        {
            return _cityMapLocator.Get4BuildingsNextToPosition(_x, _y);
        }
        
        public Building[] Get8NearBuildings()
        {
            return _cityMapLocator.Get8BuildingsNextToPosition(_x, _y);
        }

        public List<Building> GetBuildingsInRadius(int radius)
        {
            return _cityMapLocator.GetBuildingsInRadius(_x, _y, radius);
        }

        public void PayCost()
        {
            foreach (BuildingCost cost in _cost)
            {
                cost.Resource.RemoveResource(cost.Cost);
            }
            
            Spawn();
        }

        public void Spawn(SaveFace saveFace = null, List<SaveWorkerSpot> saveWorkerSpots = null)
        {
            if (_grantFace != null)
            {
                _grantFace.Grant(this, saveFace);
            }
            
            _buildingLibrary.NewBuilding(this);
            Spawned?.Invoke(saveWorkerSpots);
        }

        public bool CanAfford()
        {
            return _cost.All(x => x.CanAfford());
        }

        public bool HasAvailableSpot()
        {
            return _workerSpots.Any(x => x.Available);
        }

        public WorkerSpot GetAvailableSpot()
        {
            return _workerSpots.FirstOrDefault(x => x.Available);
        }

        public void SetPosition(CityTile currentTile)
        {
            _x = currentTile.X;
            _y = currentTile.Y;

            NotifyNeighbours();
            
            transform.SetParent(currentTile.transform);
            transform.localPosition = Vector3.zero;
            
            Destroy(_draggableObject);
            _draggableObject = null;
        }

        private void NotifyNeighbours()
        {
            Building[] buildings = Get8NearBuildings();

            foreach (var build in buildings)
            {
                if (build != null)
                {
                    build.NotifyNewNeighbour();
                }
            }
        }

        private void NotifyNewNeighbour()
        {
            if (_face != null)
            {
                _face.CalculateLevel();
            }
            
            NewNeighbour?.Invoke();
        }

        public void Hover()
        {
            _hover.SetActive(true);
        }

        public void StopHover()
        {
            _hover.SetActive(false);
        }
    }

    [Serializable]
    public class BuildingCost
    {
        [SerializeField] private Resource _resource;
        [SerializeField] private int _cost;

        public Resource Resource => _resource;
        public int Cost => _cost;

        public bool CanAfford() => _resource.Value >= _cost;
    }
}
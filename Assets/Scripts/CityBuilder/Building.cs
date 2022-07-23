using System;
using System.Linq;
using System.Security.Cryptography;
using Resources;
using UIGeneric;
using UnityEngine;
using Workers;

namespace CityBuilder
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private BuildingCost[] _cost;
        [SerializeField] private MovingBuilding _draggableObject;
        [SerializeField] private WorkerSpot[] _workerSpots;

        public MovingBuilding DraggableObject => _draggableObject;
        public BuildingCost[] Cost => _cost;

        public WorkerSpot[] WorkerSpots => _workerSpots;
        
        public void PayCost()
        {
            foreach (BuildingCost cost in _cost)
            {
                cost.Resource.RemoveResource(cost.Cost);
            }
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
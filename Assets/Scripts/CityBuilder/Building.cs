using System;
using System.Linq;
using System.Security.Cryptography;
using Resources;
using UIGeneric;
using UnityEngine;

namespace CityBuilder
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private BuildingCost[] _cost;
        [SerializeField] private MovingBuilding _draggableObject;

        public MovingBuilding DraggableObject => _draggableObject;
        public BuildingCost[] Cost => _cost;
        
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
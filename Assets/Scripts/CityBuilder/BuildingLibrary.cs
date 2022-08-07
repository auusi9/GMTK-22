using System;
using System.Collections.Generic;
using UnityEngine;

namespace CityBuilder
{
    [CreateAssetMenu(menuName = "CityBuilder/BuildingLibrary", fileName = "BuildingLibrary", order = 2)]
    public class BuildingLibrary : ScriptableObject
    {
        private Dictionary<string, List<Building>> _spawnedBuildings = new Dictionary<string, List<Building>>();

        private void OnEnable()
        {
            _spawnedBuildings.Clear();
        }

        public void NewBuilding(Building building)
        {
            string key = building.BuildingName;

            if (!_spawnedBuildings.ContainsKey(key))
            {
                _spawnedBuildings.Add(key, new List<Building>());
            }
            
            _spawnedBuildings[key].Add(building);
        }

        public void DestroyedBuilding(Building building)
        {
            string key = building.BuildingName;

            if (_spawnedBuildings.ContainsKey(key))
            {
                if (_spawnedBuildings[key].Contains(building))
                {
                    _spawnedBuildings[key].Remove(building);
                }
            }
        }

        public bool HasThisBuilding(Building building)
        {
            string key = building.BuildingName;
            return _spawnedBuildings.ContainsKey(key) && _spawnedBuildings[key].Count > 0;
        }
    }
}
using System.Collections.Generic;
using MainMenu;
using UnityEngine;

namespace CityBuilder
{
    [CreateAssetMenu(menuName = "CityBuilder/CityMapLocator", fileName = "CityMapLocator", order = 2)]
    public class CityMapLocator : ScriptableObject
    {
        private CityMap _cityMap;


        public void SetCityMap(CityMap cityMap)
        {
            _cityMap = cityMap;
        }

        public Building[] Get4BuildingsNextToPosition(int x, int y)
        {
            return _cityMap.Get4BuildingsNextToPosition(x, y);
        }
        
        public Building[] Get8BuildingsNextToPosition(int x, int y)
        {
            return _cityMap.Get8BuildingsNextToPosition(x, y);
        }

        public void SetTilePosition(CityTile tile)
        {
            _cityMap.SetTileAtPosition(tile);
        }

        public void RemoveBuildingFromPosition(Building building, int x, int y)
        {
            _cityMap.RemoveBuildingFromPosition(building, x, y);
        }
        
        public List<Building> GetBuildingsInRadius(int x, int y, int radius)
        {
            return _cityMap.GetBuildingsInRadius(x, y, radius);
        }

        public SaveBuilding[] GetBuildingsToSave()
        {
            if (_cityMap == null)
                return null;
            
            return _cityMap.GetBuildingsToSave();
        }

        public void SetSavedBuildings(SaveBuilding[] saveDataBuildings)
        {
            _cityMap.SetSavedBuildings(saveDataBuildings);
        }
    }
}
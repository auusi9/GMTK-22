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

        public Building[] GetBuildingsNextToPosition(int x, int y)
        {
            return _cityMap.GetBuildingsNextToPosition(x, y);
        }
    }
}
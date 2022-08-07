using System;
using System.Linq;
using UnityEngine;

namespace CityBuilder
{
    public class Shrubbery : MonoBehaviour
    {
        [SerializeField] private Building _building;

        private bool _active = false;
        
        private void Start()
        {
            _building.NewNeighbour += NewNeighbour;
            _building.Spawned += BuildingOnSpawned;
        }

        private void BuildingOnSpawned()
        {
            CheckForNewHouses();
        }

        private void CheckForNewHouses()
        {
            Building[] buildings = _building.Get8NearBuildings();

            bool allHouses = buildings.All(x => x != null && x.House != null);

            if (allHouses && _active == false)
            {
                foreach (var building in buildings)
                {
                    if(building != null && building.House != null)
                        building.House.EnableHiddenSpots();
                }
                _active = true;
            }
            else if(_active && !allHouses)
            {
                foreach (var building in buildings)
                {
                    if(building != null && building.House != null)
                        building.House.HideHiddenSpots();
                }
                
                _active = false;
            }
        }

        private void NewNeighbour()
        {
            CheckForNewHouses();
        }

        private void OnDestroy()
        {
            _building.NewNeighbour -= NewNeighbour;

            if (_active)
            {
                Building[] buildings = _building.Get8NearBuildings();

                foreach (var building in buildings)
                {
                    building.House.HideHiddenSpots();
                }
                
                _active = false;
            }
        }
    }
}
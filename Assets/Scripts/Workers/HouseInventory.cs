using System.Collections.Generic;
using System.Linq;
using CityBuilder;
using UnityEngine;

namespace Workers
{
    [CreateAssetMenu(menuName = "Workers/House Inventory", fileName = "House Inventory", order = 0)]
    public class HouseInventory : ScriptableObject
    {
        private List<House> _houses = new List<House>();
        
        public void AddHouse(House house)
        {
            if (!_houses.Contains(house))
            {
                _houses.Add(house);
            }
        }

        public void RemoveHouse(House house)
        {
            if (_houses.Contains(house))
            {
                _houses.Remove(house);
            }
        }
        
        public void SetWorkerToNearestHouse(Worker worker)
        {
            if (worker.CurrentSpot != null && worker.CurrentSpot.IsResting)
            {
                return;
            }
            
            House house = _houses.Where(x => x.HasEmptySpots()).OrderBy(t => (t.transform.position - worker.transform.position).sqrMagnitude)
                .FirstOrDefault();

            if (house != null)
            {
                WorkerSpot workerSpot = house.GetEmptySpot();
                workerSpot.SetWorker(worker);
            }
        }
    }
}
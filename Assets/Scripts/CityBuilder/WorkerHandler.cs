using System;
using System.Collections.Generic;
using System.Linq;
using MainMenu;
using UnityEngine;
using Workers;

namespace CityBuilder
{
    public class WorkerHandler : MonoBehaviour
    {
        [SerializeField] private int _radius = 6;
        [SerializeField] private float _checkTime = 1f;
        [SerializeField] private Building _building;

        private bool _spawned = false;
        private float _lastCheck = 0f;
        
        private void Start()
        {
            _building.Spawned += BuildingOnSpawned;
        }

        private void OnDestroy()
        {
            _building.Spawned -= BuildingOnSpawned;
        }

        private void BuildingOnSpawned(List<SaveWorkerSpot> saveWorkerSpots)
        {
            _spawned = true;
        }

        private void LateUpdate()
        {
            if (!_spawned)
            {
                return;
            }
            
            _lastCheck += Time.deltaTime;

            if (_lastCheck >= _checkTime)
            {
                HandleWorkers();
                _lastCheck = 0f;
            }
        }

        private void HandleWorkers()
        {
            List<Building> buildings = _building.GetBuildingsInRadius(_radius);

            List<Building> factories = buildings.Where(x => x.Factory != null).ToList();
            List<Building> houses = buildings.Where(x => x.House != null).ToList();

            List<WorkerSpot> tiredWorkers =
                factories.SelectMany(x => x.WorkerSpots.Where(y => !y.Available && y.Worker.IsTired).Select(j => j).ToList()).ToList();
            
            List<WorkerSpot> emptySpots =
                houses.SelectMany(x => x.WorkerSpots.Where(y => y.Available).Select(j => j).ToList()).ToList();

            foreach (var workerSpots in tiredWorkers)
            {
                if(emptySpots.Count == 0)
                    break;
                
                emptySpots[0].SetWorker(workerSpots.Worker);
                emptySpots.Remove(emptySpots[0]);
            }
            
            List<WorkerSpot> fullyRestedWorkers =
                houses.SelectMany(x => x.WorkerSpots.Where(y => !y.Available && y.Worker.IsFullyRested).Select(j => j).ToList()).ToList();
            
            List<WorkerSpot> emptyWorkSpots =
                factories.SelectMany(x => x.WorkerSpots.Where(y => y.Available).Select(j => j).ToList()).ToList();
            
            foreach (var workerSpots in fullyRestedWorkers)
            {
                if(emptyWorkSpots.Count == 0)
                    break;
                
                emptyWorkSpots[0].SetWorker(workerSpots.Worker);
                emptyWorkSpots.Remove(emptyWorkSpots[0]);
            }
        }
    }
}
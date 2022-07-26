using System;
using Resources;
using UnityEngine;
using Workers;

namespace CityBuilder
{
    public class House : MonoBehaviour
    {
        [SerializeField] private Building _building;
        [SerializeField] private Worker _worker;

        private void Start()
        {
            _building.Spawned += Spawned;
        }

        private void OnDestroy()
        {
            _building.Spawned -= Spawned;
        }

        private void Spawned()
        {
            foreach (var workerSpot in _building.WorkerSpots)
            {
                Worker worker = Instantiate(_worker, transform);
                workerSpot.SetWorker(worker);
            }
        }

        private void Update()
        {
            foreach (var workerSpot in _building.WorkerSpots)
            {
                if (!workerSpot.Available)
                    workerSpot.Worker.Rest();
            }
        }
    }
}
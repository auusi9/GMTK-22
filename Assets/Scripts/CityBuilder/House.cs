using System;
using System.Collections.Generic;
using Resources;
using Unity.VisualScripting;
using UnityEngine;
using Workers;

namespace CityBuilder
{
    public class House : MonoBehaviour
    {
        [SerializeField] private Building _building;
        [SerializeField] private Worker _worker;
        [SerializeField] private WorkerSpot[] _hiddenSpots;

        private List<Worker> _workers = new List<Worker>();

        private bool _hiddenSpotsShown = false;

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

        public void HideHiddenSpots()
        {
            if(!_hiddenSpotsShown)
                return;
            
            _hiddenSpotsShown = false;
            foreach (var workerSpot in _hiddenSpots)
            {
                workerSpot.gameObject.SetActive(false);
                _building.WorkerSpots.Remove(workerSpot);
            }

            foreach (var worker in _workers)
            {
                Destroy(worker);
            }
            
            _workers.Clear();
        }
        
        public void EnableHiddenSpots()
        {
            if(_hiddenSpotsShown)
                return;
            
            foreach (var workerSpot in _hiddenSpots)
            {
                workerSpot.gameObject.SetActive(true);
                Worker worker = Instantiate(_worker, transform);
                workerSpot.SetWorker(worker);
                _workers.Add(worker);
            }
            
            _building.WorkerSpots.AddRange(_hiddenSpots);
            _hiddenSpotsShown = true;
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
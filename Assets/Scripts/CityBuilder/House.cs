using System.Collections.Generic;
using MainMenu;
using UnityEngine;
using Workers;

namespace CityBuilder
{
    public class House : MonoBehaviour
    {
        [SerializeField] private Building _building;
        [SerializeField] private Worker _worker;
        [SerializeField] private WorkerSpot[] _hiddenSpots;
        [SerializeField] private HouseInventory _houseInventory;
        
        private bool _hiddenSpotsShown = false;
        private List<SaveWorkerSpot> _saveWorkerSpots;
        private void Awake()
        {
            _building.Spawned += Spawned;
        }

        private void OnDestroy()
        {
            _building.Spawned -= Spawned;
            _houseInventory.RemoveHouse(this);
        }

        private void Spawned(List<SaveWorkerSpot> saveWorkerSpots)
        {
            if (saveWorkerSpots != null)
            {
                foreach (var workerSpot in _building.WorkerSpots)
                {
                    workerSpot.IsResting = true;
                }

                if (saveWorkerSpots.Count != _building.WorkerSpots.Count)
                {
                    _saveWorkerSpots = saveWorkerSpots;
                }
                
                for (var i = 0; i < saveWorkerSpots.Count; i++)
                {
                    var saveWorkerSpot = saveWorkerSpots[i];

                    if (i < _building.WorkerSpots.Count && saveWorkerSpot.Worker != null)
                    {
                        _building.WorkerSpots[i].IsResting = true;
                        Worker worker = Instantiate(_worker, transform);
                        worker.Energy = saveWorkerSpot.Worker.CurrentEnergy;
                        _building.WorkerSpots[i].SetWorker(worker);
                    }
                }
                return;
            }
            
            foreach (var workerSpot in _building.WorkerSpots)
            {
                workerSpot.IsResting = true;
                Worker worker = Instantiate(_worker, transform);
                workerSpot.SetWorker(worker);
            }
            
            _houseInventory.AddHouse(this);
        }

        public bool HasEmptySpots()
        {
            return _building.HasAvailableSpot();
        }

        public WorkerSpot GetEmptySpot()
        {
            return _building.GetAvailableSpot();
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
        }
        
        public void EnableHiddenSpots()
        {
            if(_hiddenSpotsShown)
                return;

            if (_saveWorkerSpots != null)
            {
                foreach (var workerSpot in _hiddenSpots)
                {
                    workerSpot.IsResting = true;
                    workerSpot.gameObject.SetActive(true);
                }

                int oldSpots = _building.WorkerSpots.Count;
                _building.WorkerSpots.AddRange(_hiddenSpots);
                for (var i = oldSpots; i < _saveWorkerSpots.Count; i++)
                {
                    var saveWorkerSpot = _saveWorkerSpots[i];

                    if (saveWorkerSpot.Worker != null)
                    {
                        Worker worker = Instantiate(_worker, transform);
                        worker.Energy = saveWorkerSpot.Worker.CurrentEnergy;
                        _building.WorkerSpots[i].SetWorker(worker);
                    }
                }

                _hiddenSpotsShown = true;
                _saveWorkerSpots = null;
                return;
            }
            
            foreach (var workerSpot in _hiddenSpots)
            {
                workerSpot.IsResting = true;
                workerSpot.gameObject.SetActive(true);
                Worker worker = Instantiate(_worker, transform);
                workerSpot.SetWorker(worker);
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
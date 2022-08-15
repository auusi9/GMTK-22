using System;
using Resources;
using UnityEngine;
using UnityEngine.UI;

namespace Workers
{
    public class Worker : MonoBehaviour
    {
        [SerializeField] private DraggableWorker _draggableWorker;
        [SerializeField] private Image _image;
        [SerializeField] private int _maxEnergy;
        [SerializeField] private float _noEnergyEfficency = 0.6f;
        [SerializeField] private float _energyDuration = 60f;
        [SerializeField] private float _restDuration = 60f;
        [SerializeField] private WorkerInventory _workerInventory;
        [SerializeField] private HouseInventory _houseInventory;

        public WorkerSpot CurrentSpot;
        public DraggableWorker DraggableWorker => _draggableWorker;
        public bool IsTired => _energy <= 0;
        public bool IsFullyRested => _energy >= _maxEnergy;

        private float _energy;

        private void Start()
        {
            _energy = _maxEnergy;
            _workerInventory.AddWorker(this);
            _draggableWorker.RightClickEvent += GoToRest;
        }

        private void GoToRest()
        {
            _houseInventory.SetWorkerToNearestHouse(this);
        }

        public float Work()
        {
            _energy -= _maxEnergy / _energyDuration * Time.deltaTime;

            if (_energy < 0)
            {
                _energy = 0f;
                return _noEnergyEfficency;
            }

            return 1f;
        }

        private void Update()
        {
            _image.fillAmount = _energy / _maxEnergy;
        }

        public float Rest()
        {
            _energy += _maxEnergy / _restDuration * Time.deltaTime;

            if (_energy > _maxEnergy)
            {
                _energy = _maxEnergy;
            }

            return 1f;
        }

        private void OnDestroy()
        {
            _workerInventory.RemoveWorker(this);
            _draggableWorker.RightClickEvent -= GoToRest;
        }
    }
}
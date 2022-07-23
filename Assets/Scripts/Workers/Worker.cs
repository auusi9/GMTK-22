using System;
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

        public WorkerSpot CurrentSpot;
        public DraggableWorker DraggableWorker => _draggableWorker;

        private float _energy;

        private void Start()
        {
            _energy = _maxEnergy;
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
        
        
    }
}
﻿using System;
using System.Collections.Generic;
using MainMenu;
using Resources;
using UnityEngine;
using Workers;

namespace CityBuilder
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private Building _building;
        [SerializeField] private Resource _gold;
        [SerializeField] private int _coins;
        [SerializeField] private float _seconds;
        [SerializeField] private ShowAmountEarned _showAmountEarned;
        [SerializeField] private float _showAmountEarnedDelay;
        [SerializeField] private Worker _worker;

        private float _nextCoin = 0f;
        private float _nextShow = 0f;
        private int _coinsEarnedSinceLastShow = 0;

        public FactoryInfo GetFactoryInfo()
        {
            return new FactoryInfo()
            {
                Coins = _coins,
                Seconds = _seconds,
                ResourceName = _gold.ResourceName
            };
        }

        private void Awake()
        {
            _building.Spawned += BuildingOnSpawned;
        }

        private void OnDestroy()
        {
            _building.Spawned -= BuildingOnSpawned;
        }

        private void BuildingOnSpawned(List<SaveWorkerSpot> saveWorkerSpots)
        {
            if(saveWorkerSpots == null)
                return;
            
            for (int i = 0; i < saveWorkerSpots.Count; i++)
            {
                if (saveWorkerSpots[i].Worker != null)
                {
                    Worker worker = Instantiate(_worker, transform);
                    worker.Energy = saveWorkerSpots[i].Worker.CurrentEnergy;
                    _building.WorkerSpots[i].SetWorker(worker);
                }
            }
        }

        private void Update()
        {
            float coinsCreated = _nextCoin;
            _nextCoin = 0f;
            foreach (var workerSpot in _building.WorkerSpots)
            {
                if (!workerSpot.Available)
                {
                    float efficency = workerSpot.Worker.Work();
                    coinsCreated += efficency * (_coins / _seconds * Time.deltaTime);
                }
            }

            int intCoinsCreated = Mathf.FloorToInt(coinsCreated);
            _nextCoin = coinsCreated - intCoinsCreated;
            _gold.AddResource(intCoinsCreated);

            _nextShow += Time.deltaTime;
            _coinsEarnedSinceLastShow += intCoinsCreated;

            if (_nextShow >= _showAmountEarnedDelay && _coinsEarnedSinceLastShow > 0)
            {
                _showAmountEarned.Show(_coinsEarnedSinceLastShow, _gold);
                _nextShow = 0f;
                _coinsEarnedSinceLastShow = 0;
            }
        }
    }

    public class FactoryInfo
    {
        public int Coins;
        public float Seconds;
        public string ResourceName;
    }
}
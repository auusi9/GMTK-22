using System;
using System.Collections.Generic;
using System.Linq;
using Dice;
using Resources;
using UnityEngine;

namespace Workers
{
    [CreateAssetMenu(menuName = "Workers/Worker Inventory", fileName = "Worker Inventory", order = 0)]
    public class WorkerInventory : ScriptableObject
    {
        [SerializeField] private int _diceThreshHold;
        [SerializeField] private DiceInventory _diceInventory;
        [SerializeField] private Resource _workerResource;
        private List<Worker> _workers = new List<Worker>();

        private void OnDisable()
        {
            _workers.Clear();
        }

        public void AddWorker(Worker worker)
        {
            if (!_workers.Contains(worker))
            {
                _workers.Add(worker);
                _workerResource.AddResource(1);
            }

            int diceNumber = _workers.Count / _diceThreshHold;

            if (diceNumber >= _diceInventory.Dice.Count)
            {
                _diceInventory.AddNewDice();
            }
        }

        public void RemoveWorker(Worker worker)
        {
            if (_workers.Contains(worker))
            {
                _workers.Remove(worker);
                _workerResource.RemoveResource(1);
            }
        }

        public void RemoveRandomWorkers(int workers)
        {
            System.Random rnd = new System.Random();

            if (workers > _workers.Count)
                workers = _workers.Count;

            var deadWorkers = _workers.OrderBy(x => rnd.Next()).Take(workers);

            foreach (var worker in deadWorkers)
            {
                _workers.Remove(worker);
                Destroy(worker.gameObject);
            }

            _workerResource.RemoveResource(workers);
        }
    }
}
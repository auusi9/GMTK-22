using System.Collections.Generic;
using Dice;
using UnityEngine;

namespace Workers
{
    [CreateAssetMenu(menuName = "Workers/Worker Inventory", fileName = "Worker Inventory", order = 0)]
    public class WorkerInventory : ScriptableObject
    {
        [SerializeField] private int _diceThreshHold;
        [SerializeField] private DiceInventory _diceInventory;
        private List<Worker> _workers = new List<Worker>();
        
        public void AddWorker(Worker worker)
        {
            if (!_workers.Contains(worker))
            {
                _workers.Add(worker);
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
            }
        }
    }
}
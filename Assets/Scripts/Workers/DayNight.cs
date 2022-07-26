using System;
using Resources;
using UnityEngine;

namespace Workers
{
    [CreateAssetMenu(menuName = "Workers/DayNight", fileName = "DayNight", order = 0)]
    public class DayNight : ScriptableObject
    {
        [SerializeField] private WorkerInventory _workerInventory;
        [SerializeField] private Resource _worker;
        [SerializeField] private Resource _food;
        [SerializeField] private int _foodXFolk ;
        [Tooltip("Duration in minutes")]
        [SerializeField] private float _dayDuration ;

        private float _currentProgress;
        
        public Action DayFinished;
        public Action StartDay;

        public int DayCount;

        public float DayDuration => _dayDuration * 60;
        public float DayProgress => _currentProgress / DayDuration;
        public int FoodXFolk => _foodXFolk;

        public void RemoveWorkers()
        {
            _workerInventory.RemoveRandomWorkers(WorkersToRemove(out int neededFood));
            _food.RemoveResource(neededFood);
        }

        public void StartNewDay()
        {
            StartDay?.Invoke();
            DayCount++;
        }

        public int WorkersToRemove(out int neededFood)
        {
            neededFood = _foodXFolk * _worker.Value;

            if (neededFood <= _food.Value)
            {
                return 0;
            }

            int missingFood = neededFood - _food.Value;

            return Mathf.FloorToInt(missingFood / (float)_foodXFolk);
        }

        public void UpdateDayProgress(float time)
        {
            _currentProgress = time;

            if (_currentProgress >= DayDuration)
            {
                DayFinished?.Invoke();
            }
        }
    }
}
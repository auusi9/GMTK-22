using System;
using Dice;
using Resources;
using UnityEngine;
using Workers;

namespace CityBuilder
{
    public class Scenery : MonoBehaviour
    {
        [SerializeField] private Resource _resource;
        [SerializeField] private int _defaultSceneryAmount = 15;
        [SerializeField] private int _defaultImprovedSceneryAmount = 30;
        [SerializeField] private DayNight _dayNight;

        private int _currentValue = 0;

        public event Action<Face> SceneryAmountChanged;
        public event Action SceneryReset;

        private void OnEnable()
        {
            _currentValue = _defaultSceneryAmount;
            _dayNight.StartDay += OnNewDay;
        }

        private void OnDisable()
        {
            _dayNight.StartDay -= OnNewDay;
        }

        private void OnNewDay()
        {
            _currentValue = _defaultSceneryAmount;
        }

        public Resource Resource => _resource;
        public int CurrentValue => _currentValue;
        public float Percentage => _currentValue / (float)_defaultSceneryAmount;

        public void SpendScenery(Face face)
        {
            _currentValue--;
            SceneryAmountChanged?.Invoke(face);
        }

        public void ResetScenery()
        {
            _currentValue = _defaultSceneryAmount;
            SceneryReset?.Invoke();
        }
    }
}
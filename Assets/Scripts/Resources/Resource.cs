﻿using System;
using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(menuName = "Resources/New resource", fileName = "(NAME)Resource", order = 0)]
    public class Resource : ScriptableObject
    {
        [SerializeField] private int _defaultValue;
        [SerializeField] private int _maxValue = Int32.MaxValue;
        [SerializeField] private Sprite _sprite;

        public Sprite Sprite => _sprite;
        
        private int _currentValue = 0;
        private int _currentMaxValue = 0;

        public int Value => _currentValue;

        public Action<int> ResourceChanged;
        public Action<int> AddedResource;
        public Action<int> RemovedResource;
        
        private void OnEnable()
        {
            _currentValue = _defaultValue;
            _currentMaxValue = _maxValue;
        }

        public void AddResource(int value)
        {
            int oldCurrentValue = _currentValue;
            _currentValue += value;

            if (_currentValue > _currentMaxValue)
            {
                _currentValue = _currentMaxValue;
            }
            
            int delta = _currentValue - oldCurrentValue;

            if (delta > 0)
            {
                ResourceChanged?.Invoke(_currentValue);
                AddedResource?.Invoke(delta);
            }
        }
        
        public void RemoveResource(int value)
        {
            int oldCurrentValue = _currentValue;
            _currentValue -= value;

            if (_currentValue < 0)
            {
                _currentValue = 0;
            }

            int delta = oldCurrentValue - _currentValue;

            if (delta > 0)
            {
                ResourceChanged?.Invoke(_currentValue);
                RemovedResource?.Invoke(-delta);
            }
        }
    }
}
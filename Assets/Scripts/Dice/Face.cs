﻿using System;
using CityBuilder;
using Resources;
using UnityEngine;

namespace Dice
{
    [CreateAssetMenu(menuName = "Dice/Dice Face", fileName = "{Name}Face", order = 1)]
    public class Face : ScriptableObject
    {
        [SerializeField] private Resource _resource;
        [SerializeField] private bool _hasCombo;
        [SerializeField] private FaceCombo _combo;
        [SerializeField] private int _reward;
        [SerializeField] private int _maxReward;
        [SerializeField] private int _minPrice;
        [SerializeField] private int _priceXReward;

        [Header("UI")] 
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _description;

        public Sprite Sprite => _sprite;
        public string Description => _description;

        public Building _building;

        public Action Destroying;

        public Resource Resource => _resource;

        private int _currentReward;

        private void OnEnable()
        {
            _currentReward = _reward;
        }

        public void SetBuilding(Building building)
        {
            _building = building;
            _building.Destroyed += ToDestroy;
            CalculateLevel();
        }

        private void OnDestroy()
        {
            if (_building != null)
            {
                _building.Destroyed -= ToDestroy;
            }
        }

        public void ToDestroy()
        {
            Destroying?.Invoke();
            Destroy(this);
        }

        public int GetPrice()
        {
            return _minPrice + _priceXReward * _reward;
        }

        public void GiveReward()
        {
            _resource.AddResource(_currentReward);
            Debug.Log(_resource.name + " Given reward " + _currentReward);
        }

        public void GiveCombo(int combo)
        {
            if(!_hasCombo)
                return;

            if (_combo.ComboNeeded <= combo)
            {
                if (_combo.ExtraCondition == null || _combo.ExtraCondition.IsValid())
                {
                    _combo.ResourceReward.AddResource(_combo.Reward);
                }
            }
        }

        public void CalculateLevel()
        {
            if(_building == null)
                return;

            Building[] buildings = _building.GetNearBuildings();

            int number = _reward;

            foreach (var building in buildings)
            {
                if (building != null && building.Scenery != null && building.Scenery.Resource == _resource)
                {
                    number++;
                }
            }

            _currentReward = number;
        }
    }
    
    [Serializable]
    public class FaceCombo
    {
        [SerializeField] private int _comboNeeded;
        [SerializeField] private int _reward;
        [SerializeField] private Resource _resourceReward;
        [SerializeField] private FaceComboCondition _extraCondition;

        public int ComboNeeded => _comboNeeded;
        public int Reward => _reward;
        public Resource ResourceReward => _resourceReward;
        public FaceComboCondition ExtraCondition => _extraCondition;
    }

    public abstract class FaceComboCondition : ScriptableObject
    {
        public abstract bool IsValid();
    }

    public class OwnsBuilding : FaceComboCondition
    {
        public override bool IsValid()
        {
            return true;
        }
    }
}
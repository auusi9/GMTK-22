using System;
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
        [SerializeField] private string _description;

        public Sprite Sprite => _resource.Sprite;
        public Color ShadowColor => _resource.ShadowColor;
        public string Description => _description;

        [HideInInspector] public Building _building;

        public event Action Destroying;
        public event Action RewardChanged;

        public Resource Resource => _resource;
        public int CurrentReward => _currentReward;

        public bool HasCombo => _hasCombo && (_combo.ExtraCondition == null || _combo.ExtraCondition.IsValid());
        public int ComboNeeded => _combo.ComboNeeded;

        public Resource ComboResource => _combo.ResourceReward;
        
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
            return _minPrice + _priceXReward * _currentReward;
        }

        public int GiveReward(int comboGiven)
        {
            if (_currentReward - comboGiven <= 0)
                return 0;
            
            _resource.AddResource(_currentReward - comboGiven);
            Debug.Log(_resource.name + " Given reward " + (_currentReward - comboGiven));
            return (_currentReward - comboGiven);
        }

        public int GiveCombo(int combo)
        {
            if(!_hasCombo)
                return 0;

            if (_combo.ExtraCondition == null || _combo.ExtraCondition.IsValid())
            {
                int comboReward = _combo.Reward * combo;
                    _combo.ResourceReward.AddResource(comboReward);
                Debug.Log(_combo.ResourceReward.ResourceName + " Given COMBO " + comboReward);
                return comboReward;
            }

            return 0;
        }
        
        public void CalculateLevel()
        {
            if(_building == null)
                return;

            Building[] buildings = _building.Get4NearBuildings();

            int number = _reward;

            foreach (var building in buildings)
            {
                if (building != null && building.Scenery != null && building.Scenery.Resource == _resource)
                {
                    number++;
                }
            }

            _currentReward = number;
            RewardChanged?.Invoke();
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
}
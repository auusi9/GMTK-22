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
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _description;

        public Sprite Sprite => _sprite;
        public string Description => _description;

        public Action Destroying;

        public Resource Resource => _resource;
        public void SetReward(int newReward)
        {
            _reward = newReward;
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
            _resource.AddResource(_reward);
            Debug.Log(_resource.name + " Given reward " + _reward);
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
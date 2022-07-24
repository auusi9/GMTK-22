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
    }
    
    [Serializable]
    public class FaceCombo
    {
        [SerializeField] private int _comboNeeded;
        [SerializeField] private int _reward;
        [SerializeField] private Resource _resourceReward;
        [SerializeField] private FaceComboCondition _extraCondition;
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